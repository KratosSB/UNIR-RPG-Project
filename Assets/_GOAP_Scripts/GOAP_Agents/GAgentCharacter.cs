using GOAP_Core;
using GOAP_Resources;
using UnityEngine;

namespace GOAP_Agents
{
    public class GAgentCharacter : GAgent
    {
        private void Start()
        {
            base.StartAgent();
            SubGoal s1 = new SubGoal(StatesEnum.goal_killPlayer, 1, true);
            GoalsDictionary.Add(s1, 3);
            
            Beliefs.ModifyState(StatesEnum.order_attackPlayer, 1);
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

/*
SubGoal s2 = new SubGoal("goalName", 1, false);
GoalsDictionary.Add(s2, 2);

SubGoal s3 = new SubGoal("goalName", 1, false);
GoalsDictionary.Add(s3, 1);

Invoke("Method1", Random.Range(10f, 20f));
Invoke("Method2", Random.Range(1f, 5f));
*/