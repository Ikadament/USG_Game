using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        if (transform.parent != null)
        {
            transform.SetParent(null); // Detache de tout parent
        }


        DontDestroyOnLoad(gameObject);
    }
}
