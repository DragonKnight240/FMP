using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworld : MonoBehaviour
{
    internal Rigidbody RB;
    public float MoveSpeed;
    CameraController CamControl;

    private void Start()
    {
        CamControl = FindObjectOfType<CameraController>();
        RB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //RB.velocity = new Vector3(x * MoveSpeed * Time.timeScale, RB.velocity.y, z * MoveSpeed * Time.timeScale);

        RB.velocity = ((transform.forward * x) * MoveSpeed) + ((transform.right * z) * MoveSpeed) + (new Vector3(0, RB.velocity.y, 0));
    }
}
