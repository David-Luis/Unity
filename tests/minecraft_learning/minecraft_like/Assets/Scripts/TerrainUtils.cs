using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    const float PERSISTENCE = 0.5f;

    const int TERRAIN_MIN_HEIGHT = 0;
    const int TERRAIN_MAX_HEIGHT = 150;
    const float TERRAIN_SMOOTH = 0.01f;
    const int TERRAIN_OCTAVES = 4;

    const int STONE_DEPTH = 15;
    const float STONE_SMOOTH = 0.02f;
    const int STONE_OCTAVES = 5;

    const float CAVES_SMOOTH = 0.1f;
    const float CAVES_CUTOFF = 0.42f;
    const int CAVES_OCTAVES = 3;

    const float REDSTONE_SMOOTH = 0.03f;
    const int REDSTONE_OCTAVES = 3;
    const float REDSTONE_CUTOFF = 0.41f;
    const int REDSTONE_MAX_HEIGHT = 20;

    const float DIAMONDS_SMOOTH = 0.01f;
    const int DIAMOND_OCTAVES = 2;
    const float DIAMOND_CUTOFF = 0.4f;
    const int DIAMOND_MAX_HEIGHT = 25;

    public static int GenerateStoneHeight(float x, float z)
    {
        float height = MapNormalized(TERRAIN_MIN_HEIGHT, TERRAIN_MAX_HEIGHT - STONE_DEPTH, fBM(x * STONE_SMOOTH, z * STONE_SMOOTH, STONE_OCTAVES, PERSISTENCE));

        return (int)height;
    }

    public static int GenerateHeight(float x, float z)
    {
        float height = MapNormalized(TERRAIN_MIN_HEIGHT, TERRAIN_MAX_HEIGHT, fBM(x * TERRAIN_SMOOTH, z * TERRAIN_SMOOTH, TERRAIN_OCTAVES, PERSISTENCE));

        return (int)height;
    }

    public static bool IsCave(float x, float y, float z)
    {
        return fBM3D(x, y, z, CAVES_SMOOTH, CAVES_OCTAVES) < CAVES_CUTOFF;
    }
    
    public static bool IsDiamond(float x, float y, float z)
    {
        return fBM3D(x, y, z, DIAMONDS_SMOOTH, DIAMOND_OCTAVES) < DIAMOND_CUTOFF && y <= DIAMOND_MAX_HEIGHT;
    }

    public static bool IsRedstone(float x, float y, float z)
    {
        return fBM3D(x, y, z, REDSTONE_SMOOTH, REDSTONE_OCTAVES) < REDSTONE_CUTOFF && y <= REDSTONE_MAX_HEIGHT;
    }

    static float MapNormalized(float newmin, float newmax, float value)
    {
        return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(0, 1, value));
    }

    static float fBM(float x, float z, int octaves, float persistence)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        float offset = 32000f; //? to avoid have negative numbers, probably can be improved
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise((x + offset) * frequency, (z+ offset) * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistence;
            frequency *= 2;
        }

        return total / maxValue;
    }

    static float fBM3D(float x, float y, float z, float smooth, int octaves)
    {
        float XY = fBM(x * smooth, y * smooth, octaves, 0.5f);
        float YZ = fBM(y * smooth, z * smooth, octaves, 0.5f);
        float XZ = fBM(x * smooth, z * smooth, octaves, 0.5f);
                                                           
        float YX = fBM(y * smooth, x * smooth, octaves, 0.5f);
        float ZY = fBM(z * smooth, y * smooth, octaves, 0.5f);
        float ZX = fBM(z * smooth, x * smooth, octaves, 0.5f);

        return (XY + YZ + XZ + YX + ZY + ZX) / 6.0f;
    }
}
