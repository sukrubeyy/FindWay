using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Menus")] public GameObject WinPanel;
    public GameObject LosePanel;

    [Header("Buttons")] public Button WinRetryButton;
    public Button WinLobbyButton;
    public Button WinQuitButton;
    public Button WinNextLevelyButton;

    public Button LoseRetryButton;
    public Button LoseLobbyButton;
    public Button LoseQuitButton;
        
    private StateContext context;
    private void Start()
    {
        PlayerController controller = FindObjectOfType<PlayerController>();
        context = new StateContext(controller);
        
        WinRetryButton.onClick.AddListener(() => { LoadScene(GetSceneIndex()); });
        WinLobbyButton.onClick.AddListener(() =>
        {
            //0 == MainMenuScene
            LoadScene(0);
        });
        WinQuitButton.onClick.AddListener(() => { Application.Quit(); });
        WinNextLevelyButton.onClick.AddListener(() =>
        {
            if (GetSceneIndex() + 1 < 5)
                LoadScene(GetSceneIndex() + 1);
        });


        LoseRetryButton.onClick.AddListener(() =>
        {
            LoadScene(GetSceneIndex());
        });
        LoseLobbyButton.onClick.AddListener(() =>
        {
            //0 == MainMenuScene
            LoadScene(0);
        });
        LoseQuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

   

    private int GetSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

    public void FinishSuccess()
    {
        WinPanel.SetActive(true);
    }
    public void LosePanelActive()
    {
        LosePanel.SetActive(true);
    }
}