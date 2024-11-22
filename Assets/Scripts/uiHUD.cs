using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class uiHUd : MonoBehaviour
{
    private VisualElement healthBarFill;
    private const string FillElementName = "health-bar-fill";

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        healthBarFill = root.Q<VisualElement>(FillElementName);
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        // Ensure the percentage is clamped between 0 and 1
        healthPercentage = Mathf.Clamp01(healthPercentage);
        // Update the fill bar width
        healthBarFill.style.width = new Length(healthPercentage * 100, LengthUnit.Percent);
    }
}
