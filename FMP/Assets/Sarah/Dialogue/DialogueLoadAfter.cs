using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLoadAfter : MonoBehaviour
{
    public Dialogue Line;
    public string SceneToLoad;
    bool Pending = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DialogueSystem.Instance.CurrentLine == Line && !Pending)
        {
            Pending = true;
        }

        if(Pending && !DialogueSystem.Instance.PlayingDialogue)
        {
            GameManager.Instance.PlayerReturnToOverworld = FindObjectOfType<PlayerOverworld>().transform.position;
            SceneLoader.Instance.LoadNewScene(SceneToLoad);
        }
    }
}
