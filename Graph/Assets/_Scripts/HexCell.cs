using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour {
	public HexCoordinates coordinates;
	public Color color;

	public CellType cellType;

	[SerializeField]
	HexCell[] neighbors;

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
		switch (cellType) {
			case CellType.Water:
			case CellType.Mountain:
			case CellType.Island:
				return true;
				break;
			case CellType.Player1:
			case CellType.Player2:
				int i;
				for (i = 0; i < neighbors.Length; i++) {
					if (neighbors[i] != null) {
						if (neighbors[i].cellType == cellType
						    || neighbors[i].cellType == CellType.Island) {
							return true;
						}
					}
				}

				break;
		}

		return false;
	}
}
