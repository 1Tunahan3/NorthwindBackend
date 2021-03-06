using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;

namespace Business.Concrete
{
   public class AuthManager:IAuthSerivce
   {
       private IUserService _userService;
       private ITokenHelper _tokenHelper;
       

       public AuthManager(IUserService userService, ITokenHelper tokenHelper)
       {
           _userService = userService;
           _tokenHelper = tokenHelper;
       }


       public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
       {
           byte[] passwordHash, passwordSalt;
           HashingHelper.CreatePasswordHash(password,out passwordHash,out passwordSalt);
           var user = new User
           {
               Email = userForRegisterDto.Email,
               FirstName = userForRegisterDto.FirstName,
               LastName = userForRegisterDto.LastName,
               PasswordHash = passwordHash,
               PasswordSalt = passwordSalt,
               Status = true
           };
           _userService.Add(user);
           return new SuccessDataResult<User>(user,Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetUserByMail(userForLoginDto.Email);
            if (userToCheck==null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password,userToCheck.PasswordHash,userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>("şifre hatalı");
            }

            return new SuccessDataResult<User>(userToCheck);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetUserByMail(email)!=null)
            {
                return new ErrorResult("kullanıcı zaten mevcut");
            }
            return new SuccessResult();
        }

        public IDataResult<AccesToken> CreateAccessToken(User user)
        {
           var  claims = _userService.GetClaims(user);
           var  accessToken= _tokenHelper.CreateToken(user, claims);
           return new SuccessDataResult<AccesToken>(accessToken,Messages.AccesTokenCreated);
        }
    }
}
