using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {
	[SerializeField]
	int width = 6, height = 6;

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

	HexCell _activeCell;
	CellType _cellTarget;
	public HexCell activeCell {
		get {
			return _activeCell;
		}
	}

	List<int> cellsPlayer1 = new List<int>();
	List<int> cellsPlayer2 = new List<int>();

	void Awake () {
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
		cells = new HexCell[height * width];

		int i, x, z;
		for (z = 0, i = 0; z < height; z++) {
			for (x = 0; x < width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start () {
		hexMesh.Triangulate(cells);
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		if (x > 0) { //Ignore the first cell of a row
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}

		if (z > 0) { //Ignore the first row
			if ((z & 1) == 0) { //Treat only even rows
				cell.SetNeighbor(HexDirection.SE, cells[i - width]);
				if (x > 0) { //Ignore the first cell of the row
					cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
				}
			} else { //Treat only odds rows
				cell.SetNeighbor(HexDirection.SW, cells[i - width]);
				if (x < width - 1) { //Ignore the last cell of a row
					cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
				}
			}
		}

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}

	public void ColorCell (Vector3 position, Color color, CellType cellType) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		_activeCell = cells[index];
		if (_activeCell.TypeAllowed(cellType) || coordinates.Z == 0) {
			_activeCell.color = color;
			_activeCell.cellType = cellType;	
			hexMesh.Triangulate(cells);

			if (cellType == CellType.Player1) {
				cellsPlayer1.Add(index);
			} else if (cellType == CellType.Player2) {
				cellsPlayer2.Add(index);
			}
		}
		/*int i;
		for (i = 0; i < cell.GetNeighbors().Length; i++) {
			if (cell.GetNeighbor(i) != null) {
				cell.GetNeighbors()[i].color = color;
			}
		}*/
		//Debug.Log("Touched at " + coordinates.ToString()); 
	}

	public void DestroyCells (Vector3 position, int newQuantity) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		_activeCell = cells[index];
		_cellTarget = GameController.instance.playerActive == GameController.PlayerActive.Player1 ? CellType.Player2 : CellType.Player1;
		
		if (_activeCell.cellType == _cellTarget) {
			_activeCell.color = HexMapEditor.instance.colors[0]; //Water
			_activeCell.cellType = CellType.Water;
			hexMesh.Triangulate(cells);
			newQuantity--;

			for (int i = 0; i < newQuantity; i++) {
				for (int j = 0; j < _activeCell.GetNeighbors().Length; j++) {
					if (_activeCell.GetNeighbor(j) != null && _activeCell.GetNeighbor(j).cellType == _cellTarget) {
						_activeCell = _activeCell.GetNeighbor(j);
						_activeCell.color = HexMapEditor.instance.colors[0]; //Water
						_activeCell.cellType = CellType.Water;
						hexMesh.Triangulate(cells);
						break;
					}
				}
			}
		}
	}
}