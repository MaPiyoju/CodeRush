using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileScript : MonoBehaviour
{
    public GameObject nameText;
    private TextMeshProUGUI nameValue;

    private void Awake()
    {
        nameValue = nameText.GetComponent<TextMeshProUGUI>();
        nameValue.text = "Juan";
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
