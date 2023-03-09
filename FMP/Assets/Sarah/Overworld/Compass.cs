using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage CompassImage;
    public Transform Player;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360f, 0f, 1f, 1f);
        
    }
}
