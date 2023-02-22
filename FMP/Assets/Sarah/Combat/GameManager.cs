using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<GameObject> ControlledUnits;
    public List<CharacterData> UnitData;
    internal Vector3 PlayerReturnToOverworld;
    public int MaxUnits = 6;
    internal int NumRecruited = 0;
    public int MaxRecruitable = 2;
    public List<Item> Convoy;
    public int Money;

    //Progress
    internal bool CombatTutorialComplete = false;
    internal bool OverworldTutorialComplete = false;
    internal bool ArcherRecruitComplete = false;
    internal bool GauntletRecruitComplete = false;
    internal bool PostDungeon1Complete = false;
    internal bool PostDungeon2Complete = false;

    //DialogueToShow
    internal PlayAfter DialogueToPlay = PlayAfter.None;

    //Sound
    internal float MasterSlider = 0;
    internal float SFXSlider = 0;
    internal float MusicSlider = 0;
    internal float AmbianceSlider = 0;

    //Timer
    public bool inCombat = false;
    public bool StartedGame = false;


    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        UnitData = new List<CharacterData>();
    }
}
