using UnityEngine;

[CreateAssetMenu(fileName = "TaskData", menuName = "Scriptable Objects/TaskData")]
public class TaskData : ScriptableObject
{
    public string name;
    public TaskType taskType;

    [Header("Collect Task")]
    public int collectAmount;
    public ObjectData collectObject;

    [Header("Collect and Deliver Task")]
    public int deliverAmount;
    public ObjectData deliverObject;
    public string deliverZoneName;

    [Header("Explore Task")]
    public string exploreZoneName;

    [Header("Activate Task")]
    public string activateZoneName;
    public int activateAmount;
}

public enum TaskType
{
    Collect,
    CollectAndDeliver,
    Explore,
    Activate,
}
