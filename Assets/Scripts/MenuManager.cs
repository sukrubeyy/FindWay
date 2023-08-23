using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Menus")] [SerializeField] private GameObject LevelsMenu;
    [SerializeField] private GameObject HomeMenu;
    [SerializeField] private GameObject AccountMenu;
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private GameObject UpperAndBottomMenu;
    [SerializeField] private GameObject DailyBonusMenu;
    public GameObject SceneLoadingMenu;
    public Slider loadingBar;

    private GameObject activeMenu;
    private GameObject previousActiveMenu;

    public TMP_Text iconText;

    [Header("Menu Buttons")] [SerializeField]
    private Button LevelsMenuButton;

    [SerializeField] private Button claimbCoinButton;
    [SerializeField] private Button rewardedCoinButton;
    [SerializeField] private Button HomeMenuuButton;
    [SerializeField] private Button AccountMenuButton;
    [SerializeField] private Button SettingsMenuButton;

    [Header("Player Settings")] [SerializeField]
    private Slider volumeSlider;

    [SerializeField] private Toggle vibrateToggle;
    [SerializeField] private Toggle admobToggle;

    [Header("Account Menu Items")] [SerializeField]
    private Button SaveButton;

    [SerializeField] private Button ResetAllDataButton;
    [SerializeField] private TMP_InputField NickNameInputField;


    [Header("Prefabs")] [SerializeField] private GameObject levelButtonPrefab;

    private void Start()
    {
        activeMenu = HomeMenu;
        activeMenu.SetActive(true);

        ListLevel();

        SaveButton.onClick.AddListener(() => { FirebaseManager.Instance.Save(); });

        volumeSlider.onValueChanged.AddListener((sliderValue) => { DataManager.Instance.userInformation.SetVolume(sliderValue); });

        vibrateToggle.onValueChanged.AddListener(toggleValue => { DataManager.Instance.userInformation.SetVibrate(toggleValue); });

        admobToggle.onValueChanged.AddListener(toggleValue => { DataManager.Instance.userInformation.SetAdmob(toggleValue); });

        ResetAllDataButton.onClick.AddListener(() =>
        {
            DataManager.Instance.ResetPlayerData();
            iconText.text = DataManager.Instance.userInformation.GetCoinCount.ToString();
            ListLevel();
        });

        claimbCoinButton.onClick.AddListener(() =>
        {
            GetDailyBonus();
            ChangeMenu(HomeMenu);
        });
        rewardedCoinButton.onClick.AddListener(() =>
        {
            GetDailyBonus(1000);
            ChangeMenu(HomeMenu);
        });

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


    private void ListLevel()
    {
        for (int i = 0; i < LevelsMenu.transform.childCount; i++)
            Destroy(LevelsMenu.transform.GetChild(i).gameObject);

        foreach (var sceneIndex in GetAllLevelSceneIndex())
            Instantiate(levelButtonPrefab, LevelsMenu.transform).GetComponent<LevelButton>().Initialize(sceneIndex);
    }

    private void Update()
    {
        if (DataManager.Instance.userInformation.GetDailyBonusVariable)
            ChangeMenu(DailyBonusMenu);
    }

    public void ChangeMenu(GameObject menu)
    {
        previousActiveMenu = activeMenu;
        activeMenu.SetActive(false);
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    public void IntializeElementsOfUI()
    {
        volumeSlider.value = DataManager.Instance.userInformation.GetVolume();
        iconText.text = DataManager.Instance.userInformation.GetCoinCount.ToString();
        admobToggle.isOn = DataManager.Instance.userInformation.IsAdmob();
        vibrateToggle.isOn = DataManager.Instance.userInformation.IsVibrate();
        ListLevel();
    }

    public void GetDailyBonus(int coinValue = 500)
    {
        DataManager.Instance.userInformation.SetDailyBonusPossible(false);
        int value = int.Parse(iconText.text);

        value += coinValue;
        LeanTween.scale(iconText.gameObject, new Vector3(0.5f, 0.5f, 1f), 0.5f)
            .setOnComplete(() =>
            {
                iconText.text = value.ToString();
                LeanTween.scale(iconText.gameObject, new Vector3(1f, 1f, 1f), 0.5f);
            });

        DataManager.Instance.SetCoinCount(value);
        FirebaseManager.Instance.Save();
    }

    private List<int> GetAllLevelSceneIndex()
    {
        List<int> sceneList = new List<int>();
        //TODO: Burayı Android App İçin değiştirmeyi unutma - Application.persistentDataPath yapılacak
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