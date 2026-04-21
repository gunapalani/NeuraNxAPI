using AutoMapper;
using NeuraNx.Models.DTOs;
using NeuraNx.Models.ViewModels;
using NeuraNx.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuraNx.Service
{
    public class AutoMapping : Profile
    {

        public AutoMapping()
        {
            // Employee mappings
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

            // Board mappings
            CreateMap<Board, BoardDto>()
                .ForMember(dest => dest.TaskCount, opt => opt.Ignore()); // Set manually in service

            CreateMap<CreateBoardDto, Board>();
            CreateMap<UpdateBoardDto, Board>();

            // Task mappings
            CreateMap<TaskItem, TaskResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Models.Enums.TaskStatus)src.Status))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Models.Enums.TaskPriority)src.Priority))
                .ForMember(dest => dest.BoardName, opt => opt.MapFrom(src => src.Board != null ? src.Board.Name : string.Empty))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => src.AssignedTo != null ? src.AssignedTo.Name : string.Empty))
                .ForMember(dest => dest.CommentCount, opt => opt.Ignore()); // Set manually in service

            CreateMap<CreateTaskDto, TaskItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Repository.Enums.TaskStatus.Todo))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Repository.Enums.TaskPriority)src.Priority));

            CreateMap<UpdateTaskDto, TaskItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Repository.Enums.TaskStatus)src.Status))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (Repository.Enums.TaskPriority)src.Priority))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BoardId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore());

            // Comment mappings
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.Name : string.Empty));

            CreateMap<CreateCommentDto, Comment>();
            
            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TaskItemId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore());
        }

    }
}
