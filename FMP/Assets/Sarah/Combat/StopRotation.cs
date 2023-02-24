using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour
{
    internal Quaternion LastParentRotation;

    // Start is called before the first frame update
    void Start()
    {
        LastParentRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) * LastParentRotation * transform.localRotation;

        LastParentRotation = transform.parent.localRotation;
    }
}
