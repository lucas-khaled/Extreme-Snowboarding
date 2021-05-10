using UnityEngine;

namespace ExtremeSnowboarding.Script.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private Item[] availableItems;

        private void PickRandomItem(GameObject player)
        {
            //Talvez uma animação de randomização??

            player.GetComponent<Player.Player>().Coletavel = availableItems[Random.Range(0, availableItems.Length)];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if(other.GetComponent<Player.Player>().Coletavel == null)
                {
                    PickRandomItem(other.gameObject);
                    Destroy(gameObject);
                }
            }

        
        }
    }
}
