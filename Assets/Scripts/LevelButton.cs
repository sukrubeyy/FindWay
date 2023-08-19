using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Button button;
   
    private int SceneIndex;
    public void Initialize(int sceneIndex)
    {
        SceneIndex = sceneIndex;
        button.interactable = sceneIndex <= DataManager.Instance.UserData.GetLevelIndex;
        text.text = SceneIndex.ToString();
        button.onClick.AddListener(() =>
        {
                if (button.IsInteractable())
                {
                    MenuManager.Instance.SceneLoadingMenu.SetActive(true);
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
