using Firebase.Firestore;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using Firebase.Extensions;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Xml.Linq;

public class ResultManager : MonoBehaviour
{
    public GameObject retryBtn;

    private TextMeshProUGUI[] _resultStats;
    private int _loadingStat = 0;
    private Vector3[] _initPos;
    private Vector3[] _targetPos;

    async void Awake()
    {
        _resultStats = GetComponentsInChildren<TextMeshProUGUI>();


        _resultStats[0].text = Globals.lastRushScore.ToString();
        System.TimeSpan calcEllapsed = System.TimeSpan.FromSeconds(Globals.lastTime);
        _resultStats[1].text = string.Format("{0}:{1}", calcEllapsed.Minutes < 10 ? "0" + calcEllapsed.Minutes.ToString() : calcEllapsed.Minutes, calcEllapsed.Seconds < 10 ? "0" + calcEllapsed.Seconds.ToString() : calcEllapsed.Seconds);


        int expectedTime = Globals.lastRushScore * 5;
        int score = (Globals.lastRushScore * 25) + Mathf.CeilToInt((expectedTime/Globals.lastTime) * 100);
        _resultStats[2].text = score.ToString();

        _initPos = new Vector3[_resultStats.Length];
        _targetPos = new Vector3[_resultStats.Length];
        
        for(int i = 0; i < _resultStats.Length-1; i++) 
        {
            _targetPos[i] = _resultStats[i].GetComponent<RectTransform>().localPosition;
            _initPos[i] = new Vector3(_initPos[i].x, _initPos[i].y - 1000, _initPos[i].z);
            _resultStats[i].GetComponent<RectTransform>().localPosition = _initPos[i];
            Color tmpColor = _resultStats[i].color;
            _resultStats[i].color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 0);
        }


        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("users").Document(auth.CurrentUser.UserId);
        
        MatchModel newmatch = new MatchModel()
        {
            date = DateTime.Now,
            score = score,
            time = Mathf.CeilToInt(Globals.lastTime),
            type = Globals.gameMode,
            win = true,
            questions = Globals.lastNumQuestions,
            correctQuestions = Globals.lastRushScore
        };

        FirestoreManager dbManager = new FirestoreManager();
        List<Dictionary<string, object>> items = await dbManager.ReadDataByIdAsync("users", auth.CurrentUser.UserId);


        List<MatchModel> history = new List<MatchModel>();
        foreach (var item in items)
        {
            foreach (Dictionary<string, object> hst in (IEnumerable)item["history"])
            {
                var ts = (Timestamp)hst["date"];
                var currDt = ts.ToDateTime();

                MatchModel matchTmp = new()
                {
                    date = currDt,
                    score = int.Parse(hst["score"].ToString()),
                    time = int.Parse(hst["time"].ToString()),
                    type = (Enums.QuizTypes)int.Parse(hst["type"].ToString()),
                    win = bool.Parse(hst["win"].ToString()),
                    questions = int.Parse(hst["questions"].ToString()),
                    correctQuestions = int.Parse(hst["correctQuestions"].ToString())
                };
                history.Add(matchTmp);
            }

            history.Add(newmatch);

            int newExp = int.Parse(item["exp"].ToString()) + score;
            int maxStreak = int.Parse(item["streak"].ToString()) > Globals.lastRushScore ? int.Parse(item["streak"].ToString()) : Globals.lastRushScore;

            Dictionary<string, object> last = new()
            {
                { "history", history.ToArray()},
                { "lives", Globals.lives},
                { "exp", newExp },
                { "streak", maxStreak }
            };
            await docRef.UpdateAsync(last).ContinueWithOnMainThread(task =>
            {
                Debug.Log("Updated history");
            });
        }

        if (Globals.lives < 1)
            retryBtn.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_resultStats.Length > 0)
        {
            var step = 1500f * Time.deltaTime;
            if (_loadingStat < _resultStats.Length - 1)
            {
                _resultStats[_loadingStat].GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(_resultStats[_loadingStat].GetComponent<RectTransform>().localPosition, _targetPos[_loadingStat], step);

                Color tmpColor = _resultStats[_loadingStat].color;
                float fadeIn = tmpColor.a + Time.deltaTime;

                _resultStats[_loadingStat].color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, fadeIn);

                if (Vector3.Distance(_resultStats[_loadingStat].GetComponent<RectTransform>().localPosition, _targetPos[_loadingStat]) < 0.001f)
                {
                    _resultStats[_loadingStat].GetComponent<RectTransform>().localPosition = _targetPos[_loadingStat];
                    _resultStats[_loadingStat].color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1);
                    _loadingStat++;
                }
            }
        }
    }
}
