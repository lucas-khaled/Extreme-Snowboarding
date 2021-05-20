using System;
using NaughtyAttributes;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Items
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]  private float speed = 5f;
        [SerializeField]  private float height = 1f;
        [SerializeField] private MovementType movementType;
        [SerializeField] [ShowAssetPreview()] private ParticleSystem explosionParticle;

        public FuckFriend fuckfriend { private get; set; }
        public Player.Player caster { private get; set; }
        
        private bool alreadyCasted = false;
        private float relativeSpeed;

        private void Update()
        {
            Movement();
        }

        public Projectile (FuckFriend fuckFriendParametro)
        {
            this.fuckfriend = fuckFriendParametro;
        }

        private void Movement()
        {
            switch (movementType)
            {
                case MovementType.STRAIGHT:
                    MoveStraight(1);
                    break;
                case MovementType.FOWARD:
                    MoveFollowTrack(1);
                    break;
                case MovementType.STOPPED:
                    MoveStopped();
                    break;
                case MovementType.BACK:
                    MoveFollowTrack(-1);
                    break;
                case MovementType.STRAIGHT_BACK:
                    MoveStraight(-1);
                    break;
            }
        }

        #region Movement Types
        void MoveStraight(int velocityFactor)
        {
            transform.position += Vector3.right * speed * Time.deltaTime * 10 * velocityFactor;
        }
        void MoveFollowTrack(int distanceFactor)
        {
            Vector3 newPoint = new Vector3(this.transform.position.x + distanceFactor,
                this.transform.position.y,
                this.transform.position.z);

            RaycastHit hit;

            if (!alreadyCasted)
            {
                relativeSpeed = speed + caster.SharedValues.RealVelocity * 2;
                alreadyCasted = true;
            }

            if (Physics.Raycast(newPoint, transform.TransformDirection(Vector3.up), out hit, 100f, LayerMask.GetMask("Track")))
                newPoint = hit.point;
            else if (Physics.Raycast(newPoint, transform.TransformDirection(-Vector3.up), out hit, 100f, LayerMask.GetMask("Track")))
                newPoint = hit.point;

            newPoint = new Vector3(newPoint.x, newPoint.y + height, newPoint.z);

            this.transform.position = Vector3.MoveTowards(this.transform.position,              // Posicao inicial 
                newPoint,                             // Posicao destino
                relativeSpeed * Time.deltaTime);      // Velocidade movimento

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        }
        void MoveStopped()
        {
            if (!alreadyCasted)
            {
                RaycastHit hit;

                Vector3 newPoint = new Vector3(this.transform.position.x,
                    this.transform.position.y + 5f,
                    this.transform.position.z);

                if (Physics.Raycast(newPoint, transform.TransformDirection(Vector3.down), out hit, 500f, LayerMask.GetMask("Track")))
                {
                    transform.position = hit.point+ Vector3.up * height;

                    transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                }

                alreadyCasted = true;
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.gameObject.CompareTag("Player"))
            {
                Player.Player playerHitted = other.GetComponent<Player.Player>();
                if(playerHitted != caster)
                {
                    fuckfriend.StartEffects(playerHitted);
                    Destroy(gameObject);
                }
            }
            else
            {
                if(explosionParticle != null)
                    explosionParticle.Play();
            
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down*height);
        }
    }
}
