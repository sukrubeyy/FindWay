using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : Singleton<FirebaseManager>
{
    private DatabaseReference reference;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                GetUserInformationFromFirebaseDatabase();
            }
        });
    }

    public void GetUserInformationFromFirebaseDatabase()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users").Child(DataManager.Instance.userInformation.GetUserId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string json = snapshot.GetRawJsonValue();
                    var cloudData =  JsonUtility.FromJson<UserInformation>(json);
                    if (cloudData is not null)
                        DataManager.Instance.userInformation = cloudData;
                }
            });
    }

    public void DeleteData()
    {
        reference.Child("Users").Child(DataManager.Instance.userInformation.GetUserId).RemoveValueAsync().ContinueWith(task =>
        {
            if(task.IsCompletedSuccessfully)
                Debug.Log("Data Delete");
        });
    }
    public void Save()
    {
        reference.Child("Users").Child(DataManager.Instance.userInformation.GetUserId).SetRawJsonValueAsync(JsonUtility.ToJson(DataManager.Instance.userInformation));
        Debug.Log("SAVED");
    }
}