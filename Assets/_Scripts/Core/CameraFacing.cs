namespace RPG.Core
{
    using Cinemachine;
    using UnityEngine;

    public class CameraFacing : MonoBehaviour
    {
        [SerializeField] CinemachineFreeLook playerFramingCamera;

        private void Start()
        {
            playerFramingCamera = GameObject.FindGameObjectWithTag("PlayerFramingCamera")
                .GetComponent<CinemachineFreeLook>();
        }

        void LateUpdate()
        {
            if (playerFramingCamera)
            {
                transform.LookAt(2 * transform.position - playerFramingCamera.transform.position);
            }
            else
            {
                transform.forward = Camera.main.transform.forward;
            }
        }
    }
}
