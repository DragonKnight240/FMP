using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUnits : MonoBehaviour
{
    internal bool InRange = false;
    public AudioClip HealSound;
    public ParticleSystem PlayerParticles;

    internal void Heal()
    {
        print("Heal");
        foreach(CharacterData Unit in GameManager.Instance.UnitData)
        {
            Unit.CurrentHealth = Unit.HealthMax;
        }

        SoundManager.Instance.PlaySFX(HealSound);
        PlayerParticles.Play();
    }
}
