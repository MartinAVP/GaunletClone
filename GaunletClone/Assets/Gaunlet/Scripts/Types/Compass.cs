using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Four cardinal and for ordinal directions, for a total of eight unit vectors.
/// </summary>
public class Compass
{
    private static Vector3[] compass = { new Vector3(1,0,0), new Vector3(.7f,0,.7f), new Vector3(0,0,1), new Vector3(-.7f,0,.7f),
        new Vector3(-1,0,0), new Vector3(-.7f,0,-.7f), new Vector3(0,0,-1), new Vector3(.7f,0,-.7f) };

    public Vector3 this[int i]
    {
        get { return compass[i]; }
    }

    public static int Length
    {
        get { return compass.Length; }
    }
}
