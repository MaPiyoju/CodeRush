using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net.Mail;
using Firebase.Extensions;
using Firebase.Firestore;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using static UnityEditor.Progress;

public class User_welcome : MonoBehaviour
{
    public TMP_Text welcome;
    private Registration registro;
    Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    // Start is called before the first frame update
    void Start()
    {
        
        InitializeFirebase();
        registro=new Registration();
        //saludo();
    }
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        saludoAsync();
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private async Task saludoAsync()
    {
        var currentUser=FirebaseAuth.DefaultInstance.CurrentUser;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        
        DocumentReference dataRef = db.Collection("users").Document(currentUser.UserId);
        Dictionary<string, object> documentDictionary= new Dictionary<string, object>();
        await dataRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                documentDictionary = snapshot.ToDictionary();
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });

        string username = "hola";
        if (currentUser==null)
        {
            username = "Null";
            Debug.Log("user null");
        }
        
        username = currentUser.DisplayName;
        if (username == "")
        {
            username = currentUser.Email;
        }
        welcome.text = "Hola "+ documentDictionary["alias"];    
    }

}
