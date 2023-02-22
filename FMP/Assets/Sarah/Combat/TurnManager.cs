using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public TMP_Text TurnText;
    internal bool isPlayerTurn = true;
    internal UnityEvent TurnChange;
    internal int UnitsToMove = 0;

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

        TurnText.text = "Player Turn";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UnitManager.Instance.EndingCombat();
        }

        if (isPlayerTurn)
        {
            if (UnitsToMove <= 0)
            {
                isPlayerTurn = false;
                TurnChange.Invoke();
                Interact.Instance.SelectedUnit = null;
                Interact.Instance.UISelectedUnit();
                TurnText.text = "Enemy Turn";

                TurnText.GetComponentInParent<MoveToScreenLocation>().Override = true;
                TurnText.GetComponentInParent<MoveToScreenLocation>().OverrideTime = TurnText.GetComponentInParent<MoveToScreenLocation>().OverrideTimeMax;
                TurnText.GetComponentInParent<MoveToScreenLocation>().Display = true;
            }
        }
        else
        {
            foreach (GameObject Unit in UnitManager.Instance.EnemyUnits )
            {
                if (Unit.GetComponent<UnitBase>().Moving)
                {
                    return;
                }

                if (!Unit.GetComponent<UnitBase>().EndTurn)
                {
                    if (Unit.GetComponent<UnitBase>().isAlive)
                    {
                        return;
                    }
                }
            }

            if(Interact.Instance.VirtualCam.transform.position != Interact.Instance.transform.position)
            {
                return;
            }

            isPlayerTurn = true;
            TurnChange.Invoke();
            Interact.Instance.SelectedUnit = null;
            Interact.Instance.UISelectedUnit();
            UnitsToMove = UnitManager.Instance.AllyUnits.Count;
            TurnText.text = "Player Turn";
            TurnText.GetComponentInParent<MoveToScreenLocation>().Override = true;
            TurnText.GetComponentInParent<MoveToScreenLocation>().OverrideTime = TurnText.GetComponentInParent<MoveToScreenLocation>().OverrideTimeMax;
            TurnText.GetComponentInParent<MoveToScreenLocation>().Display = true;
        }
    }
}
