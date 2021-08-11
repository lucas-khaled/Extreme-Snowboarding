using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnowboarding
{
	public class CustomGizmo : MonoBehaviour
    {
        public Color color;
        public float radius;

        public GameObject esfera;

        private void Start()
        {
            Instantiate(esfera, this.transform.position, Quaternion.identity);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = color;

            Gizmos.DrawSphere(transform.position, radius);
        }

    }
}
