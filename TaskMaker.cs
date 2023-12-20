using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager
{
    internal class TaskMaker
    {
        private string taskName;
        private string taskInfo;
        private DateTime date;
        private int priority;
        private string assignedBy;
        private string status;
        public string TaskName { get { return taskName; } set { taskName = value; } }
        public string TaskInfo { get { return taskInfo; } set { taskInfo = value; } }
        public DateTime Date { get { return date; } set { date = value; } }
        public int Priority { get { return priority; } set { priority = value; } }
        public string AssignedBy { get { return assignedBy; } set { assignedBy = value; } }
        public string Status { get { return status; } set { status = value; } }
        public TaskMaker(string taskNameIn, string taskInfoIn, DateTime dateIn, string priorityIn, string assignedByIn, string statusIn)
        {
            taskInfo = taskInfoIn;
            taskName = taskNameIn;
            date = dateIn;
            priority = int.Parse(priorityIn);
            assignedBy = assignedByIn;
            status = statusIn;
        }

        public TaskMaker(string txtfile)
        {
            string[] parts = txtfile.Split('!');
            taskInfo = parts[1];
            taskName = parts[0];
            date = DateTime.Parse(parts[2]);
            priority = int.Parse(parts[3]);
            assignedBy = parts[4];
            status = parts[5];
        }
    }
}
