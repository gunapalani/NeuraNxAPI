using Microsoft.Extensions.DependencyInjection;
using NeuraNx.Service.Implementation;
using NeuraNx.Service.Interface;
using NeuraNx.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service
{
    public static class ServiceModule
    {

        public static void DependencyServices(IServiceCollection services)
        {
            services.AddTransient<IEmployeesService, EmployeesService>();
            services.AddTransient<IBoardService, BoardService>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<ICommentService, CommentService>();

        }
    }
}
