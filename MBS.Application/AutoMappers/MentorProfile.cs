using AutoMapper;
using MBS.Application.Models.Student;
using MBS.Application.Models.User;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers;

// public class MentorMapper : Profile
// {
//     public MentorMapper()
//     {
//         CreateMap<Mentor, GetMentorResponseModel>()
//             .ForMember(des => des.Id,
//                 opt => opt.MapFrom(src => src.UserId)
//             )
//             .ForMember(des => des.FullName,
//                 opt => opt.MapFrom(src => src.User.FullName)
//             )
//             .ForMember(des => des.AvatarUrl,
//                 opt => opt.MapFrom(src => src.User.AvatarUrl)
//             )
//             .ForMember(des => des.Gender,
//                 opt => opt.MapFrom(src => src.User.Gender)
//             )
//             .ForMember(des => des.Birthday,
//                 opt => opt.MapFrom(src => src.User.Birthday)
//             )
//             .ForMember(des => des.Email,
//                 opt => opt.MapFrom(src => src.User.Email)
//             )
//             .ForMember(des => des.UserName,
//                 opt => opt.MapFrom(src => src.User.UserName)
//             )
//             .ForMember(des => des.PhoneNumber,
//                 opt => opt.MapFrom(src => src.User.PhoneNumber)
//             )
//             .ForMember(des => des.EmailConfirmed,
//                 opt => opt.MapFrom(src => src.User.EmailConfirmed)
//             )
//             .ForMember(des => des.LockoutEnd,
//                 opt => opt.MapFrom(src => src.User.LockoutEnd)
//             )
//             .ForMember(des => des.LockoutEnabled,
//                 opt => opt.MapFrom(src => src.User.LockoutEnabled)
//             )
//             .ForMember(des => des.CreatedBy,
//                 opt => opt.MapFrom(src => src.User.CreatedBy)
//             )
//             .ForMember(des => des.CreatedOn,
//                 opt => opt.MapFrom(src => src.User.CreatedOn)
//             )
//             .ForMember(des => des.UpdatedBy,
//                 opt => opt.MapFrom(src => src.User.UpdatedBy)
//             )
//             .ForMember(des => des.UpdatedOn,
//                 opt => opt.MapFrom(src => src.User.UpdatedOn)
//             ).ReverseMap();
//
//
//         CreateMap<Mentor, GetStudentResponseModel>()
//             .ForMember(des => des.Id,
//                 opt => opt.MapFrom(src => src.UserId)
//             )
//             .ForMember(des => des.FullName,
//                 opt => opt.MapFrom(src => src.User.FullName)
//             )
//             .ForMember(des => des.AvatarUrl,
//                 opt => opt.MapFrom(src => src.User.AvatarUrl)
//             )
//             .ForMember(des => des.Gender,
//                 opt => opt.MapFrom(src => src.User.Gender)
//             )
//             .ForMember(des => des.Birthday,
//                 opt => opt.MapFrom(src => src.User.Birthday)
//             )
//             .ForMember(des => des.Email,
//                 opt => opt.MapFrom(src => src.User.Email)
//             )
//             .ForMember(des => des.UserName,
//                 opt => opt.MapFrom(src => src.User.UserName)
//             )
//             .ForMember(des => des.PhoneNumber,
//                 opt => opt.MapFrom(src => src.User.PhoneNumber)
//             )
//             .ForMember(des => des.EmailConfirmed,
//                 opt => opt.MapFrom(src => src.User.EmailConfirmed)
//             )
//             .ForMember(des => des.LockoutEnd,
//                 opt => opt.MapFrom(src => src.User.LockoutEnd)
//             )
//             .ForMember(des => des.LockoutEnabled,
//                 opt => opt.MapFrom(src => src.User.LockoutEnabled)
//             )
//             .ForMember(des => des.CreatedBy,
//                 opt => opt.MapFrom(src => src.User.CreatedBy)
//             )
//             .ForMember(des => des.CreatedOn,
//                 opt => opt.MapFrom(src => src.User.CreatedOn)
//             )
//             .ForMember(des => des.UpdatedBy,
//                 opt => opt.MapFrom(src => src.User.UpdatedBy)
//             )
//             .ForMember(des => des.UpdatedOn,
//                 opt => opt.MapFrom(src => src.User.UpdatedOn)
//             ).ReverseMap();
//     }
// }