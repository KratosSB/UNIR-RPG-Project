using System.Collections.Generic;
using GOAP_Resources;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP_Core
{
    public abstract class GAction : MonoBehaviour
    {
        private ActionTypeEnum actionType;
        [SerializeField] private string actionName = "Action";
        [SerializeField] private float cost = 1.0f;
        [SerializeField] private float duration = 0f;
        [SerializeField] private WorldState[] preConditionsArray = null;
        [SerializeField] private WorldState[] afterEffectsArray = null;
        [SerializeField] private bool running = false;
        
        private GameObject target;
        
        private bool loopAction = false;
        
        private Dictionary<string, int> preConditionsDictionary = null;
        private Dictionary<string, int> afterEffectsDictionary = null;
        private WorldStates agentBeliefs = null;
        private GInventory inventory;
        
        public Dictionary<string, int> AfterEffectsDictionary => afterEffectsDictionary;
        public Dictionary<string, int> PreConditionsDictionary => preConditionsDictionary;


        public float Cost => cost;
        public ActionTypeEnum ActionType
        {
            get => actionType;
            set => actionType = value;
        }

        public string ActionName
        {
            get => actionName;
            set => actionName = value;
        }

        public bool Running
        {
            get => running;
            set => running = value;
        }
        
        public bool LoopAction
        {
            get => loopAction;
            set => loopAction = value;
        }

        public float Duration => duration;
        public GInventory Inventory => inventory;
        public WorldStates AgentBeliefs => agentBeliefs;

        public GameObject Target
        {
            get => target;
            set => target = value;
        }

        public GAction()
        {
            preConditionsDictionary = new Dictionary<string, int>();
            afterEffectsDictionary = new Dictionary<string, int>();
        }

        private void Awake()
        {
            if (preConditionsArray != null)
            {
                foreach (WorldState ws in preConditionsArray)
                {
                    preConditionsDictionary.Add(ws.States.ToString(), ws.Value);
                
                }
            }
            if (afterEffectsArray != null)
            {
                foreach (WorldState ws in afterEffectsArray)
                {
                    afterEffectsDictionary.Add(ws.States.ToString(), ws.Value);
                }
            }

            inventory = GetComponent<GAgent>().Inventory;
            agentBeliefs = GetComponent<GAgent>().Beliefs;
        }

        public bool IsAchievable()
        {
            return true;
        }

        public bool IsAchievableGiven(Dictionary<string,int> conditions)
        {
            foreach (KeyValuePair<string,int> precondition in preConditionsDictionary)
            {
                if (!conditions.ContainsKey(precondition.Key))
                {
                    return false;
                }
            }

            return true;
        }
        
        public abstract bool PrePerform();
        public abstract bool Update_Action();
        public abstract bool PostPerform();
        public abstract bool PostPerformCleanUp();  //NEW Cancel Action
        //This is where resources can be added back to the queue or where states can be updated.
        //This is stuff that needs to be done if the action is completed or abandoned part way through.
        //The PostPerform() now just contains code for when the action is completed.
        public abstract bool CheckCompleted();
    }
}
