using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Player;
    public float RotationSpeed = 2;
    public float SmoothSpeed;
    public Vector3 offSet;
    float x = 0;
    float y = 0;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerOverworld>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ViewDir = transform.position - new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z);

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        Vector3 InputDir = transform.forward * x + transform.right * y;

        if(InputDir != Vector3.zero)
        {
            
            Player.transform.forward = Vector3.Slerp(Player.transform.forward, InputDir.normalized, Time.deltaTime * RotationSpeed);
        }
    }
}
