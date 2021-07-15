using System;
using GOAP_Core;
using GOAP_Resources;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP_Actions
{
    public class GMoveWanderAction : GAction
    {
        private Vector3 destination;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        
        public float wanderRadius;

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
            return true;
        }

        public override bool Update_Action()
        {
            Running = true;
                LoopAction = false;
                destination = RandomNavSphere(transform.position, wanderRadius, -1);
                _navMeshAgent.SetDestination(destination);
                _animator.SetBool("isLookingOut", false);
                _animator.SetBool("isRunning", true);
                return true;
        }

        public override bool PostPerform()
        {
            _animator.SetBool("isRunning", false);
            _animator.SetBool("isLookingOut", true);
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
                _animator.SetBool("isLookingOut", true);
                return true;
            } 
            return false;
        }
        
        public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);

            return navHit.position;
        }
    }
}