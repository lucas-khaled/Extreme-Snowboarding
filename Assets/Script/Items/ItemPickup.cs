using Photon.Pun;
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
            Item item = null;
            bool available = true;
            do
            {
                available = true;

                item = availableItems[Random.Range(0, availableItems.Length)];

                if (item.name == "Relampago" && player.SharedValues.qualification == 1)
                    available = false;

            } while (!available);

            player.SetItem(item);
            player.GetMovimentationFeedbacks().getBoxFeedback?.PlayFeedbacks();

            if (EventSystem.PlayerGeneralEvents.onItemUsed != null)
            {
                EventSystem.PlayerGeneralEvents.onItemUsed.Invoke(player, item.GetSprite());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if(!other.GetComponent<PhotonView>().IsMine || other.GetComponent<Player.Player>().Coletavel != null) return;
                
                PickRandomItem(other.gameObject.GetComponent<Player.Player>());
                
                if(PhotonNetwork.LocalPlayer.IsMasterClient)
                    PhotonNetwork.Destroy(gameObject);
                else
                    GetComponent<PhotonView>().RPC("TellMasterToDestroy_RPC", RpcTarget.MasterClient);
            }
        }

        [PunRPC]
        private void TellMasterToDestroy_RPC()
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
