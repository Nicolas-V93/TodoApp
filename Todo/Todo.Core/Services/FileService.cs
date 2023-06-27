using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pra.Todo.Core
{
    static class FileService
    {
        public static void WriteCsv(List<TodoTask> tasks, string filePath)
        {
            if (tasks.Count == 0) throw new Exception("There are no tasks to export");

            StringBuilder sb = new StringBuilder();
            foreach (TodoTask task in tasks)
            {   
                sb.Append($"{task.Description};{task.Deadline?.ToString("dd/MM/yyyy")};{task.Completed} \n");
            }

            try
            {
                using (StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    streamWriter.Write(sb);
                }          
            }
            catch (DirectoryNotFoundException)
            {
                throw new DirectoryNotFoundException("Directory does not exist");
            }
            catch (Exception)
            {
                throw new Exception("File could not be written");
            }

        }

        public static List<TodoTask> ReadCsv(string filePath)
        {
            
            try
            {       
               using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8)) 
               {
                    List<TodoTask> importedTasks = new List<TodoTask>();
                    while (!streamReader.EndOfStream)
                    {
                        string[] values = null;
                        string fileContent = streamReader.ReadLine();
                        values = fileContent.Split(';');
                        TodoTask task = new TodoTask(values[0], bool.Parse(values[2]), DateTime.Parse(values[1]));
                        importedTasks.Add(task);
                    }
                    return importedTasks;
                }
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"File {filePath} could not be found");
            }
            catch (IOException)
            {
                throw new IOException($"File {filePath} could not be opened");
            }
            catch (Exception)
            {
                throw new Exception("File could not be imported. \nMake sure file is not empty and does not contain whitespace");
            }
        }
    }
}
