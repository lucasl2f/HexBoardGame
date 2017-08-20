using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;
	public HexGrid hexGrid;

	[SerializeField]
	Toggle[] toggles;

	Color activeColor;
	CellType activeCellType;

	void Awake () {
		SelectColor(0);
	}

	void Update () {
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			HandleInput();
		}
		if (Input.GetKeyDown("0")) {
			toggles[0].isOn = true;
		}
		if (Input.GetKeyDown("1")) {
			toggles[1].isOn = true;
		}
		if (Input.GetKeyDown("2")) {
			toggles[2].isOn = true;
		}
		if (Input.GetKeyDown("3")) {
			toggles[3].isOn = true;
		}
		if (Input.GetKeyDown("4")) {
			toggles[4].isOn = true;
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
			hexGrid.ColorCell(hit.point, activeColor, activeCellType);
		}
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
		activeCellType = (CellType)index;
	}
}
