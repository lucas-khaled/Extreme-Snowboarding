using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private Item[] availableItems;

    private void PickRandomItem(GameObject player)
    {
        //Talvez uma animação de randomização??

        player.GetComponent<Player>().Coletavel = availableItems[Random.Range(0, availableItems.Length)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickRandomItem(other.gameObject);
            Destroy(gameObject);
        }

        
    }
}
