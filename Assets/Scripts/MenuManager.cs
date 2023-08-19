using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Menus")] [SerializeField] private GameObject LevelsMenu;
    [SerializeField] private GameObject HomeMenu;
    [SerializeField] private GameObject AccountMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject UpperAndBottomMenu;
    public GameObject SceneLoadingMenu;
    public Slider loadingBar;

    private GameObject activeMenu;
    private GameObject previousActiveMenu;


    [Header("Menu Buttons")] [SerializeField]
    private Button LevelsMenuButton;

    [SerializeField] private Button HomeMenuuButton;
    [SerializeField] private Button AccountMenuButton;
    [SerializeField] private Button SettingsMenuButton;

    [Header("Account Menu Items")] [SerializeField]
    private Button SaveButton;

    [SerializeField] private Button ResetAllDataButton;
    [SerializeField] private TMP_InputField NickNameInputField;


    [Header("Prefabs")] [SerializeField] private GameObject levelButtonPrefab;

    private void Start()
    {
        activeMenu = HomeMenu;
        activeMenu.SetActive(true);

        foreach (var sceneIndex in GetAllLevelSceneIndex())
            Instantiate(levelButtonPrefab, LevelsMenu.transform).GetComponent<LevelButton>().Initialize(sceneIndex);

        SaveButton.onClick.AddListener(() => { });

        ResetAllDataButton.onClick.AddListener(() => { });

        LevelsMenuButton.onClick.AddListener(() => { ChangeMenu(LevelsMenu); });

        HomeMenuuButton.onClick.AddListener(() => { ChangeMenu(HomeMenu); });

        AccountMenuButton.onClick.AddListener(() => { ChangeMenu(AccountMenu); });

        SettingsMenuButton.onClick.AddListener(() =>
        {
            if (activeMenu != SettingsMenu)
                ChangeMenu(SettingsMenu);
            else
                ChangeMenu(previousActiveMenu);
        });
    }

    public void ChangeMenu(GameObject menu)
    {
        previousActiveMenu = activeMenu;
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    private List<int> GetAllLevelSceneIndex()
    {
        List<int> sceneList = new List<int>();
        var folderPath = Application.dataPath + "/Levels/";
        string[] scenePaths = Directory.GetFiles(folderPath, "*.unity");
        foreach (string scenePath in scenePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            sceneList.Add(int.Parse(sceneName));
        }

        return sceneList;
    }
}