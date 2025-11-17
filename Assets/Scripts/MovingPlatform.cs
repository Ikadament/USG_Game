using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 3f;     // Distance verticale totale
    public float speed = 2f;            // Vitesse du mouvement

    private Rigidbody2D rb;
    private Vector3 startPos;
    private Vector2 targetPosition;

    public bool isStopped = false;
    private float movementTime = 0f; // Temps local qui ne progresse que si la plateforme est active

    public bool xMovement;

    

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            movementTime += Time.fixedDeltaTime;
        }

        // Crée un mouvement sinusoidal
        float sineValue = Mathf.Sin(movementTime * speed) * 0.5f + 0.5f; // Normalise entre 0 et 1

        if (xMovement)
        {
            // Mouvement horizontal
            targetPosition = new Vector2(startPos.x + sineValue * moveDistance, startPos.y);
        }
        else
        {
            // Mouvement vertical (défaut)
            targetPosition = new Vector2(startPos.x, startPos.y + sineValue * moveDistance);
        }

        if (isStopped) return;

        // Déplacer la plateforme de manière physique, mais l'imiter à l'axe Y uniquement
        Vector2 currentPosition = rb.position;
        Vector2 moveDirection = (targetPosition - (Vector2)transform.position); // Direction vers la position cible


        moveDirection.y = Mathf.Clamp(moveDirection.y, -moveDirection.magnitude, moveDirection.magnitude); // Limiter le déplacement sur l'axe Y

        // Applique le mouvement en respectant la physique avec MovePosition
        rb.MovePosition(currentPosition + moveDirection);
    }

    public void StopPlatform()
    {
        isStopped = true;
    }

    public void ResumePlatform()
    {
        isStopped = false;
    }
}
