using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    internal bool isPlayerTurn = true;
    internal UnityEvent TurnChange;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (TurnChange == null)
        {
            TurnChange = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerTurn)
        {
            foreach(GameObject Unit in UnitManager.Instance.AllyUnits)
            {
                if(!Unit.GetComponent<UnitBase>().MovedForTurn)
                {
                    break;
                }

                isPlayerTurn = false;
                TurnChange.Invoke();
                print("Turn Change Enemy");
            }
        }
        else
        {
            foreach (GameObject Unit in UnitManager.Instance.EnemyUnits)
            {
                if (!Unit.GetComponent<UnitBase>().MovedForTurn)
                {
                    break;
                }

                isPlayerTurn = true;
                TurnChange.Invoke();
                print("Turn Change Player");
            }
        }
    }
}
