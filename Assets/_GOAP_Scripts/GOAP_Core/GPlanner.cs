using System.Collections.Generic;
using UnityEngine;

namespace GOAP_Core
{
    public class GPlanner
    {
        public Queue<GAction> Plan(List<GAction> actionsList, Dictionary<string, int> goalDictionary, WorldStates beliefStates)
        {
            List<GAction> usableActionsList = new List<GAction>();
            foreach (GAction gAction in actionsList)
            {
                if (gAction.IsAchievable())
                {
                    usableActionsList.Add(gAction);
                }
            }

            List<Node> leavesList = new List<Node>();
            Node startNode = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

            bool success = BuildGraph(startNode, leavesList, usableActionsList, goalDictionary);

            if (!success)
            {
                Debug.Log("No plan achieved");
                return null;
            }

            Node cheapest = null;
            foreach (Node leaf in leavesList)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else
                {
                    if (leaf.Cost < cheapest.Cost)
                    {
                        cheapest = leaf;
                    }
                }
            }

            List<GAction> resultPlan = new List<GAction>();
            Node node = cheapest;
            while (node != null)
            {
                if (node.Action != null)
                {
                    resultPlan.Insert(0, node.Action);
                }

                node = node.ParentNode;
            }
        
            Queue<GAction> actionQueue = new Queue<GAction>();
            foreach (GAction gAction in resultPlan)
            {
                actionQueue.Enqueue(gAction);
            }

            Debug.Log("Plan achieved");

            return actionQueue;
        }

        private bool BuildGraph(Node parentNode, List<Node> leavesList, List<GAction> usableActionsList, Dictionary<string, int> goalDictionary)
        {
            bool foundPath = false;

            foreach (GAction gAction in usableActionsList)
            {
                if (gAction.IsAchievableGiven(parentNode.StateDictionary))
                {
                    Dictionary<string, int> currentStateDictionary =
                        new Dictionary<string, int>(parentNode.StateDictionary);
                    foreach (KeyValuePair<string, int> effectKeyValuePair in gAction.AfterEffectsDictionary)
                    {
                        if (!currentStateDictionary.ContainsKey(effectKeyValuePair.Key))
                        {
                            currentStateDictionary.Add(effectKeyValuePair.Key, effectKeyValuePair.Value);
                        }
                    }

                    Node newNode = new Node(parentNode, parentNode.Cost + gAction.Cost, currentStateDictionary, gAction);
                    if (GoalAchieved(goalDictionary, currentStateDictionary))
                    {
                        leavesList.Add(newNode);
                        foundPath = true;
                    }
                    else
                    {
                        List<GAction> subGActionsList = ActionSubset(usableActionsList, gAction);
                        bool found = BuildGraph(newNode, leavesList, subGActionsList, goalDictionary);
                        if (found)
                        {
                            foundPath = true;
                        }
                    }
                }
            }

            return foundPath;
        }

        private bool GoalAchieved(Dictionary<string, int> goalDictionary, Dictionary<string, int> currentStateDictionary)
        {
            foreach (KeyValuePair<string, int> goalKeyValuePair in goalDictionary)
            {
                if (!currentStateDictionary.ContainsKey(goalKeyValuePair.Key))
                {
                    return false;
                }
            }

            return true;
        }

        private List<GAction> ActionSubset(List<GAction> usableActionsList, GAction gActionToRemove)
        {
            List<GAction> gActionsSubset = new List<GAction>();
            foreach (GAction gAction in usableActionsList)
            {
                if (!gAction.Equals(gActionToRemove))
                {
                    gActionsSubset.Add(gAction);
                }
            }

            return gActionsSubset;
        }
    }
}