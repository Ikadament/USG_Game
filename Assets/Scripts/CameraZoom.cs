using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private GameObject NpcCube;
    Camera cam;

    public float zoomValue = 4f;

    private float velocity = 0f;
    private Vector3 moveVelocity = Vector3.zero;

    public float smoothTime = 0.25f;
    private float baseCamSize = 5f;

    private Vector3 baseCamPosition = new Vector3(0, 0, -10);
    private Vector3 npcPositionCam;

    public float yOffset = 2f;

    private Coroutine currentCoroutine = null;

    private void Start()
    {
        cam = GetComponent<Camera>();
        NpcCube = GameObject.FindWithTag("Npc");
    }

    public void MakeCameraZoom()
    {
        npcPositionCam = new Vector3(NpcCube.transform.position.x, NpcCube.transform.position.y + yOffset, transform.position.z);

        // Stopper la coroutine en cours pour eviter les conflits
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        velocity = 0f;
        moveVelocity = Vector3.zero;

        currentCoroutine = StartCoroutine(SmoothZoom());

    }

    private IEnumerator SmoothZoom()
    {
        while (Mathf.Abs(cam.orthographicSize - zoomValue) > 0.01f || (transform.position - npcPositionCam).sqrMagnitude > 0.01f) 
        {
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoomValue, ref velocity, smoothTime);

            transform.position = Vector3.SmoothDamp(transform.position, npcPositionCam, ref moveVelocity, smoothTime);

            yield return null;
        }

        cam.orthographicSize = zoomValue;
        transform.position = npcPositionCam;

        currentCoroutine = null;
    } 

    public void MakeCameraDezoom()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        velocity = 0f;
        moveVelocity = Vector3.zero;

        currentCoroutine = StartCoroutine(SmoothDezoom());
    }

    private IEnumerator SmoothDezoom()
    {
        while (Mathf.Abs(cam.orthographicSize - baseCamSize) > 0.01f || (transform.position - baseCamPosition).sqrMagnitude > 0.01f)
        {
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, baseCamSize, ref velocity, smoothTime);

            
            transform.position = Vector3.SmoothDamp(transform.position, baseCamPosition, ref moveVelocity, smoothTime);
            yield return null;
        }

        cam.orthographicSize = baseCamSize;
        transform.position = baseCamPosition;

        currentCoroutine = null;
    }

    
}
