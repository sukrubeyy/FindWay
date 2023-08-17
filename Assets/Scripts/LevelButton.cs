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
                    SceneManager.LoadScene(SceneIndex);
                }
        });
    }
}
