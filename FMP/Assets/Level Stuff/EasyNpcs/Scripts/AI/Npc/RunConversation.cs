using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Npc_AI;
using Rotation;
using TMPro;

public class RunConversation : MonoBehaviour
{
    bool first;
    NpcBase me;
    NpcBase partner;
    Tuple<List<string>,List<string>> conversation = null;

    Rotate rotate;
    
    public delegate void EventOnEnd();
    public event EventOnEnd convOnEnd;

    private void Awake()
    {
        me = GetComponent<NpcBase>();
    }

    private void Start()
    {
        if (!first)
            GetComponent<NavMeshAgent>().SetDestination(transform.position);

        rotate = gameObject.AddComponent<Rotate>();
        rotate.RotateTo(partner.transform);
    }
    
    public void Set(NpcBase npcPartner, bool order = false, Tuple<List<string>, List<string>> conver = null)
    {
        first = order;
        partner = npcPartner;
        conversation = conver;
        ChangeState_ToTalking();

        StartConversation();
    }

    void ChangeState_ToTalking()
    {
        if (me.ChangeState(NpcState.Talking) == true)
        {
            partner.ChangeState(NpcState.Talking);
        }
        
        OnDestroy();
    }

    void StartConversation()
    {
        GetComponent<NavMeshAgent>().SetDestination(Position_1FloatAway_FromPartner());
        
        if (first)
        {
            If_First();
        }
    }

    Vector3 Position_1FloatAway_FromPartner()
    {
        float distance = Vector3.Distance(transform.position, partner.transform.position);
        float distanceOffset = distance - 1;
        Vector3 myPos = transform.position;
        Vector3 partnerPos = partner.transform.position;
        float desX = (myPos.x + partnerPos.x * distanceOffset) / distance;
        float desY = (myPos.y + partnerPos.y * distanceOffset) / distance;
        float desZ = (myPos.z + partnerPos.z * distanceOffset) / distance;

        return new Vector3(desX, desY, desZ);
    }

    void If_First()
    {
        Tuple<List<string>, List<string>> chosenConv = Choose_Conversation();
        if (chosenConv != null)
        {
            StartCoroutine(Start_Talk(chosenConv));
        }
    }

    Tuple<List<String>, List<String>> Choose_Conversation()
    {
        if (conversation == null)
        {
            return ChooseConversation();
        }
        else
        {
            return conversation;
        }
    }

    Tuple<List<string>, List<string>> ChooseConversation()
    {
        Job[] jobs = { me.job, partner.job };
        Gender[] genders = { me.gender, partner.gender };

        return FindDialogue.GetDialgoue(genders, jobs);
    }

    IEnumerator Start_Talk(Tuple<List<string>, List<string>> chosenConv)
    {
        RunConversation partnerConv = AddRunConv_ToPartner();
        StartCoroutine(Talk(chosenConv.Item1));
        yield return new WaitForSeconds(4);

        partnerConv.RecieveRequest(chosenConv);
    }

    RunConversation AddRunConv_ToPartner()
    {
        RunConversation partnerConv = partner.gameObject.AddComponent<RunConversation>();
        partnerConv.Set(me);

        return partnerConv;
    }

    public IEnumerator Talk(List<string> text)
    {
        for (int i = 0; i < text.Count; i++)
        {
            if (!text[i].StartsWith(" "))
            {
                me.textMesh.text = text[i];
                yield return new WaitForSeconds(4);

                if (i != text.Count - 1)
                {
                    me.textMesh.text = null;
                    yield return new WaitForSeconds(4);
                }
            }
        }

        OnEnd_Conversation();
    }

    void RecieveRequest(Tuple<List<string>, List<string>> chosenConv)
    {
        StartCoroutine(Talk(chosenConv.Item2));
    }

    public void OnEnd_Conversation()
    {
        if (convOnEnd != null)
            convOnEnd.Invoke();

        me.ChangeState(NpcState.Idle);
        GetComponentInChildren<TMP_Text>().text = null;
    }

    private void OnDestroy()
    {
        Destroy(rotate);
    }
}
