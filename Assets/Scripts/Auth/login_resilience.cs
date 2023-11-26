using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class login_resilience : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Resilience();
    }

    // Update is called once per frame
    void Update()
    {
        //Resilience();
    }

    private void Resilience()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            Debug.Log("Current is: "+auth.CurrentUser.UserId);
            SceneManager.LoadScene("Home");
        }
        else {
            Debug.Log("NO Current is: ");
        }
    }
}
