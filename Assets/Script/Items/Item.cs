using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Item : ScriptableObject
{
    [FormerlySerializedAs("attributesToChange")] [SerializeField]
    protected Effect[] effectsToApply;

    public abstract void Activate(Player player);

    public void StartEffects(Player player)
    {
        foreach (Effect effect in effectsToApply)
        {
            effect.StartEffect(player);
        }
    }
}
