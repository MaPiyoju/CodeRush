using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class ProfileScript : MonoBehaviour
{
    private LoadStatsScript loadStatsScript;

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
       if(Globals.usuario != null)
        {
            loadStatsScript = GetComponent<LoadStatsScript>();

            nameValue = nameText.GetComponent<TextMeshProUGUI>();
            nameValue.text = Globals.usuario.name;

            aliasValue = aliasText.GetComponent<TextMeshProUGUI>();
            aliasValue.text = Globals.usuario.alias;

            dateValue = dateText.GetComponent<TextMeshProUGUI>();
            System.DateTime first_login = Globals.usuario.first_login;
            CultureInfo ci = new CultureInfo("es-ES");
            string FechaFormateada = first_login.ToString("MMMM", ci) + " del " + first_login.ToString("yyyy");
            FechaFormateada = char.ToUpper(FechaFormateada[0]) + FechaFormateada.Substring(1);
            dateValue.text = "Usuario desde " + FechaFormateada;

            highestStreakValue = highestStreakText.GetComponent<TextMeshProUGUI>();
            highestStreakValue.text = Globals.usuario.streak.ToString();

            categoryValue = categoryText.GetComponent<TextMeshProUGUI>();
            categoryValue.text = DeterminarNivelExperiencia(Globals.usuario.exp);

            int maxTime = Globals.usuario.history.Max(x => x.time);
            longerTimeValue = longerTimeText.GetComponent<TextMeshProUGUI>();
            longerTimeValue.text = ConvertirSegundosAMinutos(maxTime);

            if(Globals.usuario.history.Count() > 0)
            {
                IEnumerable<DateTime> days = Globals.usuario.history.Select(x => x.date);
                int streakDays = GetConsecutiveDays(Globals.usuario.history.ToArray()[0].date, days).Count();
                streakDaysValue = streakDaysText.GetComponent<TextMeshProUGUI>();
                streakDaysValue.text = streakDays.ToString();
            }
        }
    }

    public string ConvertirSegundosAMinutos(int segundos)
    {
        int hor, min, seg;
        hor = (segundos / 3600);
        min = ((segundos - hor * 3600) / 60);
        seg = segundos - (hor * 3600 + min * 60);
        return min + ":" + seg;
    }

    static IEnumerable<DateTime> GetConsecutiveDays(DateTime startDate, IEnumerable<DateTime> days)
    {
        var wantedDate = startDate;
        foreach (var day in days.Where(d => d >= startDate).OrderBy(d => d))
        {
            if (day == wantedDate)
            {
                yield return day;
                wantedDate = wantedDate.AddDays(1);
            }
            else
            {
                yield break;
            }
        }
    }

    public string DeterminarNivelExperiencia(int exp)
    {
        if (exp >= 0 && exp < 2000)
        {
            return "Newbie";
        }
        else if (exp >= 2000 && exp < 4000)
        {
            return "Gateador";
        }
        else if (exp >= 4000 && exp < 6000)
        {
            return "Iniciado";
        }
        else if (exp >= 6000 && exp < 8000)
        {
            return "Aprendiz";
        }
        else if (exp >= 8000 && exp < 10000)
        {
            return "Experto";
        }
        else if (exp >= 10000)
        {
            return "Pro";
        }
        else
        {
            return "Desconocido"; // Valor por defecto si el rango no se encuentra
        }
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
