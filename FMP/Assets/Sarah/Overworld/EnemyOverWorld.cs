using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class EnemyOverWorld : MonoBehaviour
{
    //Patrol
    public float MoveSpeed;
    public GameObject PatrolLocationsParent;
    internal List<Transform> PatrolLocations;
    internal Transform CurrentNode;
    internal int NodeNum = 0;

    //Chase
    public float ChaseSpeed;
    public float AggroRange;
    internal bool PlayerInRange;
    internal GameObject Player;

    //Reach Player
    bool ReachedPlayer = false;
    public List<string> CombatMapNames;

    //public float StartVel;
    Rigidbody RB;

    //Sound
    public List<AudioClip> SpottedPlayerSounds;
    public AudioClip SpottedPlayer;
    public List<AudioClip> LostPlayerSounds;
    public AudioClip LostPlayer;

    Animator Anim;

    internal bool PendingCombat = false;

    //public AudioClip IdleSound;

    // Start is called before the first frame update
    void Start()
    {
        PatrolLocations = new List<Transform>();

        Anim = GetComponent<Animator>();

        if(PatrolLocationsParent)
        {
            foreach(Transform transform in PatrolLocationsParent.transform)
            {
                PatrolLocations.Add(transform);
            }

            CurrentNode = PatrolLocations[0];
        }
        else
        {
            Destroy(this);
        }

        if (FindObjectOfType<PlayerOverworld>())
        {
            Player = FindObjectOfType<PlayerOverworld>().gameObject;
        }
        RB = GetComponent<Rigidbody>();

        if(RB == null)
        {
            RB = gameObject.AddComponent<Rigidbody>();
            RB.freezeRotation = true;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ReachedPlayer)
        {
            GameManager.Instance.PostProcessingComabt(Player.GetComponent<PlayerOverworld>(), RandomMap());
            return;
        }

        //StartVel = RB.velocity.y;

        if (!PlayerInRange)
        {
            if (new Vector3(CurrentNode.position.x, transform.position.y, CurrentNode.position.z) == new Vector3(transform.position.x, transform.position.y, transform.position.z))
            {
                NodeNum += 1;
                if (NodeNum >= PatrolLocations.Count)
                {
                    NodeNum = 0;
                }

                CurrentNode = PatrolLocations[NodeNum];
                transform.LookAt(new Vector3(CurrentNode.position.x, transform.position.y, CurrentNode.position.z));
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(CurrentNode.position.x, transform.position.y, CurrentNode.position.z), MoveSpeed * Time.timeScale);
                transform.LookAt(new Vector3(CurrentNode.position.x, transform.position.y, CurrentNode.position.z));
            }

        }
        else
        {
            transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z), ChaseSpeed * Time.timeScale);
        }

        if (Player)
        {
            if ((transform.position - Player.transform.position).magnitude < AggroRange)
            {
                if(!PlayerInRange)
                {
                    Anim.SetTrigger("Chase");
                    SoundManager.Instance.PlaySFX(RandomSound(SpottedPlayerSounds));
                }

                PlayerInRange = true;
            }
            else
            {
                if (PlayerInRange)
                {
                    Anim.SetTrigger("Walk");
                    SoundManager.Instance.PlaySFX(RandomSound(LostPlayerSounds));
                }
                PlayerInRange = false;
            }
        }

        //RB.velocity = new Vector3(RB.velocity.x, StartVel, RB.velocity.z);
    }

    internal string RandomMap()
    {
        return CombatMapNames[Random.Range(0, CombatMapNames.Count - 1)];
    }

    internal AudioClip RandomSound(List<AudioClip> AudioClips)
    {
        return AudioClips[Random.Range(0, AudioClips.Count - 1)];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!collision.transform.GetComponent<PlayerOverworld>().AoEDisappear.gameObject.activeInHierarchy)
            {
                Time.timeScale = 0;
                ReachedPlayer = true;
                if (GameManager.Instance)
                {
                    if (Player)
                    {
                        GameManager.Instance.PlayerReturnToOverworld = Player.transform.position;
                        GameManager.Instance.PlayerReturnToOverworld.y += 1;
                    }

                    GameManager.Instance.inCombat = true;

                    SoundManager.Instance.PlaySFX(Player.GetComponent<PlayerOverworld>().EnterBattleSound);
                }
            }
        }
    }
}
