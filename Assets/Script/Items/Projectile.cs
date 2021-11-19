using System;
using MoreMountains.Feedbacks;
using NaughtyAttributes;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.Items
{
    [RequireComponent(typeof(Collider))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float height = 1f;
        [SerializeField] private float fuel = 20; 
        [SerializeField] private MovementType movementType;
        [FormerlySerializedAs("explosionParticle")] [SerializeField] [ShowAssetPreview()] private MMFeedbacks explosionFeedbacks;

        public FuckFriend fuckfriend { private get; set; }
        public Player.Player target { private get; set; }
        public Player.Player caster { private get; set; }
        
        private bool alreadyCasted = false;
        private float relativeSpeed;
        private bool isUp = false;
        private Vector3 pointDesUp;
        private PhotonView _photonView;

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            switch (movementType)
            {
                case MovementType.MOVE_TRACKING_TARGET:
                    pointDesUp = transform.position + new Vector3(5, 20, 0);
                    break;
                
            }
        }

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
                case MovementType.MOVE_TRACKING_TARGET:
                    if (target != null)
                        MoveTrackingTarget(target.gameObject);
                    else
                        MoveStraight(1);
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

            if (Physics.Raycast(newPoint, Vector3.up, out hit, 100f, LayerMask.GetMask("Track")))
                newPoint = hit.point;
            else if (Physics.Raycast(newPoint, -Vector3.up, out hit, 100f, LayerMask.GetMask("Track")))
                newPoint = hit.point;

            newPoint = new Vector3(newPoint.x, newPoint.y + height, newPoint.z);

            this.transform.position = Vector3.MoveTowards(this.transform.position,              // Posicao inicial 
                newPoint,                             // Posicao destino
                relativeSpeed * Time.deltaTime);      // Velocidade movimento

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        }

        void MoveTrackingTarget(GameObject target)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            float velocity = 1500;
            Vector3 vectorDest = Vector3.right;


            if (!isUp && Vector3.Distance(transform.position, pointDesUp) <= 1f)
                isUp = true;

            if (fuel > 0)
            {
                Vector3 pointDes;

                if (isUp)
                {
                    pointDes = target.transform.position;
                    vectorDest = (pointDes - transform.position).normalized;
                }
                else
                {
                    pointDes = pointDesUp;
                    vectorDest = (pointDesUp - transform.position).normalized;
                }
                transform.LookAt(pointDes);

                rb.velocity = vectorDest * (Time.deltaTime * ((velocity + caster.GetComponent<Rigidbody>().velocity.x) / 1.2f));

                fuel -= Time.deltaTime;
            }
            else if (!alreadyCasted)
            {
                rb.velocity = Vector3.right * (Time.deltaTime * velocity);
                transform.rotation = Quaternion.LookRotation(vectorDest);
                transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
                rb.useGravity = true;
                alreadyCasted = true;
            }

            if (fuel <= 0 && alreadyCasted)
                if (transform.rotation.x < 90)
                    transform.Rotate(vectorDest * Time.deltaTime * 15);
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
            if(!_photonView.IsMine) return;
            
            if (other.gameObject.CompareTag("Player"))
            {
                Player.Player playerHitted = other.GetComponent<Player.Player>();
                PhotonView view = other.GetComponent<PhotonView>();
                
                if(playerHitted != caster && view != null)
                {
                    InstantiateParticle();
                    fuckfriend.StartEffects(view);
                    _photonView.RPC("TellMasterToDestroy", RpcTarget.MasterClient);
                }
            }
            else if(other.gameObject.CompareTag("Track") && !other.gameObject.CompareTag("Foguete") && !other.gameObject.CompareTag("ItemBox"))
            {
                InstantiateParticle();

                _photonView.RPC("TellMasterToDestroy", RpcTarget.MasterClient);

            }
        }

        private void InstantiateParticle()
        {
            if (explosionFeedbacks != null)
            {
                explosionFeedbacks.PlayFeedbacks();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down*height);
        }

        [PunRPC]
        private void TellMasterToDestroy()
        {
            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.Destroy(gameObject);
        }
    }
}
