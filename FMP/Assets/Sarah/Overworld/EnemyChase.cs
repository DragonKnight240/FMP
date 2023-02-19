using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    EnemyOverWorld EnemyMain;

    // Start is called before the first frame update
    private void Start()
    {
        EnemyMain = GetComponentInParent<EnemyOverWorld>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(EnemyMain.SpottedPlayer);
            EnemyMain.PlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySFX(EnemyMain.LostPlayer);
            EnemyMain.PlayerInRange = false;
        }
    }
}
