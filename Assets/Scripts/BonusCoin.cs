using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCoin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            FindObjectOfType<AudioManager>().Play("gold");
        }
    }
}
