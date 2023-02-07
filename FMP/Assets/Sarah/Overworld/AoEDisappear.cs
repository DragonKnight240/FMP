using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoEDisappear : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (GameManager.Instance.EnemyCombatStarter.Contains(other.gameObject))
            {
                GameManager.Instance.EnemyCombatStarter.Add(other.gameObject);
            }
        }
    }
}
