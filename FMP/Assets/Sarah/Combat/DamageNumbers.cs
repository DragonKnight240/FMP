using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public GameObject UpLocation;
    public GameObject StartLocation;
    public float Speed = 5;
    public List<GameObject> DamageCanvas;
    public GameObject FarDamageCanvas;
    public GameObject FarDamageUpLocation;
    public GameObject FarStartLocation;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject Canvas in DamageCanvas)
        {
            Canvas.GetComponent<CanvasGroup>().alpha = 0;
            Canvas.gameObject.SetActive(false);
        }

        FarDamageCanvas.GetComponent<CanvasGroup>().alpha = 0;
        FarDamageCanvas.gameObject.SetActive(false);
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
        }

        if(FarDamageCanvas.activeInHierarchy)
        {
            FarDamageCanvas.transform.position = Vector3.MoveTowards(FarDamageCanvas.transform.position, FarDamageUpLocation.transform.position, Speed * Time.deltaTime);
        }
    }

    internal void ResetDamageNumber()
    {
        foreach (GameObject Canvas in DamageCanvas)
        {
            Canvas.transform.position = StartLocation.transform.position;
            //Canvas.GetComponentInChildren<TMP_Text>().text = "Miss";
        }
    }

    internal void ResetFarDamageNumber()
    {
        FarDamageCanvas.transform.position = FarStartLocation.transform.position;
    }

    internal void PlayFarDamage(int Damage)
    {
        if (Damage <= 0)
        {
            FarDamageCanvas.GetComponentInChildren<TMP_Text>().text = "Miss";
        }
        else
        {
            FarDamageCanvas.GetComponentInChildren<TMP_Text>().text = Damage.ToString();
        }
        FarDamageCanvas.SetActive(true);
        FarDamageCanvas.GetComponent<UIFade>().ToFadeIn();
        FarDamageCanvas.GetComponent<UIFade>().Both = true;
    }

    internal void PlayDamage(int AttackID, int Damage)
    {
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
