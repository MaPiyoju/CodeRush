using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using System;

public class LoadStatsScript : MonoBehaviour
{
    //private UsersModel[] mockData;
    public GameObject prefabRegistro;
    public GameObject scrollViewContent;
    private string defaultImage= "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";
    private bool datosCargados = false;
    private string experiencia;

    async void Awake()
    {
        obtenerExperienciaUsuario();
        //await ObtenerDatosAsync();
        await ObtenerDatosPorExperienciaAsync(experiencia);
        
        datosCargados = true;
    }
    
    // Start is called before the first frame update
    public void Start()
    {

    }

    public async Task ObtenerDatosPorExperienciaAsync(string nivelExperiencia)
    {
        FirestoreManager dbManager = new FirestoreManager();
        List<Dictionary<string, object>> items = await dbManager.ReadDataAsync("users");

        var usuariosFiltrados = new List<UsersModel>();
        foreach (var item in items)
        {
            string nombre = item["alias"].ToString();
            string puntaje = item["exp"].ToString();
            string pic = "";
            string nivelActual = DeterminarNivelExperiencia(int.Parse(puntaje));

            if (item.ContainsKey("pic") && !string.IsNullOrEmpty(item["pic"].ToString()))
            {
                pic = item["pic"].ToString();
            }
            else
            {
                pic = defaultImage;
            }
            if (nivelActual.Equals(nivelExperiencia, StringComparison.OrdinalIgnoreCase))
            {
                usuariosFiltrados.Add(new UsersModel(nombre, int.Parse(puntaje), pic));
            }
        }
        MostrarUsuarios(usuariosFiltrados.ToArray());
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

    private void MostrarUsuarios(UsersModel[] usuariosMostrar)
    {

        // Eliminar los objetos hijos del scrollViewContent
        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        int incremento = 0;

        usuariosMostrar = usuariosMostrar.OrderByDescending(x => x.Puntaje).ToArray();
        for (int i = 0; i < usuariosMostrar.Length; i++)
        {
            GameObject objTmp = Instantiate(prefabRegistro, transform);
            objTmp.GetComponentsInChildren<TextMeshProUGUI>()[0].text = (i + 1).ToString();
            objTmp.GetComponentsInChildren<TextMeshProUGUI>()[1].text = usuariosMostrar[i].Name;
            objTmp.GetComponentsInChildren<TextMeshProUGUI>()[2].text = usuariosMostrar[i].Puntaje.ToString();

            byte[] imageBytes = System.Convert.FromBase64String(usuariosMostrar[i].Base64Image);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            objTmp.GetComponentsInChildren<RawImage>()[0].texture = texture;

            objTmp.GetComponent<RectTransform>().position = new Vector3(objTmp.GetComponent<RectTransform>().position.x, objTmp.GetComponent<RectTransform>().position.y + incremento, objTmp.GetComponent<RectTransform>().position.z);
            incremento -= 150;

            objTmp.transform.SetParent(scrollViewContent.transform);
        }
    }

    private void obtenerExperienciaUsuario()
    {
        experiencia = DeterminarNivelExperiencia(Globals.exp);
        //Debug.Log("EXPERIENCIA="+experiencia);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
