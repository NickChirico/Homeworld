using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Camera Control Script
 * 
 */

public class CameraController : MonoBehaviour
{
    private CameraInputActions cameraActions;
    private InputAction movement;
    private Transform cameraTransform;
    private Camera cam;

    // panning movement
    [SerializeField]
    private float panSpeed = 5f;
    [SerializeField]
    private float panAcceleration = 10f;
    [SerializeField]
    private float panDamping = 15f;

    // zoom
    [SerializeField]
    private float zoomStep = 0.5f;
    [SerializeField]
    private float zoomMinSize = 4f;
    [SerializeField]
    private float zoomMaxSize = 40f;
    [SerializeField]
    private float zoomDampening = 7.5f;

    // rotation
    [SerializeField]
    private float rotateMaxSpeed = 1f;
    [SerializeField]
    private float rotateSnapBackTime = 0.5f;

    // screen-edge motion
    [SerializeField]
    [Range( 0f, 0.1f )]
    private float edgeTolerance = 0.05f;

    // bool checks
    [SerializeField]
    private bool enableEdgeScroll = true, enableDragScroll = true;

    // instance vars
    private float currentSpeed;
    private float targetZoom;
    private Vector3 targetPos;
    private Vector3 horzVelocity;
    private Vector3 lastPosition;
    private Vector3 startDrag;
    private Quaternion baseRotation = Quaternion.Euler( 0f, y: 0f, 0f );
    private bool rotating = false;


    private void Awake() {
        cameraActions = new CameraInputActions();
        cam = this.GetComponentInChildren<Camera>();
        cameraTransform = cam.transform;
        targetZoom = cam.orthographicSize;
    }

    private void Update()
    {
        _getKeyboardMovement();
        if ( enableEdgeScroll )
            _checkMouseAtScreenEdge();
        if ( enableDragScroll && !rotating )
            _dragCamera();

        _updateVelocity();
        _updateBasePos();
        _updateZoom();
    }

    private void OnEnable() {
        lastPosition = this.transform.position;
        movement = cameraActions.CameraActions.Movement;
        cameraActions.CameraActions.RotateKey.performed += EnableRotate;
        cameraActions.CameraActions.RotateKey.canceled += CancelRotate;
        cameraActions.CameraActions.Rotate.performed += RotateCamera;
        cameraActions.CameraActions.Zoom.performed += ZoomCamera;

        cameraActions.CameraActions.Enable();
    }

    private void EnableRotate( InputAction.CallbackContext context ) {
        rotating = true;
    }

    private void CancelRotate( InputAction.CallbackContext context ) {
        rotating = false;
        StartCoroutine( snapBack() );
    }

    private void OnDisable() {
        cameraActions.CameraActions.RotateKey.performed -= EnableRotate;
        cameraActions.CameraActions.RotateKey.canceled -= CancelRotate;
        cameraActions.CameraActions.Rotate.performed -= RotateCamera;
        cameraActions.CameraActions.Zoom.performed -= ZoomCamera;
        cameraActions.Disable();
    }

    //

    private void _updateVelocity() {
        horzVelocity = ( this.transform.position - lastPosition ) / Time.deltaTime;
        horzVelocity.y = 0;
        lastPosition = this.transform.position;
    }

    private void _getKeyboardMovement() {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * _getCameraRight()
            + movement.ReadValue<Vector2>().y * _getCameraForward();
        inputValue = inputValue.normalized;
        if ( inputValue.sqrMagnitude > 0.1f ) {
            targetPos += inputValue;
        }

        rotating = cameraActions.CameraActions.RotateKey.IsPressed();
    }

    private Vector3 _getCameraRight() {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }

    private Vector3 _getCameraForward() {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private void _updateBasePos() {
        if ( targetPos.sqrMagnitude > 0.1f ) {
            currentSpeed = Mathf.Lerp( currentSpeed, panSpeed, Time.deltaTime * panAcceleration );
            this.transform.position += targetPos * currentSpeed * Time.deltaTime;
        } else { // <= 0
            horzVelocity = Vector3.Lerp(horzVelocity, Vector3.zero, Time.deltaTime * panDamping );
            this.transform.position += horzVelocity * Time.deltaTime;
        }
        targetPos = Vector3.zero;
    }

    // 

    private void RotateCamera( InputAction.CallbackContext context ) {
        if(!rotating) {
            return;
        }
        float val = context.ReadValue<Vector2>().x;
        this.transform.rotation = Quaternion.Euler( 0f, val * rotateMaxSpeed + transform.rotation.eulerAngles.y, 0f );
    }

    private void ZoomCamera( InputAction.CallbackContext context ) {
        if( context.ReadValue<Vector2>().y > 0 ) {
            // zoom in - size gets smaller
            if(cam.orthographicSize - zoomStep <= zoomMinSize ) {
                targetZoom = zoomMinSize;
            } else {
                targetZoom = cam.orthographicSize - zoomStep;
            }
        } else {
            // zoom out - size gets bigger
            if ( cam.orthographicSize + zoomStep >= zoomMaxSize ) {
                targetZoom = zoomMaxSize;
            } else {
                targetZoom = cam.orthographicSize + zoomStep;
            }
        }
    }

    private void _updateZoom() {
        cam.orthographicSize = Mathf.Lerp( cam.orthographicSize, targetZoom, Time.deltaTime * zoomDampening );
    }

    private IEnumerator snapBack() {
        float elapsedTime = 0;
        while (elapsedTime < rotateSnapBackTime) {
            this.transform.rotation = Quaternion.Lerp( this.transform.rotation, baseRotation, ( elapsedTime / rotateSnapBackTime ) );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.transform.rotation = baseRotation;
        yield return null;
    }

    //

    private void _checkMouseAtScreenEdge() {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if ( mousePos.x < edgeTolerance * Screen.width ) {
            moveDirection += -_getCameraRight();
        } else if ( mousePos.x > ( 1f - edgeTolerance ) * Screen.width ) {
            moveDirection += _getCameraRight();
        }

        if ( mousePos.y < edgeTolerance * Screen.height ) {
            moveDirection += -_getCameraForward();
        } else if ( mousePos.y > ( 1f - edgeTolerance ) * Screen.height ) {
            moveDirection += _getCameraForward();
        }

        targetPos += moveDirection;
    }

    private void _dragCamera() {
        if ( !Mouse.current.middleButton.isPressed )
            return;

        Plane plane = new Plane( Vector3.up, Vector3.zero );
        Ray ray = cam.ScreenPointToRay( Mouse.current.position.ReadValue() );
        if(plane.Raycast(ray, out float distance ) ) {
            if ( Mouse.current.middleButton.wasPressedThisFrame ) {
                startDrag = ray.GetPoint( distance );
            } else {
                targetPos += startDrag - ray.GetPoint( distance );
            }
        }
    }
}


