using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WireButtonDoor : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField] private ButtonBehavior linkedButton;
    [SerializeField] private float lineWidth = 0.05f;
    [SerializeField] private float transparency = 0.5f;

    private void Start()
    {
        line = GetComponent<LineRenderer>();

        Color redTransparent = new Color(1f, 0f, 0f, transparency);
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = redTransparent;
        line.endColor = redTransparent;

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = 2;
        line.useWorldSpace = true;

        line.sortingLayerName = "Default";
        line.sortingOrder = 999;
    }

    private void Update()
    {
        if (linkedButton == null || gameObject == null)
            return;

        Vector3 doorPos = gameObject.transform.position;
        Vector3 buttonPos = linkedButton.transform.position;

        doorPos.z = -0.1f;
        buttonPos.z = -0.1f;

        line.SetPosition(0, doorPos);
        line.SetPosition(1, buttonPos);
    }
}
