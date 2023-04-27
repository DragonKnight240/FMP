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
    internal MagicOrb[] Orbs;
    internal bool PendingAction = false;

    private void Awake()
    {
        if (Instance == null)
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

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;

        TurnText.text = "Player Turn";
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerTurn)
        {
            if (UnitsToMove <= 0 && Interact.Instance.VirtualCam.activeInHierarchy && !PendingAction)
            {
                //print("Turn Change to Enemy");
                isPlayerTurn = false;
                TurnChange.Invoke();
                Interact.Instance.SelectedUnit = null;
                Interact.Instance.UISelectedUnit();
                TurnText.text = "Enemy Turn";

                if (ToolTipManager.Instance)
                {
                    if (!ToolTipManager.Instance.CompletedTurn1)
                    {
                        ToolTipManager.Instance.CompletedTurn1 = true;
                    }
                }

                TurnText.GetComponentInParent<MoveToScreenLocation>().Override = true;
                TurnText.GetComponentInParent<MoveToScreenLocation>().OverrideTime = TurnText.GetComponentInParent<MoveToScreenLocation>().OverrideTimeMax;
                TurnText.GetComponentInParent<MoveToScreenLocation>().Display = true;
            }
        }
        else
        {
            foreach (GameObject Unit in UnitManager.Instance.EnemyUnits )
            {
                if (Unit.GetComponent<UnitBase>().Moving || PendingAction)
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

            foreach(MagicOrb Orb in Orbs)
            {
                Orb.DealDamage();
            }
        }
    }
}
