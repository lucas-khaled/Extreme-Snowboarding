using UnityEngine;

namespace ExtremeSnowboarding.Script.Items
{
    [RequireComponent(typeof(Collider))]
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private Item[] availableItems;

        private void PickRandomItem(Player.Player player)
        {
            //Talvez uma animação de randomização??

            Item item = availableItems[Random.Range(0, availableItems.Length)];

            player.GetItem(item);

            if (EventSystem.PlayerGeneralEvents.onFuckFriendChange != null)
            {
                EventSystem.PlayerGeneralEvents.onFuckFriendChange.Invoke(player, item.GetSprite());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if(other.GetComponent<Player.Player>().Coletavel == null)
                {
                    PickRandomItem(other.gameObject.GetComponent<Player.Player>());
                    Destroy(gameObject);
                }
            }
        }
    }
}
