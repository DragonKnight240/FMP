using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public GameObject ObjectToMove;
    public GameObject MoveToTarget;
    public float Speed;
    internal bool Activate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Activate)
        {
            ObjectToMove.transform.position = Vector3.MoveTowards(transform.position, MoveToTarget.transform.position, Speed * Time.deltaTime);

            if(ObjectToMove.transform.position == MoveToTarget.transform.position)
            {
                Activate = false;
            }
        }
    }
}
