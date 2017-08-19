using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMethods : MonoBehaviour {
	public static int life = 5;
	public string lifeName;
	// Use this for initialization
	void Start () {
		EditorTools.DebugNameAndValue(() => life, life);
	}
}
