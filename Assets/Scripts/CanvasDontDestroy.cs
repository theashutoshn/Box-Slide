using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasDontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
