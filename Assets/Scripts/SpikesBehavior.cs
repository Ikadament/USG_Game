using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikesBehavior : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("die");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
