using Avalonia.Controls;
using KanbanTable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace KanbanTable.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ObservableCollection<Table>> Tasks { get; set; }

        public MainWindowViewModel()
        {
            this.Tasks = new ObservableCollection<ObservableCollection<Table>>();
            for(int i = 0; i < 3; i++)
            {
                this.Tasks.Add(new ObservableCollection<Table>());
            }
        }

        public void AddTask(int numberOfList)
        {
            this.Tasks[numberOfList].Add(new Table("New Task"));
        }

        public void ClearData()
        {
            this.Tasks.Clear();
            for(int i = 0; i < 3; i++)
            {
                this.Tasks.Add(new ObservableCollection<Table>());
            }
        }

        public async void SaveFileDialogue(Window parent)
        {
            var taskPath = new SaveFileDialog().ShowAsync(parent);
            string? path = await taskPath;
           
            if (path is not null)
            {
                this.SaveCollection(path);
            }
        }

        public async void LoadFileDialogue(Window parent)
        {
            var taskPath = new OpenFileDialog().ShowAsync(parent);
            string[]? path = await taskPath;
            if (path is not null)
            {
                this.ReadCollection(string.Join("/", path));
            }
        }

        public void SaveCollection(string fileName)
        {
            using (var writer = new StreamWriter(fileName))
            {
                var xs = new XmlSerializer(typeof(ObservableCollection<ObservableCollection<Table>>));
                xs.Serialize(writer, this.Tasks);
            }
        }

        public void ReadCollection(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<ObservableCollection<Table>>));
            using (StreamReader sr = new StreamReader(fileName))
            {
                var tasks = (ObservableCollection<ObservableCollection<Table>>)xs.Deserialize(sr);
                this.Tasks.Clear();
                foreach (var task in tasks)
                {
                    this.Tasks.Add(task);
                }
            }
        }

        public void RemoveTask(Table task)
        {
            for(int i = 0; i < 3; i++)
            {
                this.Tasks[i].Remove(task);
            }
        }
       public void CloseWindow(Window parent)
       {
            parent.Close();
       }
    }
}
