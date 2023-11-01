using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject retryBtn;

    private TextMeshProUGUI[] _resultStats;
    private int _loadingStat = 0;
    private Vector3[] _initPos;
    private Vector3[] _targetPos;

    private void Awake()
    {
        _resultStats = GetComponentsInChildren<TextMeshProUGUI>();


        _resultStats[0].text = Globals.lastRushScore.ToString();
        System.TimeSpan calcEllapsed = System.TimeSpan.FromSeconds(Globals.lastTime);
        _resultStats[1].text = string.Format("{0}:{1}", calcEllapsed.Minutes < 10 ? "0" + calcEllapsed.Minutes.ToString() : calcEllapsed.Minutes, calcEllapsed.Seconds < 10 ? "0" + calcEllapsed.Seconds.ToString() : calcEllapsed.Seconds);

        _initPos = new Vector3[_resultStats.Length];
        _targetPos = new Vector3[_resultStats.Length];
        
        for(int i = 0; i < _resultStats.Length; i++) 
        {
            _targetPos[i] = _resultStats[i].GetComponent<RectTransform>().localPosition;
            _initPos[i] = new Vector3(_initPos[i].x, _initPos[i].y - 1000, _initPos[i].z);
            _resultStats[i].GetComponent<RectTransform>().localPosition = _initPos[i];
            Color tmpColor = _resultStats[i].color;
            _resultStats[i].color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 0);
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
        var step = 1500f * Time.deltaTime;
        if (_loadingStat < _resultStats.Length)
        {
            _resultStats[_loadingStat].GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(_resultStats[_loadingStat].GetComponent<RectTransform>().localPosition, _targetPos[_loadingStat], step);

            Color tmpColor = _resultStats[_loadingStat].color;
            float fadeIn = tmpColor.a + Time.deltaTime;

            _resultStats[_loadingStat].color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, fadeIn);

            if (Vector3.Distance(_resultStats[_loadingStat].GetComponent<RectTransform>().localPosition, _targetPos[_loadingStat]) < 0.001f)
            {
                _resultStats[_loadingStat].GetComponent<RectTransform>().localPosition  = _targetPos[_loadingStat];
                _resultStats[_loadingStat].color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, 1);
                _loadingStat++;
            }
        }
        
    }
}
