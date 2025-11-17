using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTriggerMessage : MonoBehaviour
{
    public GameObject messagePopUpCanvas;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingSquareOrange"))
        {
            Debug.Log("Orange");
            messagePopUpCanvas.SetActive(true);
        }
    }
}
