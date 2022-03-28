using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility.ReadOnly;


public enum CameraMode
{
    KEYBOARD,
    CLICKDRAG,
    MOUSEBORDER
}

public class IsometricCameraController : MonoBehaviour
{
    /*This is probably the biggest class out of all the classes. This is just the camera movement controller for an isometric view.*/

    [Header("Camera Connections")]
    [SerializeField] Camera _mainCamera = null;
    [SerializeField] Rigidbody _rigidbody = null;

    [Header("General Movement Settings")]
    public CameraMode _cameraMode = CameraMode.CLICKDRAG;

    [Header("Traditional Camera Movement Settings")]
    [SerializeField] public float _cameraMoveSpeed = 10f;
    public bool _interacting = false;
    bool _clicked = false;

    [Header("Click And Drag Movement Settings")]
    [SerializeField] public float _panningSpeed = 25f;
    [SerializeField] private float _exposedField = 10f;
    [SerializeField] private LayerMask _groundLayer = 0;
    [SerializeField, Range(0, 1)] private float _clickDragSmooth = 0.5f;
    private Vector3 _dragStart;
    [ReadOnly] public bool _dragging;

    [Header("Mouse Motivated Movement Settings (League of Legends)")]
    [SerializeField] public float _mMPanningSpeed = 25f;
    [SerializeField] public float _panBorderThickness = 50f;

    [Header("Rigidbody/Sliding Camera Movement Settings")]
    [SerializeField] public bool _enableSlidingMovement = false;

    [Header("Camera Zoom Settings")]
    [SerializeField] public bool _cameraZoomEnabled = false;
    [SerializeField] public float _cameraZoomSpeed = 5f;
    [SerializeField] public float _maxZoomInValue = 0.7f;
    [SerializeField] public float _maxZoomOutValue = 6.37f;

    [Header("Camera Sprint")]
    [SerializeField] public bool _enableSprintSpeed = true;
    [SerializeField] public float _cameraSprintSpeed = 20f;
    [SerializeField] public float _mouseMotivatedSprintSpeed = 35f;

    [Header("Camera Bounds")]
    [SerializeField] float _maxXValue = 50f;
    [SerializeField] float _minXValue = -50f;
    [SerializeField] float _maxZValue = 50f;
    [SerializeField] float _minZValue = -50f;

    //Traditional Movement Values
    private Vector3 forward, right;
    private float _elapsedTime = 0f;

    //Centering on Object Values
    private Vector3 _finalLerpPosition;
    private float _movementTime = 3f;

    //Click and Drag Values
    private Vector3 _origin;
    private Vector3 _difference;
    private Vector3 _resetCamera;

