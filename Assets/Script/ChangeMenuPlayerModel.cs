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
            if(playerModel.gameObject.tag == "Player" && onTriggerEnter != null)
                onTriggerEnter.Invoke(playerModel);

        }
    }
}
