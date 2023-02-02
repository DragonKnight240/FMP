using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverworld : MonoBehaviour
{
    internal Rigidbody RB;
    public float MoveSpeed;
    public Transform Cam;
    public float SmoothRate;
    float RotationSmooth;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 Dir = new Vector3(x, 0, z).normalized;

        if(Dir.magnitude >= 0.01)
        {
            float targetAngle = Mathf.Atan2(Dir.x, Dir.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationSmooth, SmoothRate * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 MoveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            RB.velocity = (MoveDir.normalized * MoveSpeed * Time.deltaTime);
        }

        //RB.velocity = ((transform.forward * z) * MoveSpeed) + ((transform.right * x) * MoveSpeed) + (new Vector3(0, RB.velocity.y, 0));
        //RB.velocity = new Vector3(x * MoveSpeed * Time.timeScale, RB.velocity.y, z * MoveSpeed * Time.timeScale);
    }
}
