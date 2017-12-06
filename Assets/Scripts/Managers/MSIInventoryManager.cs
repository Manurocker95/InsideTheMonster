using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSIInventoryManager : MonoBehaviour
{
    private static MSIInventoryManager instance;
    public static MSIInventoryManager Instance { get { return instance; } }

    [SerializeField] private Button[] vInventoryButtons = new Button[2];
    [SerializeField] private Button CombinationButton;

    private MSIInventoryItem[] vItems = new MSIInventoryItem[2];
    private int itemCount = 0;

    // Use this for initialization
    void Start ()
    {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
        CombinationButton.gameObject.SetActive(false);
	}
	
    public void AddItem(MSIInventoryItem item)
    {
        if(itemCount >= 2)
        {
            DropItem(0);
        }

        vItems[itemCount] = item;
        vInventoryButtons[itemCount].image.sprite = item.getInventorySprite;
        itemCount++;

        if(itemCount == 2)
            CheckItemCombination(vItems[0], vItems[1], true);
    }

    public void DropItem(int arrayPos)
     {
        vItems[arrayPos].gameObject.SetActive(true);
        vItems[arrayPos].DropToScene();
        vItems[arrayPos] = null;
        vInventoryButtons[arrayPos].image.sprite = null;
        if(arrayPos == 0)
        {
            vItems[0] = vItems[1];
            vInventoryButtons[0].image.sprite = vInventoryButtons[1].image.sprite;
            vItems[1] = null;
            vInventoryButtons[1].image.sprite = null;
        }
        itemCount--;
        CombinationButton.gameObject.SetActive(false);
    }

    public void DropItem(MSIInventoryItem item)
    {
        for (int i = 0; i < itemCount; i++)
        {
            if (item.Equals(vItems[i]))
            {
                DropItem(i);
                return;
            }
        }
        Debug.LogError("Item " + item.itemName + " can't be dropped");
    }
    public void DropItem(string itemName)
    {
        for(int i = 0; i < itemCount; i++)
        {
            if(itemName.Equals(vItems[i].itemName))
            {
                DropItem(i);
                return;
            }
        }
        Debug.LogError("Item " + itemName + " can't be dropped");
    }

    public bool CheckItemCombination(MSIInventoryItem item1, MSIInventoryItem item2, bool fromInventory)
    {
        if(!item1.otherName.Equals("") && !item2.otherName.Equals("") &&
            item1.otherName.Equals(item2.itemName) && item2.otherName.Equals(item1.itemName))
        {
            if(fromInventory)
            {
                CombinationButton.gameObject.SetActive(true);
                CombinationButton.image.sprite = Resources.Load<Sprite>("InventorySprites/" + item1.combinationName);
            }
            return true;
        }
        else
        {
            if (fromInventory)
            {
                CombinationButton.gameObject.SetActive(false);
            }
                return false;
        }
    }

    public void CombineItems(MSIInventoryItem item1, MSIInventoryItem item2, bool fromInventory)
    {
        GameObject newObject = Instantiate(Resources.Load("SceneItemPrefabs/" + item1.combinationName)) as GameObject;

        MSIInventoryItem component = newObject.GetComponent<MSIInventoryItem>();

        if (fromInventory)
        {
            DeleteInventoryItem(item1);
            DeleteInventoryItem(item2);
            AddItem(component);
            CombinationButton.gameObject.SetActive(false);
        }
        newObject.SetActive(false);
    }

    public void CombineItemsInInventory()
    {
        CombineItems(vItems[0], vItems[1], true);
    }

    public void DeleteInventoryItem(MSIInventoryItem item)
    {
        for (int i = 0; i < itemCount; i++)
        {
            if (item.Equals(vItems[i]))
            {
                if(i == 0)
                {
                    Destroy(vItems[0].gameObject);
                    vItems[0] = vItems[1];
                    vInventoryButtons[0].image.sprite = vInventoryButtons[1].image.sprite;
                    vItems[1] = null;
                    vInventoryButtons[1].image.sprite = null;
                    
                    itemCount--;
                }
                else
                {
                    vInventoryButtons[1].image.sprite = null;
                    Destroy(vItems[1].gameObject);
                }
                return;
            }
        }
    }

}
