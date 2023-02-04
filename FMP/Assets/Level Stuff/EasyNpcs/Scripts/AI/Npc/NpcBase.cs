using UnityEngine;
using UnityEngine.AI;
using Rotation;
using TMPro;

public class NpcBase : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    protected Animator anim;

    public float VisionRange = 10;
    public float VisionAngle = 25f;
    public LayerMask VisionLayers;

    [HideInInspector]
    public TMP_Text textMesh;

    public Job job;
    public Gender gender;

    public NpcState currentState { get; protected set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        textMesh = GetComponentInChildren<TMP_Text>();
    }

    public virtual bool ChangeState(NpcState newState)
    {
        if (IsStageChangeAble(newState))
        {
            NpcState oldState = currentState;
            currentState = newState;
            OnStateChanged(oldState, newState);

            return true;
        }

        return false;
    }

    bool IsStageChangeAble(NpcState newState)
    {
        if (newState != NpcState.Idle)
        {
            if (currentState == newState)
                return false;

            int currentStateAdvantage = StateAdvantageNumber(currentState);
            int newStateAdvantage = StateAdvantageNumber(newState);
            if (newStateAdvantage < currentStateAdvantage)
            {
                return false;
            }
        }

        return true;
    }

    int StateAdvantageNumber(NpcState state)
    {
        switch (state)
        {
            case NpcState.Idle:
                return 0;

            case NpcState.GoingToWork:
                return 1;

            case NpcState.Working:
                return 1;

            case NpcState.GoingHome:
                return 1;

            case NpcState.Talking:
                return 2;

            case NpcState.Scared:
                return 4;

            case NpcState.Patrol:
                return 1;

            case NpcState.Chase:
                return 1;

            case NpcState.Attack:
                return 1;
        }

        return -1;
    }

    protected virtual void OnStateChanged(NpcState prevState, NpcState newState)
    {
        TurnOffBehaviour(prevState);
        switch (newState)
        {
            case NpcState.Idle:
                OnIdle();
                break;

            case NpcState.GoingToWork:
                OnGoingToWork();
                break;

            case NpcState.Working:
                OnWorking();
                break;

            case NpcState.Scared:
                OnScared();
                break;

            case NpcState.Patrol:
                OnPatrol();
                break;

            case NpcState.Attack:
                OnAttack();
                break;
        }
    }

    protected virtual void TurnOffBehaviour(NpcState prevState)
    {
        switch (prevState)
        {
            case NpcState.Talking:
                Destroy(GetComponent<RunConversation>());
                break;

            case NpcState.Attack:
                Destroy(GetComponent<Rotate>());
                break;
        }
    }

    protected virtual void OnIdle() { }

    protected virtual void OnGoingToWork() { }

    protected virtual void OnWorking() { }

    protected virtual void OnScared() { }

    protected virtual void OnPatrol() { }

    protected virtual void OnAttack() { }

    private void OnEnable()
    {
        ChangeState(NpcState.Idle);
    }

    public void OnDisable()
    {
        TurnOffBehaviour(currentState);
        anim.SetFloat("Speed", 0);
    }

    public void OnDestruction(GameObject destroyer)
    {
        enabled = false;
    }
}

