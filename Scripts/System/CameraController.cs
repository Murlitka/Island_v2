using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed, movementTime, zoomingSpeed;

    [SerializeField, Header("Limits for zoom")]
    private float highLimit;
    [SerializeField]
    private float lowLimit;


    private Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Camera_Move();
    }


    void Camera_Move()
    {
        //X movement with dependence from camera position.y
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            newPosition.x += (movementSpeed * transform.position.y/lowLimit);
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            newPosition.x -= (movementSpeed * transform.position.y / lowLimit);

        //Y movement with dependence from camera position.y
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                newPosition.z += (movementSpeed * transform.position.y / lowLimit);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            newPosition.z -= (movementSpeed * transform.position.y / lowLimit);

        //Zoom
        if (Input.mouseScrollDelta.y < 0)
        {
            //High limit
            if (transform.position.y < highLimit)
                newPosition.y += zoomingSpeed;
        }
        else if (Input.mouseScrollDelta.y > 0)
        {
            //Low limit
            if (transform.position.y > lowLimit)
                newPosition.y -= zoomingSpeed;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
    }
}
