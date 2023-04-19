using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour 
{
    float TimerDisable = 0;
    public float TimeDisable = 10;

    private void Start()
    {
        //Destroy(this.gameObject, 10); 
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
