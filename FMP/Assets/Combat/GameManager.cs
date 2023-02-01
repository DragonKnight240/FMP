using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> ControlledUnits;
    internal GameObject EnemyCombatStarter;
    internal Vector3 PlayerReturnToOverworld;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        ControlledUnits = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void LoadingCombat()
    {
        foreach (GameObject Unit in ControlledUnits)
        {

        }
    }
}
