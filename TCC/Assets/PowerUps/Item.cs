using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    protected Effect[] atributesToChange;
    public abstract void Activate(Player player);
}
