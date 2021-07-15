using GOAP_Core;
using GOAP_Resources;
using UnityEngine;

namespace GOAP_Agents
{
    public class GAgentRabbit : GAgent
    {
        private void Start()
        {
            base.StartAgent();
            SubGoal s1 = new SubGoal(StatesEnum.goal_eat_flower, 1, false);
            GoalsDictionary.Add(s1, 3);
            SubGoal s2 = new SubGoal(StatesEnum.goal_wander, 1, false);
            GoalsDictionary.Add(s2, 2);
            Beliefs.ModifyState(StatesEnum.belief_full_feeded, 0);

            Invoke("Starve", Random.Range(2.0f, 5.0f));
        }

        private void Starve()
        {
            Beliefs.ModifyState(StatesEnum.belief_starving, 0);
            Invoke("Starve", Random.Range(5.0f, 10.0f));
        }
    }
}