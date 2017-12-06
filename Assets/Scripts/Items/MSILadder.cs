using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSILadder : MSIInventoryItem
{
    public Transform ladderTargetTransform;
    public GameObject ladderPositionCollider;
    public float forceFixedY;
    public float rotateSpeed = 2f;

    bool rotating = false;
    float rotateTime = 4;


    public override void DropToScene()
    {
        if (MSIDragon.Instance.isInContactWithLadder)
        {
            transform.position = ladderTargetTransform.position;
            transform.rotation = ladderTargetTransform.rotation;
            canBeAddedToInventory = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
            ladderPositionCollider.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.position = new Vector2(MSIDragon.Instance.transform.position.x, MSIDragon.Instance.transform.position.y - forceFixedY);
            transform.rotation = Quaternion.identity;
        }
        MSIDragon.Instance.addWaitingTime();
    }

    void Update()
    {
        if(rotating && rotateTime > 0)
        {
            rotateTime -= Time.deltaTime;
            transform.Rotate(transform.position, rotateSpeed);

        }
    }

    public void RotateToLaunch()
    {
        rotating = true;
    }
}
