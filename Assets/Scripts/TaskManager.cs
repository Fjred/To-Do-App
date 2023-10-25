using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class TaskManager : MonoBehaviour
{
    public TaskUI taskUiPrefab;
    public Transform taskParent;
    public TMP_InputField descriptionInput;

    private List<TaskUI> tasks = new();

    private void Start()
    {
        LoadFromJson();
    }
    public void GetInput()
    {
        if (string.IsNullOrWhiteSpace(descriptionInput.text)) return;

        Task task = new Task()
        {
            Description = descriptionInput.text,
            Date = DateTime.Now.ToString("yyyy - MM - dd"),
            Finished = false,
            Removed = false
        };

        AddTask(task);
        
        descriptionInput.text = "";
    }

    void AddTask(Task task)
    {
        TaskUI taskUi = Instantiate(taskUiPrefab, taskParent);
        taskUi.SetTask(task);

        tasks.Add(taskUi);

    }

    public void SaveToJson()
    {
        if (tasks.Count == 0) return;
        List<Task> list = new();
        
        foreach (var taskUI in tasks)
        {
            // patikrina ar taskas yra paremovintas, jei ne ideda i lista, jei taip, istrina is listo
            if(taskUI.task.Removed == false)
            {
                list.Add(taskUI.task);
            }
            else
            {
                list.Remove(taskUI.task);
            }
            
        }

        //string json = JsonUtility.ToJson(list);
        string json = JsonConvert.SerializeObject(list);
        string path = Path.Combine(Application.persistentDataPath, "tasks.json");

        File.WriteAllText(path, json);
    }
    public void LoadFromJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "tasks.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            //var taskList = JsonUtility.FromJson<List<Task>>(json);

            var taskList = JsonConvert.DeserializeObject<List<Task>>(json);

            foreach(var task in taskList)
            {
                AddTask(task);   
            }
        }
    }
    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    private void OnApplicationPause(bool pause)
    {
        SaveToJson();
    }
}