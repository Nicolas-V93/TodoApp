using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra.Todo.Core
{
    public class TodoTask
    {
        private const int MaxLength = 50;
        private string description;
        private DateTime? deadline;

        public int Id { get; }
        public bool Completed { get; set; }
        public bool IsOverdue 
        { 
            get
            {
                return !Completed && Deadline < DateTime.Today;
            }
        }
        public bool IsDueToday 
        { 
            get
            {
                return !Completed && Deadline == DateTime.Today;
            }
        }
        public string Description
        {
            get 
            {
                return description.Length <= MaxLength ? description : description.Substring(0, MaxLength);
            }
            set 
            {
                if (string.IsNullOrWhiteSpace(value)) throw new Exception("Please enter a valid description");
                description = value.Trim();
            }
        }
        public DateTime? Deadline
        {
            get { return deadline; }
            set 
            {
                if (value == null) throw new Exception("Please select a date");
                deadline = value;
            }
        }

        public TodoTask(string description, bool completed, DateTime? deadline)
        {
            Description = description;
            Completed = completed;
            Deadline = deadline;
        }

        public TodoTask(int id, string description, bool completed, DateTime? deadline) 
            : this(description, completed, deadline)
        {
            Id = id;
        }

        public override string ToString()
        {
            CultureInfo ci = new CultureInfo("nl-NL");
            string str = $"{Description} ({Deadline?.ToString("dddd dd MMMM yyyy", ci)})";
            return Completed ? $"🗹 {str}" : $"☐ {str}" ;
        }
    }
}



