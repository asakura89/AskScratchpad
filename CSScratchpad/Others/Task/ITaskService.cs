using System;
using System.Collections.Generic;

namespace CSScratchpad.Others.Task {
    public interface ITaskService {
        IList<TaskModel> GetAll();
        TaskModel Get(String id);
        Boolean Find(String id);
        void Add(TaskModel task);
        void Update(String id, TaskModel task);
        void Remove(String id);
    }
}