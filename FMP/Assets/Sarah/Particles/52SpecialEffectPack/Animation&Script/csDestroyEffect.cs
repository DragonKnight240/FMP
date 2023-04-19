using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour 
{
    float TimerDisable = 0;
    public float TimeDisable = 10;
    public bool ToDestroy = false;

    private void Start()
    {
        if (ToDestroy)
        {
            Destroy(gameObject, TimeDisable); 
        }
    }

    void Update()
    {
        TimerDisable += Time.unscaledDeltaTime;

        if (TimerDisable >= TimeDisable)
        {
            TimerDisable = 0;
            gameObject.SetActive(false);
        }
    }
}
