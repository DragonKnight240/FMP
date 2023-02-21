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

        if(Display)
        {
            transform.position = Vector3.MoveTowards(transform.position, InSightLocation, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, OutSightLocation, Speed * Time.deltaTime);
        }
    }
}
