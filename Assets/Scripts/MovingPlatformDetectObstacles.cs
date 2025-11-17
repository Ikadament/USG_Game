using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformDetectObstacles : MonoBehaviour
{
    public float offset = 0.2f;

    //overlap box collision

    private Vector2 pointUp;
    private Vector2 pointDown;
    private Vector2 sizeUp;
    private Vector2 sizeDown;
    private Vector2 substractBoxVector = new Vector2(0.05f, 0.3f);


    public LayerMask groundLayer;
    public LayerMask platformLayer;

    public bool isCollidingGroundUp;
    public bool isCollidingGroundDown;

    public bool isCollidingPlatformUp;
    public bool isCollidingPlatformDown;

    private MovingPlatform movingPlatform;
    private MovingPlatform movingPlatformUp;
    private MovingPlatform movingPlatformDown;
    public bool hasStoppedPlatformUp = false;
    public bool hasStoppedPlatformDown = false;

    public bool stoppedBecauseGroundUp;

    public bool stoppedBecausePlatformUp;
    public bool stoppedBecausePlatformDown;


    // Start is called before the first frame update
    void Start()
    {
        groundLayer = LayerMask.GetMask("ground", "PortalOne");
        Debug.Log(groundLayer);
        platformLayer = LayerMask.GetMask("MovingPlatform");
    }

    // Update is called once per frame
    void Update()
    {
        pointUp = (Vector2)transform.position + Vector2.up * offset;
        sizeUp = new Vector2(transform.localScale.x, transform.localScale.y) - substractBoxVector;

        pointDown = (Vector2)transform.position + Vector2.down * offset;
        sizeDown = new Vector2(transform.localScale.x, transform.localScale.y) - substractBoxVector;
    }

    private void FixedUpdate()
    {
        isCollidingGroundUp = Physics2D.OverlapBox(pointUp, sizeUp, 0, groundLayer);
        isCollidingGroundDown = Physics2D.OverlapBox(pointDown, sizeDown, 0, groundLayer);

        isCollidingPlatformUp = Physics2D.OverlapBox(pointUp, sizeUp, 0, platformLayer);
        isCollidingPlatformDown = Physics2D.OverlapBox(pointDown, sizeDown, 0, platformLayer);

        PlatformCollisionDetected();
    }



    private void OnDrawGizmos()
    {
        //if (!Application.isPlaying) return; // Ne dessine que quand le jeu tourne
        Vector2 pointUp = (Vector2)transform.position + Vector2.up * offset;
        Vector2 sizeUp = new Vector2(transform.localScale.x, transform.localScale.y) - substractBoxVector;

        Vector2 pointDown = (Vector2)transform.position + Vector2.down * offset;
        Vector2 sizeDown = new Vector2(transform.localScale.x, transform.localScale.y) - substractBoxVector;

        // Dessiner pour le bas
        Gizmos.color = isCollidingGroundDown || isCollidingPlatformDown ?
            (isCollidingGroundDown ? Color.green : Color.yellow) : Color.red;
        Gizmos.DrawWireCube(pointDown, sizeDown);

        // Dessiner pour le haut
        Gizmos.color = isCollidingGroundUp || isCollidingPlatformUp ?
            (isCollidingGroundUp ? Color.green : Color.yellow) : Color.red;
        Gizmos.DrawWireCube(pointUp, sizeUp);

    }

    private void PlatformCollisionDetected()
    {

        if (isCollidingPlatformDown && isCollidingGroundUp)
        {
            Collider2D collider = Physics2D.OverlapBox(pointDown, sizeDown, 0, platformLayer);
            if (collider != null)
            {
                Debug.Log(collider);
                GameObject touchedPlatformDown = collider.gameObject;
                movingPlatformDown = touchedPlatformDown.GetComponent<MovingPlatform>();

                if (!hasStoppedPlatformDown)
                {
                    Debug.Log("STOP");
                    movingPlatformDown.StopPlatform();
                    hasStoppedPlatformDown = true;  // Indique que la plateforme a �t� arr�t�e
                    stoppedBecauseGroundUp = true;
                }
            } 
        }

        if (isCollidingPlatformUp && isCollidingGroundDown)
        {
            Collider2D collider = Physics2D.OverlapBox(pointUp, sizeUp, 0, platformLayer);
            if (collider != null)
            {
                GameObject touchedPlatformUp = collider.gameObject;
                movingPlatformUp = touchedPlatformUp.GetComponent<MovingPlatform>();

                if (!hasStoppedPlatformUp)
                {
                    Debug.Log("StoppingPlatformUp because collidingplatformUp and groundDown");
                    movingPlatformUp.StopPlatform();
                    hasStoppedPlatformUp = true;  // Indique que la plateforme a �t� arr�t�e
                    stoppedBecausePlatformUp = true;
                }
            }
        }

        if(isCollidingPlatformUp && isCollidingPlatformDown)
        {
            Collider2D colliderUp = Physics2D.OverlapBox(pointUp, sizeUp, 0, platformLayer);
            Collider2D colliderDown = Physics2D.OverlapBox(pointDown, sizeDown, 0, platformLayer);
            if(colliderUp != null && colliderDown != null)
            {
                GameObject touchedPlatformUp = colliderUp.gameObject;
                movingPlatformUp = touchedPlatformUp.GetComponent<MovingPlatform>();

                GameObject touchedPlatformDown = colliderDown.gameObject;
                movingPlatformDown = touchedPlatformDown.GetComponent<MovingPlatform>();

                if (!hasStoppedPlatformDown)
                {
                    movingPlatformUp.StopPlatform();
                    movingPlatformDown.StopPlatform();

                    hasStoppedPlatformUp = true;
                    hasStoppedPlatformDown = true;

                    stoppedBecausePlatformUp = true;
                    stoppedBecausePlatformDown = true;
                }
            }
        }

        // Si l'objet n'est plus en collision avec le sol ou la plateforme et que la plateforme a ete arretee, on relance la plateforme
        if (stoppedBecauseGroundUp && !isCollidingGroundUp)
        {
            Debug.Log("ResumeGroundUp");
            movingPlatformDown.ResumePlatform();

            hasStoppedPlatformDown = false;  // Reinitialise l'etat de la plateforme
            stoppedBecauseGroundUp = false;
        }
        else if (stoppedBecausePlatformUp && !isCollidingPlatformUp)
        {
            Debug.Log("ResumeGroundDown");
            movingPlatformUp.ResumePlatform();

            hasStoppedPlatformUp = false;  // Reinitialise l'etat de la plateforme
            stoppedBecausePlatformUp = false;

            if (movingPlatformDown != null)
            {
                Debug.Log("ResumePlatformDown");
                movingPlatformDown.ResumePlatform();

                hasStoppedPlatformDown = false;
                stoppedBecausePlatformDown = false;
            }
        }
        else if (stoppedBecausePlatformUp && stoppedBecausePlatformDown)
        {
            if(stoppedBecausePlatformUp && !isCollidingPlatformUp)
            {
                Debug.Log("ResumePlatformUP");
                movingPlatformUp.ResumePlatform();

                hasStoppedPlatformUp = false;
                stoppedBecausePlatformUp = false;
            } 
            else if (stoppedBecausePlatformDown && !isCollidingPlatformDown)
            {
                Debug.Log("ResumePlatformDown");
                movingPlatformDown.ResumePlatform();

                hasStoppedPlatformDown = false;
                stoppedBecausePlatformDown = false;
            }
        }
    }
}
