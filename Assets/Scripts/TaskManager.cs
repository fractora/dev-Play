using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public List<TaskData> tasks;

    public static TaskManager Instance;

    public void Awake()
    {
        Instance = this;
    }

    //fonction pour definir les taches du joueur
    public List<PlayerTaskProgress> SetTasksForPlayer(int amount, PlayerTasks playerTasks)
    {
        int questNumber = tasks.Count;
        List<PlayerTaskProgress> selectedTasks = new List<PlayerTaskProgress>();

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, questNumber);
            TaskData selectedTask = tasks[randomIndex];
            selectedTasks.Add(new PlayerTaskProgress
            {
                taskData = selectedTask,
                amount = 0,
                isCompleted = false
            });
        }

        return selectedTasks;
    }

    //fonction pour mettre a jour le progress de la tache du joueur
    public void UpdatePlayerTask(PlayerTaskProgress taskProgress)
    {
        taskProgress.amount++;

        if (TaskIsCompleted(taskProgress))
        {
            taskProgress.isCompleted = true;
            print("Tache complete: " + taskProgress.taskData.name);
        }
        else
        {
            taskProgress.isCompleted = false;
        }
    }

    //fonction pour verifier si la tache est complete
    bool TaskIsCompleted(PlayerTaskProgress taskProgress)
    {
        switch (taskProgress.taskData.taskType)
        {
            case TaskType.Collect:
                return taskProgress.amount >= taskProgress.taskData.collectAmount;
            case TaskType.CollectAndDeliver:
                return taskProgress.amount >= taskProgress.taskData.deliverAmount;
            case TaskType.Explore:
                return taskProgress.amount >= 1; //on met 1 quand la zone a etait explorer
            case TaskType.Activate:
                return taskProgress.amount >= taskProgress.taskData.activateAmount;
            default:
                return false;
        }
    }
}
