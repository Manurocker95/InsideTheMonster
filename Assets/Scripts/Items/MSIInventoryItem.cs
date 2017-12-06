using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSIInventoryItem : MonoBehaviour
{
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] protected bool canBeAddedToInventory = false;
    [SerializeField] private string otherItemName = "";
    [SerializeField] private string combinationResultName = "";

    private float fixedPlayerY = 0;

    public Sprite getInventorySprite { get { return inventorySprite; } }
    public bool inventoriable { get { return inventoriable; } }
    public string otherName { get { return otherItemName; } }
    public string combinationName { get { return combinationResultName; } }
    public string itemName { get; private set; }

    // Use this for initialization
    void Start ()
    {
        itemName = gameObject.name;

    }

    public virtual void DropToScene()
    {
        transform.position = new Vector2(MSIDragon.Instance.transform.position.x, MSIDragon.Instance.transform.position.y - fixedPlayerY);

        MSIDragon.Instance.addWaitingTime();
    }

    public virtual void HandlePlayerCollision()
    {
        if(canBeAddedToInventory)
        {
            fixedPlayerY = MSIDragon.Instance.transform.position.y - transform.position.y;
            MSIInventoryManager.Instance.AddItem(this);
            gameObject.SetActive(false);
        }
    }
}
