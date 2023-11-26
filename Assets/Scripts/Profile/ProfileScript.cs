using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileScript : MonoBehaviour
{
    public GameObject nameText;
    private TextMeshProUGUI nameValue;
    public GameObject aliasText;
    private TextMeshProUGUI aliasValue;
    public GameObject dateText;
    private TextMeshProUGUI dateValue;

    public GameObject longerTimeText;
    private TextMeshProUGUI longerTimeValue;
    public GameObject categoryText;
    private TextMeshProUGUI categoryValue;
    public GameObject streakDaysText;
    private TextMeshProUGUI streakDaysValue;
    public GameObject highestStreakText;
    private TextMeshProUGUI highestStreakValue;

    private void Awake()
    {
        nameValue = nameText.GetComponent<TextMeshProUGUI>();
        nameValue.text = Globals.usuario.name;

        Debug.Log("nombre: "+nameValue.text);

        aliasValue = aliasText.GetComponent<TextMeshProUGUI>();
        aliasValue.text = Globals.usuario.alias;

        dateValue = dateText.GetComponent<TextMeshProUGUI>();
        System.DateTime first_login = Globals.usuario.first_login;
        dateValue.text = "Usuario desde "+first_login.ToString();

        highestStreakValue = highestStreakText.GetComponent<TextMeshProUGUI>();
        highestStreakValue.text = Globals.usuario.streak.ToString();

        categoryValue = categoryText.GetComponent<TextMeshProUGUI>();
        categoryValue.text = Globals.usuario.exp.ToString();

        longerTimeValue = longerTimeText.GetComponent<TextMeshProUGUI>();
        longerTimeValue.text = "222";

        streakDaysValue = streakDaysText.GetComponent<TextMeshProUGUI>();
        streakDaysValue.text = "5";

        // max tiempo max scor recorrer la lista de array y filtar el más alto
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
