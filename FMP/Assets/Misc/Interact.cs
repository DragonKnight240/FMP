using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public static Interact Instance;
    internal UnitBase SelectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit Hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hit))
            {
                if(Hit.transform.GetComponent<UnitBase>())
                {
                    SelectedUnit = Hit.transform.GetComponent<UnitBase>();
                    print("Selected Unit");
                }

                if(Hit.transform.GetComponent<Tile>() && SelectedUnit)
                {
                    print("Move Unit");
                    if (SelectedUnit.Move(Hit.transform.GetComponent<Tile>()))
                    {
                        SelectedUnit = null;
                    }
                }
            }
        }
    }
}
