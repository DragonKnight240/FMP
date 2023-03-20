using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCombatEndMainMenu : MonoBehaviour
{
    internal void Menu()
    {
        GameManager.Instance.ReturnToDefault();
    }
}
