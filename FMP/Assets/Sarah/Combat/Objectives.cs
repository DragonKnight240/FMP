using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    public string ObjectiveName;
    public int ObjectiveNum;
    public Sprite Icon;
    public Image image;
    internal Vector2 position;
    public PlayAfter GameManagerCheck;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.z);
    }
}
