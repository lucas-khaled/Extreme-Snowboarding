using System;
using System.Linq;
using ExtremeSnowboarding.Multiplayer;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class PlayerMenu : MonoBehaviour
    {
        [BoxGroup("Settings")] 
        [SerializeField]
        private MultiplayerInstantiationSettings instantiationSettings;

        [BoxGroup("Aniamtor references")]
        public Animator animatorRenato;
        public AnimatorOverrideController overrider;
        [HideInInspector] public AnimatorOverrider controllerToOverride;


        [BoxGroup("Interface")]
        [SerializeField]
        private Canvas canvas;
        [SerializeField] [BoxGroup("Interface")]
        private FlexibleColorPicker primaryColorPicker;
        [SerializeField] [BoxGroup("Interface")]
        private FlexibleColorPicker secondaryColorPicker;

        [FormerlySerializedAs("maleMesh")] [BoxGroup("Mesh")] [SerializeField]
        private PlayerMesh[] maleMeshes;
        [FormerlySerializedAs("femaleMesh")] [SerializeField] [BoxGroup("Mesh")]
        private PlayerMesh[] femaleMeshes;
        [FormerlySerializedAs("meshRendereres")] [FormerlySerializedAs("meshRenderer")] [SerializeField] [BoxGroup("Mesh")]
        private SkinnedMeshRenderer[] meshRenderers;
    

        [HideInInspector]
        public Color primaryColor = Color.red;
        [HideInInspector]
        public Color secondaryColor = Color.white;

        private Material myMaterial;
        private PlayerMesh[] selectedMeshes;

        public void ChangePrimaryColor(string colorString, bool setOnPicker = true)
        {
            Color color;
            if (!colorString.Contains("#"))
                colorString = "#" + colorString;
            
            if (ColorUtility.TryParseHtmlString(colorString, out color))
            {
                ChangePrimaryColor(color);
                if(setOnPicker)
                    primaryColorPicker.color = color;
            }
        }

        public void ChangeSecondaryColor(string colorString, bool setOnPicker = true)
        {
            Color color;
            if (!colorString.Contains("#"))
                colorString = "#" + colorString;
            
            if (ColorUtility.TryParseHtmlString(colorString, out color))
            {
                ChangeSecondaryColor(color);
                if(setOnPicker)
                    secondaryColorPicker.color = color;
            }
        }

        public void ChangePrimaryColor(Color color, bool saveOnPrefs = true)
        {
            Debug.Log("Primary color set: "+ColorUtility.ToHtmlStringRGB(color));
            myMaterial.SetColor("_PrimaryColor", color);
            primaryColor = color;
            if (saveOnPrefs)
            {
                Debug.Log("Saving Primary Color: " + ColorUtility.ToHtmlStringRGB(color));
                PlayerPrefs.SetString("PrimaryColor", ColorUtility.ToHtmlStringRGB(color));
            }
        }

        public void ChangeSecondaryColor(Color color, bool saveOnPrefs = true)
        {
            Debug.Log("Secondary color set: "+ColorUtility.ToHtmlStringRGB(color));
            myMaterial.SetColor("_SecondaryColor", color);
            secondaryColor = color;
            if (saveOnPrefs)
            {
                Debug.Log("Saving Secondary Color: " + ColorUtility.ToHtmlStringRGB(color));
                PlayerPrefs.SetString("SecondaryColor", ColorUtility.ToHtmlStringRGB(color));
            }
        }

        public void OnPrimaryColorPickerChange()
        {
            ChangePrimaryColor(primaryColorPicker.color);
        }

        public void OnSecondaryColorPickerChange()
        {
            ChangeSecondaryColor(secondaryColorPicker.color);
        }

        public void SetFemaleMeshes()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = femaleMeshes[i].mesh;
            }

            selectedMeshes = femaleMeshes;
            PlayerPrefs.SetString("Mesh", "Female");
        }

        public void SetMaleMeshes()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = maleMeshes[i].mesh;
            }

            selectedMeshes = maleMeshes;
            PlayerPrefs.SetString("Mesh", "Male");
        }

        public string[] GetSelectedMeshesNames()
        {
            return (from mesh in selectedMeshes select mesh.meshName).ToArray();
        }

        public string GetOverriderName(AnimatorOverrideController animatorOverrideController)
        {
            return instantiationSettings.GetNameByAnimator(animatorOverrideController);
        }

        public void ChangeOverrider(AnimatorOverrideController overriderRef)
        {
            overrider = overriderRef;
        }

        public void RandomizePrimaryColor()
        {
            primaryColor = Random.ColorHSV();
            primaryColorPicker.color = primaryColor;
            ChangePrimaryColor(primaryColor, false);
        }
        
        public void RandomizeSecondaryColor()
        {
            secondaryColor = Random.ColorHSV();
            secondaryColorPicker.color = secondaryColor;
            ChangeSecondaryColor(secondaryColor, false);
        }

        private void Awake()
        {
            myMaterial = new Material(instantiationSettings.playerShader);
            SetMaterials();

            myMaterial.SetTexture("_Color1Mask", instantiationSettings.playerMask01);
            myMaterial.SetTexture("_Color2Mask", instantiationSettings.playerMask02);

            canvas.worldCamera = Camera.main;
            selectedMeshes = maleMeshes;
        }

        private void SetMaterials()
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.material = myMaterial;
            }
        }
    }
}
