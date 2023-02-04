using UnityEngine;

public interface IAttackable
{
    void Attacked(GameObject attacker, Attack attack);

    void OnDestruction(GameObject destroyer);
}
