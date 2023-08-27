using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public ObjectOfPool[] objects;
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].pooledObject = new Queue<GameObject>();
            for (int j = 0; j < objects[i].count; j++)
            {
                GameObject obj = Instantiate(objects[i].objectPrefab,transform);
                obj.SetActive(false);
                objects[i].pooledObject.Enqueue(obj);
            }
        }
    }

    public GameObject GetPoolObject(PoolObjectType type)
    {
        var foundObject = objects.FirstOrDefault(x => x.type == type).pooledObject.Dequeue();
        foundObject.SetActive(true);
        return foundObject;
    }


    public void SendPool(PoolObjectType type,GameObject poolObj)
    {
        poolObj.SetActive(false);
        objects.FirstOrDefault(x => x.type == type).pooledObject.Enqueue(poolObj);
    }
}

[Serializable]
public struct ObjectOfPool
{
    public int count;
    public Queue<GameObject> pooledObject;
    public PoolObjectType type;
    public GameObject objectPrefab;
}

public enum PoolObjectType
{
    Stone
}