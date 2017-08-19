using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryList))]
public class CustomInventoryList : Editor {

	InventoryList db;
	int i, itemListCount;

	void OnEnable () {
		db = (InventoryList)target;
	}

	public override void OnInspectorGUI () {

		GUILayout.BeginVertical("box");
		GUILayout.Space(5);
		GUILayout.Label("INVENTORY ITEMS",
		                EditorStyles.centeredGreyMiniLabel);
		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		GUILayout.Space(5);
		if (GUILayout.Button(" + ")) {
			AddItem();
		}
		GUILayout.Space(5);
		if (GUILayout.Button("Sort by price")) {
			db.SortList();
		}
		GUILayout.Space(5);
		GUILayout.EndHorizontal();
		GUILayout.Space(5);

		GUILayout.EndVertical();

		GUILayout.Space(20);

		itemListCount = db.itemList.Count;
		for (i = 0; i < itemListCount; i++) {
			GUILayout.BeginHorizontal("box");

			db.itemList[i].itemIcon = (Texture2D)EditorGUILayout.ObjectField("",
			                                                                 db.itemList[i].itemIcon,
			                                                                 typeof(Texture2D),
			                                                                 true,
			                                                                 GUILayout.MaxWidth(50));

			db.itemList[i].itemName = GUILayout.TextField(db.itemList[i].itemName,
			                                              GUILayout.Width(100));

			GUILayout.BeginVertical("box");

			GUILayout.BeginHorizontal();
			GUILayout.Label("Price");
			db.itemList[i].itemPrice = EditorGUILayout.IntField(db.itemList[i].itemPrice);

			GUILayout.Label("Weight");
			db.itemList[i].itemWeight = EditorGUILayout.FloatField(db.itemList[i].itemWeight);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			db.itemList[i].itemConsumable = EditorGUILayout.Toggle("Consumable",
			                                                       db.itemList[i].itemConsumable);

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			db.itemList[i].itemRare = EditorGUILayout.Toggle("Rare",
			                                                 db.itemList[i].itemRare);
			
			GUILayout.EndHorizontal();

			/*GUILayout.BeginHorizontal();
			db.itemList[i].itemObj = (GameObject)EditorGUI.ObjectField(new Rect(50,
			                                                                    100,
			                                                                    80,
			                                                                    40),
			                                                           "Object",
			                                                           db.itemList[i].itemObj,
			                                                           typeof(GameObject));

			if (GUI.Button(new Rect(3, 25, 100, 20), "Check Dependencies"))
				Selection.objects = EditorUtility.CollectDependencies(new GameObject[] { db.itemList[i].itemObj });
			else
				EditorGUI.LabelField(new Rect(3, 40, 100, 80),
				                     "Missing:",
				                     "Select an object first");

			GUILayout.EndHorizontal();*/

			GUILayout.EndVertical();

			if (GUILayout.Button(" - ", GUILayout.Width(20), GUILayout.Height(20))) {
				RemoveItem(i);
				return;
			}

			GUILayout.EndHorizontal();
		}

		//base.OnInspectorGUI();
	}

	void AddItem () {
		db.itemList.Add(new Inventory());
	}

	void RemoveItem (int index) {
		db.itemList.RemoveAt(index);
	}

	void OnDisable () {
		Debug.Log("Saving");
		AssetDatabase.SaveAssets();
	}
}
