using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public GameObject UpLocation;
    internal Vector3 StartLocation;
    public float Speed = 5;
    public List<GameObject> DamageCanvas;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject Canvas in DamageCanvas)
        {
            Canvas.GetComponent<CanvasGroup>().alpha = 0;
            Canvas.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject Canvas in DamageCanvas)
        {
            if (Canvas.activeInHierarchy)
            {
                Canvas.transform.position = Vector3.MoveTowards(Canvas.transform.position, UpLocation.transform.position, Speed * Time.deltaTime);
            }
            else
            {
            }
        }
    }

    internal void ResetDamageNumber()
    {
        foreach (GameObject Canvas in DamageCanvas)
        {
            Canvas.transform.position = StartLocation;
            Canvas.GetComponentInChildren<TMP_Text>().text = "Miss";
        }
    }

    internal void PlayDamage(int AttackID, int Damage)
    {
        StartLocation = DamageCanvas[AttackID].transform.position;

        if(Damage <= 0)
        {
            DamageCanvas[AttackID].GetComponentInChildren<TMP_Text>().text = "Miss";
        }
        else
        {
            DamageCanvas[AttackID].GetComponentInChildren<TMP_Text>().text = Damage.ToString();
        }
        DamageCanvas[AttackID].SetActive(true);
        DamageCanvas[AttackID].GetComponent<UIFade>().ToFadeIn();
        DamageCanvas[AttackID].GetComponent<UIFade>().Both = true;
    }
}
