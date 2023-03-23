using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LookAtCamera : MonoBehaviour
{
    Camera Cam;
    public bool Reverse = false;
    public Vector3 StartOffset;
    internal Vector3 StartRotation;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(StartOffset);
        StartRotation = transform.rotation.eulerAngles;

        Cam = FindObjectOfType<Camera>();

        if(Cam.name != "Main Camera")
        {
            Cam = FindObjectOfType<CinemachineBrain>().GetComponent<Camera>();
        }
    }

    private void LateUpdate()
    {
        transform.LookAt(Cam.transform);

        transform.Rotate(StartRotation.x, StartRotation.y, StartRotation.z);

        if (Reverse)
        {
            transform.Rotate(0, 180, 0);
        }
    }

}
