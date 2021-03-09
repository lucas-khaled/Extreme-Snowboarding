using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [SerializeField]
    protected Effect[] attributesToChange;

    public abstract void Activate(Player player);

    public void StartEffects(Player player)
    {
        Debug.Log("used: " + name);
        foreach (Effect effect in attributesToChange)
        {
            player.StartCoroutine(effect.StartEffect(player));
        }
    }
}
