using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Systems))]
public class GameManager : MonoBehaviour
{
    public static Systems systems;

    // Start is called before the first frame update
    void Awake()
    {
        systems = GetComponent<Systems>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
