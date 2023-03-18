using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public static Compass Instance; 
    public RawImage CompassImage;
    public Transform Player;
    internal List<Objectives> Markers = new List<Objectives>();
    public GameObject IconPrefab;
    public GameObject OverworldIconPrefab;

    public List<Objectives> MainObjectives;

    float CompassUnit;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        CompassUnit = CompassImage.rectTransform.rect.width / 360f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Markers.Count == 0)
        {
            if(MainObjectives.Count > 0)
            {
                if(MainObjectives[GameManager.Instance.MainObjectNum] != null && GameManager.Instance.MainObjectNum < MainObjectives.Count)
                {
                    AddMarker(MainObjectives[GameManager.Instance.MainObjectNum]);
                }
            }
        }

        CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360f, 0f, 1f, 1f);

        foreach(Objectives Marker in Markers)
        {
            Marker.image.rectTransform.anchoredPosition = CompassPosition(Marker);
        }
        
    }

    public void RemoveObjective(Objectives Marker)
    {
        if(Marker.image)
        {
            Destroy(Marker.image.gameObject);
        }

        if(Marker.ObjectMarkerOverworld)
        {
            Destroy(Marker.ObjectMarkerOverworld);
        }

        GameManager.Instance.MainObjectNum++;
    }

    public void AddMarker(Objectives Marker)
    {
        GameObject NewMarker = Instantiate(IconPrefab, CompassImage.transform);
        GameObject NewOverworldMarker = Instantiate(OverworldIconPrefab, Marker.gameObject.transform);
        Marker.ObjectMarkerOverworld = NewOverworldMarker;
        Marker.image = NewMarker.GetComponent<Image>();
        Marker.image.sprite = Marker.Icon;

        Markers.Add(Marker);
    }

    Vector2 CompassPosition(Objectives Marker)
    {
        Vector2 PlayerPos = new Vector2(Player.transform.position.x, Player.transform.position.z);
        Vector2 PlayerForward = new Vector2(Player.transform.forward.x, Player.transform.forward.z);

        float Angle = Vector2.SignedAngle(Marker.position - PlayerPos, PlayerForward);

        return new Vector2(CompassUnit * Angle, 0f);
    }
}
