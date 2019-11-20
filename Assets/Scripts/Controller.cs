using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    public List<UnitSerializeInfo> SerializedUnits = new List<UnitSerializeInfo>();
    private List<GameObject> Units = new List<GameObject>();
    public GameObject UnitPrefab;

    protected UnitTeam team;

    public virtual void Start()
    {
        DontDestroyOnLoad(gameObject);
        SpawnUnits();
    }

    private void SpawnUnits()
    {
        foreach(UnitSerializeInfo info in SerializedUnits)
        {
            GameObject lastObject = Instantiate(UnitPrefab, transform.position, Quaternion.identity);
            Units.Add(lastObject);
            info.team = team;
            UnitSerializeInfo.DeserializeUnit(lastObject, info);
        }

        Units.ForEach((GameObject u) =>
        {
            u.GetComponent<BattleUnit>().MoveToStartPosition();
        });
    }

}
