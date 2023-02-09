using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance; 
    Camera Cam;
    public Transform FollowTarget;
    public Vector3 offSet;
    public float SmoothSpeed;
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
            if (!Interact.Instance.CombatMenu.CombatMenuObject.activeInHierarchy || !Interact.Instance.CombatMenu.AttackMenuObject.activeInHierarchy)
            {
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                RB.velocity = new Vector3((x * SmoothSpeed * 100 * Time.timeScale), 0, (z * SmoothSpeed * 100 * Time.timeScale));

                if (TileManager.Instance.Grid[TileManager.Instance.Width - 1, 0].transform.position.x < transform.position.x) //Bottom Right X
                {
                    if (RB.velocity.x > 0)
                    {
                        RB.velocity = new Vector3(0, RB.velocity.y, RB.velocity.z);
                    }
                }

                if (TileManager.Instance.Grid[0, 0].transform.position.x > transform.position.x) //Bottom left X
                {
                    if (RB.velocity.x < 0)
                    {
                        RB.velocity = new Vector3(0, RB.velocity.y, RB.velocity.z);
                    }
                }

                if (TileManager.Instance.Grid[0, 0].transform.position.z - (TileManager.Instance.Height * 3) > transform.position.z) //Bottom Left Z
                {
                    if (RB.velocity.z < 0)
                    {
                        //Down
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, 0);
                    }
                }

                if (TileManager.Instance.Grid[0, TileManager.Instance.Height - 1].transform.position.z - (TileManager.Instance.Height * 3) < transform.position.z) //Top Left Z
                {
                    if (RB.velocity.z > 0)
                    {
                        //Top
                        RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, 0);
                    }
                }
            }

        }
    
    }

    private void LateUpdate()
    {
        if(ButtonMovement)
        {
            if(Interact.Instance.CombatMenu.CombatMenuObject.activeInHierarchy || Interact.Instance.CombatMenu.AttackMenuObject.activeInHierarchy || UnitManager.Instance.EnemyMoving)
            {
                LerpTo();
            }
            return;
        }

        LerpTo();
    }

    void LerpTo()
    {
        if (FollowTarget)
        {
            Vector3 DesiredPosition = FollowTarget.position + offSet;
            Vector3 SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed * Time.deltaTime);
            
            transform.position = SmoothPosition;
        }
    }
}
