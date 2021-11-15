using ExtremeSnowboarding.Script.EstadosPlayer;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using Photon.Pun;

namespace ExtremeSnowboarding.Script.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private bool isHole = false;
        private Collider myCollider;

        [SerializeField]
        private PathCreator path;

        private void Awake()
        {
            myCollider = GetComponent<Collider>();
            myCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if(!other.GetComponent<PhotonView>().IsMine) return;
                
                Player.Player player = other.GetComponent<Player.Player>();
                string state = player.SharedValues.actualState;
                if (isHole)
                {
                    if (state != "Dead" && state != "Flying")
                    {
                        Debug.Log("Colisão buraco");
                        //player.ChangeState(new Dead());
                        player.ChangeState(new Flying());
                        player.gameObject.GetComponent<PathFollower>().pathCreator = path;
                    }
                }

                else if (!player.SharedValues.Etherium)
                    player.ChangeState(new Fallen(true));
            }
        }
    }
}
