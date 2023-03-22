using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public bool ObjectHover = false; 
    public GameObject PivotPoint;
    public float Speed;
    internal float OGY;

    private void Start()
    {
        if(ObjectHover)
        {
            OGY = transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectHover)
        {
            transform.position = new Vector3(transform.position.x, OGY + Mathf.Sin(Time.time) * Speed, transform.position.z);
        }
        else
        {
            transform.RotateAround(PivotPoint.transform.position, new Vector3(0, 1, 0), Speed * Time.deltaTime);
        }
    }
}
