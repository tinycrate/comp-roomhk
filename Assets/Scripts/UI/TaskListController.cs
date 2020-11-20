using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskListController : MonoBehaviour {

    private List<KeyValuePair<ITask, GameObject>> tasks = new List<KeyValuePair<ITask, GameObject>>();
    public IEnumerable<ITask> Tasks => tasks.Select(x => x.Key);

    public void AddTask(ITask task) {
        tasks.Add(new KeyValuePair<ITask, GameObject>(task, SpawnObject(task)));
        UpdateTaskList();
    }

    public void RemoveTask(ITask task) {
        var index = tasks.FindIndex(x => x.Key == task);
        if (index < 0) {
            Debug.LogError("Attempted to remove a task from TaskList that does not exist.");
        } else {
            tasks.RemoveAt(index);
        }
        UpdateTaskList();
    }

    public void SwapTask(ITask oldTask, ITask newTask) {
        var index = tasks.FindIndex(x => x.Key == oldTask);
        if (index < 0) {
            Debug.LogError("Attempted to swap a task from TaskList that does not exist.");
        } else {
            tasks[index] = new KeyValuePair<ITask, GameObject>(newTask, SpawnObject(newTask));
        }
    }

    private GameObject SpawnObject(ITask task) {

        return null;
    }

    private void UpdateTaskList() {
        
    }

}
