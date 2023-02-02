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

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerTurn)
        {
            foreach (GameObject Unit in UnitManager.Instance.AllyUnits)
            {
                if (!Unit.GetComponent<UnitBase>().EndTurn)
                {
                    if (Unit.GetComponent<UnitBase>().isAlive)
                    {
                        return;
                    }
                }
            }

            isPlayerTurn = false;
            TurnChange.Invoke();
            Interact.Instance.SelectedUnit = null;
            print("Turn Change Enemy");
        }
        else
        {
            foreach (GameObject Unit in UnitManager.Instance.EnemyUnits )
            {
                if (!Unit.GetComponent<UnitBase>().EndTurn)
                {
                    if (!Unit.GetComponent<UnitBase>().EndTurn)
                    {
                        return;
                    }
                }
            }

            isPlayerTurn = true;
            TurnChange.Invoke();
            Interact.Instance.SelectedUnit = null;
            print("Turn Change Player");
        }
    }
}
