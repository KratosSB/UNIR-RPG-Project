using GOAP_Resources;
using UnityEngine;

namespace GOAP_Core
{
    [System.Serializable]
    public class WorldState
    {
        [SerializeField] [FixedEnumNames] private StatesEnum states;  
        [SerializeField] private int value;

        public int Value => value;
        public StatesEnum States => states;
    }
}