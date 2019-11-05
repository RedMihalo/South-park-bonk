using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Button ActiveButton;

    public Transform UnitPositionTransform;
    public Vector3 UnitPosition
    {
        get => UnitPositionTransform.position;
        // get => GetWorldBounds().center;
    }

    private GridController parentController;
    public GridController ParentController
    {
        set => parentController = value;
        get => parentController;
    }

    // Start is called before the first frame update
    void Start()
    {
        ActiveButton.onClick.AddListener(() => BattleScreenManager.MoveUnit(UnitPosition));
    }
    

    public Bounds GetWorldBounds()
    {
        Vector3[] v = new Vector3[4];
        transform.Find("Canvas").GetComponent<RectTransform>().GetWorldCorners(v);

        Vector3 size = new Vector3(Mathf.Abs(v[1].x - v[2].x), Mathf.Abs(v[1].y - v[0].y), 0);

        Vector3 center = new Vector3(
            (v[1].x + v[2].x) / 2 , 
            (v[0].y + v[1].y) / 2, 
            0);

        return new Bounds(center, size);
    }

    public void SetTileTargetable(bool bTargerable)
    {
        ActiveButton.interactable = bTargerable;

        Image img = ActiveButton.gameObject.GetComponent<Image>();
        Color imgColor = img.color;
        imgColor.a = bTargerable ? 66 : 200;
        img.color = imgColor;
    }


}
