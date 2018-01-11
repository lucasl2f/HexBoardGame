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
	CellType _activeCellType;

	bool _destruction;

	public bool destruction {
		get {
			return _destruction;
		}
	}

	[SerializeField]
	Toggle destructionToggle;

	public CellType activeCellType {
		get {
			return _activeCellType;
		}	
	}

	public static HexMapEditor instance;

	void Awake () {
		instance = this;

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
			if (_destruction) {
				hexGrid.DestroyCells(hit.point, Dice.RollD6());
			} else {
				hexGrid.ColorCell(hit.point, activeColor, _activeCellType);
			}
		}
	}

	public void SelectColor (int index) {
		activeColor = colors[index];
		_activeCellType = (CellType)index;
	}

	public void SetDestruction () {
		_destruction = destructionToggle.isOn;		
	}
}
