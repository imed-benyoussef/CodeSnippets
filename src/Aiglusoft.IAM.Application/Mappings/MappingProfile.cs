
namespace Aiglusoft.IAM.Application.Mappings
{
  using Aiglusoft.IAM.Application.DTOs;
  using Aiglusoft.IAM.Domain.Model.ClientAggregates;
  using Aiglusoft.IAM.Domain.Model.UserAggregates;
  using AutoMapper;
  using System;

  public class MappingProfile : Profile

  {
    public MappingProfile()
    {
      CreateMap<User, UserDto>();
      CreateMap<Client, ClientDto>();
    }
  }
}
