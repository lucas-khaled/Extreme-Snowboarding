using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class Mochileiro : MonoBehaviour
    {
        [SerializeField] private float velocidade;

        private bool stop = false;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!stop)
            Move();
        }

        void Move()
        {
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y + 1, transform.position.z), Vector3.down, out hit, 2, LayerMask.GetMask("Track")))
            {
                transform.position = Vector3.MoveTowards(transform.position, hit.point, velocidade * Time.deltaTime);
            }
            else if (Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y + 1, transform.position.z), Vector3.up, out hit, 2, LayerMask.GetMask("Track")))
            {
                transform.position = Vector3.MoveTowards(transform.position, hit.point, velocidade * Time.deltaTime);
            }

            ClampRotationByGround(hit);
        }

        void ClampRotationByGround(RaycastHit hit)
        {
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 100 * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "MochilaColisor")
            {
                stop = true;
                this.GetComponent<Animator>().SetBool("Idle", true);
            }
        }
    }
}
