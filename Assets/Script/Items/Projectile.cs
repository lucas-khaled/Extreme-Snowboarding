using System;
using NaughtyAttributes;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Items
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float height = 1f;
        [SerializeField] private float fuel = 1000f;
        [SerializeField] private MovementType movementType;
        [SerializeField] [ShowAssetPreview()] private GameObject explosionParticle;

        public FuckFriend fuckfriend { private get; set; }
        public Player.Player caster { private get; set; }
        
        private bool alreadyCasted = false;
        private float relativeSpeed;
        private bool lockGroundColision = true;
        private GameObject target;
        
        private void Start()
        {
            Invoke("ChangeLockGroundColision", 1f);
        }

        private void Update()
        {
            Movement();
        }

        public Projectile (FuckFriend fuckFriendParametro, GameObject target = null)
        {
            this.fuckfriend = fuckFriendParametro;
            this.target = target;
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
                case MovementType.TRACK_TARGET:
                    MoveTrackingTarget(target);
                    break;
            }
        }

        #region Movement Types
        void MoveStraight(int velocityFactor)
        {
            transform.position += speed * Time.deltaTime * 10 * velocityFactor * Vector3.right;
        }
        void MoveFollowTrack(int distanceFactor)
        {
            Vector3 newPoint = new Vector3(this.transform.position.x + distanceFactor,
                this.transform.position.y,
                this.transform.position.z);

            RaycastHit hit;

            if (!alreadyCasted)
            {
                relativeSpeed = speed + caster.GetComponent<Rigidbody>().velocity.x;
                alreadyCasted = true;
            }

            if (Physics.Raycast(newPoint, transform.TransformDirection(Vector3.up), out hit, 100f, LayerMask.GetMask("Track")))
                newPoint = hit.point;
            else if (Physics.Raycast(newPoint, transform.TransformDirection(-Vector3.up), out hit, 100f, LayerMask.GetMask("Track")))
                newPoint = hit.point;

            newPoint = new Vector3(newPoint.x, newPoint.y + height, newPoint.z);

            this.transform.position = Vector3.MoveTowards(this.transform.position,              // Posicao inicial 
                newPoint,                                                                       // Posicao destino
                relativeSpeed * Time.deltaTime);                                                // Velocidade movimento

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

        void MoveTrackingTarget(GameObject target)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            float velocity = 1500;
            Vector3 vectorDest = Vector3.right;

            if (fuel > 0)
            {
                transform.LookAt(target.transform);
                vectorDest = (target.transform.position - transform.position).normalized;
                rb.velocity = vectorDest * Time.deltaTime * velocity;

                fuel -= Time.deltaTime;
            }
            else if (!alreadyCasted)
            {
                rb.velocity = Vector3.right * Time.deltaTime * velocity;
                transform.rotation = Quaternion.LookRotation(vectorDest);
                rb.useGravity = true;
                alreadyCasted = true;
            }

            if (fuel <= 0 && alreadyCasted)
                if (transform.rotation.x < 90)
                    transform.Rotate(vectorDest * Time.deltaTime * 15);
        }

        #endregion

        private void ChangeLockGroundColision()
        {
            lockGroundColision = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Player.Player playerHitted = other.GetComponent<Player.Player>();
                if (playerHitted != caster)
                {
                    if (explosionParticle != null)
                    {
                        GameObject explosionParticleGO = Instantiate(explosionParticle, transform.position, Quaternion.identity);
                        explosionParticleGO.GetComponent<ParticleSystem>().Play();
                    }
                    fuckfriend.StartEffects(playerHitted);
                    //string[] animation = { "Caiu-Escorregou" };
                    //playerHitted.ChangeAnimationTo(animation);
                    Destroy(gameObject);
                }
            }
            else if (!other.gameObject.CompareTag("ItemBox") && (!lockGroundColision && other.gameObject.CompareTag("Track")))
            {
                if (explosionParticle != null) 
                {
                    GameObject explosionParticleGO = Instantiate(explosionParticle, transform.position, Quaternion.identity);
                    explosionParticleGO.GetComponent<ParticleSystem>().Play();
                }
                else
                    Debug.Log("no explosion particle");

                Destroy(gameObject);
                Debug.Log(other.gameObject.name);

                Debug.Log("Lock: " + !lockGroundColision + " e "+ other.gameObject.CompareTag("Track"));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down*height);
        }
    }
}
