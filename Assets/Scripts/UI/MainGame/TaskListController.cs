using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskListController : MonoBehaviour {

    public GameObject ContainerGameObject;

    [Header("Task Entry Prefabs")] 
    public GameObject DeployableTaskObject;

    private List<KeyValuePair<ITask, GameObject>> tasks = new List<KeyValuePair<ITask, GameObject>>();
    public IEnumerable<ITask> Tasks => tasks.Select(x => x.Key);

    private GameObject unassignedObjectPool;

    public void Start() {
        foreach (var task in GameManager.GetInstance.Tasks) {
            AddTask(task);
        }
    }

    public void AddTask(ITask task) {
        tasks.Add(new KeyValuePair<ITask, GameObject>(task, SpawnObject(task)));
        UpdateTaskList();
    }

    public void RemoveTask(ITask task) {
        var index = tasks.FindIndex(x => x.Key == task);
        if (index < 0) {
            Debug.LogError("Attempted to remove a task from TaskList that does not exist.");
        } else {
            if (tasks[index].Value != null) Destroy(tasks[index].Value);
            tasks.RemoveAt(index);
        }
        UpdateTaskList();
    }

    public void SwapTask(ITask oldTask, ITask newTask) {
        var index = tasks.FindIndex(x => x.Key == oldTask);
        if (index < 0) {
            Debug.LogError("Attempted to swap a task from TaskList that does not exist.");
        } else {
            if (tasks[index].Value != null) Destroy(tasks[index].Value);
            tasks[index] = new KeyValuePair<ITask, GameObject>(newTask, SpawnObject(newTask));
        }
    }

    private GameObject SpawnObject(ITask task) {
        GameObject spawnedObject = null;
        if (task is DeployableTask) {
            spawnedObject = Instantiate(DeployableTaskObject, ContainerGameObject.transform);
        }
        if (spawnedObject != null) {
            var taskEntryController = spawnedObject.GetComponent<ITaskEntryController>();
            if (taskEntryController != null) {
                taskEntryController.TaskBeingDisplayed = task;
            } else {
                Debug.LogWarning("The spawned task entry does not have a ITaskEntryController");
            }
        }
        return spawnedObject;
    }

    private void UpdateTaskList() {
        var count = 0;
        foreach (var task in tasks.Where(task => task.Value != null)) {
            task.Value.transform.SetSiblingIndex(count++);
        }
    }

}
