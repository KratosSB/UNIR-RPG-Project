using System.Collections.Generic;
using UnityEngine;

namespace GOAP_Core
{
    public class GInventory
    {
        private List<GameObject> itemList = new List<GameObject>();
        public List<GameObject> ItemList => itemList;

        public void AddItem(GameObject itemToAdd)
        {
            itemList.Add(itemToAdd);
        }

        public GameObject FindItemWithTag(string tagToFind)
        {
            foreach (GameObject item in itemList)
            {
                if (item == null) break; //Avoid error on resource deletion
                if (item.CompareTag(tagToFind))
                {
                    return item;
                }
            }

            return null;
        }

        public void RemoveItem(GameObject itemToRemove)
        {
            itemList.Remove(itemToRemove);
        }
    }
}
