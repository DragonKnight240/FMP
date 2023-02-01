using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Player;
    GameObject Cam;
    float x = 0;
    float y = 0;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerOverworld>().gameObject;
        Cam = FindObjectOfType<Camera>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        x += Input.GetAxis("Mouse X");
        y += Input.GetAxis("Mouse Y");

        transform.localRotation = Quaternion.Euler(0, x, 0);
        transform.localRotation = Quaternion.Euler(-y, 0, 0);

        //transform.LookAt(Player.transform);

    }
}
