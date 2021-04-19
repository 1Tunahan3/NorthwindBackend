using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:Controller
    {
        private IAuthSerivce _authSerivce;

        public AuthController(IAuthSerivce authSerivce)
        {
            _authSerivce = authSerivce;
        }


        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authSerivce.Login(userForLoginDto);
            if (!userToLogin.Succes)
            {
                return BadRequest(userToLogin.Message);
            }

            var  result= _authSerivce.CreateAccessToken(userToLogin.Data);
           if (result.Succes)
           {
               return Ok(result.Data);
           }

           return BadRequest(result.Message);
        }



        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authSerivce.UserExists(userForRegisterDto.Email);
            if (!userExists.Succes)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = _authSerivce.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authSerivce.CreateAccessToken(registerResult.Data);
            if (result.Succes)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
