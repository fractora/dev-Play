using UnityEngine;

public class TaskArea : MonoBehaviour
{
    [SerializeField]
    private TaskData task;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2D>().canInteract = true;
            collision.GetComponent<PlayerController2D>().currentTaskArea = this;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController2D>().canInteract = false;
            collision.GetComponent<PlayerController2D>().currentTaskArea = null;
        }
    }

    public void OnPlayerInteract(PlayerController2D player)
    {
        PlayerTasks playerTasks = player.GetComponent<PlayerTasks>();

        if(playerTasks.HaveQuest(task))
        {
            TaskManager.Instance.UpdatePlayerTask(playerTasks.playerTasks.Find(t => t.taskData == task));
        }
    }
}
