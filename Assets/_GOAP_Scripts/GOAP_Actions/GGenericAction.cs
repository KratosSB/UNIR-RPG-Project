using System;
using GOAP_Core;
using GOAP_Resources;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP_Actions
{
    [RequireComponent(typeof(GAgent))]
    [RequireComponent(typeof(Fighter))]
    [RequireComponent(typeof(Mover))]
    public class GGenericAction : GAction
    {
        private GAgent _agent;
        private Fighter _fighter;
        private Mover _mover;

        private void Reset()
        {
            ActionName = GetType().Name;
        }

        private void Start()
        {
            ActionType = ActionTypeEnum.Movement;
            _agent = GetComponent<GAgent>();
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
        }

        public override bool PrePerform()
        {
            //throw new NotImplementedException();
            return true;
        }
        
        public override bool Update_Action()
        {
            //throw new NotImplementedException();
            this.Running = true;
            return true;
        }
        
        public override bool PostPerform()
        {
            //throw new NotImplementedException();
            return true;
        }
        
        public override bool PostPerformCleanUp()
        {
            //throw new NotImplementedException();
            return true;
        }

        public override bool CheckCompleted()
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}