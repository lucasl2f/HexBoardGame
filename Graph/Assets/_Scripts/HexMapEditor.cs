using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour
{

    public Color[] colors;
    public HexGrid hexGrid;

    [SerializeField]
    Toggle[] toggles;

    Color activeColor;
    CellType _activeCellType;

    [SerializeField]
    Toggle destructionToggle;

    public CellType activeCellType
    {
        get
        {
            return _activeCellType;
        }
    }

    public static HexMapEditor instance;

    void Awake()
    {
        instance = this;

        SelectColor(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleInput();
        }
        /*if (Input.GetKeyDown("0"))
        {
            toggles[0].isOn = true;
        }
        if (Input.GetKeyDown("1"))
        {
            toggles[1].isOn = true;
        }
        if (Input.GetKeyDown("2"))
        {
            toggles[2].isOn = true;
        }
        if (Input.GetKeyDown("3"))
        {
            toggles[3].isOn = true;
        }
        if (Input.GetKeyDown("4"))
        {
            toggles[4].isOn = true;
        }*/
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            //if (_destruction)
            //{
            //    hexGrid.DestroyCells(hit.point, Dice.RollD6());
            //}
            switch (GameController.instance.cardActive)
            {
                case GameController.Cards.Construction:
                    CheckActiveCell();
                    hexGrid.ColorCell(hit.point, activeColor, _activeCellType);
                    break;

                case GameController.Cards.Destruction:
                    CheckActiveCell();
                    hexGrid.DestroyCells(hit.point, Dice.RollD6());
                    break;

                case GameController.Cards.SayNo:
                    break;

                case GameController.Cards.Refresh:
                    break;

                case GameController.Cards.Modification:
                    hexGrid.CheckModification(hit.point);
                    break;

                default: //NONE
                    break;
            }
        }
    }

    public void SelectColor(int index)
    {
        activeColor = colors[index];
        _activeCellType = (CellType)index;
    }

    public void CheckActiveCell()
    {
        switch (GameController.instance.playerActive)
        {
            case GameController.PlayerActive.Player1:
                SelectColor(1);
                break;

            case GameController.PlayerActive.Player2:
                SelectColor(2);
                break;
        }
    }
}
