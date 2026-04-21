using Microsoft.Extensions.DependencyInjection;
using NeuraNx.Repository.Implementation;
using NeuraNx.Repository.Interface;
using NeuraNx.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Repository
{
    public static class RepositoryModule
    {
        public static void DependencyServices(IServiceCollection services)
        {
            services.AddTransient<IEmployeesRepository, EmployeesRepository>();
            services.AddTransient<IBoardRepository, BoardRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
        }
    }
}
