using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory {
	public string itemName = "New Item";
	public Texture2D itemIcon = null;
	public GameObject itemObj = null;
	public bool itemConsumable = false;
	public bool itemRare = false;
	public int itemPrice = 0;
	public float itemWeight = 0f;
}
