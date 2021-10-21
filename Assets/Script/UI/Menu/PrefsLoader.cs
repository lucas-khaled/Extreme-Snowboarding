using System.Collections;
using System.Collections.Generic;
using ExtremeSnowboarding.Script.UI.Menu;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace ExtremeSnowboarding.Menu
{
    public class PrefsLoader : MonoBehaviour
    {
        [SerializeField] private PlayerMenu playerMenu;
        [SerializeField] private TMP_InputField[] nickNameInputs;
        
        private void Start()
        {
            GetMeshOnPrefs();
            GetColorOnPrefs();
            GetNickOnPrefs();
        }

        private void GetMeshOnPrefs()
        {
            if (PlayerPrefs.HasKey("Mesh"))
            {
                string mesh = PlayerPrefs.GetString("Mesh");
                if (mesh == "Male")
                    playerMenu.SetMaleMeshes();
                else if(mesh == "Female")
                    playerMenu.SetFemaleMeshes();
            }
        }

        private void GetColorOnPrefs()
        {
            if (PlayerPrefs.HasKey("PrimaryColor"))
            {
                string hexaColor = PlayerPrefs.GetString("PrimaryColor");
                Debug.Log("Tem primary color: "+hexaColor);
                playerMenu.ChangePrimaryColor(hexaColor);
            }
            else
                playerMenu.RandomizePrimaryColor();

            if (PlayerPrefs.HasKey("SecondaryColor"))
            {
                string hexaColor = PlayerPrefs.GetString("SecondaryColor");
                Debug.Log("Tem secondary color: "+hexaColor);
                playerMenu.ChangeSecondaryColor(hexaColor);
            }
            else
                playerMenu.RandomizeSecondaryColor();
        }

        private void GetNickOnPrefs()
        {
            if (PlayerPrefs.HasKey("Nickname"))
            {
                string nickName = PlayerPrefs.GetString("Nickname");
                foreach (var input in nickNameInputs)
                    input.text = nickName;

                PhotonNetwork.NickName = nickName;
            }
        }
    }
}
