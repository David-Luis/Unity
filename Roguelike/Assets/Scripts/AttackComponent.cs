using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : BaseComponent
{
    public int strength = 100;

    public override void ActOnOtherComponent(BaseComponent component)
    {
        if (component is DestructibleComponent)
        {
            ((DestructibleComponent)component).ReceiveAttack(strength);
        }
    }
}
