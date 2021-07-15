using GOAP_Core;
using UnityEngine;

namespace GOAP_Utils
{
    [ExecuteInEditMode]
    public class GAgentVisual : MonoBehaviour
    {
        public GAgent thisAgent;
        void Start()
        {
            thisAgent = this.GetComponent<GAgent>();
        }
    }
}
