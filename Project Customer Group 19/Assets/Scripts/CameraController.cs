using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    //[SerializeField] private float fastSpeed;  //Faster camera movement option.
    [Header("Camera settings")]
    [SerializeField] private float cameraSpeed;
    [SerializeField, Tooltip("Lower values make it smoother, you can also go into the decimal eg. 0.5")]
    private float smoothDelta;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float tilt = 50f;

    [Header("Debug settings")]
    [SerializeField] private float pivotDebugRadius = 2;
    [SerializeField] private Vector3 pivot;
    [SerializeField] private float zoomAmount = 1;
    [SerializeField] private float rotationY;
    //private Vector3 tempZoom;
    //private float amountToZoom;
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;

    //=================================================================
    //                           Start()
    //=================================================================
    void Start()
    {
        dragStartPosition = Input.mousePosition;
        rotateStartPosition = Input.mousePosition;
    }

    //=================================================================
    //                           Update()
    //=================================================================
    void Update()
    {
        HandleMouseInput();
        HandleKeyboardInputs();
        CameraMovement();
    }

    //=================================================================
    //                        HandleMouseInput()
    //=================================================================
    private void HandleMouseInput()
    {
        //Panning
        if (Input.GetMouseButton(2))
        {
            if (Input.GetMouseButtonDown(2))
            {
                dragStartPosition = Input.mousePosition;
            }

            dragCurrentPosition = Input.mousePosition;
            Vector2 difference = (dragStartPosition - dragCurrentPosition).normalized;
            pivot += new Vector3(difference.x, 0, difference.y) * cameraSpeed * Time.deltaTime;

            dragStartPosition = Input.mousePosition;
        }

        //Zooming in and out with scroll wheel
        if (Input.mouseScrollDelta.y != 0)
        {
            SetZoom(zoomAmount - (zoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime));
        }

        //Rotation
        if (Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1))
            {
                rotateStartPosition = Input.mousePosition;
            }

            rotateCurrentPosition = Input.mousePosition;
            Vector2 difference = (rotateStartPosition - rotateCurrentPosition).normalized;
            //rotationX += difference.y * rotationSpeed * Time.deltaTime;
            rotationY += difference.x * rotationSpeed * Time.deltaTime;

            rotateStartPosition = Input.mousePosition;
        }
    }

    //=================================================================
    //                        HandleKeyboardInput()
    //=================================================================
    private void HandleKeyboardInputs()
    {
        //Panning
        Vector3 moveDirection = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
        moveDirection.y = 0;
        pivot += moveDirection * cameraSpeed * Time.deltaTime;

        //Zooming in and out
        if (Input.GetKey(KeyCode.R))
        {
            SetZoom(zoomAmount - zoomSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.F))
        {
            SetZoom(zoomAmount + zoomSpeed * Time.deltaTime);
        }

        //Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            rotationY-= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationY+= rotationSpeed * Time.deltaTime;
        }
    }

    //=================================================================
    //                        SetZoom(float zoom)
    //=================================================================
    private void SetZoom(float zoom)
    {
        zoomAmount = Mathf.Clamp(zoom, minZoom, maxZoom);
    }

    //=================================================================
    //                        CameraMovement()
    //=================================================================
    private void CameraMovement()
    {
        Quaternion targetRotation = Quaternion.Euler(new Vector3(tilt, rotationY, 0));

        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,smoothDelta*Time.deltaTime);

        transform.position = pivot + -transform.forward * zoomAmount;
    }


    //Debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(pivot, pivotDebugRadius);
    }
}
