using System;
using System.Collections.Generic;
using Scratch;

namespace CSScratchpad.Script {
    class PrintTodoList : Common, IRunnable {
        public void Run() {
            var taskGroupList = new List<TaskGroup>
            {
                new TaskGroup
                {
                    TaskGroupId = "blj",
                    GroupTitle = "belanjaan",
                    TaskList = new List<Task>
                    {
                        new Task
                        {
                            TaskId = "0020",
                            Title = "Beli ikan",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        },
                        new Task
                        {
                            TaskId = "0420",
                            Title = "Beli kentang",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        },
                        new Task
                        {
                            TaskId = "0320",
                            Title = "Beli cabe",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        },
                        new Task
                        {
                            TaskId = "0342",
                            Title = "Beli kecap",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        }
                    }
                },
                new TaskGroup
                {
                    TaskGroupId = "knt",
                    GroupTitle = "kantor",
                    TaskList = new List<Task>
                    {
                        new Task
                        {
                            TaskId = "09872",
                            Title = "fixing bug web site js",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        },
                        new Task
                        {
                            TaskId = "09873",
                            Title = "integrasi modul baru",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        }
                    }
                },
                new TaskGroup
                {
                    TaskGroupId = "mbdq",
                    GroupTitle = "must be done quick",
                    TaskList = new List<Task>
                    {
                        new Task
                        {
                            TaskId = "97986",
                            Title = "ngepel rumah",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        },
                        new Task
                        {
                            TaskId = "97768",
                            Title = "potong rumput",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        },
                        new Task
                        {
                            TaskId = "97868",
                            Title = "siram halaman",
                            Note = String.Empty,
                            Recurrence = RecurrenceType.None,
                            RecurrenceCount = 0,
                            Done = false
                        }
                    }
                }
            };

            Dbg(taskGroupList);
        }

        public class Task {
            public String TaskId { get; set; }
            public String Title { get; set; }
            public String Note { get; set; }
            public RecurrenceType Recurrence { get; set; }
            public Int32 RecurrenceCount { get; set; }
            public Boolean Done { get; set; }
        }

        public class TaskGroup {
            public String TaskGroupId { get; set; }
            public String GroupTitle { get; set; }
            public List<Task> TaskList { get; set; }
        }

        public class TaggedTask {
            public String TaskId { get; set; }
            public String TagId { get; set; }
        }

        /*public class TaskGroup {
            public String TaskId { get; set; }
            public String TagId { get; set; }
        }*/

        /*public class Tag {
            public String TagId { get; set; }
            public String Note { get; set; }
            public Decimal Price { get; set; }
            public RecurrenceType Recurrence { get; set; }
            public Int32 RecurrenceCount { get; set; }
        }*/

        public class LoginContext {
            public String ContextId { get; set; }
            public String CurrentUserEmail { get; set; }
            public String CurrentUsername { get; set; }
            public DateTime LoginTime { get; set; }
            public String LoginLocation { get; set; }
        }

        public enum RecurrenceType {
            None,
            Days,
            Weeks,
            Months,
            Years
        }
    }
}
