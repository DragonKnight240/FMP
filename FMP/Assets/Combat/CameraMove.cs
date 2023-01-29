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
    internal bool ShouldFollow = true;
    internal Rigidbody RB;
    public bool ButtonMovement = true;

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
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ButtonMovement)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            RB.velocity = new Vector3((x * SmoothSpeed * Time.timeScale), 0, (z * SmoothSpeed * Time.timeScale));
        }
    }

    private void LateUpdate()
    {
        if(ButtonMovement)
        {
            return;
        }

        if (FollowTarget)
        {
            Vector3 DesiredPosition = FollowTarget.position + offSet;
            Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed * Time.deltaTime);

            transform.position = SmoothPosition;
        }
    }
}
