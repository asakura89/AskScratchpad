using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSScratchpad.Others.Task {
    public class TaskService : ITaskService {

        readonly IList<TaskModel> taskList;

        public TaskService() {
            taskList = new List<TaskModel> {
                new TaskModel {
                    Id = "974746",
                    Title = "task 1",
                    Done = false
                },
                new TaskModel {
                    Id = "977945",
                    Title = "task 2",
                    Done = false
                },
                new TaskModel {
                    Id = "973717",
                    Title = "task 3",
                    Done = false
                },
                new TaskModel {
                    Id = "976598",
                    Title = "task 4",
                    Done = false
                },
                new TaskModel {
                    Id = "976969",
                    Title = "task 5",
                    Done = false
                }
            };
        }

        public IList<TaskModel> GetAll() => taskList;

        public TaskModel Get(String id) => taskList.FirstOrDefault(task => task.Id == id);

        public Boolean Find(String id) => Get(id) != null;

        public void Add(TaskModel data) => taskList.Add(data);

        public void Update(String id, TaskModel data) {
            Boolean exist = Find(id);
            if (exist) {
                TaskModel task = Get(id);
                ObjectExt.TransferTo(data, task);
            }
        }

        public void Remove(String id) {
            Boolean exist = Find(id);
            if (exist) {
                TaskModel task = Get(id);
                taskList.Remove(task);
            }
        }

        public static class ObjectExt {
            public static void TransferTo<TSource, TDest>(TSource source, TDest dest) where TSource : class where TDest : class {
                Type sType = source.GetType();
                Type dType = dest.GetType();

                PropertyInfo[] sProps = sType.GetProperties();
                foreach (PropertyInfo sProp in sProps) {
                    PropertyInfo dProp = dType.GetProperty(sProp.Name);
                    if (dProp != null && dProp.PropertyType == sProp.PropertyType)
                        dProp.SetValue(dType, sProp.GetValue(sType, null), null);
                }
            }
        }
    }
}