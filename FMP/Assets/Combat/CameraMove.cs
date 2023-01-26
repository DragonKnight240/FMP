using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance; 
    Camera Cam;
    internal Transform FollowTarget;
    public Vector3 offSet;
    public float SmoothSpeed;
    public float CloseEnough = 2;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        Cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (FollowTarget)
        {
                Vector3 DesiredPosition = FollowTarget.position + offSet;
                Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed);

                transform.position = SmoothPosition;
        }
    }
}
