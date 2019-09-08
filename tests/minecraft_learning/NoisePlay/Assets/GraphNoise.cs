using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphNoise : MonoBehaviour {

	float t = 0;
    float inc = 0.01f;

    float t2 = 0;
    float inc2 = 0.001f;

    void Update () 
	{
        t += inc;
        float n = Mathf.PerlinNoise(t,1);
        Grapher.Log(n, "Perlin1", Color.yellow);

        t2 += inc2;
        float n2 = Mathf.PerlinNoise(t2, 1);
        Grapher.Log(n2, "Perlin2", Color.green);

        float n3;
        n3 = (n + n2) * 0.5f;

        Grapher.Log(n3, "Total", Color.red);
    }
}
