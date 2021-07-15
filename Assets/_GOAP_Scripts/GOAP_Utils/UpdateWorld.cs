using System.Collections.Generic;
using GOAP_Core;
using UnityEngine;
using UnityEngine.UI;

namespace GOAP_Utils
{
    public class UpdateWorld : MonoBehaviour
    {
        [SerializeField] private Text states;

        void LateUpdate()
        {
            Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
            states.text = "";
            foreach (KeyValuePair<string,int> state in worldStates)
            {
                states.text += state.Key + ", " + state.Value + "\n";
            }
        }
    }
}
