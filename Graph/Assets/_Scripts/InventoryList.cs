using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryList : ScriptableObject {
	public List<Inventory> itemList;

	public void SortList () {
		if (itemList.Count > 0) {
			itemList.Sort(delegate (Inventory a, Inventory b) {
				return (a.itemPrice).CompareTo(b.itemPrice);
			});
		}
	}
}
