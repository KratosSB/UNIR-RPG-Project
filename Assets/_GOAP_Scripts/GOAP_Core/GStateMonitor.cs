using GOAP_Core;
using GOAP_Resources;
using UnityEngine;

public class GStateMonitor : MonoBehaviour
{
    [SerializeField] private string state;
    [FixedEnumNames] [SerializeField] private StatesEnum stateEnum;
    [SerializeField] private float stateStrength;
    [SerializeField] private float stateDecayRate;
    private WorldStates beliefs;
    [SerializeField] private GameObject resourcePrefab;
    [SerializeField] private string queueName;
    [FixedEnumNames] [SerializeField] private ListsEnum listsNameEnum;
    [SerializeField] private string worldState;
    [FixedEnumNames] [SerializeField] private StatesEnum worldStateEnum;
    [SerializeField] private GAction action;

    private bool stateFound = false;
    private float initialStrength;

    void Awake()
    {
        beliefs = GetComponent<GAgent>().Beliefs;
        initialStrength = stateStrength;
    }

    void LateUpdate()
    {
        if (action.Running)
        {
            stateFound = false;
            stateStrength = initialStrength;
        }

        if (!stateFound && beliefs.HasState(state))
        {
            stateFound = true;
        }

        if (stateFound)
        {
            stateStrength -= stateDecayRate * Time.deltaTime;
            if (stateStrength <= 0)
            {
                Vector3 location = new Vector3(transform.position.x, resourcePrefab.transform.position.y, transform.position.z);
                GameObject prefabResource = Instantiate(resourcePrefab, location, resourcePrefab.transform.rotation);
                stateFound = false;
                stateStrength = initialStrength;
                beliefs.RemoveState(state);
                GWorld.Instance.GetResourceQueueDictionary(queueName).AddResource(prefabResource);
                GWorld.Instance.GetWorld().ModifyState(worldState, 1);
            }
        }
    }
}
