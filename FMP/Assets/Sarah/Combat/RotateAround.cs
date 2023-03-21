using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject PivotPoint;
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(PivotPoint.transform.position, new Vector3(0, 1, 0), Speed * Time.deltaTime);
    }
}
