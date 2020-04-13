using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    public virtual void ActOnOtherComponent(BaseComponent component) { }
    public virtual void ProccessTurn() { }
}
