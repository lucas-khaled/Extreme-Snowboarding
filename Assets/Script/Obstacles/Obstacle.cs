using ExtremeSnowboarding.Script.EstadosPlayer;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Obstacles
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private bool isHole = false;
        private Collider myCollider;

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
                if (isHole && other.gameObject.GetComponent<Player.Player>().SharedValues.actualState != "Dead")
                    player.ChangeState(new Dead());
                //player.ChangeState(new Flying());

                else if (!player.SharedValues.Etherium)
                    player.ChangeState(new Fallen());
            }
            else if (other.gameObject.CompareTag("Projectile"))
                Destroy(other.gameObject);
        }
    }
}
