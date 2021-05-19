using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class PlayerMenu : MonoBehaviour
    {
        [BoxGroup("Shader References")]
        public Shader changeColorShader;
    
        [SerializeField] [BoxGroup("Shader References")]
        private Texture2D mask01;
        [SerializeField] [BoxGroup("Shader References")]
        private Texture2D mask02;
   

        [BoxGroup("Interface")]
        [SerializeField]
        private Canvas canvas;
        [SerializeField] [BoxGroup("Interface")]
        private FlexibleColorPicker primaryColorPicker;
        [SerializeField] [BoxGroup("Interface")]
        private FlexibleColorPicker secondaryColorPicker;

        [FormerlySerializedAs("maleMesh")] [BoxGroup("Mesh")] [SerializeField]
        private Mesh[] maleMeshes;
        [FormerlySerializedAs("femaleMesh")] [SerializeField] [BoxGroup("Mesh")]
        private Mesh[] femaleMeshes;
        [FormerlySerializedAs("meshRendereres")] [FormerlySerializedAs("meshRenderer")] [SerializeField] [BoxGroup("Mesh")]
        private SkinnedMeshRenderer[] meshRenderers;
    

        [HideInInspector]
        public Color primaryColor = Color.red;
        [HideInInspector]
        public Color secondaryColor = Color.white;

        private Material myMaterial;

        public Texture2D GetTexture(int index)
        {
            return (index <= 1) ? mask01 : mask02;
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

        public void SetFemaleMesh()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = femaleMeshes[i];
            }
        }

        public void SetMaleMeshes()
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].sharedMesh = maleMeshes[i];
            }
        }

        public Mesh[] GetSelectedMeshes()
        {
            Mesh[] returnMeshes = new Mesh[meshRenderers.Length];

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                returnMeshes[i] = meshRenderers[i].sharedMesh;
            }

            return returnMeshes;
        }

        private void Awake()
        {
            myMaterial = new Material(changeColorShader);
            SetMaterials();

            myMaterial.SetTexture("_Color1Mask", mask01);
            myMaterial.SetTexture("_Color2Mask", mask02);

            canvas.worldCamera = Camera.main;

            primaryColor = Random.ColorHSV();
            secondaryColor = Random.ColorHSV();

            primaryColorPicker.color = primaryColor;
            secondaryColorPicker.color = secondaryColor;

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
