using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class swipe_menu : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;

    public GameObject statsLoad;

    // Start is called before the first frame update
    void Start()
    {
        // Escuchar el evento de clic para cada bot�n en los elementos del carrusel
        for (int i = 0; i < transform.childCount; i++)
        {
            Button button = transform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                int index = i; // Almacena el �ndice actual para evitar problemas de cierre
                button.onClick.AddListener(() => OnButtonClick(index));
            }
        }

        InitCarouselBasedOnExperience();       
    }

    void OnButtonClick(int buttonIndex)
    {
        string nivel = transform.GetChild(buttonIndex).name;
        //Debug.Log("Bot�n presionado: " + nivel);
        _ = statsLoad.GetComponent<LoadStatsScript>().ObtenerDatosPorExperienciaAsync(nivel);
    }

    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.5f, 1.5f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    public void InitCarouselBasedOnExperience()
    {
        string nivelExperiencia = statsLoad.GetComponent<LoadStatsScript>().DeterminarNivelExperiencia(Globals.exp); // Obtener nivel de experiencia del usuario
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Button>().name.Equals(nivelExperiencia, StringComparison.OrdinalIgnoreCase))
            {
                // Mover el carrusel al bot�n correspondiente
                scroll_pos = 1f / (transform.childCount - 1) * i;
                scrollbar.GetComponent<Scrollbar>().value = scroll_pos;

                // Resaltar el bot�n correspondiente
                transform.GetChild(i).localScale = new Vector2(1.5f, 1.5f);
            }
            else
            {
                // Reducir otros botones
                transform.GetChild(i).localScale = new Vector2(0.8f, 0.8f);
            }
        }
    }
}
