using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EditorTools {

	public static void DebugObj (System.Object obj) {
		//string name = DebugNameAndValue(() => obj);
		//Debug.Log(name + ": " + obj.ToString());
	}

	public static string DebugNameAndValue <T> (Expression<Func<T>> memberExpression, System.Object obj) {
		MemberExpression expressionBody = (MemberExpression)memberExpression.Body;

		Debug.Log(expressionBody.Member);
		Debug.Log(expressionBody.Expression);
		Debug.Log(expressionBody.Member);

		Debug.Log(expressionBody.Member.Name + ": " + obj.ToString());

		return expressionBody.Member.Name;
	}
}
