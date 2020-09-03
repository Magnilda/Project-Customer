using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    //[SerializeField] private float fastSpeed;  //Faster camera movement option.
    [SerializeField] private float normalSpeed;
    [SerializeField, Tooltip("Lower values make it smoother, you can also go into the decimal eg. 0.5")]
    private float smoothDelta;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    [Header("Debug settings")]
    [SerializeField] private float pivotDebugRadius = 2;
    [SerializeField] private Vector3 pivot;
    [SerializeField] private float zoomAmount = 1;
    private bool mouseDragging;
    [SerializeField] private float rotationY;
    [SerializeField] private float rotationX = 50f;
    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    //private Vector3 rotateStartPosition;
    //private Vector3 rotateCurrentPosition;

    //=================================================================
    //                           Start()
    //=================================================================

    void Start()
    {
        dragStartPosition = Input.mousePosition;
        //rotateStartPosition = Input.mousePosition;
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
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragStartPosition = Input.mousePosition;
            }

            dragCurrentPosition = Input.mousePosition;
            Vector2 difference = new Vector2();
            difference = (dragStartPosition - dragCurrentPosition).normalized;
            pivot += new Vector3(difference.x, 0, difference.y) * normalSpeed * Time.deltaTime;

            dragStartPosition = Input.mousePosition;
        }

        //Zooming in and out with scroll wheel
        if (Input.mouseScrollDelta.y != 0)
        {
            SetZoom(zoomAmount - (zoomSpeed * Input.mouseScrollDelta.y * Time.deltaTime));
        }

        //Rotation
        //Future plans......to be continued.....
    }

    //=================================================================
    //                        HandleKeyboardInput()
    //=================================================================
    private void HandleKeyboardInputs()
    {
        //Panning
        Vector3 moveDirection = (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")).normalized;
        pivot += moveDirection * normalSpeed * Time.deltaTime;

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
            rotationY--;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationY++;
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
        transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));   //Needs fixing....
        Vector3 targetPosition = pivot + -transform.forward * zoomAmount;

        transform.position = Vector3.Slerp(transform.position, targetPosition, Time.deltaTime * smoothDelta);
    }


    //Debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(pivot, pivotDebugRadius);
    }
}
