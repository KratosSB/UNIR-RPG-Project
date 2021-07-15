using System.Collections.Generic;
using RPG.Core;
using RPG.Attributes;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] private float maxSpeed = 5.66f;
        [SerializeField] private float maxNavPathLength = 30f;
        
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private Health _health;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _animator = GetComponent<Animator>();
        }
        
        //Route calculation based on whether or not the character is dead
        void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        //Starts the move action and invokes MoveTo
        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        //Checks if there is a valid path to the destination
        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            bool hasPAth =  NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navMeshPath);
            if (!hasPAth || navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLenght(navMeshPath) > maxNavPathLength) return false;

            return true;
        }

        //Set the agent's destination point and its speed
        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;
        }

        //Cancels the agent movement
        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }
    
        //Updates agent animation based on position and speed
        private void UpdateAnimator()
        {
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);  //Local velocity
            float speed = localVelocity.z;
            _animator.SetFloat("forwardSpeed", speed);
        }

        //Calculates the distance of the specified path
        private float GetPathLenght(NavMeshPath navMeshPath)
        {
            float total = 0;
            if (navMeshPath.corners.Length < 2) return total;
            for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
            {
                float distance = Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
                total += distance;
                //Fix - exit earlier
                if (total > maxNavPathLength)
                {
                    return total;
                }
            }

            return total;
        }
        
        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>) state;

            //Awake fix -> avoid navmesh not initialized at portals loading
            if (_navMeshAgent != null)
            {
                //Todo enable this after disable load position of other scene at portal
                _navMeshAgent.enabled = false;
                transform.position = ((SerializableVector3) data["position"]).ToVector();
                _navMeshAgent.enabled = true;
            }
            else
            {
                transform.position = ((SerializableVector3) data["position"]).ToVector();
            }
            transform.eulerAngles = ((SerializableVector3) data["rotation"]).ToVector();
        }
        
        [System.Serializable]
        private struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        private object CaptureStateStruct()
        {
            MoverSaveData saveData = new MoverSaveData();
            saveData.position = new SerializableVector3(transform.position);
            saveData.rotation = new SerializableVector3(transform.eulerAngles);
            return saveData;
        }
        
        public void RestoreStateStruct(object state)
        {
            MoverSaveData data = (MoverSaveData) state;
            if (_navMeshAgent != null)
            {
                _navMeshAgent.Warp(data.position.ToVector());
            }
            else
            {
                transform.position = data.position.ToVector();
            }
            transform.eulerAngles = data.rotation.ToVector();
        }
    }
}