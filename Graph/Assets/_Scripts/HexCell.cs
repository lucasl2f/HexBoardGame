using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Color color;

    public CellType cellType;

    [SerializeField]
    HexCell[] neighbors;

    [SerializeField]
    TextMesh myTypeText;

    HexCell _cellTemp;

    public int myIndex;

    public bool startingPoint;

    void Update()
    {
        myTypeText.text = cellType.ToString();
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public HexCell GetNeighbor(int index)
    {
        return neighbors[index];
    }

    public HexCell[] GetNeighbors()
    {
        return neighbors;
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public bool TypeAllowed(CellType cellType)
    {
        //Debug.Log("active: " + HexMapEditor.instance.hexGrid.activeCell.cellType);
        //Debug.Log("cellType: " + cellType);

        _cellTemp = HexMapEditor.instance.hexGrid.activeCell;

        switch (cellType)
        {
            case CellType.Water:
                break;
            case CellType.Mountain:
            case CellType.Island:
                if (_cellTemp.cellType != CellType.Player1
                    && _cellTemp.cellType != CellType.Player2)
                {
                    return true;
                }
                break;
            case CellType.Player1:
                if (_cellTemp.cellType == CellType.Player1
                    || _cellTemp.cellType == CellType.Water)
                {
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        if (neighbors[i] != null)
                        {
                            if (neighbors[i].cellType == cellType
                                || neighbors[i].cellType == CellType.Island)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;
            case CellType.Player2:
                if (HexMapEditor.instance.hexGrid.activeCell.cellType == CellType.Player2
                    || HexMapEditor.instance.hexGrid.activeCell.cellType == CellType.Water)
                {
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        if (neighbors[i] != null)
                        {
                            if (neighbors[i].cellType == cellType
                                || neighbors[i].cellType == CellType.Island)
                            {
                                return true;
                            }
                        }
                    }
                }
                break;
        }

        return false;
    }
}
