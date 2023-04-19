using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePlaySound : MonoBehaviour
{
    public Dialogue Line;
    public AudioClip SoundToPlay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Line == DialogueSystem.Instance.CurrentLine)
        {
            SoundManager.Instance.PlaySFX(SoundToPlay);
            Destroy(this);
        }
    }
}
