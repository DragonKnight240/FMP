using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUnits : MonoBehaviour
{

    internal void Heal()
    {
        print("Heal");
        foreach(CharacterData Unit in GameManager.Instance.UnitData)
        {
            Unit.CurrentHealth = 1;
                //Unit.HealthMax;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Heal();
        }
    }
}
