using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Button button;
    private int SceneIndex;

    private MenuManager _menuManager;
    private void Start()
    {
        _menuManager = FindObjectOfType<MenuManager>();
    }

    public void Initialize(int sceneIndex)
    {
        SceneIndex = sceneIndex;
        button.interactable = sceneIndex <= DataManager.Instance.userInformation.GetLevelIndex;
        text.text = SceneIndex.ToString();
        button.onClick.AddListener(() =>
        {
                if (button.IsInteractable())
                {
                    string json = JsonUtility.ToJson(DataManager.Instance.userInformation.GetCustomizationSettings);
                    //System.IO.File.WriteAllText(Application.dataPath+"/Customization/CustomizationSettings.json",json);
                    System.IO.File.WriteAllText(PathHelper.Path.CustomizationFolderPath+PathHelper.FileName.CustomizationJsonName,json);
                    _menuManager.SceneLoadingMenu.SetActive(true);
                    StartCoroutine(LoadLevel(SceneIndex));
                }
        });
    }

    private IEnumerator LoadLevel(int index)
    {
        AsyncOperation operation =  SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            MenuManager.Instance.loadingBar.value = progress;
            yield return null;
        }
    }
}
