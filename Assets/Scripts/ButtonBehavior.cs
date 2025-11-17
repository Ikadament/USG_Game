using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public bool isPressed;
    public float offset = 0.2f;

    public LayerMask layers;

    //overlap box collision

    private Vector2 point;
    private Vector2 size;

    private float substractBoxValueY = 0.5f;

    //spring joint body refs
    private SpringJoint2D springJoint;
    private Vector2 lastPosition;
    [SerializeField] private float springOffset;

    //button placement
    public bool isPlacedOnFloor = false;

    //son
    private bool wasPressedLastFrame = false;

    public enum ButtonPlacement
    {
        Floor,
        Ceiling,
        WallLeft,
        WallRight
    }

    public ButtonPlacement placement = ButtonPlacement.Floor; // Par defaut au sol

    private void Start()
    {
        layers = LayerMask.GetMask("ground");

        //unused springJoint 

        //springJoint = GetComponent<SpringJoint2D>();

        //// Initialise la position precedente et le connectedAnchor
        /// 
        //if (springJoint != null && springJoint.connectedBody == null)
        //{
        //    lastPosition = transform.position;
        //    UpdateConnectedAnchor();
        //}
    }

    private void Update()
    {

        switch (placement)
        {
            case ButtonPlacement.Floor:
                point = transform.position + Vector3.up * offset;
                size = new Vector2(transform.localScale.x, transform.localScale.y - substractBoxValueY);
                break;

            case ButtonPlacement.Ceiling:
                point = transform.position + Vector3.down * offset;
                size = new Vector2(transform.localScale.x, transform.localScale.y - substractBoxValueY);
                break;

            case ButtonPlacement.WallLeft:
                point = transform.position + Vector3.left * offset;
                size = new Vector2(transform.localScale.x - substractBoxValueY, transform.localScale.y);
                break;

            case ButtonPlacement.WallRight:
                point = transform.position + Vector3.right * offset;
                size = new Vector2(transform.localScale.x - substractBoxValueY, transform.localScale.y);
                break;
        }

        //// Verifie si la position a change depuis la derniere frame
        //if ((Vector2)transform.position != lastPosition)
        //{
        //    // Si oui, on met a jour le connectedAnchor
        //    if (springJoint != null && springJoint.connectedBody == null)
        //    {
        //        UpdateConnectedAnchor();
        //        lastPosition = transform.position; // Mets a jour la position actuelle
        //    }
        //}
    }

    private void FixedUpdate()
    {
        isPressed = Physics2D.OverlapBox(point, size, 0, layers);

        if (isPressed && !wasPressedLastFrame)
        {
            // La condition vient JUSTE de devenir vraie
            FindObjectOfType<AudioManager>().Play("buttonPressed");
        }

        // Met a jour l'etat precedent
        wasPressedLastFrame = isPressed;
    }

    // methode pour mettre a jour connectedAnchor
    private void UpdateConnectedAnchor()
    {
        //if (springJoint != null)
        //{
        //    springJoint.connectedAnchor = (Vector2)transform.position + Vector2.down * offset;
        //}
    }

    //debug gizmos
    private void OnDrawGizmos()
    {

        Vector2 gizmoPoint = Vector2.zero;
        Vector2 gizmoSize = Vector2.zero;

        switch (placement)
        {
            case ButtonPlacement.Floor:
                gizmoPoint = (Vector2)transform.position + Vector2.up * offset;
                gizmoSize = new Vector2(transform.localScale.x, transform.localScale.y - substractBoxValueY);
                break;

            case ButtonPlacement.Ceiling:
                gizmoPoint = (Vector2)transform.position + Vector2.down * offset;
                gizmoSize = new Vector2(transform.localScale.x, transform.localScale.y - substractBoxValueY);
                break;

            case ButtonPlacement.WallLeft:
                gizmoPoint = (Vector2)transform.position + Vector2.left * offset;
                gizmoSize = new Vector2(transform.localScale.x - substractBoxValueY, transform.localScale.y);
                break;

            case ButtonPlacement.WallRight:
                gizmoPoint = (Vector2)transform.position + Vector2.right * offset;
                gizmoSize = new Vector2(transform.localScale.x - substractBoxValueY, transform.localScale.y);
                break;
        }

        Gizmos.color = isPressed ? UnityEngine.Color.green : UnityEngine.Color.red;
        Gizmos.DrawWireCube(gizmoPoint, gizmoSize);
    }
}
