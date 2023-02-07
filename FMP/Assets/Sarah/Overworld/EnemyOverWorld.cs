using System.Collections;
using System.Collections.Generic;
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
    public string CombatMapName;

    // Start is called before the first frame update
    void Start()
    {
        PatrolLocations = new List<Transform>();

        if(PatrolLocationsParent)
        {
            foreach(Transform transform in PatrolLocationsParent.transform)
            {
                PatrolLocations.Add(transform);
            }

            CurrentNode = PatrolLocations[0];
        }

        Player = FindObjectOfType<PlayerOverworld>().gameObject;
        GetComponentInChildren<AoEDisappear>().gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance)
        {
            foreach (GameObject Enemy in GameManager.Instance.EnemyCombatStarter)
            {
                if (Enemy == this.gameObject)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        if (ReachedPlayer)
        {
            return;
        }

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
                PlayerInRange = true;
            }
            else
            {
                PlayerInRange = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Time.timeScale = 0;
            ReachedPlayer = true;
            GameManager.Instance.EnemyCombatStarter.Clear();
            GameManager.Instance.EnemyCombatStarter.Add(gameObject);
            GameManager.Instance.PlayerReturnToOverworld = FindObjectOfType<PlayerOverworld>().transform.position;
            SceneLoader.Instance.LoadNewScene(CombatMapName);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Time.timeScale = 0;
            ReachedPlayer = true;
            if (GameManager.Instance)
            {
                GameManager.Instance.EnemyCombatStarter.Clear();
                GameManager.Instance.EnemyCombatStarter.Add(gameObject);
                GetComponentInChildren<AoEDisappear>().gameObject.SetActive(true);

                if (Player)
                {
                    GameManager.Instance.PlayerReturnToOverworld = Player.transform.position;
                }
            }
            SceneLoader.Instance.LoadNewScene(CombatMapName);
        }
    }
}
