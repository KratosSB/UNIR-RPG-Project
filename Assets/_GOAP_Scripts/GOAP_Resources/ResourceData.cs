using UnityEngine;

[CreateAssetMenu(fileName = "GTypeResource", menuName = "GOAP/GOAP resource data", order = 1)]
public class ResourceData : ScriptableObject
{
    [SerializeField] private string resourceTag;
    [SerializeField] private  string resourceQueue;
    [SerializeField] private  string resourceState;

    public string ResourceTag => resourceTag;
    public string ResourceQueue => resourceQueue;
    public string ResourceState => resourceState;
}
