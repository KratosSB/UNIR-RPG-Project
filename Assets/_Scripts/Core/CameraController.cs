namespace RPG.Core
{
    using UnityEngine;
    using Cinemachine;
    public class CameraController : MonoBehaviour
    {
        [SerializeField] GameObject freeLookCamera;
        CinemachineFreeLook freeLookComponent;
        //PlayerController playerControllerScript;

        private void Awake()
        {
            freeLookComponent = freeLookCamera.GetComponent<CinemachineFreeLook>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                freeLookComponent.m_XAxis.m_MaxSpeed = 500;
            }
            if (Input.GetMouseButtonUp(1))
            {
                freeLookComponent.m_XAxis.m_MaxSpeed = 0;
            }
            
            if (Input.mouseScrollDelta.y != 0)
            {
                freeLookComponent.m_YAxis.m_MaxSpeed = 100;
            }
        }
    }
}