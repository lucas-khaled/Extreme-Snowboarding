using System;
using System.Linq;
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

        public void SetFemaleMesh()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = femaleMeshes[i].mesh;
            }

            selectedMeshes = femaleMeshes;
        }

        public void SetMaleMeshes()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = maleMeshes[i].mesh;
            }

            selectedMeshes = maleMeshes;
        }

        public string[] GetSelectedMeshesNames()
        {
            return (from mesh in selectedMeshes select mesh.meshName).ToArray();
        }

        private void Awake()
        {
            myMaterial = new Material(instantiationSettings.playerShader);
            SetMaterials();

            myMaterial.SetTexture("_Color1Mask", instantiationSettings.playerMask01);
            myMaterial.SetTexture("_Color2Mask", instantiationSettings.playerMask02);

            canvas.worldCamera = Camera.main;

            primaryColor = Random.ColorHSV();
            secondaryColor = Random.ColorHSV();

            primaryColorPicker.color = primaryColor;
            secondaryColorPicker.color = secondaryColor;

            selectedMeshes = maleMeshes;

            ChangePrimaryColor(primaryColor);
            ChangeSecondaryColor(secondaryColor);
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
