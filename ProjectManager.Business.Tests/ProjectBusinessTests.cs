using NBench;
using System;
using System.Collections.Generic;
using ProjectManager.Business.ServiceRequests;
using ProjectManager.Entities;
using ProjectManager.Repositories;

namespace ProjectManager.Business.Tests
{
    public class ProjectBusinessTests
    {
        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            //Mapper.Reset();
        }

        [PerfBenchmark(Description = "***** Result for GetAllParentTask *****",
                                                              NumberOfIterations = 2,
                                                              RunMode = RunMode.Throughput,
                                                              TestMode = TestMode.Measurement, SkipWarmups = false)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 1000)]
        public void BenchMarkGetAllParentTask()
        {
            IEnumerable<ParentTaskViewModel> parentTasks;
            IRepositoryDAO<ProjectTask> taskRepository = new RepositoryDAO<ProjectTask>();
            IRepositoryDAO<ParentTask> parenttaskRepository = new RepositoryDAO<ParentTask>();
            IParentTaskBusinessBL taskbusiness = new ParentTaskBusinessBL(parenttaskRepository);
            IRepositoryDAO<Project> projectRepository = new RepositoryDAO<Project>();
            IRepositoryDAO<User> userRepository = new RepositoryDAO<User>();
            IProjectBusinessBL projectBusiness = new ProjectBusinessBL(projectRepository, userRepository, taskRepository);

            TaskBusinessBL taskBusiness = new TaskBusinessBL(taskRepository, taskbusiness, projectBusiness, userRepository);
            parentTasks = taskBusiness.GetAllParentTasksBL();            
        }

        [PerfBenchmark(Description = "***** Result for GetTasks *****",
                                                           NumberOfIterations = 2,
                                                           RunMode = RunMode.Throughput,
                                                           TestMode = TestMode.Measurement, SkipWarmups = false)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 1000)]
        public void BenchMarkGetTasks()
        {
            IEnumerable<TaskViewModel> tasks;
            IRepositoryDAO<ProjectTask> taskRepository = new RepositoryDAO<ProjectTask>();
            IRepositoryDAO<ParentTask> parenttaskRepository = new RepositoryDAO<ParentTask>();
            IParentTaskBusinessBL taskbusiness = new ParentTaskBusinessBL(parenttaskRepository);

            IRepositoryDAO<Project> projectRepository = new RepositoryDAO<Project>();
            IRepositoryDAO<User> userRepository = new RepositoryDAO<User>();
            IProjectBusinessBL projectBusiness = new ProjectBusinessBL(projectRepository, userRepository, taskRepository);

            TaskBusinessBL taskBusiness = new TaskBusinessBL(taskRepository, taskbusiness, projectBusiness, userRepository);
            tasks = taskBusiness.GetAllTasksBL();
        }

        [PerfBenchmark(Description = "***** Result for GetTask By ID *****",
                                                          NumberOfIterations = 2,
                                                          RunMode = RunMode.Throughput,
                                                          TestMode = TestMode.Measurement, SkipWarmups = false)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 1000)]
        public void BenchMarkGetTaskById()
        {
            TaskViewModel task;
            IRepositoryDAO<ProjectTask> taskRepository = new RepositoryDAO<ProjectTask>();
            IRepositoryDAO<ParentTask> parenttaskRepository = new RepositoryDAO<ParentTask>();
            IParentTaskBusinessBL taskbusiness = new ParentTaskBusinessBL(parenttaskRepository);


            IRepositoryDAO<Project> projectRepository = new RepositoryDAO<Project>();
            IRepositoryDAO<User> userRepository = new RepositoryDAO<User>();
            IProjectBusinessBL projectBusiness = new ProjectBusinessBL(projectRepository, userRepository, taskRepository);

            TaskBusinessBL taskBusiness = new TaskBusinessBL(taskRepository, taskbusiness, projectBusiness, userRepository);
            task = taskBusiness.GetById(1);
        }


        [PerfBenchmark(Description = "***** Result for AddTask *****",
                                                      NumberOfIterations = 1,
                                                      RunMode = RunMode.Throughput,
                                                      TestMode = TestMode.Measurement, SkipWarmups = false)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 100)]
        public void BenchMarkSaveTask()
        {
            TaskViewModel task = new TaskViewModel
            {
                TaskName = "Add Task from nbench",
                StartDate = Convert.ToDateTime("01/01/2018"),
                ProjectId = 1,
                ProjectName = "Project-1",
                ParentTaskId = 1,
                ParentTaskName = "P1-Task",
                Priority = 15,
                EndDate = Convert.ToDateTime("12/12/2018"),
                TaskId = 0
            };

            IRepositoryDAO<ProjectTask> taskRepository = new RepositoryDAO<ProjectTask>();
            IRepositoryDAO<ParentTask> parenttaskRepository = new RepositoryDAO<ParentTask>();
            IParentTaskBusinessBL taskbusiness = new ParentTaskBusinessBL(parenttaskRepository);

            IRepositoryDAO<Project> projectRepository = new RepositoryDAO<Project>();
            IRepositoryDAO<User> userRepository = new RepositoryDAO<User>();
            IProjectBusinessBL projectBusiness = new ProjectBusinessBL(projectRepository, userRepository, taskRepository);

            TaskBusinessBL taskBusiness = new TaskBusinessBL(taskRepository, taskbusiness, projectBusiness, userRepository);
            taskBusiness.Save(task);
        }

        [PerfCleanup]
        public void Cleanup()
        {

        }
    }
}
