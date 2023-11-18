using Firebase;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using System.Collections.Generic;

public class Registration : MonoBehaviour
{

    private FirebaseApp _app;
    public TMP_InputField inUser;
    public TMP_InputField inEmail;
    public TMP_InputField inPass1;
    public TMP_InputField inPass2;
    private ScenesManager scenes;
    public string uu;


    public State CurrentState { get; private set; }

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                _app = Firebase.FirebaseApp.DefaultInstance;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }
    
    public void Singup()
    {
        StartCoroutine(this.registro());
    }
    private IEnumerator registro()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        var user = inUser.text;
        var email = inEmail.text;
        var password1 = inPass1.text;
        var password2 = inPass2.text;
        if (password1!=password2)
        {
            ColorBlock color = inPass1.colors;
            color.normalColor = new Color(1, 0.8f, 0.8f);
            inPass2.colors = color;
        }
        if (string.IsNullOrEmpty(user)) {
            ColorBlock color = inPass1.colors;
            color.normalColor = new Color(1, 0.8f, 0.8f);
            inUser.colors = color;
        }
        else
        {
            ColorBlock color = inPass1.colors;
            color.normalColor = new Color(0.4078432f, 0.8f, 0.7490196f);
            inUser.colors = color;
        }
        if (string.IsNullOrEmpty(email))
        {
            ColorBlock color = inPass1.colors;
            color.normalColor = new Color(1, 0.8f, 0.8f);
            inEmail.colors = color;
        }
        else
        {
            ColorBlock color = inPass1.colors;
            color.normalColor = new Color(0.4078432f, 0.8f, 0.7490196f);
            inEmail.colors = color;
        }

        if(password1 == password2 && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(user)) 
        {
            ColorBlock color = inPass1.colors;
            color.normalColor = new Color(0.4078432f, 0.8f, 0.7490196f);
            inPass2.colors = color;
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password1.ToString());
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            }
            if (registerTask.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + registerTask.Exception);
            }
            else
            {
                // Firebase user has been created.
                ScenesManager changeScene = new ScenesManager();
                addData(user.ToString());
                changeScene.LoadScene(Enums.Scenes.Home);
            }

        }
        
    }

    private void addData(string user)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DateTime firstTmp = DateTimeOffset.FromUnixTimeMilliseconds((long)auth.CurrentUser.Metadata.CreationTimestamp).LocalDateTime;
        DateTime lastTmp = DateTimeOffset.FromUnixTimeMilliseconds((long)auth.CurrentUser.Metadata.LastSignInTimestamp).LocalDateTime;

        DocumentReference docRef = db.Collection("users").Document(auth.CurrentUser.UserId);
        UserModel newUser = new UserModel()
        {
            name=user,
            alias=user,
            exp=0, 
            first_login= firstTmp, 
            last_login= lastTmp,
            lives=3, 
            streak=0,
            pic="",
            history=new List<MatchModel>()
        };
        docRef.SetAsync(newUser).ContinueWithOnMainThread(task => {
            Debug.Log("Added data to the USERs document in the USERS collection.");
        });
    }
}
