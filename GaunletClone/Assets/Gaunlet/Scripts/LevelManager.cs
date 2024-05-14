using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int level;

    void Start()
    {
        UIManager ui = FindAnyObjectByType<UIManager>();
        ui.setLevel(level);
    }
}
