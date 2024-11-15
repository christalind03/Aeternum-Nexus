using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectEvents : MonoBehaviour
{
    private UIDocument main;
    
    private Button topR;
    private Button topL;
    private Button botR;
    private Button botL;

    private void Awake()
    {
        main = GetComponent<UIDocument>();

        topR = main.rootVisualElement.Q("TopRight") as Button;
        topL = main.rootVisualElement.Q("TopLeft") as Button;
        botR = main.rootVisualElement.Q("BottomRight") as Button;
        botL = main.rootVisualElement.Q("BottomLeft") as Button;

        topR.RegisterCallback<ClickEvent>(OnTopRight);
        topL.RegisterCallback<ClickEvent>(OnTopLeft);
        botR.RegisterCallback<ClickEvent>(OnBottomRight);
        botL.RegisterCallback<ClickEvent>(OnBottomLeft);
    }

    private void OnTopRight(ClickEvent click)
    { 
        {
            Debug.Log("Enter scene: Level1");
        }
    }
    private void OnTopLeft(ClickEvent click)
    {
        {
            Debug.Log("Currently locked(level2)");
        }
    }
    private void OnBottomRight(ClickEvent click)
    {
        {
            Debug.Log("Enter scene: Level1-Boss");
        }
    }
    private void OnBottomLeft(ClickEvent click)
    {
        {
            Debug.Log("Currently locked(level2)");
        }
    }
}
