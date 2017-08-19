using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetDatabaseCreator : MonoBehaviour {
	[MenuItem("Assets/Create/InventoryList")]
	public static void CreateInventoryDB () {
		InventoryList asset = ScriptableObject.CreateInstance<InventoryList>();

		AssetDatabase.CreateAsset(asset, "Assets/InventoryList.asset");
		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();
	}
}
