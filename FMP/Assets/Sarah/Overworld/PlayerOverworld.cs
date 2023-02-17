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
    internal AoEDisappear AoEDisappear;
    public float YMax = 2f;

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

        RB.velocity = Vector3.zero;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 Dir = new Vector3(x, 0, z).normalized;

        if(Dir.magnitude >= 0.1)
        {
            float TempVelocity = RB.velocity.y;
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

            RB.velocity = (MoveDir.normalized * MoveSpeed * Time.deltaTime);
            RB.velocity = new Vector3(RB.velocity.x, TempVelocity, RB.velocity.z);

            if (RB.velocity.y > YMax)
            {
                RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            }
        }
        else
        {
            if (RB.velocity.y < YMax)
            {
                RB.velocity = new Vector3(0, RB.velocity.y, 0);
            }
            else
            {
                RB.velocity = new Vector3(0, 0, 0);
            }
        }
    }
}
