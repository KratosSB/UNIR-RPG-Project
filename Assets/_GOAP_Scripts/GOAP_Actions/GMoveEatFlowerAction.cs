using System;
using GOAP_Core;
using GOAP_Resources;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP_Actions
{
    public class GMoveEatFlowerAction : GAction
    {
        private Vector3 destination;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        private void Reset()
        {
            ActionName = GetType().Name;
        }

        private void Start()
        {
            ActionType = ActionTypeEnum.Movement;
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        public override bool PrePerform()
        {
            Target = GWorld.Instance.GetResourceQueueDictionary(ListsEnum.ListFlowers).RemoveResource();
            if (Target == null) return false;
            Inventory.AddItem(Target);
            GWorld.Instance.GetWorld().ModifyState(StatesEnum.state_free_flower, -1);
            return true;
        }

        public override bool Update_Action()
        {
            if (Target != null)
            {
                Running = true;
                LoopAction = false;
                destination = Target.transform.position;
                _navMeshAgent.SetDestination(destination);
                _animator.SetBool("isLookingOut", false);
                _animator.SetBool("isRunning", true);
                return true;
            }
            return false;
        }

        public override bool PostPerform()
        {
            GWorld.Instance.GetResourceQueueDictionary(ListsEnum.ListFlowers).AddResource(Target);
            Inventory.RemoveItem(Target);
            GWorld.Instance.GetWorld().ModifyState(StatesEnum.state_free_flower, 1);
            AgentBeliefs.RemoveState(StatesEnum.belief_starving);
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isLookingOut", false);
            return true;
        }
        
        public override bool PostPerformCleanUp()
        {
            return true;
        }

        public override bool CheckCompleted()
        {
            //Check distance to target
            float distanceToTarget = Vector3.Distance(destination, this.transform.position);
            if (distanceToTarget < 2.0f)
            {
                _animator.SetBool("isRunning", false);
                _animator.SetBool("isLookingOut", false);
                return true;
            }
            return false;
        }
    }
}