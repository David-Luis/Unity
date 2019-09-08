﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{
    public static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
