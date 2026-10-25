using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTasks : MonoBehaviour
{
    public List<PlayerTaskProgress> playerTasks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTasks = TaskManager.Instance.SetTasksForPlayer(5, this);
    }

    public bool HaveQuest(TaskData task)
    {
        foreach (var playerTask in playerTasks)
        {
            if (playerTask.taskData == task)
            {
                return true;
            }
        }
        return false;
    }
}

[Serializable]
public struct PlayerTaskProgress
{
    public TaskData taskData;
    public int amount;
    public bool isCompleted;
}
