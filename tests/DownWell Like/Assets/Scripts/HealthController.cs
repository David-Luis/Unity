using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float life = 1;

    public void RemoveLife(float amount)
    {
        life -= amount;
    }
}
