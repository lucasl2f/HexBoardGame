﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    [SerializeField]
    int width = 6, height = 6;

    public int GetCellsCount
    {
        get
        {
            return width * height;
        }
    }

    [SerializeField]
    HexCell cellPrefab;
    [SerializeField]
    Text cellLabelPrefab;

    [SerializeField]
    Color defaultColor = Color.white;
    //, touchedColor = Color.magenta;

    Canvas gridCanvas;
    HexMesh hexMesh;
    HexCell[] cells;

    HexCell _activeCell, _cellTemp;
    CellType _cellTarget;
    public HexCell activeCell
    {
        get
        {
            return _activeCell;
        }
    }

    List<int> cellsPlayer1 = new List<int>();
    List<int> cellsPlayer2 = new List<int>();

    List<int> _cellsVisitedTemp = new List<int>();

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        cells = new HexCell[height * width];

        int i, x, z;
        for (z = 0, i = 0; z < height; z++)
        {
            for (x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

        //CreateCellPlayers(2, -1, height * width, CellType.Player1);
        //CreateCellPlayers(4, -1, height * width + 1, CellType.Player2);
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        if (x > 0)
        { //Ignore the first cell of a row
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }

        if (z > 0)
        { //Ignore the first row
            if ((z & 1) == 0)
            { //Treat only even rows
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if (x > 0)
                { //Ignore the first cell of the row
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            { //Treat only odds rows
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if (x < width - 1)
                { //Ignore the last cell of a row
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }
        else
        {
            if (x == 1)
            {
                cell.startingPoint = true;
                cell.cellType = CellType.Player1;
                cell.color = HexMapEditor.instance.colors[1];
                cell.myIndex = cell.coordinates.X + cell.coordinates.Z * width + cell.coordinates.Z / 2;
                cellsPlayer1.Add(cell.myIndex);
            }
            else if (x == (width - 2))
            {
                cell.startingPoint = true;
                cell.cellType = CellType.Player2;
                cell.color = HexMapEditor.instance.colors[2];
                cell.myIndex = cell.coordinates.X + cell.coordinates.Z * width + cell.coordinates.Z / 2;
                cellsPlayer2.Add(cell.myIndex);
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }

    public void ColorCell(Vector3 position, Color color, CellType cellType)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        _activeCell = cells[index];
        if (ConstructionAllowed(cellType))
        { // || coordinates.Z == 0
            _activeCell.color = color;
            _activeCell.cellType = cellType;
            hexMesh.Triangulate(cells);

            if (cellType == CellType.Player1)
            {
                cellsPlayer1.Add(index);
            }
            else if (cellType == CellType.Player2)
            {
                cellsPlayer2.Add(index);
            }
        }
    }

    bool ConstructionAllowed(CellType cellType)
    {
        _cellTemp = _activeCell;
        _cellsVisitedTemp.Clear();

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
                    for (int i = 0; i < HexMapEditor.instance.hexGrid.GetCellsCount; i++)
                    {
                        for (int j = 0; j < _cellTemp.GetNeighbors().Length; j++)
                        {
                            if (_cellTemp.GetNeighbors()[j] != null)
                            {
                                if (_cellTemp.GetNeighbors()[j].cellType == cellType
                                    && cellsPlayer1.Contains(_cellTemp.GetNeighbors()[j].myIndex))
                                {
                                    return true;
                                }
                                else if (j == _cellTemp.GetNeighbors().Length - 1)
                                {
                                    Debug.Log("false on " + j);
                                    return false;
                                }
                                else if (_cellTemp.GetNeighbors()[j].cellType == CellType.Island
                                && !_cellsVisitedTemp.Contains(_cellTemp.GetNeighbors()[j].myIndex))
                                {
                                    _cellsVisitedTemp.Add(_cellTemp.GetNeighbors()[j].myIndex);
                                    _cellTemp = _cellTemp.GetNeighbors()[j];
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
            case CellType.Player2:
                if (_cellTemp.cellType == CellType.Player2
                    || _cellTemp.cellType == CellType.Water)
                {
                    for (int i = 0; i < HexMapEditor.instance.hexGrid.GetCellsCount; i++)
                    {
                        for (int j = 0; j < _cellTemp.GetNeighbors().Length; j++)
                        {
                            if (_cellTemp.GetNeighbors()[j] != null)
                            {
                                if (_cellTemp.GetNeighbors()[j].cellType == cellType
                                    && cellsPlayer2.Contains(_cellTemp.GetNeighbors()[j].myIndex))
                                {
                                    return true;
                                }
                                else if (j == _cellTemp.GetNeighbors().Length - 1)
                                {
                                    Debug.Log("false on " + j);
                                    return false;
                                }
                                else if (_cellTemp.GetNeighbors()[j].cellType == CellType.Island
                                && !_cellsVisitedTemp.Contains(_cellTemp.GetNeighbors()[j].myIndex))
                                {
                                    _cellsVisitedTemp.Add(_cellTemp.GetNeighbors()[j].myIndex);
                                    _cellTemp = _cellTemp.GetNeighbors()[j];
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
        }

        return false;
    }

    public void DestroyCells(Vector3 position, int newQuantity)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        _activeCell = cells[index];
        _cellTarget = GameController.instance.playerActive == GameController.PlayerActive.Player1 ? CellType.Player2 : CellType.Player1;
        if (_activeCell.cellType == _cellTarget && !_activeCell.startingPoint)
        {
            _activeCell.color = HexMapEditor.instance.colors[0]; //Water
            _activeCell.cellType = CellType.Water;
            hexMesh.Triangulate(cells);
            newQuantity--;

            if (_cellTarget == CellType.Player1)
            {
                cellsPlayer1.Remove(_activeCell.myIndex);
            }
            else if (_cellTarget == CellType.Player2)
            {
                cellsPlayer2.Remove(_activeCell.myIndex);
            }

            for (int i = 0; i < newQuantity; i++)
            {
                for (int j = 0; j < _activeCell.GetNeighbors().Length; j++)
                {
                    _cellTemp = _activeCell.GetNeighbor(j);
                    if (_cellTemp != null
                    && _cellTemp.cellType == _cellTarget
                    && !_cellTemp.startingPoint)
                    {
                        _activeCell = _cellTemp;
                        _activeCell.color = HexMapEditor.instance.colors[0]; //Water
                        _activeCell.cellType = CellType.Water;
                        hexMesh.Triangulate(cells);

                        if (_cellTarget == CellType.Player1)
                        {
                            cellsPlayer1.Remove(_activeCell.myIndex);
                        }
                        else if (_cellTarget == CellType.Player2)
                        {
                            cellsPlayer2.Remove(_activeCell.myIndex);
                        }
                        break;
                    }
                }
            }
        }
    }

    public void CheckModification(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        _activeCell = cells[index];

        switch (_activeCell.cellType)
        {
            case CellType.Water:
            case CellType.Mountain:
                ColorCell(position, HexMapEditor.instance.colors[3], CellType.Island);
                break;

            case CellType.Island:
                ColorCell(position, HexMapEditor.instance.colors[4], CellType.Mountain);
                break;
        }
    }
}