using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class QuizManager : MonoBehaviour
{
    //GameObject props
    public GameObject questionTxt;
    public GameObject optionPrefab;
    public GameObject optionContainer;
    public GameObject inputTxt;

    private Enums.QuizTypes quizType = 0;
    private GameObject[] _options;


    //Question vars
    private QuestionModel[] _questions;
    
    //Position questions vars
    private RectTransform _transform;
    private Vector3 _targetPos;
    private Enums.QuestionType _type = Enums.QuestionType.Closed;
    private bool _moveEnable = false;
    private bool _moveOptsEnable = false;
    private int _optCont = 0;
    private Vector3 _targetOptsPos;
    private Vector3 _targetTxtPos;

    //Camera vars
    private Camera _camera;
    private int _bcChange = 0;
    private float _tChange = 0f;

    //Practice mode vars
    public GameObject practiceGO;
    private Slider _slider;

    //Rush mode vars
    public GameObject rushGO;
    public TextMeshProUGUI rushGO_Count;
    private AttemptItemHandler[] _failMarks;
    private int _attempts = 0;
    private int _correct = 0;
    private bool _canAnimate = false;


    async void Awake()
    {
        var seed = Environment.TickCount;
        var random = new System.Random(seed);

        _options = new GameObject[0];
        _slider = practiceGO.GetComponentInChildren<Slider>();

        _transform = GetComponent<RectTransform>();
        _targetPos = _transform.position;
        _camera = Camera.main;

        FirestoreManager dbManager = new FirestoreManager();
        List<Dictionary<string, object>> items = await dbManager.ReadDataAsync("questions");
        _questions = new QuestionModel[items.Count];
        int cont = 0;
        foreach (var item in items)
        {
            string[] opts = ((IEnumerable)item["answers"]).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
            
            _questions[cont] = new QuestionModel(item["question"].ToString(), item["type"].ToString() == "open" ? Enums.QuestionType.Open : Enums.QuestionType.Closed, opts, int.Parse(item["correct"].ToString() ?? "0"), (Enums.QuestionDifficulty)Enum.Parse(typeof(Enums.QuestionDifficulty), item["difficulty"].ToString()), (Enums.QuestionCategory)Enum.Parse(typeof(Enums.QuestionCategory), item["category"].ToString()));
            cont++;
        }

        _failMarks = rushGO.GetComponentsInChildren<AttemptItemHandler>();

        Globals.lastNumQuestions = 0;

        GenerateQuestion();

        quizType = Globals.gameMode;
        if (quizType == Enums.QuizTypes.Practice)
        {
            practiceGO.SetActive(true);
        }
        else
        {
            rushGO.SetActive(true);
            rushGO.GetComponentInChildren<TimeHandler>().SetTimer();
        }

        _targetTxtPos = inputTxt.GetComponent<RectTransform>().localPosition;
        inputTxt.SetActive(false);
    }

    void GenerateQuestion()
    {
        ClearOptions();

        var seed = Environment.TickCount;
        var random = new System.Random(seed);
        int ranPregunta= random.Next(0, _questions.Length-1);

        questionTxt.GetComponent<TextMeshProUGUI>().text = _questions[ranPregunta].Question;
        _options = new GameObject[_questions[ranPregunta].Answers.Length];
        _type = _questions[ranPregunta].Type;

        //In case of closed question
        if (_type == Enums.QuestionType.Closed)
        {
            for (int i = 0; i < _options.Length; i++)
            {
                _options[i] = Instantiate(optionPrefab, optionContainer.transform);
                _options[i].GetComponentInChildren<TextMeshProUGUI>().text = _questions[ranPregunta].Answers[i];
                if (i == _questions[ranPregunta].Correct)
                    _options[i].GetComponentInChildren<OptionHandler>().correctOption = true;
                else
                    _options[i].GetComponentInChildren<OptionHandler>().correctOption = false;

                _options[i].GetComponentInChildren<OptionHandler>().quizManager = this.gameObject;

            }

            _options = ShuffleOptions(_options);

            int initY = 20;
            for (int i = 0; i < _options.Length; i++)
            {
                Vector3 initPos = _options[i].GetComponent<RectTransform>().localPosition;
                _options[i].GetComponent<RectTransform>().localPosition = new Vector3(initPos.x, initY, initPos.z);
                initY -= 240;
                _options[i].SetActive(false);
                _targetOptsPos = initPos;
            }
        }
        else
        {
            Vector3 initPos = inputTxt.GetComponent<RectTransform>().localPosition;
            inputTxt.GetComponent<RectTransform>().localPosition = new Vector3(initPos.x + 1150, initPos.y, initPos.z);
            inputTxt.SetActive(true);
            inputTxt.GetComponent<OptionHandler>().enterEnabled = true;
        }
        _transform.position = new Vector3(-550, _transform.position.y, _transform.position.z);
        _moveEnable = true;

        Globals.lastNumQuestions++;
        _canAnimate = true;
    }

    GameObject[] ShuffleOptions(GameObject[] baseArr)
    {
        var seed = Environment.TickCount;
        var rnd = new System.Random(seed);
        var res = new GameObject[baseArr.Length];

        res[0] = baseArr[0];
        for (int i = 1; i < baseArr.Length; i++)
        {
            int j = rnd.Next(i);
            res[i] = res[j];
            res[j] = baseArr[i];
        }
        return res;
    }

    void ClearOptions()
    {
        if (_options.Length > 0)
        {
            foreach (GameObject go in _options)
            {
                Destroy(go);
            }
        }
    }

    public void Answered(bool isCorrect)
    {
        if (isCorrect == true)
        {
            if (quizType == Enums.QuizTypes.Practice)
                _slider.value += 0.1F;
            else
            {
                _correct++;
                rushGO_Count.text = _correct.ToString();
            }

            _bcChange = 1;
        }
        else
        {
            if (quizType != Enums.QuizTypes.Practice) { 
                _failMarks[_attempts].Mark();
                _attempts++;
            }
            _bcChange = 2;
            Vibration.Vibrate(250);
        }
        _tChange = Time.time;
        if (_attempts == 3)//Lose
        {
            StartCoroutine("ShowResults");
        }
        else
        {
            GenerateQuestion();
        }
    }

    IEnumerator ShowResults()
    {
        rushGO.GetComponentInChildren<TimeHandler>().StopTimer();
        Globals.lastRushScore = _correct;
        Globals.lastTime = rushGO.GetComponentInChildren<TimeHandler>().GetTime();

        Globals.lives--;
        LifeHandler.UpdateLife();

        yield return new WaitForSeconds(2);
        ScenesManager.Instance.LoadScene(Enums.Scenes.Results);
    }

    private void LerpCamera()
    {
        float t = Mathf.PingPong(Time.time, 1f) / 1f;
        Color targetColor = (_bcChange == 1) ? Color.green : Color.red;
        Color lerp = Color.Lerp(Color.white, targetColor, t);
        _camera.backgroundColor = lerp;
        if (Time.time - _tChange > 0.2f)
        {
            _bcChange = 0;
            _camera.backgroundColor = Color.white;
        }
    }

    private void Update()
    {
        var step = 5000f * Time.deltaTime;
        if (_moveEnable && _canAnimate)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _targetPos, step);

            if (Vector3.Distance(_transform.position, _targetPos) < 0.001f)
            {
                _transform.position = _targetPos;
                _moveEnable = false;

                if (_type == Enums.QuestionType.Closed) {
                    for (int i = 0; i < _options.Length; i++)
                    {
                        Vector3 initPos = _options[i].GetComponent<RectTransform>().localPosition;
                        _options[i].GetComponent<RectTransform>().localPosition = new Vector3(initPos.x - 1100, initPos.y, initPos.z);
                        _options[i].SetActive(true);
                    }
                    _moveOptsEnable = true;
                }
                else
                {
                    _moveOptsEnable = true;
                }
            }
        }

        if (_moveOptsEnable)
        {
            RectTransform optTmp = _type == Enums.QuestionType.Closed ? _options[_optCont].GetComponent<RectTransform>() : inputTxt.GetComponent<RectTransform>();
            Vector3 optTarget = _type == Enums.QuestionType.Closed ? _targetOptsPos : _targetTxtPos;

            Vector3 targetTmp = new Vector3(optTarget.x, optTmp.localPosition.y, optTarget.z);
            optTmp.localPosition = Vector3.MoveTowards(optTmp.localPosition, targetTmp, step);

            if (Vector3.Distance(optTmp.localPosition, targetTmp) < 0.001f)
            {
                optTmp.localPosition = targetTmp;
                _optCont++;
                if (_optCont == _options.Length || _type == Enums.QuestionType.Open)
                {
                    _optCont = 0;
                    _moveOptsEnable = false;
                    _canAnimate = false;
                }
            }
        }

        if (_bcChange > 0) {
            LerpCamera();
        }
    }
}
