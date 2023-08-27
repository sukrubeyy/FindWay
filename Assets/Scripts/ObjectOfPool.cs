using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjectOfPool
{
    public int count;
    public Queue<GameObject> pooledObject;
    public PoolObjectType type;
    public GameObject objectPrefab;
}