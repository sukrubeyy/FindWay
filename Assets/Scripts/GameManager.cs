using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
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

    public GameObject MainMenu;
    public Button mainMenuQuit;
    public Button mainMenuLobbyButton;
    public Button mainMenuButton;

    private void Start()
    {
        mainMenuQuit.onClick.AddListener(() => { Application.Quit(); });

        mainMenuLobbyButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); });

        mainMenuButton.onClick.AddListener(() =>
        {
            MainMenu.SetActive(!MainMenu.activeSelf);
            var cacheState = MainMenu.activeSelf ? State.GameMainMenuState : State.Playmode;
            StateContext.Instance.Transition(cacheState);
        });


        WinRetryButton.onClick.AddListener(() => { LoadScene(GetSceneIndex()); });
        WinLobbyButton.onClick.AddListener(() =>
        {
            //0 == MainMenuScene
            LoadScene(0);
        });
        WinQuitButton.onClick.AddListener(() => { Application.Quit(); });
        WinNextLevelyButton.interactable = false;
        WinNextLevelyButton.onClick.AddListener(() => { LoadScene(GetSceneIndex() + 1); });


        LoseRetryButton.onClick.AddListener(() => { LoadScene(GetSceneIndex()); });
        LoseLobbyButton.onClick.AddListener(() =>
        {
            //0 == MainMenuScene
            LoadScene(0);
        });
        LoseQuitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void Update()
    {
        //For win state and lose state
        if (StateContext.Instance.GetCurrentState is  State.WinState || StateContext.Instance.GetCurrentState is State.LoseState)
        {
            mainMenuButton.gameObject.SetActive(false);
            MainMenu.SetActive(false);
        }
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