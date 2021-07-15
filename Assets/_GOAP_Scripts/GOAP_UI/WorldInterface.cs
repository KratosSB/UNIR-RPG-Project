using GOAP_Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace GOAP_UI
{
    public class WorldInterface : MonoBehaviour
    {
        private ResourceData resourceData;
        private GameObject focusObject;
        private GameObject newResourcePrefab;
        public GameObject[] allResourceGameObjects;
        private Vector3 goalPosition;
        public NavMeshSurface navMeshSurface;
        public GameObject hospitalGameObject;
        private Vector3 clickOffset = Vector3.zero; //avoid move object on click
        private bool offsetCalculated = false;
        private bool deleteResource = false;
    
        [SerializeField] private bool resizeY = false;
        public void MouseOnHoverTrash()
        {
            deleteResource = true;
        }

        public void MouseOutHoverTrash()
        {
            deleteResource = false;
        }
        
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;  //Fix UI raycast to 3D world
            
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out hit)) return;

                offsetCalculated = false;
                clickOffset = Vector3.zero;

                Resource resource = hit.transform.gameObject.GetComponent<Resource>();

                if (resource != null)
                {
                    focusObject = hit.transform.gameObject;
                    resourceData = resource.Info;
                }
                else if (newResourcePrefab != null)
                {
                    goalPosition = hit.point;
                    if (resizeY)
                    {
                        goalPosition = new Vector3(hit.point.x, hit.point.y + (newResourcePrefab.GetComponent<Collider>().bounds.size.y / 2f), hit.point.z);
                    }
                    focusObject = Instantiate(newResourcePrefab, goalPosition, newResourcePrefab.transform.rotation);
                    resourceData = focusObject.GetComponent<Resource>().Info;
                }
                if (focusObject)
                {
                    focusObject.GetComponent<Collider>().enabled = false;
                }
            }
            else if (focusObject && Input.GetMouseButtonUp(0))
            {
                if (deleteResource)
                {
                    GWorld.Instance.GetResourceQueueDictionary(resourceData.ResourceQueue).RemoveResource(focusObject);
                    GWorld.Instance.GetWorld().ModifyState(resourceData.ResourceState, -1);
                    Destroy(focusObject);
                }
                else
                {
                    focusObject.transform.parent = hospitalGameObject.transform;
                    GWorld.Instance.GetResourceQueueDictionary(resourceData.ResourceQueue).AddResource(focusObject);
                    GWorld.Instance.GetWorld().ModifyState(resourceData.ResourceState, 1);
                    focusObject.GetComponent<Collider>().enabled = true;
                }

                //Add navmesh dynamically
                navMeshSurface.BuildNavMesh();
                focusObject = null;
            }
            else if (focusObject && Input.GetMouseButton(0))
            {
                int layerMask = 1 << 6; //Layer 6 is my floor layer
                RaycastHit hitMove;
                Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(rayMove, out hitMove, Mathf.Infinity, layerMask)) return;  //Only raycast floor

                if (!offsetCalculated)
                {
                    clickOffset = hitMove.point - focusObject.transform.position;
                    offsetCalculated = true;
                }
                goalPosition = hitMove.point - clickOffset;

                if (resizeY)
                {
                    goalPosition = new Vector3(hitMove.point.x, hitMove.point.y + (newResourcePrefab.GetComponent<Collider>().bounds.size.y / 2f), hitMove.point.z);
                }
                focusObject.transform.position = goalPosition;
            }

            if (focusObject && (Input.GetKeyDown(KeyCode.Less) || Input.GetKeyDown(KeyCode.Comma)))
            {
                focusObject.transform.Rotate(0, 90, 0);
            }
            else if (focusObject && (Input.GetKeyDown(KeyCode.Greater) || Input.GetKeyDown(KeyCode.Period)))
            {
                focusObject.transform.Rotate(0, -90, 0);
            }
        }
    }
}
