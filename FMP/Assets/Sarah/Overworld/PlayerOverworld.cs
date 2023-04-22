using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
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
    internal bool CanMove;
    Animator Anim;
    internal bool isOnGround = true;

    public CinemachineFreeLook FreelookCam;
    public PostProcessVolume PPVolume;

    public GameObject HealPartical;

    public AudioClip EnterBattleSound;

    private void Start()
    {
        RB = GetComponent<Rigidbody>();

        if (!FindObjectOfType<MainMenu>().isActiveAndEnabled)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        AoEDisappear = GetComponentInChildren<AoEDisappear>(true);
        AoEDisappear.gameObject.SetActive(false);

        if (GameManager.Instance)
        {
            if(GameManager.Instance.PlayerReturnToOverworld != null && GameManager.Instance.StartedGame)
            {
                transform.position = GameManager.Instance.PlayerReturnToOverworld;
                AoEDisappear.gameObject.SetActive(true);
            }
        }

        RB.velocity = Vector3.zero;

        Anim = GetComponentInChildren<Animator>(true);
    }

    private void Update()
    {
        if (!CanMove)
        {
            Anim.ResetTrigger("Fall");
            Anim.ResetTrigger("Move");
            Anim.SetTrigger("Idle");
            RB.velocity = Vector3.zero;
            return;
        }

        if (Time.timeScale == 0)
        {
            Anim.ResetTrigger("Fall");
            Anim.ResetTrigger("Move");
            Anim.SetTrigger("Idle");
            RB.velocity = Vector3.zero;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!CanMove)
        {
            return;
        }

        if (Time.timeScale == 0)
        {
            return;
        }

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

            RB.velocity = (MoveDir.normalized * MoveSpeed * Time.timeScale);
            RB.velocity = new Vector3(RB.velocity.x, TempVelocity, RB.velocity.z);

            if (RB.velocity.y > YMax)
            {
                RB.velocity = new Vector3(RB.velocity.x, 0, RB.velocity.z);
            }

            Vector3 FlatVel = new Vector3(RB.velocity.x, 0f, RB.velocity.z);

            if(FlatVel.magnitude > MoveSpeed)
            {
                Vector3 LimitedVel = FlatVel.normalized * MoveSpeed * Time.timeScale;
                RB.velocity = new Vector3(LimitedVel.x, RB.velocity.y, LimitedVel.z);
            }

            if (RB.velocity.y < 0 && !isOnGround)
            {
                Anim.ResetTrigger("Move");
                Anim.ResetTrigger("Idle");
                Anim.SetTrigger("Fall");
            }
            else
            {
                Anim.ResetTrigger("Fall");
                Anim.ResetTrigger("Idle");
                Anim.SetTrigger("Move");
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

            if (isOnGround)
            {
                Anim.ResetTrigger("Fall");
                Anim.ResetTrigger("Move");
                Anim.SetTrigger("Idle");
            }
            else
            {
                Anim.ResetTrigger("Move");
                Anim.ResetTrigger("Idle");
                Anim.SetTrigger("Fall");
            }
        }
    }

    private void LateUpdate()
    {
        
    }

    private void OnDestroy()
    {
        //print("Destroy");
        PPVolume.weight = 0;
    }
}
