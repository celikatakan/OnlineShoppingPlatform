using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.Business.DataProtection;
using OnlineShoppingPlatform.Business.Operations.Product.Dtos;
using OnlineShoppingPlatform.Business.Operations.User.Dtos;
using OnlineShoppingPlatform.Business.Types;
using OnlineShoppingPlatform.Data.Entities;
using OnlineShoppingPlatform.Data.Repository;
using OnlineShoppingPlatform.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShoppingPlatform.Business.Operations.User
{
    // UserManager class implements the IUserService interface and handles user-related operations
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;

        // Constructor to inject dependencies
        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
        }
        // Adds a new user and returns a ServiceMessage indicating success or failure
        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email address is already in use."
                };
            }
            // Create a new UserEntity instance and protect the password
            var userEntity = new UserEntity()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),
                PhoneNumber = user.PhoneNumber,
                UserType = Data.Enums.UserType.Customer,

            };
            _userRepository.Add(userEntity);  // Add the user entity to the repository
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "An error occurred during user registration: " + ex.Message
                };
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "User successfully registered."
            };

        }
        // Retrieves all users and returns a list of UserInfoDto
        public async Task<List<UserInfoDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAll()
                  .Select(x => new UserInfoDto
                  {
                      Id = x.Id,
                      FirstName = x.FirstName,
                      LastName = x.LastName,
                      Email = x.Email,
                      PhoneNumber = x.PhoneNumber,
                  }).ToListAsync();
            return users;
        }
        // Logs in a user and returns a ServiceMessage with user information if successful
        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            // Fetch the user entity based on email
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "The username or password is incorrect."
                };
            }
            // Unprotect the stored password for verification
            var unprotectedPassword = _protector.UnProtect(userEntity.Password);

            if (unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        PhoneNumber = userEntity.PhoneNumber,
                        UserType = userEntity.UserType,
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "The username or password is incorrect."
                };
            }
        }
    }
}
