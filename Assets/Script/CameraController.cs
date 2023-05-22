using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform followUnit;

    public Transform cameraTransform;
    public float movementSpeed;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;
    public Vector3 mouseZoomAmount;

    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    public Vector3 maxCamClamp;
    public Vector3 minCamClamp;
    public Vector3 maxZoomClamp;
    public Vector3 minZoomClamp;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    private void LateUpdate()
    {
        //HandleMovementInput();
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // HANDLE MOUSE ZOOM
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * mouseZoomAmount;
        }

        if (followUnit != null)
        {
            newPosition = followUnit.position;
        } else {
            // HANDLE MOUSE DRAG MOVEMENT
            if (Input.GetMouseButtonDown(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(1))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
        }
        

        // HANDLE MOUSE DRAG ROTATION
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;
            

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }

        // APPLY TO CAMERA
        newPosition = new Vector3(Mathf.Clamp(newPosition.x, minCamClamp.x, maxCamClamp.x), Mathf.Clamp(newPosition.y, minCamClamp.y, maxCamClamp.y), Mathf.Clamp(newPosition.z, minCamClamp.z, maxCamClamp.z));
        newZoom.y = Mathf.Clamp(newZoom.y, minZoomClamp.y, maxZoomClamp.y);
        newZoom.z = Mathf.Clamp(newZoom.z, minZoomClamp.z, maxZoomClamp.z);


        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime); ;

    }

    void HandleMovementInput()
    {
        if (followUnit != null)
        {
            newPosition = followUnit.position;
        } else
        {
            // HANDLE FORWARDS AND BACKWARDS CAMERA MOVEMENT
            if (Input.GetKey(KeyCode.W))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                newPosition += (transform.forward * -movementSpeed);
            }


            // HANDLE RIGHT AND LEFT CAMERA MOVEMENT
            if (Input.GetKey(KeyCode.D))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                newPosition += (transform.right * -movementSpeed);
            }
        }
        


        // HANDLE CAMERA ROTATE AROUND MOVEMENT
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }


        // HANDLE CAMERA ZOOM IN N OUT
        if (Input.GetKey(KeyCode.R))
        {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmount;
        }

        // APPLY TO CAMERA
        newPosition = new Vector3(Mathf.Clamp(newPosition.x, -16, 16), 2f, Mathf.Clamp(newPosition.z, -17, 26));
        newZoom.y = Mathf.Clamp(newZoom.y, 10, 32);
        newZoom.z = Mathf.Clamp(newZoom.z, -32, -10);


        

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime); ;

    }
}
