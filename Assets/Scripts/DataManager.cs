using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public UserData UserData;
    
    public void Reset()
    {
        UserData = new UserData();
        UserData.playerSettings = new PlayerSettings();
        //TODO:Save Operation
    }
}
