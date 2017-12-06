using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSIRock : MSIInventoryItem
{
    public Transform RockTargetTransform;
    public float forceFixedY;
    public float flyTime = 3f;

    public override void DropToScene()
    {
        if (MSIDragon.Instance.isOnLadder)
        {
            transform.position = RockTargetTransform.position;
            transform.rotation = RockTargetTransform.rotation;
            transform.parent = RockTargetTransform;
            canBeAddedToInventory = false;
            GetComponent<BoxCollider2D>().isTrigger = true;
            MSIDragon.Instance.GetLaunched(flyTime);
        }
        else
        {
            transform.position = new Vector2(MSIDragon.Instance.transform.position.x, MSIDragon.Instance.transform.position.y - forceFixedY);
            transform.rotation = Quaternion.identity;
        }
        MSIDragon.Instance.addWaitingTime();
    }
}
