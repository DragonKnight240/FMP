using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueParticles : MonoBehaviour
{
    public Dialogue Line;
    public GameObject ParticleObject;
    internal bool Activated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Line == DialogueSystem.Instance.CurrentLine && !Activated)
        {
            Activated = true;
            ParticleObject.SetActive(true);
        }
    }
}
