using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get{
            if(_instance is null)
            _instance = FindObjectOfType<T>();
            else if(_instance is not null)
                Destroy(FindObjectOfType<T>());

            
            return _instance;
        }
    }
}
