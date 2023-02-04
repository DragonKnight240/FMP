using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Npc_AI
{
    public class AttackedScrollingText : MonoBehaviour, IAttackable
    {
        public ScrollingText Text;
        public Color color;

        CharacterStats stats;

        void Start()
        {
            stats = GetComponent<CharacterStats>();
        }

        public void Attacked(GameObject attacker, Attack attack)
        {
            var text = attack.Damage.ToString();

            var scrollingText = Instantiate(Text, transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
            scrollingText.SetText(text);
            scrollingText.SetColor(color);
        }

        public void OnDestruction(GameObject destroyer)
        {

        }
    }
}