using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple script which assigns the amount of score the attached object is worth.
/// </summary>
public class TreasureValue : MonoBehaviour
{

    ///The value of the treasure
    [SerializeField]
    private int treasureValue = 0;

    /// <summary>
    /// Returns the value of the treasure object
    /// </summary>
    /// <returns></returns>
    /// Treasure Value
    public int getTreasureValue()
    {
        return this.treasureValue;
    }
}
