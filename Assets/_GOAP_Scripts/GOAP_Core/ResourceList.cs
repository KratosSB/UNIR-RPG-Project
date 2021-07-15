//Sealed class
using System.Collections.Generic;
using GOAP_Resources;
using UnityEngine;

namespace GOAP_Core
{
    public class ResourceList
    {
        private List<GameObject> gameObjectList = new List<GameObject>();
        private string tagName="";
        private string modState="";
        private string modBelief="";
        
        public ResourceList(string tagName, string modState, string modBelief, WorldStates worldStates)
        {
            if (tagName != StatesEnum._none_.ToString())
            {
                this.tagName = tagName;
            }

            this.modState = modState;
            this.modBelief = modBelief;
            
            if (tagName != StatesEnum._none_.ToString())
            {
                GameObject[] resources = GameObject.FindGameObjectsWithTag(tagName);
                foreach (GameObject gameObjectItem in resources)
                {
                    gameObjectList.Add(gameObjectItem);
                }
            }

            if (modState != "")
            {
                worldStates.ModifyState(modState, gameObjectList.Count);
            }
        }

        public List<GameObject> GameObjectList => gameObjectList;
        public string TagName => tagName;
        public string ModState => modState;
        public string ModBelief => modBelief;

        public void AddResource(GameObject resourceItem)
        {
            gameObjectList.Add(resourceItem);
        }

        public bool RemoveResource(GameObject resourceItem)
        {
            if (gameObjectList.Contains(resourceItem))
            {
                return gameObjectList.Remove(resourceItem);
            }

            return false;
        }

        public GameObject RemoveResource()
        {
            if (gameObjectList.Count == 0) return null;
            GameObject result = gameObjectList[gameObjectList.Count - 1];
            gameObjectList.Remove(result);
            return result;
        }
        
        public bool ContainsResource(GameObject resourceItem)
        {
            return gameObjectList.Contains(resourceItem);
        }
    }
}