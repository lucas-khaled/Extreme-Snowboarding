using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeSnowboarding;

namespace ExtremeSnowboarding.Script
{
    public class VoidReturnTrack : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                Player.Player player = collision.gameObject.GetComponent<Player.Player>();
                collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + player.SharedValues.CharacterHeight + 5, collision.transform.position.z);
            }
            else
            {
                Debug.Log("Piroca");
            }
        }
    }
}
