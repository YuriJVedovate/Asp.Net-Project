using MyTasksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTasksAPI.Repositories.Contracts
{
    public interface ITaskRepository
    {
        public List<Task> Synchronization(List<Task> tasks);

        public List<Task> Restoration(ApplicationUser User, DateTime dateLastSync);
    }
}
