using UnityEngine;
using UnityEngine.Events;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class OnMenuPlayerTrigger : MonoBehaviour
    {

        [SerializeField]private UnityEvent<Collider> onTriggerEnter;

        void OnTriggerEnter(Collider playerModel)
        {
            if(playerModel.gameObject.tag == "Player" && onTriggerEnter != null)
                onTriggerEnter.Invoke(playerModel);

        }
    }
}