    private Vector2 MouseAxis
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }

    // Checks if mouse is over UI
    public static bool IsMouseOverUi
    {
        get
        {
            var events = EventSystem.current;
            return events != null && events.IsPointerOverGameObject();
        }
    }

    private bool drag;


    #region Singleton Pattern
    public static IsometricCameraController Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton == null) { Singleton = this; }
        else { Destroy(gameObject); }
    }
    #endregion

    private void Start()
    {
        if (_mainCamera != null)
        {
            forward = _mainCamera.transform.forward;
            right = _mainCamera.transform.right;
            _resetCamera = _mainCamera.transform.position;
        }
        else
        {
            forward = Camera.main.transform.forward;
            right = Camera.main.transform.right;
            _resetCamera = transform.position;
        }


        forward.y = 0f;
        forward = Vector3.Normalize(forward);
        right = Vector3.Normalize(right);



    }

    private void OnEnable()
    {
        ModalWindowController.OnInteractStart += InteractStarted;
        ModalWindowController.OnInteractEnd += InteractEnded;
        
    }

    private void OnDisable()
    {
        ModalWindowController.OnInteractStart -= InteractStarted;
        ModalWindowController.OnInteractEnd -= InteractEnded;
    }

    void InteractStarted()
    {
        _interacting = true;
    }

    void InteractEnded()
    {
        _interacting = false;
        _clicked = false;
        _elapsedTime = 0f;
        _finalLerpPosition = transform.position;
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

        if (_mainCamera != null && _cameraZoomEnabled)
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
        else if (_mainCamera == null && _cameraZoomEnabled)
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


    public void MoveToPosition(Vector3 finalPosition, float movementTime)
    {
        #region Making sure the Camera doesn't go out of bounds
        if (finalPosition.x > _maxXValue)
        {
            finalPosition = new Vector3(_maxXValue, transform.position.y, transform.position.z);
        }
        if (finalPosition.z > _maxZValue)
        {
            finalPosition = new Vector3(transform.position.x, transform.position.y, _maxZValue);
        }
        if (finalPosition.x < _minXValue)
        {
            finalPosition = new Vector3(_minXValue, transform.position.y, transform.position.z);
        }
        if (finalPosition.z < _minZValue)
        {
            finalPosition = new Vector3(transform.position.x, transform.position.y, _minZValue);
        }
        #endregion

        _finalLerpPosition = new Vector3(finalPosition.x, 0f, finalPosition.z);
        //_movementTime = movementTime;+
        _movementTime = 3f;

    }

    //public IEnumerator MoveToPosition(Vector3 finalPosition, float movementTime)
    //{
    //    if (finalPosition.x > _maxXValue)
    //    {
    //        finalPosition = new Vector3(_maxXValue, transform.position.y, transform.position.z);
    //    }
    //    if (finalPosition.z > _maxZValue)
    //    {
    //        finalPosition = new Vector3(transform.position.x, transform.position.y, _maxZValue);
    //    }
    //    if (finalPosition.x < _minXValue)
    //    {
    //        finalPosition = new Vector3(_minXValue, transform.position.y, transform.position.z);
    //    }
    //    if (finalPosition.z < _minZValue)
    //    {
    //        finalPosition = new Vector3(transform.position.x, transform.position.y, _minZValue);
    //    }

    //    //_finalLerpPosition = finalPosition;
    //    //_movementTime = movementTime;

    //    _elapsedTime += Time.deltaTime;
    //    var end = Time.time + movementTime;
    //    float _movementPercentage = _elapsedTime / _movementTime;
    //    while (Time.time < end)
    //    {
    //        transform.position = Vector3.Lerp(transform.position, finalPosition, _movementPercentage);
    //        yield return null;
    //    }

    //}

    void CameraBounds()
    {
        if (transform.position.x > _maxXValue)
        {
            transform.position = new Vector3(_maxXValue, transform.position.y, transform.position.z);
        }
        if (transform.position.z > _maxZValue)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _maxZValue);
        }
        if (transform.position.x < _minXValue)
        {
            transform.position = new Vector3(_minXValue, transform.position.y, transform.position.z);
        }
        if (transform.position.z < _minZValue)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _minZValue);
        }
    }

    //Reeee
    private void Update()
    {

        if (_interacting && !_clicked)
        {

            _elapsedTime += Time.deltaTime;
            float _movementPercentage = _elapsedTime / _movementTime;
            transform.position = Vector3.Lerp(transform.position, _finalLerpPosition, _movementPercentage);

            if (transform.position == _finalLerpPosition)
            {
                _elapsedTime = 0f;
                _clicked = true;

            }
        }



        if (!_interacting)
        {
            if (_cameraMode == CameraMode.KEYBOARD) { HandleInput(); }

            #region Mouse Motivated Movement
            if (_cameraMode == CameraMode.MOUSEBORDER)
            {
                Vector3 upMovement = new Vector3();
                Vector3 rightMovement = new Vector3();
                if (!_enableSprintSpeed)
                {
                    if (Input.mousePosition.y >= Screen.height - _panBorderThickness)
                    {
                        upMovement = forward * _mMPanningSpeed * Time.deltaTime;
                    }
                    else if (Input.mousePosition.y <= _panBorderThickness)
                    {
                        upMovement = -forward * _mMPanningSpeed * Time.deltaTime;
                    }
                    else if (Input.mousePosition.x >= Screen.width - _panBorderThickness)
                    {
                        rightMovement = right * _mMPanningSpeed * Time.deltaTime;
                    }
                    else if (Input.mousePosition.x <= _panBorderThickness)
                    {
                        rightMovement = -right * _mMPanningSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    if (Input.mousePosition.y >= Screen.height - _panBorderThickness)
                    {
                        upMovement = forward * _mouseMotivatedSprintSpeed * Time.deltaTime;
                    }
                    else if (Input.mousePosition.y <= _panBorderThickness)
                    {
                        upMovement = -forward * _mouseMotivatedSprintSpeed * Time.deltaTime;
                    }
                    else if (Input.mousePosition.x >= Screen.width - _panBorderThickness)
                    {
                        rightMovement = right * _mouseMotivatedSprintSpeed * Time.deltaTime;
                    }
                    else if (Input.mousePosition.x <= _panBorderThickness)
                    {
                        rightMovement = -right * _mouseMotivatedSprintSpeed * Time.deltaTime;
                    }
                }

                transform.position += upMovement;
                transform.position += rightMovement;
            }
            #endregion



            #region Click and Drag Movement

            //if (!_interacting)
            //{
            //    if (_clickDragMovementEnabled && Input.GetMouseButton(0) && MouseAxis != Vector2.zero)
            //    {
            //        Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);

            //        desiredMove *= _panningSpeed;
            //        desiredMove *= Time.deltaTime;
            //        desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            //        desiredMove = transform.InverseTransformDirection(desiredMove);

            //        transform.Translate(desiredMove, Space.Self);
            //    }
            //}

            #endregion

            #region Click and Drag Maybe Best

            //if (Input.GetMouseButton(0))
            //{
            //    Vector3 newPosition = new Vector3();
            //    newPosition.x = Input.GetAxis("Mouse X") * _panningSpeed * Time.deltaTime;
            //    newPosition.z = Input.GetAxis("Mouse Y") * _panningSpeed * Time.deltaTime;
            //    transform.Translate(-newPosition);
            //}

            #endregion


            #region Click and Drag but Sad

            if (_cameraMode == CameraMode.CLICKDRAG)
            {
                if (Input.GetMouseButtonDown(0) && !IsMouseOverUi)
                {
                    Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit, _groundLayer))
                    {
                        _dragging = true;
                        _dragStart = hit.point;
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _dragging = false;
                }
                else if (Input.GetMouseButton(0) && _dragging)
                {
                    Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit, _groundLayer))
                    {
                        Vector3 diff = _dragStart - Vector3.Lerp(_dragStart, hit.point, _clickDragSmooth);
                        diff.y = 0;
                        transform.position += diff;
                    }
                }

                CameraBounds();
            }

            #endregion
        }

        //HandleInput();

        //CameraBounds();


    }



    private void LateUpdate()
    {
        #region Click and Drag But Bad
        /*
        if (Input.GetMouseButton(0))
        {
            _difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - transform.position;
            
            if (drag == false)
            {
                drag = true;
                _origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            transform.position = new Vector3(_origin.x - _difference.x, _origin.y - _difference.y, _origin.z - _difference.z);

            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minXValue, _maxXValue), transform.position.y, Mathf.Clamp(transform.position.z, _minZValue, _maxZValue));
        }
        */
        #endregion

        #region Click and Drag Maybe Better

        //if (Input.GetMouseButtonDown(0))
        //{
        //    _origin = Input.mousePosition;
        //    return;
        //}

        ////if (!Input.GetMouseButtonDown(0)) return;

        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _origin);
        //Vector3 move = new Vector3(pos.x * _panningSpeed, 0f, pos.y * _panningSpeed);

        //transform.Translate(move, Space.World);

        #endregion


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
