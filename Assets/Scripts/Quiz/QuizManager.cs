using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    //GameObject props
    public GameObject questionTxt;
    public GameObject optionPrefab;
    public GameObject optionContainer;
    public GameObject statusSlider;

    private GameObject[] _options;

    private QuestionModel[] testQuestions = new QuestionModel[5];
    private string[] opts = new string[4];
    private Slider _slider;

    
    //Position questions vars
    private RectTransform _transform;
    private Vector3 _targetPos;
    private bool _moveEnable = false;
    private bool _moveOptsEnable = false;
    private int _optCont = 0;
    private Vector3 _targetOptsPos;

    void Awake()
    {
        var seed = Environment.TickCount;
        var random = new System.Random(seed);

        for (int i = 0; i < 4; i++)
        {
            opts[i] = $"Opción {i}";
        }

        for (int i = 0; i < testQuestions.Length; i++)
        {
            int ranCorrect = random.Next(0, 4);
            testQuestions[i] = new QuestionModel($"Pregunta {i}", Enums.QuestionType.Closed, opts, ranCorrect, Enums.QuestionDifficulty.Easy, Enums.QuestionCategory.Logic);
        }
        _options = new GameObject[0];
        _slider = statusSlider.GetComponentInChildren<Slider>();

        _transform = GetComponent<RectTransform>();
        _targetPos = _transform.position;
    }

    private void Start()
    {
        GenerateQuestion();
    }

    void GenerateQuestion()
    {
        ClearOptions();

        var seed = Environment.TickCount;
        var random = new System.Random(seed);
        int ranPregunta= random.Next(0, testQuestions.Length);

        questionTxt.GetComponent<TextMeshProUGUI>().text = testQuestions[ranPregunta].Question;
        _options = new GameObject[testQuestions[ranPregunta].Answers.Length];
        
        for (int i = 0; i < _options.Length; i++)
        {
            _options[i] = Instantiate(optionPrefab, optionContainer.transform);
            _options[i].GetComponentInChildren<TextMeshProUGUI>().text = testQuestions[ranPregunta].Answers[i];
            if (i == testQuestions[ranPregunta].Correct)
                _options[i].GetComponentInChildren<OptionHandler>().correctOption = true;
            else
                _options[i].GetComponentInChildren<OptionHandler>().correctOption = false;

            _options[i].GetComponentInChildren<OptionHandler>().quizManager = this.gameObject;
            
        }

        int[] indices = new int[_options.Length];
        for (int i = 0; i < _options.Length; i++)
        {
            indices[i] = i;
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
        _transform.position = new Vector3(-550, _transform.position.y, _transform.position.z);
        _moveEnable = true;

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
            _slider.value += 0.1F;
        else
            Debug.Log("Mal");


        GenerateQuestion();
    }

    private void Update()
    {
        var step = 3000f * Time.deltaTime;
        if (_moveEnable)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, _targetPos, step);
            
            if (Vector3.Distance(_transform.position, _targetPos) < 0.001f)
            {
                _transform.position = _targetPos;
                _moveEnable = false;

                for (int i = 0; i < _options.Length; i++)
                {
                    Vector3 initPos = _options[i].GetComponent<RectTransform>().localPosition;
                    _options[i].GetComponent<RectTransform>().localPosition = new Vector3(initPos.x - 1100, initPos.y, initPos.z);
                    _options[i].SetActive(true);
                }

                _moveOptsEnable = true;
            }
        }

        if (_moveOptsEnable)
        {
            RectTransform optTmp = _options[_optCont].GetComponent<RectTransform>();
            Vector3 targetTmp = new Vector3(_targetOptsPos.x, optTmp.localPosition.y, _targetOptsPos.z);
            optTmp.localPosition = Vector3.MoveTowards(optTmp.localPosition, targetTmp, step);

            if (Vector3.Distance(optTmp.localPosition, targetTmp) < 0.001f)
            {
                optTmp.localPosition = targetTmp;
                _optCont++;
                if (_optCont == _options.Length)
                {
                    _optCont = 0;
                    _moveOptsEnable = false;
                }
            }
        }
    }
}
