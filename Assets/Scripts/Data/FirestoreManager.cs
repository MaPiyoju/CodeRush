using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

public class FirestoreManager
{

    readonly FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    public async Task<List<Dictionary<string, object>>> ReadDataAsync(string collection)
    {
        try
        {
            var items = new List<Dictionary<string, object>>();
            CollectionReference dataRef = db.Collection(collection);
            await dataRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    var entry = new List<string>();

                    Dictionary<string, object> documentDictionary = document.ToDictionary();
                    items.Add(documentDictionary);
                }
            });
            return items;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return new List<Dictionary<string, object>>();
        }
    }
}
