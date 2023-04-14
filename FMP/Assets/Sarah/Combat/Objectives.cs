using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Zone
{
    Dungeon1,
    Dungeon2,
    Village,
    ScarletWoods,
    Desert,
    OutsideVillage
}

public class Objectives : MonoBehaviour
{
    public bool ShowObjectiveOverworld = true;
    //public GameObject PromptPrefab;
    public Sprite Icon;
    internal Image image;
    internal Vector2 position;
    public PlayAfter GameManagerCheck;
    internal GameObject ObjectMarkerOverworld;
    internal bool InRange = false;

    //Other Objectives
    public Zone ZoneObjective;

    // Start is called before the first frame update
    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.z);
    }
}
