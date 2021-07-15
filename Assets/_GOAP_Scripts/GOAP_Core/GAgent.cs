using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Attributes;
using UnityEngine;

namespace GOAP_Core
{
    public class GAgent : MonoBehaviour
    {
        [SerializeField] private string agentName = "John Doe";
        [SerializeField] private string agentId = Guid.NewGuid().ToString(); //Todo change this
        [SerializeField] private float maxSpeed = 5.66f;
        [SerializeField] private float maxNavPathLength = 30f;
        [SerializeField] private float remainingDistance = 2f;
        
        [SerializeField] private List<GAction> actionList = new List<GAction>();
        [SerializeField] private GAction currentAction = null;

        private Health _health;

        private Dictionary<SubGoal, int> goalsDictionary = new Dictionary<SubGoal, int>();
        private GInventory inventory = new GInventory();
        private WorldStates beliefs = new WorldStates();
        
        private GPlanner planner;
        private Queue<GAction> actionQueue;
        private SubGoal currentSubGoal = null;
        private bool invoked = false;
        
        public GAction CurrentAction => currentAction;
        public List<GAction> ActionList => actionList;
        public GInventory Inventory => inventory;
        public WorldStates Beliefs => beliefs;
        public Dictionary<SubGoal, int> GoalsDictionary => goalsDictionary;
        public float MAXSpeed => maxSpeed;
        public float MAXNavPathLength => maxNavPathLength;
        public float RemainingDistance => remainingDistance;
        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        public void StartAgent()
        {
            GAction[] actionsArray = GetComponents<GAction>();
            foreach (GAction action in actionsArray)
            {
                actionList.Add(action);
            }
        }

        private void CompleteAction()
        {
            currentAction.Running = false;
            currentAction.PostPerform();
            //currentAction.PostPerformCleanUp();  //NEW Cancel Action
            invoked = false;
        }

        //NEW Cancel Action
        void CancelAction()
        {
            currentAction.Running = false;
            currentAction.PostPerformCleanUp();
            invoked = false;
        }

        //NEW Cancel Action
        public void CancelCurrentGoal()
        {
            Debug.Log("Cancel this goal");

            // Cancel the CompleteAction method as this has a timer on it which we don't want to run
            CancelInvoke("CompleteAction");

            // Use CancelAction instead of CompleteAction
            CancelAction();

            // Remove the current action and queue
            currentAction = null;

            if (actionQueue.Count > 0)
                actionQueue.Clear();
        }

        void LateUpdate()
        {
            //Todo Move action end?
            
            //Waiting action to finish execution
            if (currentAction != null && currentAction.Running)
            {
                //Check conditions met and complete execution
                if (currentAction.CheckCompleted())
                {
                    InitCompleteAction();
                    return;
                }
                
                if (currentAction.LoopAction) currentAction.Update_Action();
                return;
            }

            if (planner == null || actionQueue == null)
            {
                planner = new GPlanner();
                var sortedGoals = from keyValuePair in goalsDictionary
                    orderby keyValuePair.Value descending
                    select keyValuePair;

                foreach (KeyValuePair<SubGoal, int> subGoal in sortedGoals)
                {
                    actionQueue = planner.Plan(actionList, subGoal.Key.SGoals, beliefs);
                    if (actionQueue != null)
                    {
                        Debug.Log(agentName + " (" + agentId + "): " + "Plan achieved");
                        currentSubGoal = subGoal.Key;
                        break;
                    }
                }
            }

            if (actionQueue != null && actionQueue.Count == 0)
            {
                if (currentSubGoal.Remove && goalsDictionary.Count > 0)
                {
                    goalsDictionary.Remove(currentSubGoal);
                }

                planner = null;
            }
            
            //New action to execute from the actions queue
            if (actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                
                if (currentAction.PrePerform())
                {
                    //Execute one time
                    currentAction.Update_Action();
                }
                else
                {
                    actionQueue = null;
                }
            }
        }

        private void InitCompleteAction()
        {
            if (!invoked)
            {
                Invoke(nameof(CompleteAction), currentAction.Duration);
                invoked = true;
            }
        }
    }
}
