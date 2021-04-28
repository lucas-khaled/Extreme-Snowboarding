using System;
using System.Collections.Generic;
using System.Linq;
using Script.EventSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script.Editor.Analysis
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
            if (scene.name == "Fase1")
            {
                EffectGeneralEvents.onEffectStarted += OnEffectStarted;
                EffectGeneralEvents.onEffectEnded += OnEffectEnded;
            }
            else
            {
                EffectGeneralEvents.onEffectStarted -= OnEffectStarted;
                EffectGeneralEvents.onEffectEnded -= OnEffectEnded;
            }
        }

        void OnEffectStarted(Effect effect, Player player)
        {
            activeEffects.Add(new EffectPlayerStructure(effect, player));
        }

        void OnEffectEnded(Effect effect, Player player)
        {
            activeEffects.Remove(activeEffects.Find((x) => x.effect.Name == effect.Name && x.effect.FloatValue == effect.FloatValue));
        }
    }

    struct EffectPlayerStructure
    {
        public Effect effect;
        public Player player;

        public EffectPlayerStructure(Effect effect, Player player)
        {
            this.effect = effect;
            this.player = player;
        }
    }
}