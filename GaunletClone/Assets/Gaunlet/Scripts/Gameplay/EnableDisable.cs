using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisable : MonoBehaviour
{
    [SerializeField] protected GameObject obj;

    public void Enable()
    {
        Debug.Log(name + " enable");
        obj.SetActive(true);
    }

    public void Disable()
    {
        Debug.Log(name + " disable");
        obj.SetActive(false);
    }
}
