using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
    public Shader changeColorShader;
    
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;

    [Header("Interface")]
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private FlexibleColorPicker primaryColorPicker;
    [SerializeField]
    private FlexibleColorPicker secondaryColorPicker;

    [Header("Meshs")]
    [SerializeField]
    private Mesh maleMesh;
    [SerializeField]
    private Mesh femaleMesh;

    [HideInInspector]
    public Color primaryColor = Color.red;
    [HideInInspector]
    public Color secondaryColor = Color.white;

    private Material myMaterial;

    private void Awake()
    {
        myMaterial = new Material(changeColorShader);
        meshRenderer.material = myMaterial;

        canvas.worldCamera = Camera.main;

        primaryColor = Random.ColorHSV();
        secondaryColor = Random.ColorHSV();

        primaryColorPicker.color = primaryColor;
        secondaryColorPicker.color = secondaryColor;

        ChangePrimaryColor(primaryColor);
        ChangeSecondaryColor(secondaryColor);
    }

    public void ChangePrimaryColor(string colorString)
    {
        
        Color color;
        if (ColorUtility.TryParseHtmlString(colorString, out color))
            ChangePrimaryColor(color);
    }

    public void ChangeSecundaryColor(string colorString)
    {
        
        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        ChangeSecondaryColor(color);
    }

    public void ChangePrimaryColor(Color color)
    {
        myMaterial.SetColor("_PrimaryColor", color);
        primaryColor = color;
    }

    public void ChangeSecondaryColor(Color color)
    {
        myMaterial.SetColor("_SecondaryColor", color);
        secondaryColor = color;
    }

    public void OnPrimaryColorPickerChange()
    {
        ChangePrimaryColor(primaryColorPicker.color);
    }

    public void OnSecondaryColorPickerChange()
    {
        ChangeSecondaryColor(secondaryColorPicker.color);
    }
}
