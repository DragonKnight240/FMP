using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    public string ObjectiveName;
    //public GameObject PromptPrefab;
    public Sprite Icon;
    internal Image image;
    internal Vector2 position;
    public PlayAfter GameManagerCheck;
    internal GameObject ObjectMarkerOverworld;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.z);
    }
}
