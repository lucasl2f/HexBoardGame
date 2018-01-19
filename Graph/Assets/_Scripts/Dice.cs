using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Dice
{
    public static int RollD6()
    {
        int result = Random.Range(1, 7);
        Debug.Log("D6 result: " + result.ToString());
        return result;
    }
}
