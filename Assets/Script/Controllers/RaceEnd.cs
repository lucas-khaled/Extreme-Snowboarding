using ExtremeSnowboarding.Script.EstadosPlayer;
using ExtremeSnowboarding.Script.EventSystem;
using UnityEngine;
using System.Collections;
using Photon.Pun;

namespace ExtremeSnowboarding.Script.Controllers
{
    [RequireComponent(typeof(PhotonView))]
    public class RaceEnd : MonoBehaviour
    {
        private int quantityOfActivePlayer;
        private Player.Player firstPlayer;
        private PhotonView _photonView;
        
        private void Awake()
        {
            PlayerGeneralEvents.onPlayerDeath += OnPlayerDeath;
            quantityOfActivePlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            _photonView = GetComponent<PhotonView>();
        }

        private void OnPlayerDeath(Player.Player player)
        {
            _photonView.RPC("RPC_ReducePlayerQuantity", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
        }

        [PunRPC]
        private void RPC_ReducePlayerQuantity(int ID)
        {
            quantityOfActivePlayer--;

            Player.Player player = PhotonView.Find(ID).GetComponent<Player.Player>();
            CorridaController.instance.PlayerFinishedRace(player);
                
            if (quantityOfActivePlayer <= 0)
            {
                StartCoroutine(EndRace());
            }
        }

        private IEnumerator EndRace()
        {
            yield return new WaitForSeconds(10);
            CorridaController.instance.ReturnToMainMenu();
            PlayerGeneralEvents.onPlayerDeath -= OnPlayerDeath;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                PhotonView view = other.GetComponent<PhotonView>();
                if(!view.IsMine) return;
                
                other.gameObject.GetComponent<Player.Player>().ChangeState(new RaceEndState());
                _photonView.RPC("RPC_ReducePlayerQuantity", RpcTarget.All, view.ViewID);
            }
        }
    }
}
