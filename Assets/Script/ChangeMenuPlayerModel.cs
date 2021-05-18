using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ExtremeSnowboarding
{
    public class ChangeMenuPlayerModel : MonoBehaviour
    {

        [SerializeField]private UnityEvent<Collider> onTriggerEnter;

        void OnTriggerEnter(Collider playerModel)
        {
            Debug.Log("EntrouCarai");
            if(playerModel.gameObject.tag == "Player" && onTriggerEnter != null)
            {
                onTriggerEnter.Invoke(playerModel);
                Debug.Log("InvokouCrarai");
            }


        }
    }
}
