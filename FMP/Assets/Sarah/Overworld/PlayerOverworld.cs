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
    AoEDisappear AoEDisappear;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        AoEDisappear = GetComponentInChildren<AoEDisappear>(true);
        AoEDisappear.gameObject.SetActive(false);

        if (GameManager.Instance)
        {
            if(GameManager.Instance.PlayerReturnToOverworld != null)
            {
                transform.position = GameManager.Instance.PlayerReturnToOverworld;
                AoEDisappear.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 Dir = new Vector3(x, 0, z).normalized;

        if(Dir.magnitude >= 0.01)
        {
            //Turns off auto kill on nearby enemies
            if(AoEDisappear)
            {
                if(AoEDisappear.gameObject.activeInHierarchy)
                {
                    AoEDisappear.gameObject.SetActive(false);
                }
            }

            //Finds turn angle
            float targetAngle = Mathf.Atan2(Dir.x, Dir.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            //Smooths angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref RotationSmooth, SmoothRate * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 MoveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            if (RB.velocity.y < 0)
            {
                RB.velocity = (new Vector3(MoveDir.x, RB.velocity.y, MoveDir.z).normalized * MoveSpeed * Time.deltaTime);
            }
            else
            {
                RB.velocity = (MoveDir.normalized * MoveSpeed * Time.deltaTime);
            }
        }

        //RB.velocity = ((transform.forward * z) * MoveSpeed) + ((transform.right * x) * MoveSpeed) + (new Vector3(0, RB.velocity.y, 0));
        //RB.velocity = new Vector3(x * MoveSpeed * Time.timeScale, RB.velocity.y, z * MoveSpeed * Time.timeScale);
    }
}
