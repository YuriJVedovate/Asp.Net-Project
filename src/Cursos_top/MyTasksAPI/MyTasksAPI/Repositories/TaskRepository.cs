using MyTasksAPI.Database;
using MyTasksAPI.Models;
using MyTasksAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyTasksAPI.Repositories.Contracts
{
    public class TaskRepository : ITaskRepository
    {
        private readonly MyTasksContext _base;

        public TaskRepository(MyTasksContext banco)
        {
            _base = banco;

        }

        public List<Task> Restoration(ApplicationUser User, DateTime dateLastSync)
        {

            var query = _base.Tasks.Where(a => a.UserId == User.Id).AsQueryable();

            if (dateLastSync != null)
            {
                query.Where(a => a.Insert >= dateLastSync || a.Update <= dateLastSync);
            }

            return query.ToList<Task>();
        }

        public List<Task> Synchronization(List<Task> tasks)
        {

            var newTasks = tasks.Where(a => a.IdTaskApi == 0).ToList();
            var changeTasks = tasks.Where(a => a.IdTaskApi != 0).ToList();

            if (newTasks.Count() > 0)
            {
                foreach (var task in newTasks)
                {
                    _base.Tasks.Add(task);
                }
            }


            if (changeTasks.Count() > 0)
            {
                foreach (var task in changeTasks)
                {
                    _base.Tasks.Update(task);
                }
            }

            _base.SaveChanges();

            return newTasks.ToList();
        }
    }
}
