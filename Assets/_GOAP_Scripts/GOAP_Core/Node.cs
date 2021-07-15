using System.Collections.Generic;

namespace GOAP_Core
{
    [System.Serializable]
    public class Node
    {
        private float cost;
        private Node parentNode;
        private Dictionary<string, int> stateDictionary;
        private GAction action;
        
        public float Cost => cost;
        public Node ParentNode => parentNode;
        public Dictionary<string, int> StateDictionary => stateDictionary;
        public GAction Action => action;
        
        public Node(Node parentNode, float cost, Dictionary<string, int> allStatesDictionary, GAction action)
        {
            this.parentNode = parentNode;
            this.cost = cost;
            this.stateDictionary = new Dictionary<string, int>(allStatesDictionary);
            this.action = action;
        }

        public Node(Node parentNode, float cost, Dictionary<string, int> allStatesDictionary, Dictionary<string, int> beliefStatesDictionary, GAction action)
        {
            this.parentNode = parentNode;
            this.cost = cost;
            this.stateDictionary = new Dictionary<string, int>(allStatesDictionary);
            foreach (KeyValuePair<string, int> beliefState in beliefStatesDictionary)
            {
                if (!stateDictionary.ContainsKey(beliefState.Key))
                {
                    stateDictionary.Add(beliefState.Key, beliefState.Value);
                }
            }
            this.action = action;
        }
    }
}