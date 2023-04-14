using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToScreenLocation : MonoBehaviour
{
    internal Vector3 InSightLocation;
    internal Vector3 OutSightLocation;
    public GameObject InSightObject;
    public GameObject OutSightObject;
    internal bool Display = false;
    public float Speed = 3;
    public float DivideShowHeight = 5.0f;
    public bool Override = false;
    public float OverrideTimeMax = 5;
    internal float OverrideTime = 0;
    internal float OverrideTimer = 0;

    internal bool Both = false;
    internal float BothDelay = 1f;
    internal float BothDelayTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (InSightObject == null)
        {
            InSightLocation = transform.position;
        }
        else
        {
            InSightLocation = InSightObject.transform.position;
        }

        if(OutSightObject == null)
        {
            OutSightLocation = transform.position;
        }
        else
        {
            OutSightLocation = OutSightObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.Instance)
        {
            if (SceneLoader.Instance.LoadingScreen.GetComponent<CanvasGroup>().alpha > 0)
            {
                if (UnitManager.Instance)
                {
                    if (!UnitManager.Instance.SetupFinished)
                    {
                        return;
                    }
                }

            }
        }

        if (!Override)
        {
            if (Input.mousePosition.y > Screen.height / DivideShowHeight)
            {
                Display = true;
            }
            else
            {
                Display = false;
            }
        }
        else if(OverrideTime != 0)
        {
            OverrideTimer += Time.deltaTime;

            if(OverrideTimer >= OverrideTime)
            {
                OverrideTimer = 0;
                OverrideTime = 0;
                Override = false;
            }
        }

        if(Display)
        {
            transform.position = Vector3.MoveTowards(transform.position, InSightLocation, Speed );
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, OutSightLocation, Speed );
        }

        if (Both)
        {
            if (transform.position.ToString() == OutSightLocation.ToString())
            {
                BothDelayTimer += Time.unscaledDeltaTime;

                if (BothDelayTimer >= BothDelay)
                {
                    BothDelayTimer = 0;
                    Both = false;
                    Display = true;
                }
            }

            if (transform.position.ToString() == InSightLocation.ToString())
            {
                BothDelayTimer += Time.unscaledDeltaTime;

                if (BothDelayTimer >= BothDelay)
                {
                    BothDelayTimer = 0;
                    Both = false;
                    Display = false;
                }
            }
        }
    }
}
