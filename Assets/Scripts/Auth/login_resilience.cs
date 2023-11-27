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

    private async void Resilience()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            Debug.Log("Current is: "+auth.CurrentUser.UserId);
            FirestoreManager dbManager = new FirestoreManager();
            List<Dictionary<string, object>> items = await dbManager.ReadDataByIdAsync("users", auth.CurrentUser.UserId);
            foreach (var item in items)
            {
                Globals.lives = int.Parse(item["lives"].ToString());
                Globals.exp = int.Parse(item["exp"].ToString());
                //LifeHandler.UpdateLife();
            }

            SceneManager.LoadScene("Home");
        }
        else {
            Debug.Log("NO Current is: ");
        }
    }
}
