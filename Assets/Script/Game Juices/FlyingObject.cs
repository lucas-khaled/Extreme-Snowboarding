using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class FlyingObject : MonoBehaviour
    {
        [SerializeField]
        private Transform[] pontos;
        [SerializeField]
        private float velocidade;

        private Transform desiredPoint;

        private void Start()
        {
            GetNextPoint();
        }

        private void Update()
        {
            Debug.Log(desiredPoint.position);
            if (desiredPoint != null)
            {
                if (Vector3.Distance(this.transform.position, desiredPoint.position) < 1f)
                {
                    GetNextPoint();
                }
                else
                {
                    MoveToNextPoint();
                }
            }
        }

        private void GetNextPoint()
        {
            bool gotNextPoint = false;

            do
            {
                Transform newPoint = pontos[Random.Range(0, pontos.Length)];
                Vector3 direction = this.transform.position - newPoint.position;

                RaycastHit hit;

                if (Physics.Raycast(this.transform.position, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Track")))
                    gotNextPoint = false;
                else
                {
                    gotNextPoint = true;
                    desiredPoint = newPoint;
                }

            } while (!gotNextPoint);
        }

        private void MoveToNextPoint()
        {
            Vector3.MoveTowards(this.transform.position, desiredPoint.position, velocidade);
        }
    }
}
