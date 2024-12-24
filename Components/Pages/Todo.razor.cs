using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MauiApp1.Models;
using Microsoft.AspNetCore.Components;

namespace MauiApp1.Components.Pages
{
    public partial class Todo : ComponentBase
    {
        private string newTask = string.Empty;
        private List<TodoItem> todoList = new();
        private string message = string.Empty;

        private void AddTask()
        {
            if (!string.IsNullOrWhiteSpace(newTask))
            {
                todoList.Add(new TodoItem { Id = Guid.NewGuid(), TaskName = newTask, IsCompleted = false });
                newTask = string.Empty;
                message = "Task added successfully!";
            }
        }

        private void RemoveTask(Guid taskId)
        {
            var taskToRemove = todoList.FirstOrDefault(t => t.Id == taskId);
            if (taskToRemove != null)
            {
                todoList.Remove(taskToRemove);
                message = "Task removed successfully!";
            }
            else
            {
                message = "Task not found.";
            }
        }

        private void ToggleTaskStatus(TodoItem task)
        {
            task.IsCompleted = !task.IsCompleted;
            message = $"Task '{task.TaskName}' status toggled!";
        }

        private void SaveTasks()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "tasks.json");
                var json = JsonSerializer.Serialize(todoList);
                System.IO.File.WriteAllText(filePath, json);

                message = "Tasks saved successfully to the Desktop!";
            }
            catch (Exception ex)
            {
                message = $"Error saving tasks: {ex.Message}";
            }
        }

        private void LoadTasks()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "tasks.json");

                if (System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);
                    todoList = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new();
                    message = "Tasks loaded successfully from the Desktop!";
                }
                else
                {
                    message = "No tasks file found on the desktop.";
                }
            }
            catch (Exception ex)
            {
                message = $"Error loading tasks: {ex.Message}";
            }
        }

    }
}