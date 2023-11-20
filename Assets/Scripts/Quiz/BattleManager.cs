using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;
using Slider = UnityEngine.UI.Slider;

public class BattleManager : MonoBehaviour
{
    //GameObject props
    public GameObject questionTxt;
    public GameObject optionPrefab;
    public GameObject optionContainer;
    public GameObject inputTxt;

    private Enums.QuizTypes quizType = Enums.QuizTypes.Battle;
    private GameObject[] _options;

    //Question vars
    private QuestionModel[] _questions;

    //Position questions vars
    private RectTransform _transform;
    private Vector3 _targetPos;
    private Enums.QuestionType _type = Enums.QuestionType.closed;
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
    private Slider _playerSlider;
    private Slider _enemySlider;

    //Battle mode vars
    public GameObject playerGO;
    public GameObject enemyGO;

    public GameObject playerSkull;
    public GameObject enemySkull;

    private Vector3 _playerSkullOriginal;
    private Vector3 _enemySkullOriginal;

    private AttemptItemHandler[] _playerFailMarks;
    private AttemptItemHandler[] _enemyFailMarks;

    private int _playerAttempts = 0;
    private int _enemyAttempts = 0;
    private int _correct = 0;
    private bool _canAnimate = false;


    async void Awake()
    {
        var seed = Environment.TickCount;
        var random = new System.Random(seed);

        _options = new GameObject[0];

        _playerSlider = playerGO.GetComponentInChildren<Slider>();
        _enemySlider = enemyGO.GetComponentInChildren<Slider>();

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

            _questions[cont] = new QuestionModel(item["question"].ToString(), item["type"].ToString() == "open" ? Enums.QuestionType.open : Enums.QuestionType.closed, opts, int.Parse(item["correct"].ToString() ?? "0"), (Enums.QuestionDifficulty)Enum.Parse(typeof(Enums.QuestionDifficulty), item["difficulty"].ToString()), (Enums.QuestionCategory)Enum.Parse(typeof(Enums.QuestionCategory), item["category"].ToString()));
            cont++;
        }

        _playerFailMarks = playerGO.GetComponentsInChildren<AttemptItemHandler>();
        _enemyFailMarks = enemyGO.GetComponentsInChildren<AttemptItemHandler>();

        _playerSkullOriginal = playerSkull.transform.position;
        _enemySkullOriginal = enemySkull.transform.position;

        Globals.lastNumQuestions = 0;

        GenerateQuestion();

        Globals.gameMode = quizType;
        
        playerGO.GetComponentInChildren<TimeHandler>().SetTimer();

        _targetTxtPos = inputTxt.GetComponent<RectTransform>().localPosition;
        inputTxt.SetActive(false);
    }

    void GenerateQuestion()
    {
        ClearOptions();

        var seed = Environment.TickCount;
        var random = new System.Random(seed);
        int ranPregunta = random.Next(0, _questions.Length - 1);

        questionTxt.GetComponent<TextMeshProUGUI>().text = _questions[ranPregunta].Question;
        _options = new GameObject[_questions[ranPregunta].Answers.Length];
        _type = _questions[ranPregunta].Type;

        //In case of closed question
        if (_type == Enums.QuestionType.closed)
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
            _playerSlider.value += 0.2F;
            _correct++;
            _bcChange = 1;

            if(_playerSlider.value == 1)
            {
                _playerSlider.value = 0F;
                StartCoroutine("Attack");
            }
        }
        else
        {
            _playerFailMarks[_playerAttempts].Mark();
            _playerAttempts++;
            _bcChange = 2;
            Vibration.Vibrate(250);
        }
        _tChange = Time.time;
        if (_playerAttempts == 3 || _enemyAttempts == 3)//Finish match
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
        playerGO.GetComponentInChildren<TimeHandler>().StopTimer();
        Globals.lastTime = playerGO.GetComponentInChildren<TimeHandler>().GetTime();

        Globals.lastRushScore = _correct;
        Globals.lives--;
        LifeHandler.UpdateLife();

        yield return new WaitForSeconds(1);

        Transform[] gos = GetComponents<Transform>();
        Transform[] pgos = playerGO.GetComponents<Transform>();
        Transform[] egos = enemyGO.GetComponents<Transform>();
        var random = new System.Random();
        foreach (Transform go in gos.Concat(pgos).Concat(egos).ToArray())
        {
            go.gameObject.AddComponent<Rigidbody2D>();
            go.gameObject.GetComponent<Rigidbody2D>().gravityScale = random.Next(80, 250);
        }
        yield return new WaitForSeconds(2);
        ScenesManager.Instance.LoadScene(Enums.Scenes.Results);
    }

    IEnumerator Attack()
    {
        float speed = 1500;
        Vector3 targetPos = new Vector3(playerSkull.transform.position.x, playerSkull.GetComponent<SkullHandler>().targetPos.y, playerSkull.GetComponent<SkullHandler>().targetPos.z);
        
        while (Vector2.Distance(playerSkull.transform.position, targetPos) > 0.05f)
        {
            Vector2 dir = targetPos - playerSkull.transform.position;
            playerSkull.transform.Translate(dir.normalized * speed * Time.deltaTime);
            yield return null;
        }

        /*Update target*/
        playerSkull.transform.position = new Vector2(targetPos.x, targetPos.y);//Set position as target pos
        _enemyFailMarks[_enemyAttempts].Mark();
        _enemyAttempts++;
        
        yield return new WaitForSeconds(0.5F);

        playerSkull.transform.position = _playerSkullOriginal;
    }

    IEnumerator ReceiveAttack()
    {
        float speed = 500;
        Vector3 targetPos = new Vector3(enemySkull.transform.position.x, enemySkull.GetComponent<SkullHandler>().targetPos.y, enemySkull.GetComponent<SkullHandler>().targetPos.z);

        while (Vector2.Distance(enemySkull.transform.position, targetPos) > 0.05f)
        {
            Vector2 dir = targetPos - enemySkull.transform.position;
            enemySkull.transform.Translate(dir.normalized * speed * Time.deltaTime);
            yield return null;
        }

        /*Update target*/
        enemySkull.transform.position = new Vector2(targetPos.x, targetPos.y);//Set position as target pos
        _playerFailMarks[_playerAttempts].Mark();
        _playerAttempts++;

        yield return new WaitForSeconds(0.5F);

        enemySkull.transform.position = _enemySkullOriginal;
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

                if (_type == Enums.QuestionType.closed)
                {
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
            RectTransform optTmp = _type == Enums.QuestionType.closed ? _options[_optCont].GetComponent<RectTransform>() : inputTxt.GetComponent<RectTransform>();
            Vector3 optTarget = _type == Enums.QuestionType.closed ? _targetOptsPos : _targetTxtPos;

            Vector3 targetTmp = new Vector3(optTarget.x, optTmp.localPosition.y, optTarget.z);
            optTmp.localPosition = Vector3.MoveTowards(optTmp.localPosition, targetTmp, step);

            if (Vector3.Distance(optTmp.localPosition, targetTmp) < 0.001f)
            {
                optTmp.localPosition = targetTmp;
                _optCont++;
                if (_optCont == _options.Length || _type == Enums.QuestionType.open)
                {
                    _optCont = 0;
                    _moveOptsEnable = false;
                    _canAnimate = false;
                }
            }
        }

        if (_bcChange > 0)
        {
            LerpCamera();
        }
    }
}
