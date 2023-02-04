using UnityEngine.Events;
using UnityEngine;
using TMPro;


public class NpcCanvas : MonoBehaviour
{
    TMP_Text text;

    Canvas canvas;
    public Camera PlayerCam;

    private void Awake()
    {
        if(text == null)
            text = GetComponentInChildren<TMP_Text>();
        if(canvas == null)
            canvas = GetComponent<Canvas>();
        if (PlayerCam == null)
            PlayerCam = Camera.main;
     
    }

    private void Start()
    {
        var parent = GetComponentInParent<NpcBase>();
        if (parent == null)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        canvas.transform.LookAt(PlayerCam.transform.position);
    }
}
