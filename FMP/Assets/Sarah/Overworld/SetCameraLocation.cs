using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetCameraLocation : MonoBehaviour
{
    public GameObject CameraLocation;
    public PlayAfter ActiveOnReturn;
    internal GameObject MainCamera;
    internal Camera Cam;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = FindObjectOfType<PlayerOverworld>().transform.parent.GetComponentInChildren<CinemachineFreeLook>(true).gameObject;

        Cam = FindObjectOfType<Camera>();

        if (Cam.name != "Main Camera")
        {
            Cam = FindObjectOfType<CinemachineBrain>().GetComponent<Camera>();
        }

        switch (ActiveOnReturn)
        {
            case PlayAfter.PostTutorial:
                {
                    if(GameManager.Instance.DialogueToPlay == PlayAfter.PostTutorial)
                    {
                        print("Post tutorial");
                        Time.timeScale = 1;
                        CameraLocation.SetActive(true);
                        MainCamera.SetActive(false);
                    }
                    break;
                }
            case PlayAfter.ArcherRecruit:
                {
                    if (GameManager.Instance.DialogueToPlay == PlayAfter.ArcherRecruit)
                    {
                        Time.timeScale = 1;
                        CameraLocation.SetActive(true);
                        MainCamera.SetActive(false);
                    }
                    break;
                }
            case PlayAfter.GauntletRecruit:
                {
                    if (GameManager.Instance.DialogueToPlay == PlayAfter.GauntletRecruit)
                    {
                        Time.timeScale = 1;
                        CameraLocation.SetActive(true);
                        MainCamera.SetActive(false);
                    }
                    break;
                }
            case PlayAfter.PostDungeon1:
                {
                    if (GameManager.Instance.DialogueToPlay == PlayAfter.PostDungeon1)
                    {
                        Time.timeScale = 1;
                        CameraLocation.SetActive(true);
                        MainCamera.SetActive(false);
                    }
                    break;
                }
            case PlayAfter.PostDungeon2:
                {
                    if (GameManager.Instance.DialogueToPlay == PlayAfter.PostDungeon2)
                    {
                        Time.timeScale = 1;
                        CameraLocation.SetActive(true);
                        MainCamera.SetActive(false);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(MainCamera.activeInHierarchy);
        print(!CameraLocation.activeInHierarchy);

        if(MainCamera.activeInHierarchy && !CameraLocation.activeInHierarchy)
        {
            //print("Destroy");
            Destroy(this);
        }
        else
        {
            if(Cam.transform.position.ToString() == CameraLocation.transform.position.ToString())
            {
                Time.timeScale = 0;

                if (!DialogueSystem.Instance.PlayingDialogue)
                {
                    Time.timeScale = 1;
                    //print("Reactive");
                    CameraLocation.SetActive(false);
                    MainCamera.SetActive(true);
                }
            }
        }
    }
}
