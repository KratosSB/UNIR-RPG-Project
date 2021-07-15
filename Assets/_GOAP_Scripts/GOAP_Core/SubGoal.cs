using System.Collections.Generic;
using GOAP_Resources;

namespace GOAP_Core
{
    [System.Serializable]
    public class SubGoal
    {
        private Dictionary<string, int> sGoals;
        private bool remove;
        
        public Dictionary<string, int> SGoals => sGoals;
        public bool Remove => remove;
        
        public SubGoal(string name, int priority, bool removeAtEnd)
        {
            InitSubGoal(name, priority, removeAtEnd);
        }
        
        public SubGoal(StatesEnum name, int priority, bool removeAtEnd)
        {
            InitSubGoal(name.ToString(), priority, removeAtEnd);
        }
        
        private void InitSubGoal(string name, int priority, bool removeAtEnd)
        {
            sGoals = new Dictionary<string, int>();
            sGoals.Add(name, priority);
            remove = removeAtEnd;
        }
    }
}