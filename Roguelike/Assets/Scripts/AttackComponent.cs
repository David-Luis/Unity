using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : BaseComponent
{
    public int strength = 100;

    [TagSelector]
    public string[] TagFilterArray = new string[] { };

    private bool CanAttackGameObject(GameObject gameObject)
    {
        foreach (var tag in TagFilterArray)
        {
            if (gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }

    public override void ActOnOtherComponent(BaseComponent component)
    {
        if (component is DestructibleComponent && CanAttackGameObject(component.gameObject))
        {
            ((DestructibleComponent)component).ReceiveAttack(strength);
        }
    }
}
