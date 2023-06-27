using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Pra.Todo.Core
{
    public class TodoApp
    {
        public List<TodoTask> Tasks { get; } = new List<TodoTask>();
        public List<TodoTask> OpenTasks
        { 
            get
            {  
                return Tasks.Where(task => !task.Completed).ToList();
            }
        } 

        public TodoApp()
        {
            GetAllTasks();
            //UpdateOpenTasks();
            SortLists();
        }

         
        #region Public Methods
        public bool Addtask(TodoTask task)
        {
            string sql = $"insert into taken (omschrijving, afgerond, deadline) " +
                         $"values ('{Helper.HandleQuotes(task.Description)}', '{task.Completed}', '{task.Deadline?.ToString("yyyy-MM-dd")}')";

            if (DBService.ExecuteCommand(sql))
            {
                sql = "select max(id) from taken";
                int taskId = int.Parse(DBService.ExecuteScalar(sql));
                Tasks.Add(new TodoTask(taskId, task.Description, task.Completed, task.Deadline));
                //UpdateOpenTasks();
                SortLists();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateTask(TodoTask task) 
        {
            string sql = $"update taken" +
                         $" set omschrijving = '{Helper.HandleQuotes(task.Description)}', deadline = '{task.Deadline?.ToString("yyyy-MM-dd")}' ," +
                         $" afgerond = '{task.Completed}'" +
                         $" where id = '{task.Id}'";

            if (DBService.ExecuteCommand(sql))
            {
                //UpdateOpenTasks();
                SortLists();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveTask(TodoTask task)
        {
            string sql = $"delete from taken" +
                         $" where id = '{task.Id}'";

            if (DBService.ExecuteCommand(sql))
            {
                Tasks.Remove(task);
                if(!task.Completed) OpenTasks.Remove(task);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Export(string filePath)
        {
            FileService.WriteCsv(Tasks, filePath);
        }

        public void Import(string filePath)
        {
            List<TodoTask> importedTasks = FileService.ReadCsv(filePath);
            foreach(TodoTask task in importedTasks)
            {
                Addtask(task);
            }         
        }
        #endregion

        #region Private Methods
        private void GetAllTasks()
        {
            string sql = "select * from taken";
            DataTable dataTable = DBService.ExecuteSelect(sql);

            if (dataTable != null)
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Tasks.Add(new TodoTask((int)dr["Id"], dr["Omschrijving"].ToString(), (bool)dr["Afgerond"], (DateTime)dr["Deadline"]));
                }
            }
        }

        //private void UpdateOpenTasks()
        //{
        //    OpenTasks.Clear();
        //    foreach (TodoTask task in Tasks.Where(task => !task.Completed))
        //    {
        //        OpenTasks.Add(task);
        //    }
        //}

        private void SortLists()
        {
            Tasks.Sort((x, y) =>
            {
                int sort = x.Completed.CompareTo(y.Completed);
                if (sort == 0) sort = DateTime.Compare((DateTime)x.Deadline, (DateTime)y.Deadline);
                if (x.Completed && y.Completed) sort = DateTime.Compare((DateTime)y.Deadline, (DateTime)x.Deadline);
                return sort;
            });
            OpenTasks.Sort((x, y) => DateTime.Compare((DateTime)x.Deadline, (DateTime)y.Deadline));

            // if using private setters, alternative option:

            // OpenTasks.OrderBy(task => task.Deadline);
        }
        #endregion
    }
}
