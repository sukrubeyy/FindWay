using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Color color;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private CustomizationButtonType type; 
   
    void Start()
    {
        image.color=color;
        button.onClick.AddListener(() =>
        {
            switch(type)
            {
                case CustomizationButtonType.Body:
                    CustomizationObject.Instance.SetBodyColor(color);
                    break;
                case CustomizationButtonType.Eyes:
                    CustomizationObject.Instance.SetEyesColor(color);
                    break;
                case CustomizationButtonType.Arms:
                    CustomizationObject.Instance.SetArmsColor(color);
                    break;
            }
        });
    }
}