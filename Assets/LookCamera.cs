using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public float Speed = 1;
    public float ZoomSpeed = 1;
    public float PanSpeed = 1;

    private Camera ChildCamera;

    // Start is called before the first frame update
    void Start()
    {
        ChildCamera = GetComponentInChildren<Camera>();
    }


    float Down(KeyCode code)
    {
        return Input.GetKey(code) ? 1 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation *= Quaternion.AngleAxis(, Vector3.right);
       // transform.rotation *= Quaternion.AngleAxis(Speed, Vector3.up);

        transform.Rotate(0,Speed,0, Space.World);

        transform.Rotate( PanSpeed * (Down(KeyCode.W) - Down(KeyCode.S)),0,0, Space.Self);

        ChildCamera.transform.position += ChildCamera.transform.forward * Input.mouseScrollDelta.y * ZoomSpeed;
    }

    private void OnDrawGizmos()
    {
        if (ChildCamera == null)
            ChildCamera = GetComponentInChildren<Camera>();

        if (ChildCamera != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, ChildCamera.transform.position);
        }
    }


}
