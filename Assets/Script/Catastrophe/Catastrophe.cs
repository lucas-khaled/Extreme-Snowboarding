using System.Collections;
using ExtremeSnowboarding.Script.Controllers;
using ExtremeSnowboarding.Script.EstadosPlayer;
using UnityEngine;

namespace ExtremeSnowboarding.Script.Catastrophe
{
    public class Catastrophe : MonoBehaviour
    {
        [Header("Camera Shake")]
        [SerializeField]
        private bool deactivateCameraShake;

        [Header("Values")]
        [SerializeField]
        private float velocity;
        [SerializeField]
        private float shakingDuration;
        [SerializeField]
        private float magnetude;
        [SerializeField]
        private float tempoEspera;
        [SerializeField]
        private float aceleracao;
        [SerializeField]
        private float maxVel;

        [Header("References")]
        [SerializeField]
        private ParticleSystem particles;

        private GameCamera cameraScript;
        private Vector3 nextMovementPoint;
        private bool isMoving;

        void Start()
        {
            isMoving = false;
            StartCoroutine(CatastropheStartTimer());
            cameraScript = GameObject.FindGameObjectWithTag("CorridaController").GetComponent<CorridaController>().camera;
        }

        void Update()
        {
            Movement();
        }

        IEnumerator CatastropheStartTimer()
        {
            yield return new WaitForSeconds(tempoEspera);
            isMoving = true;
            StartCatastrophe();
        }


        void StartCatastrophe()
        {
            //audioSource.Play();

            this.GetComponent<AudioSource>().mute = false;

            // Animação catastrophe iniciando
            this.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponent<SphereCollider>().enabled = true;
            particles.Play();

            cameraScript.StartCoroutine(cameraScript.CameraShake(deactivateCameraShake, shakingDuration, magnetude));

            CorridaController.instance.catastrophe = this.gameObject;
            // Aviso?

            GetNextMovementPoint();
        }

        Vector3 GetNextMovementPoint()
        {
            Vector3 newPoint = new Vector3(this.transform.position.x + 5f, 
                this.transform.position.y, 
                this.transform.position.z);            

            RaycastHit hit;

            if (Physics.Raycast(newPoint, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, LayerMask.GetMask("Track")))
                newPoint = hit.point;
            else if (Physics.Raycast(newPoint, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity, LayerMask.GetMask("Track")))
                newPoint = hit.point;

            return newPoint;
        }

        void Movement()
        {
            if (isMoving)
            {
                if (Vector3.Distance(this.transform.position, nextMovementPoint) <= 0.01f)
                    nextMovementPoint = GetNextMovementPoint();

                this.transform.position = Vector3.MoveTowards(this.transform.position,     // Posicao inicial 
                    nextMovementPoint,           // Posicao destino
                    velocity * Time.deltaTime);  // Velocidade movimento

                if (velocity <= maxVel) 
                velocity += (aceleracao / maxVel) * Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!isMoving) return;
            
            if(other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player.Player>().SharedValues.actualState != "Dead")
            {
                other.gameObject.GetComponent<Player.Player>().ChangeState(new Dead());
            }
            else if (other.gameObject.CompareTag("Projectile"))
                Destroy(other.gameObject);
        }
    }
}
