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
    public float GridSmoothSpeed = 1;
    internal bool ShouldFollow = true;
    internal Rigidbody RB;
    public bool ButtonMovement = true;
    internal float BaseSmoothSpeed;
    internal bool Override = false;
    public float ZoomOutSpeed;
    public float MaxZoomOut = 95;
    internal float MinZoomIn;

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

        MinZoomIn = offSet.y;
        offSet.y = CameraMove.Instance.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Interact.Instance.CombatMenu.LevelScreen.activeInHierarchy || Interact.Instance.CombatMenu.AttackScreen.activeInHierarchy 
            || Interact.Instance.CombatMenu.ItemNotification.activeInHierarchy || Interact.Instance.CombatMenu.EXPBar.isActiveAndEnabled 
            || Interact.Instance.CombatMenu.ClassEXPBar.isActiveAndEnabled)
        {
            return;
        }

        if (ButtonMovement)
        {
            if ((!Interact.Instance.CombatMenu.CombatMenuObject.activeInHierarchy || !Interact.Instance.CombatMenu.AttackMenuObject.activeInHierarchy) /*&& FollowTarget == null*/)
            {
                if(!BoundaryCameraMove())
                {
                    float x = Input.GetAxisRaw("Horizontal");
                    float z = Input.GetAxisRaw("Vertical");

                    RB.velocity = new Vector3((x * SmoothSpeed * 100 * Time.timeScale), RB.velocity.y, (z * SmoothSpeed * 100 * Time.timeScale));

                    if(RB.velocity != Vector3.zero)
                    {
                        GameManager.Instance.ToolTipCheck(Tutorial.CMoveCamera);
                    }
                }

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
        if (Interact.Instance.CombatMenu.LevelScreen.activeInHierarchy || Interact.Instance.CombatMenu.AttackScreen.activeInHierarchy
            || Interact.Instance.CombatMenu.ItemNotification.activeInHierarchy || Interact.Instance.CombatMenu.EXPBar.isActiveAndEnabled
            || Interact.Instance.CombatMenu.ClassEXPBar.isActiveAndEnabled)
        {
            return;
        }

        if (ButtonMovement)
        {
            if(Interact.Instance.CombatMenu.CombatMenuObject.activeInHierarchy || Interact.Instance.CombatMenu.AttackMenuObject.activeInHierarchy || UnitManager.Instance.EnemyMoving || FollowTarget)
            {
                LerpTo();
            }
            Zoom();
            return;
        }

        LerpTo();
    }

    bool BoundaryCameraMove()
    {
        Vector3 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Check if the mouse cursor is at the edge of the screen
        if (mousePosition.x >= screenWidth - 5)
        {
            // Move the camera to the right
            RB.velocity = new Vector3(SmoothSpeed * 100 * Time.timeScale, RB.velocity.y, RB.velocity.z);
            GameManager.Instance.ToolTipCheck(Tutorial.CMoveCamera);
            return true;
        }
        else if (mousePosition.x <= 5)
        {
            // Move the camera to the left
            RB.velocity = new Vector3(-SmoothSpeed * 100 * Time.timeScale, RB.velocity.y, RB.velocity.z);
            GameManager.Instance.ToolTipCheck(Tutorial.CMoveCamera);
            return true;
        }

        if (mousePosition.y >= screenHeight - 5)
        {
            // Move the camera up
            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, SmoothSpeed * 100 * Time.timeScale);
            GameManager.Instance.ToolTipCheck(Tutorial.CMoveCamera);
            return true;
        }
        else if (mousePosition.y <= 5)
        {
            // Move the camera down
            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y, -SmoothSpeed * 100 * Time.timeScale);
            GameManager.Instance.ToolTipCheck(Tutorial.CMoveCamera);
            return true;
        }

        return false;
    }

    void LerpTo()
    {
        if (FollowTarget)
        {
            Vector3 DesiredPosition = FollowTarget.position + offSet + (new Vector3(offSet.x,0,offSet.z));
            Vector3 SmoothPosition;

            if (!FollowTarget.GetComponent<InteractOnGrid>())
            {
                SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, SmoothSpeed * Time.deltaTime);
            }
            else
            {
                SmoothPosition = Vector3.Lerp(transform.position, DesiredPosition, GridSmoothSpeed * Time.deltaTime);
            }

            transform.position = SmoothPosition;
        }
    }

    void Zoom()
    {
        float Scroll = Input.GetAxis("Mouse ScrollWheel") * ZoomOutSpeed;

        offSet.y -= Scroll;

        if (offSet.y > MaxZoomOut)
        {
            offSet.y = MaxZoomOut;
        }
        else if(offSet.y < MinZoomIn)
        {
            offSet.y = MinZoomIn;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, offSet.y, transform.position.z), ZoomOutSpeed * Time.deltaTime);
    }
}
