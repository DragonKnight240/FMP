using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public class SaveObject
    {
        public List<GameObject> Units;
        public Vector3 PlayerLocationOverworld;
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
        SaveObject saveObject = new SaveObject {Units = GameManager.Instance.ControlledUnits, PlayerLocationOverworld = GameManager.Instance.PlayerReturnToOverworld };
        string Json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + "/save.txt", Json);
    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/save.txt"))
        {
            string Json = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject LoadObject = JsonUtility.FromJson<SaveObject>(Json);

            GameManager.Instance.ControlledUnits = LoadObject.Units;
            GameManager.Instance.PlayerReturnToOverworld = LoadObject.PlayerLocationOverworld;

            print("Loaded");
        }
        else
        {
            print("NO SAVE");
        }

    }
}
