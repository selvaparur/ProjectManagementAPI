using ProjectManager.Business.ServiceRequests;
using ProjectManager.Entities;
using ProjectManager.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.Business
{
    public interface IParentTaskBusinessBL
    {
        ParentTaskViewModel Save(ParentTaskViewModel model);
        IEnumerable<ParentTaskViewModel> GetAll();
    }

    public class ParentTaskBusinessBL : IParentTaskBusinessBL
    {
        readonly IRepositoryDAO<ParentTask> _parentTaskRepository;

        public ParentTaskBusinessBL(IRepositoryDAO<ParentTask> parentTaskRepository)
        {
            _parentTaskRepository = parentTaskRepository;
        }

        public IEnumerable<ParentTaskViewModel> GetAll()
        {
            var entities = _parentTaskRepository.GetAll();
            var models = new List<ParentTaskViewModel>();
            entities.ToList().ForEach(p => models.Add(ToModel(p)));

            return models;
        }

        public ParentTaskViewModel Save(ParentTaskViewModel model)
        {
            var entity = _parentTaskRepository.GetById(model.ParentTaskId);
            if (entity == null)
            {
                entity = ToEntity(model);
                _parentTaskRepository.Insert(entity);
                model.ParentTaskId = entity.ParentTaskId;
            }
            else
            {
                entity.ParentTaskTitle = model.ParentTaskName;
                _parentTaskRepository.Update(entity);
            }

            return model;
        }

        private ParentTask ToEntity(ParentTaskViewModel model)
        {
            return new ParentTask
            {
                ParentTaskId = model.ParentTaskId,
                ParentTaskTitle = model.ParentTaskName
            };
        }

        private ParentTaskViewModel ToModel(ParentTask entity)
        {
            return new ParentTaskViewModel
            {
                ParentTaskId = entity.ParentTaskId,
                ParentTaskName = entity.ParentTaskTitle
            };
        }
    }
}
