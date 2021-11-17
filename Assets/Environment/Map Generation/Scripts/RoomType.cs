using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public enum RoomEntrances { LRTB, LRB, LR, LRT };
    public RoomEntrances roomType;

    public void destroyRoom()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
        Destroy(gameObject);

    }
}
