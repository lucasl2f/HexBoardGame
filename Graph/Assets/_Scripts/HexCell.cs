using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public Color color;

	public CellType cellType;

	[SerializeField]
	HexCell[] neighbors;

	[SerializeField]
	TextMesh myTypeText;

	void Update() {
		myTypeText.text = cellType.ToString();
	}

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public HexCell GetNeighbor (int index) {
		return neighbors[index];
	}

	public HexCell[] GetNeighbors () {
		return neighbors;
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public bool TypeAllowed (CellType cellType) {
		//Debug.Log("active: " + HexMapEditor.instance.hexGrid.activeCell.cellType);
		//Debug.Log("cellType: " + cellType);

		switch (cellType) {
			case CellType.Water:
			break;
			case CellType.Mountain:
			case CellType.Island:
				if (HexMapEditor.instance.hexGrid.activeCell.cellType != CellType.Player1
					&&HexMapEditor.instance.hexGrid.activeCell.cellType != CellType.Player2) {
					return true;
				}
				break;
			case CellType.Player1:
				if (HexMapEditor.instance.hexGrid.activeCell.cellType == CellType.Player1
					|| HexMapEditor.instance.hexGrid.activeCell.cellType == CellType.Water) {
					for (int i = 0; i < neighbors.Length; i++) {
						if (neighbors[i] != null) {
							if (neighbors[i].cellType == cellType
								|| neighbors[i].cellType == CellType.Island) {
								return true;
							}
						}
					}
				}
				break;
			case CellType.Player2:
				if (HexMapEditor.instance.hexGrid.activeCell.cellType == CellType.Player2
					|| HexMapEditor.instance.hexGrid.activeCell.cellType == CellType.Water) {
					for (int i = 0; i < neighbors.Length; i++) {
						if (neighbors[i] != null) {
							if (neighbors[i].cellType == cellType
								|| neighbors[i].cellType == CellType.Island) {
								return true;
							}
						}
					}
				}
				break;
		}

		return false;
	}

	/*public bool TypeAllowedToDestroy (CellType cellType) {
		switch (cellType) {
			case CellType.Water:
			case CellType.Mountain:
			case CellType.Island:
			return false;

			case CellType.Player1:
				if (GameController.instance.playerActive == GameController.PlayerActive.Player2) {
					return true;
				}

			break;

			case CellType.Player2:
				if (GameController.instance.playerActive == GameController.PlayerActive.Player1) {
					return true;
				}
			break;
		}
		
		return false;
	}*/
}
