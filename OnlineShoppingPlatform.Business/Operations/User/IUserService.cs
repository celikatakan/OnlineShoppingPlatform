using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.Business.Operations.User.Dtos;
using OnlineShoppingPlatform.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user);
        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
        Task<List<UserInfoDto>> GetAllUsers();
       

    }
}
