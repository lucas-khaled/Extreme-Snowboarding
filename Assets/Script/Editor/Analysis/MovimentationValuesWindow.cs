using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExtremeSnowboarding.Script.Attributes;
using ExtremeSnowboarding.Script.EventSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtremeSnowboarding.Script.Editor.Analysis
{
    public class MovimentationValuesWindow : EditorWindow
    {
        private List<MemberInfo> movimentationMembers = new List<MemberInfo>();
        private List<Player.Player> instatiatedPlayers = new List<Player.Player>();
        private List<bool> playerFoldouts = new List<bool>();
    
    
        [MenuItem("Tools/Analyze/Player Movimentation Showcase")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            MovimentationValuesWindow window = (MovimentationValuesWindow)GetWindow(typeof(MovimentationValuesWindow));
            window.titleContent = new GUIContent("Player Movimentation");
            window.Show();
        }

        private void OnEnable()
        {
            GetMovimentationMembers();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("LoadedScene");
            if (scene.name == "Fase1Teste" || scene.name == "Fase2")
            {
                PlayerGeneralEvents.onPlayerInstantiate += PlayerInstatiated;
            }
            else
            {
                ClearPlayers();
                PlayerGeneralEvents.onPlayerInstantiate -= PlayerInstatiated;
            }
        }
    
        private void OnGUI()
        {
            if (instatiatedPlayers.Count > 0 && Application.isPlaying)
            {
                ShowMovimentationPropertiesAndValues();
            }
            else
            {
                ShowMovimentationProperties();
            }
        }

        private void ShowMovimentationPropertiesAndValues()
        {
            if (movimentationMembers.Count > 0)
            {
                int index = 0;
                foreach (Player.Player player in instatiatedPlayers)
                {
                    playerFoldouts[index] = EditorGUILayout.Foldout(playerFoldouts[index], "Player " + (index+1));

                    if (playerFoldouts[index])
                    {
                        ShowPlayerValues(player);
                    }

                    index++;
                }
            }
        }

        private void ShowPlayerValues(Player.Player player)
        {
            foreach (MemberInfo member in movimentationMembers)
            {
                PropertyInfo property = (PropertyInfo) member;

                object obj = null;
                string value = "N/A";
            
                obj = property.GetValue(player.SharedValues);

                if (obj != null)
                    value = obj.ToString();
            
                GUILayout.Label(member.Name + ": "+value);
            }
        }

        void ClearPlayers()
        {
            instatiatedPlayers.Clear();
            playerFoldouts.Clear();
        }
    
        void PlayerInstatiated(Player.Player player)
        {
            instatiatedPlayers.Add(player);
            playerFoldouts.Add(false);
        }
    

        private void ShowMovimentationProperties()
        {
            if (movimentationMembers.Count > 0)
            {
                foreach (MemberInfo member in movimentationMembers)
                {
                    GUILayout.Label("#"+member.Name);
                }
            }
        }

        void GetMovimentationMembers()
        {
            Assembly[] assembies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assembies)
            {
                Type[] types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                    MemberInfo[] members = type.GetMembers(flags);

                    foreach (MemberInfo member in members)
                    {
                        if (member.CustomAttributes.ToArray().Length > 0)
                        {
                            MovimentationValueAttribute attribute = member.GetCustomAttribute<MovimentationValueAttribute>();
                            if (attribute != null)
                                movimentationMembers.Add(member);
                        }
                    }
                }
            }
        }
    }
}
