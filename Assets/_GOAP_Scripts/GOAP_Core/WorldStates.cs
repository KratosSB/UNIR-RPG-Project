using System.Collections.Generic;
using GOAP_Resources;

namespace GOAP_Core
{
    public class WorldStates
    {
        private Dictionary<string, int> states;

        public WorldStates()
        {
            states = new Dictionary<string, int>();
        }

        public bool HasState(string key)
        {
            return states.ContainsKey(key);
        }

        public void AddState(string key, int value)
        {
            states.Add(key, value);
        }
        
        public void AddState(StatesEnum key, int value)
        {
            AddState(key.ToString(), value);
        }

        public void ModifyState(string key, int value)
        {
            if (states.ContainsKey(key))
            {
                states[key] += value;
                if (states[key] <= 0)
                {
                    RemoveState(key);
                }
            }
            else
            {
                states.Add(key, value);
            }
        }

        public void ModifyState(StatesEnum key, int value)
        {
            ModifyState(key.ToString(), value);
        }

        public void RemoveState(string key)
        {
            if (states.ContainsKey(key))
            {
                states.Remove(key);
            }
        }
        
        public void RemoveState(StatesEnum key)
        {
            RemoveState(key.ToString());
        }

        public void SetState(string key, int value)
        {
            if (states.ContainsKey(key))
            {
                states[key] = value;
            }
            else
            {
                states.Add(key, value);
            }
        }

        public Dictionary<string, int> GetStates()
        {
            return states;
        }
    }
}