using ExtremeSnowboarding.Script.EstadosPlayer;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;

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
                Player.Player player = other.GetComponent<Player.Player>();
                string state = other.gameObject.GetComponent<Player.Player>().SharedValues.actualState;
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
