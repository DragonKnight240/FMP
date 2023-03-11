using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public class SaveObject
    {
        public List<CharacterData> Units;
        public Vector3 PlayerLocationOverworld;
        public Quaternion PlayerRotationOverworld;
    }

    public static SaveSystem Instance;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Save()
    {
        SaveObject saveObject = new SaveObject {Units = GameManager.Instance.UnitData, PlayerLocationOverworld = GameManager.Instance.PlayerReturnToOverworld, PlayerRotationOverworld = GameManager.Instance.PlayerReturnRotation };
        string Json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + "/save.txt", Json);
    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string Json = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject LoadObject = JsonUtility.FromJson<SaveObject>(Json);

            GameManager.Instance.UnitData = LoadObject.Units;
            GameManager.Instance.PlayerReturnToOverworld = LoadObject.PlayerLocationOverworld;

            print("Loaded");
        }
        else
        {
            print("NO SAVE");
        }

    }
}
