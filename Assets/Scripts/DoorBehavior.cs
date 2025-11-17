using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private Animator doorAnimator;
    public BoxCollider2D doorBoxCollider;

    public bool isOpen;

    // external refs
    //private GameObject button;
    //private ButtonBehavior buttonBehavior;
    [SerializeField] private ButtonBehavior linkedButton;

    public bool makeDoorUntouchable = false;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();

        //button = GameObject.FindWithTag("Button");
        //buttonBehavior = button.GetComponent<ButtonBehavior>();
    }

    private void Update()
    {
        if (linkedButton.isPressed)
        {
            doorAnimator.SetBool("buttonPressed", true);

            if (makeDoorUntouchable)
            {
                doorBoxCollider.enabled = false;
            }
        }
        else
        {
            doorAnimator.SetBool("buttonPressed", false);

            if (makeDoorUntouchable)
            {
                doorBoxCollider.enabled = true;
            }
        }
    }
}
