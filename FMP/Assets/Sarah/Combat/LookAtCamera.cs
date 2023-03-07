using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera Cam;
    public bool Reverse = false;

    // Start is called before the first frame update
    void Start()
    {
        Cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.LookAt(Cam.transform);

        if(Reverse)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
