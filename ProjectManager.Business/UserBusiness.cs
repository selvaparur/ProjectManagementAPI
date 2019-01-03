using ProjectManager.Business.ServiceRequests;
using ProjectManager.Entities;
using ProjectManager.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectManager.Business
{
    public interface IUserBusinessBL
    {
        void Save(UserViewModel user);
        IEnumerable<UserViewModel> GetAll();
        UserViewModel GetById(int id);
        void Delete(int id);
    }

    public class UserBusinessBL : IUserBusinessBL
    {
        readonly IRepositoryDAO<User> _userRepository;

        public UserBusinessBL(IRepositoryDAO<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public void Delete(int id)
        {
            _userRepository.Delete(id);
        }

        public IEnumerable<UserViewModel> GetAll()
        {
            var users = new List<UserViewModel>();
            var entities = _userRepository.GetAll();

            entities.ToList().ForEach(u => users.Add(ToUserViewModel(u)));            

            return users;
        }

        public void Save(UserViewModel user)
        {
            var entity = ToUserEntity(user);
            if (user.UserId == 0)
                _userRepository.Insert(entity);
            else
            {
                _userRepository.Update(entity);
            }
        }

        public UserViewModel GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return new UserViewModel();
            return ToUserViewModel(user);
        }

        private UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeId = user.EmployeeId,
                ProjectId = user.ProjectId.GetValueOrDefault(),
                TaskId = user.TaskId.GetValueOrDefault()
            };
        }

        private User ToUserEntity(UserViewModel userRequest)
        {
            return new User
            {
                UserId = userRequest.UserId,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                EmployeeId = userRequest.EmployeeId
            };
        }


    }

}
