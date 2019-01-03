using ProjectManager.Business.ServiceRequests;
using ProjectManager.Entities;
using ProjectManager.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ProjectManager.Business
{
    public interface ITaskBusinessBL
    {
        void Save(TaskViewModel model);
        void Complete(TaskViewModel model);
        IEnumerable<ParentTaskViewModel> GetAllParentTasksBL();
        IEnumerable<TaskViewModel> GetAllTasksBL();
        IEnumerable<TaskViewModel> GetByProjectId(int id);
        TaskViewModel GetById(int id);
    }

    public class TaskBusinessBL : ITaskBusinessBL
    {
        readonly IRepositoryDAO<ProjectTask> _taskRepository;
        readonly IParentTaskBusinessBL _parentTaskBusiness;
        readonly IProjectBusinessBL _projectBusiness;
        readonly IRepositoryDAO<User> _userRepository;
        public TaskBusinessBL(IRepositoryDAO<ProjectTask> taskRepository,
            IParentTaskBusinessBL parentTaskBusiness,
            IProjectBusinessBL projectBusiness,
            IRepositoryDAO<User> userRepository)
        {
            _taskRepository = taskRepository;
            _parentTaskBusiness = parentTaskBusiness;
            _projectBusiness = projectBusiness;
            _userRepository = userRepository;
        }

       
        public void Save(TaskViewModel model)
        {
            ParentTaskViewModel parentTaskViewModel;
            // Parent task
            if (string.IsNullOrEmpty(model.ParentTaskName))
            {
                parentTaskViewModel = SaveParentTask(model);
            }
            else
            {
                var entity = _taskRepository.GetById(model.TaskId);
                if (entity == null)
                {
                    entity = ToEntity(model);
                    _taskRepository.Insert(entity);
                }
                else
                {
                    entity.ParentTaskId = model.ParentTaskId;
                    entity.ProjectId = model.ProjectId;
                    entity.Title = model.TaskName;
                    entity.StartDate = model.StartDate;
                    entity.EndDate = model.EndDate;
                    entity.Priority = model.Priority;                      
                    _taskRepository.Update(entity);
                }

                var userEntity = _userRepository.GetById(model.ManagerId);
                if (userEntity != null)
                {
                    userEntity.TaskId = entity.TaskId;
                    _userRepository.Update(userEntity);
                }
            }
        }

        public IEnumerable<TaskViewModel> GetByProjectId(int id)
        {
            throw new NotImplementedException();
        }

        public TaskViewModel GetById(int id)
        {
            var allTasks = GetAllTasksBL();
            return allTasks.FirstOrDefault(t => t.TaskId == id);
        }

        public IEnumerable<TaskViewModel> GetAllTasksBL()
        {
            var parentTasks = _parentTaskBusiness
                .GetAll()
                .ToList();

            var projects = _projectBusiness.GetAll();
            var tasks = _taskRepository.GetAll();
            var users = _userRepository.GetAll();

            var models = new List<TaskViewModel>();
            foreach (var task in tasks)
            {
                var parentTaskName = string.Empty;
                var status = "No";
                var projectName = string.Empty;
                var managerName = string.Empty;
                var managerId = 0;

                var pt = parentTasks.FirstOrDefault(p => p.ParentTaskId == task.ParentTaskId);
                if (pt != null)
                    parentTaskName = pt.ParentTaskName;

                var project = projects.FirstOrDefault(p => p.ProjectId == task.ProjectId);
                if (project != null)
                {
                    projectName = project.ProjectName;
                }

                var user = users.FirstOrDefault(p => p.TaskId == task.TaskId);
                if (user != null)
                {
                    managerName = string.Format("{0} {1}", user.FirstName, user.LastName);
                    managerId = user.UserId;
                }

                models.Add(new TaskViewModel
                {
                    TaskId = task.TaskId,
                    TaskName = task.Title,
                    ParentTaskName = parentTaskName,
                    ParentTaskId = task.ParentTaskId,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    ProjectId = task.ProjectId,
                    ProjectName = projectName,
                    Priority = task.Priority,
                    ManagerId = managerId,
                    ManagerName = managerName,
                    Status = string.IsNullOrEmpty(task.Status) ? status : task.Status
                });
            }

            return models;
        }

        public IEnumerable<ParentTaskViewModel> GetAllParentTasksBL()
        {
            return _parentTaskBusiness.GetAll();
        }

        public void Complete(TaskViewModel model)
        {
            var task = _taskRepository.GetById(model.TaskId);
            if (task == null) return;

            task.Status = "Yes";
            task.EndDate = DateTime.Now;
            _taskRepository.Update(task);
        }
        private ParentTaskViewModel SaveParentTask(TaskViewModel model)
        {
            var parentTaskModel = new ParentTaskViewModel
            {
                ParentTaskId = model.TaskId,
                ParentTaskName = model.TaskName
            };

            var parentViewModel = _parentTaskBusiness.Save(parentTaskModel);
            return parentViewModel;
        }

        private TaskViewModel ToModel(ProjectTask entity)
        {
            return new TaskViewModel
            {
                TaskId = entity.TaskId,
                ParentTaskId = entity.ParentTaskId,
                ProjectId = entity.ProjectId,
                TaskName = entity.Title,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                Priority = entity.Priority
            };
        }

        private ProjectTask ToEntity(TaskViewModel model)
        {
            return new ProjectTask
            {
                TaskId = model.TaskId,
                ParentTaskId = model.ParentTaskId,
                ProjectId = model.ProjectId,
                Title = model.TaskName,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Priority = model.Priority
            };
        }
    }
}
