using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Banco.Database.Usuarios.Domain.Entities;
using Api.Banco.Usuarios.Dtos.Api.Banco.Usuario.Api.DTOs;
using AutoMapper;
namespace Api.Banco.Usuarios.Domains.Entities
{
    using static System.Runtime.InteropServices.JavaScript.JSType;

    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UsuarioTb, UsuarioDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdUsuario));

        }
    }
}
