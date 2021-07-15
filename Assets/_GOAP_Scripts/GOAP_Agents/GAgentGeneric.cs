using GOAP_Core;
using UnityEngine;

namespace GOAP_Agents
{
    public class GAgentGeneric : GAgent
    {
        private void Start()
        {
            base.StartAgent();
            SubGoal s1 = new SubGoal("goalName", 1, false);
            GoalsDictionary.Add(s1, 3);
        
            SubGoal s2 = new SubGoal("goalName", 1, false);
            GoalsDictionary.Add(s2, 2);
        
            SubGoal s3 = new SubGoal("goalName", 1, false);
            GoalsDictionary.Add(s3, 1);
        
            Invoke("Method1", Random.Range(10f, 20f));
            Invoke("Method2", Random.Range(1f, 5f));
        }

        private void Method1()
        {
            Beliefs.ModifyState("state", 0);
            Invoke("Method1", Random.Range(10f, 20f));
        }
    
        private void Method2()
        {
            Beliefs.ModifyState("state", 0);
            Invoke("Method2", Random.Range(1f, 5f));
        }
    }
}
