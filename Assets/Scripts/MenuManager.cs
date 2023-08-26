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
    [SerializeField] private GameObject DailyBonusMenu;
    private GameObject activeMenu;
    private GameObject previousActiveMenu;
    public bool isDailyBonus;
    public TMP_Text iconText;

    [Header("Loading Screen")] 
    public GameObject SceneLoadingMenu;
    public Slider loadingBar;

    [Header("Menu Buttons")] [SerializeField]
    private Button LevelsMenuButton;

    [SerializeField] private Button claimbCoinButton;
    [SerializeField] private Button rewardedCoinButton;
    [SerializeField] private Button HomeMenuuButton;
    [SerializeField] private Button AccountMenuButton;
    [SerializeField] private Button SettingsMenuButton;
    [SerializeField] private Button SaveButton;
    [SerializeField] private Button ResetAllDataButton;
    [SerializeField] private Button GithubButton;
    [SerializeField] private Button PortfolyoButton;
    [SerializeField] private Button LinkedlnButton;

    [Header("Player Settings")] [SerializeField]
    private Slider volumeSlider;

    [SerializeField] private Toggle vibrateToggle;
    [SerializeField] private Toggle admobToggle;


    [Header("Prefabs")] [SerializeField] private GameObject levelButtonPrefab;

    [Header("Customization")] public GameObject CustomizationCharacter;


    public LayerMask layerMask;

    public Button eyesButton;
    public GameObject eyesColorPallete;
    public Button bodyButton;
    public GameObject bodyColorPallete;
    public Button armsButton;
    public GameObject armsColorPallete;

    private void Start()
    {
        activeMenu = HomeMenu;
        activeMenu.SetActive(true);

        ListLevel();

        eyesButton.onClick.AddListener(() => { eyesColorPallete.SetActive(!eyesColorPallete.activeSelf); });

        bodyButton.onClick.AddListener(() => { bodyColorPallete.SetActive(!bodyColorPallete.activeSelf); });

        armsButton.onClick.AddListener(() => { armsColorPallete.SetActive(!armsColorPallete.activeSelf); });


        SaveButton.onClick.AddListener(() => { FirebaseManager.Instance.Save(); });

        volumeSlider.onValueChanged.AddListener((sliderValue) => { DataManager.Instance.userInformation.SetVolume(sliderValue); });

        vibrateToggle.onValueChanged.AddListener(toggleValue =>
        {
            DataManager.Instance.userInformation.SetVibrate(toggleValue);
            FirebaseManager.Instance.Save();
        });

        admobToggle.onValueChanged.AddListener(toggleValue =>
        {
            DataManager.Instance.userInformation.SetAdmob(toggleValue);
            FirebaseManager.Instance.Save();
        });

        ResetAllDataButton.onClick.AddListener(() =>
        {
            // DataManager.Instance.ResetPlayerData();
            // iconText.text = DataManager.Instance.userInformation.GetCoinCount.ToString();
            FirebaseManager.Instance.Reset();
            ListLevel();
        });

        GithubButton.onClick.AddListener(() => { Application.OpenURL("https://github.com/sukrubeyy"); });

        PortfolyoButton.onClick.AddListener(() => { Application.OpenURL("https://sukrucay.com.tr"); });

        LinkedlnButton.onClick.AddListener(() => { Application.OpenURL("https://www.linkedin.com/in/şükrü-çay-a0a8461a3/"); });

        claimbCoinButton.onClick.AddListener(() =>
        {
            isDailyBonus = false;
            GetDailyBonus();
            ChangeMenu(HomeMenu);
        });
        rewardedCoinButton.onClick.AddListener(() =>
        {
            isDailyBonus = false;
            GetDailyBonus(1000);
            ChangeMenu(HomeMenu);
        });

        LevelsMenuButton.onClick.AddListener(() => { ChangeMenu(LevelsMenu); });

        HomeMenuuButton.onClick.AddListener(() => { ChangeMenu(HomeMenu); });

        AccountMenuButton.onClick.AddListener(() => { ChangeMenu(AccountMenu); });

        SettingsMenuButton.onClick.AddListener(() =>
        {
            if (activeMenu != SettingsMenu)
            {
                ChangeMenu(SettingsMenu);
            }
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
        if (isDailyBonus)
        {
            ChangeMenu(DailyBonusMenu);
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 150, layerMask))
            {
                if (hit.collider is not null)
                {
                    Vector3 directionToMouse = (hit.point - hit.transform.position).normalized;
            
                    float rotationSpeed = 1.0f; // Döndürme hızını ayarlayabilirsiniz
                    Quaternion targetRotation = Quaternion.LookRotation(directionToMouse) * Quaternion.Euler(0, 0, 0);
            
                    hit.transform.rotation = Quaternion.Slerp(hit.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    public void ChangeMenu(GameObject menu)
    {
        if (HomeMenu == menu)
            CustomizationCharacter.SetActive(true);
        else
            CustomizationCharacter.SetActive(false);
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
        isDailyBonus = false;
        //DataManager.Instance.userInformation.SetDailyBonusPossible(false);
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
        var folderPath = PathHelper.Path.LevelsPath;
        string[] scenePaths = Directory.GetFiles(folderPath, "*.unity");
        foreach (string scenePath in scenePaths)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            sceneList.Add(int.Parse(sceneName));
        }

        return sceneList;
    }
}