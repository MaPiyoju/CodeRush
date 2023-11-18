using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class logout : MonoBehaviour
{
    // Start is called before the first frame update
    public void logout_user()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("SignIn");
    }
}
