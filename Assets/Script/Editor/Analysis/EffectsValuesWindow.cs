using System.Collections.Generic;
using ExtremeSnowboarding.Script.Items.Effects;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtremeSnowboarding.Script.Editor.Analysis
{
    public class EffectsValuesWindow : EditorWindow
    {
        private List<EffectPlayerStructure> activeEffects = new List<EffectPlayerStructure>();
        
        [MenuItem("Tools/Analyze/Effect Showcase")]
        private static void ShowWindow()
        {
            var window = GetWindow<EffectsValuesWindow>();
            window.titleContent = new GUIContent("Effects");
            window.Show();
        }

        private void OnGUI()
        {
            if (activeEffects.Count > 0)
            {
                ShowEffectsStructure();
            }
        }

        private void ShowEffectsStructure()
        {
            foreach (EffectPlayerStructure structure in activeEffects)
            {
                if (structure.player.name == "Player1")
                {
                    
                }
            }
        }
        

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
           
        }
        
    }

    struct EffectPlayerStructure
    {
        public Effect effect;
        public Player.Player player;

        public EffectPlayerStructure(Effect effect, Player.Player player)
        {
            this.effect = effect;
            this.player = player;
        }
    }
}