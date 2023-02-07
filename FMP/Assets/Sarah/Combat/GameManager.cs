using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> ControlledUnits;
    internal List<GameObject> EnemyCombatStarter;
    internal Vector3 PlayerReturnToOverworld;
    public int MaxUnits = 6;
    internal int NumRecruited = 0;
    public int MaxRecruitable = 2;

    //Timer
    public bool inCombat = false;


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

        List<GameObject> EnemyCombatStarter = new List<GameObject>();
    }

    private void Update()
    {
        if(!inCombat)
        {

        }
    }
}
