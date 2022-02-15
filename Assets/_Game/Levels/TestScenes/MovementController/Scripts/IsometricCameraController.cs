using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    /*This is probably the biggest class out of all the classes. This is just the camera movement controller for an isometric view.*/

    [Header("Camera Connections")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] Rigidbody _rigidbody;

    [Header("Traditional Camera Movement Values")]
    [SerializeField] private float _cameraMoveSpeed = 10f;

    [Header("Rigidbody/Sliding Camera Movement Values")]
    [SerializeField] private bool _enableSlidingMovement = false;

    [Header("Camera Zoom Values")]
    [SerializeField] private float _cameraZoomSpeed = 5f;
    [SerializeField] private float _maxZoomInValue = 0.7f;
    [SerializeField] private float _maxZoomOutValue = 6.37f;

    [Header("Camera Sprint")]
    [SerializeField] private bool _enableSprintSpeed = true;
    [SerializeField] private float _cameraSprintSpeed = 20f;
    
    
    private Vector3 forward, right;

    private void Start()
    {
        if (_mainCamera != null)
        {
            forward = _mainCamera.transform.forward;
            right = _mainCamera.transform.right;
        }
        else
        {
            forward = Camera.main.transform.forward;
            right = Camera.main.transform.right;
        }

       
        forward.y = 0f;
        forward = Vector3.Normalize(forward);
        right = Vector3.Normalize(right);


    }

    void HandleInput()
    {
        /*Do not look at this math. Or do and let me know how it works. I basically had no idea how to make motion work so I took a tutorial on isometric 
         player movement and frankensteined the following unholy abomination from it to make it work for the camera. Bear in mind that this script is attached
         to the CameraController empty and not the Camera itself.*/

        Vector3 rightMovement;
        Vector3 upMovement;


        //Vector3 movementNormalized;
        //Debug.Log("Right: " + right);
        //Debug.Log("Forward: " + forward);
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        //Debug.Log(Input.GetAxisRaw("Horizontal"));

        //I also added a sprint mechanic which i hate calling it a "mechanic" cuz it's literally just changing a float value.
        if (_enableSprintSpeed && Input.GetKey(KeyCode.LeftShift))
        {
            rightMovement = right * _cameraSprintSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
            upMovement = forward * _cameraSprintSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            //movementNormalized = (rightMovement + upMovement).normalized;
        }
        else
        {
            rightMovement = right * _cameraMoveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
            upMovement = forward * _cameraMoveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            //movementNormalized = (rightMovement + upMovement).normalized;
        }

        if (_mainCamera != null)
        {
            _mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _cameraZoomSpeed;

            if (_mainCamera.orthographicSize > _maxZoomOutValue)
            {
                _mainCamera.orthographicSize = _maxZoomOutValue;
            }
            if (_mainCamera.orthographicSize < _maxZoomInValue)
            {
                _mainCamera.orthographicSize = _maxZoomInValue;
            }
        }
        /*So, this else state doesn't really need to be here, but in case some designer or somebody forgets to connect a camera to the serialized camera
         variable in the beginning, this will just hook onto the default main camera.*/
        else
        {
            Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _cameraZoomSpeed;
            if (Camera.main.orthographicSize > _maxZoomOutValue)
            {
                Camera.main.orthographicSize = _maxZoomOutValue;
            }
            if (Camera.main.orthographicSize < _maxZoomInValue)
            {
                Camera.main.orthographicSize = _maxZoomInValue;
            }
        }

        //This is where movement gets calculated.
        if (!_enableSlidingMovement)
        {

            transform.position += rightMovement;
            //Debug.Log(rightMovement);

            transform.position += upMovement;
            //Debug.Log(upMovement);

            //Debug.Log(movementNormalized);
            //transform.position += movementNormalized;
        }
       


    }

    //Reeee
    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Vector3 rightMovement;
        Vector3 upMovement;

        if (_enableSprintSpeed && Input.GetKey(KeyCode.LeftShift))
        {
            rightMovement = right * _cameraSprintSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
            upMovement = forward * _cameraSprintSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            //movementNormalized = (rightMovement + upMovement).normalized;
        }
        else
        {
            rightMovement = right * _cameraMoveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
            upMovement = forward * _cameraMoveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

            //movementNormalized = (rightMovement + upMovement).normalized;
        }

        if (_enableSlidingMovement)
        {
             
            if (_rigidbody != null)
            {
                _rigidbody.AddForce(rightMovement * 3f, ForceMode.Impulse);
                _rigidbody.AddForce(upMovement * 3f, ForceMode.Impulse);
            }
            
        }
    }
}
