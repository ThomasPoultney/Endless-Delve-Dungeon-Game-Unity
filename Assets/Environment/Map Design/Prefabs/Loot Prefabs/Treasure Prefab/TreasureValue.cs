using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureValue : MonoBehaviour
{
    [SerializeField]
    private int treasureValue = 0;

    public int getTreasureValue()
    {
        return this.treasureValue;
    }
}
