﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Models;
using Company.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Company.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private IConfiguration _config;
        private readonly CompanyContext _context;
        public UserProfileController(CompanyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpGet]
        [ActionName("allEmp")]
        public IEnumerable<UserProfile> allEmp()
        {
            return _context.UserProfile;
        }
        [HttpGet]
        [ActionName("Use")]
        public IEnumerable<UserProfile> Use()
        {
            return _context.UserProfile.Where(wh=>wh.IsActive==true);
        }
        [HttpGet]
        [ActionName("getByID/{userid}")]
        public async Task<IActionResult> GetEmp1ByID([FromRoute] int Userid)
        {
            var id = await _context.UserProfile.FindAsync(Userid);

            if (id == null)
            {
                return NotFound();
            }

            return Ok(id);
        }
        [AllowAnonymous]
        [HttpPost]
        [ActionName("register")]
        public async Task<IActionResult> PostUse([FromBody] Register register)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            response.Add("status", 0);
            response.Add("message", "");
            if (string.IsNullOrEmpty(register.Emailid))
            {
                response["message"] = "Email Id required!";
                return BadRequest(response);
            }
            if (string.IsNullOrEmpty(register.Address))
            {
                response["message"] = "Address is required!";
                return BadRequest(response);
            }
            if (string.IsNullOrEmpty(register.Age.ToString()))
            {
                response["message"] = "Age is required";
                return (BadRequest(response));
            }
            if (string.IsNullOrEmpty(register.Firstname))
            {
                response["message"] = "Enter your name!";
                return (BadRequest(response));
            }
            if (string.IsNullOrEmpty(register.Lastname))
            {
                response["message"] = "Enter your last name!";
                return (BadRequest(response));
            }
            if (string.IsNullOrEmpty(register.Gender))
            {
                response["message"] = "Enter your gender!";
                return BadRequest(response);
            }
            if (string.IsNullOrEmpty(register.Password))
            {
                response["message"] = "Select a password!";
                return (BadRequest(response));
            }
            if (string.IsNullOrEmpty(register.Phone))
            {
                response["message"] = "Enter your phone number!";
                return (BadRequest(response));
            }
            if (string.IsNullOrEmpty(register.Hobbies))
            {
                response["message"] = "Hobbies cannot be kept blank!";
                return (BadRequest(response));
            }
            if (string.IsNullOrEmpty(register.Username))
            {
                response["message"] = "Select a unique username!";
                return (BadRequest(response));
            }
            bool d = _context.Login.Any(wh => wh.Username == register.Username);

            if (d == true)
            {
                response["message"] = "This username already exsists!";
                return (BadRequest(response));
            }
            Models.Login login = new Models.Login();
            login.Username = register.Username;
            login.Password = register.Password;
            login.Createdon = DateTime.Now;
            _context.Login.Add(login);
            _context.SaveChanges();

            UserProfile userp = new UserProfile();
            userp.Firstname = register.Firstname;
            userp.Lastname = register.Lastname;
            userp.Gender = register.Gender;
            userp.Age = register.Age;
            userp.Address = register.Address;
            userp.Phone = register.Phone;
            userp.Emailid = register.Emailid;
            userp.Hobbies = register.Hobbies;
            userp.Loginid = Convert.ToInt32(login.Loginid);
            _context.UserProfile.Add(userp);
            _context.SaveChanges();
            response["status"] = 1;
            response["message"] = "Registration successfull!";
            response.Add("result", userp);
            return CreatedAtAction("Use", new { id = userp.Userid }, response);
        }

        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> postLogin([FromBody] Log_in obj_log_in)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            response.Add("status", 0);
            response.Add("message", "");
            if (string.IsNullOrEmpty(obj_log_in.Username))
            {
                response["message"] = "Username cannot be blank!";
                return BadRequest(response);
            }
            if (string.IsNullOrEmpty(obj_log_in.Password))
            {
                response["message"] = "Enter password!!";
                return BadRequest(response);
            }
            Login obj_login = _context.Login.Where(wh => wh.Username == obj_log_in.Username && wh.Password == obj_log_in.Password).FirstOrDefault();

            if (obj_login == null)
            {
                response["message"] = "Username or password is incorrect!";
                return (BadRequest(response));
            }
            else
            {
                UserProfile obj_userProfile = _context.UserProfile.Where(wh => wh.Loginid == obj_login.Loginid).SingleOrDefault();
                Response obj_response = new Response();
                obj_response.Address = obj_userProfile.Address;
                obj_response.Age = obj_userProfile.Age;
                obj_response.Emailid = obj_userProfile.Emailid;
                obj_response.Firstname = obj_userProfile.Firstname;
                obj_response.Gender = obj_userProfile.Gender;
                obj_response.Lastname = obj_userProfile.Lastname;
                obj_response.Phone = obj_userProfile.Phone;
                obj_response.Hobbies = obj_userProfile.Hobbies;
                return Ok(obj_response);
            }
    }
        [HttpPost]
        [ActionName("update")]
        public async Task<IActionResult> PostUserUpdate([FromBody] UserProfile user)
        {
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            response.Add("status", 0);
            response.Add("message", "");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _context.UserProfile.SingleOrDefault(i => i.Userid == user.Userid);
            if(result != null)
            {
                result.Address = user.Address;
                result.Age = user.Age;
                result.Emailid = user.Emailid;
                result.Firstname = user.Firstname;
                result.Gender = user.Gender;
                result.Lastname = user.Lastname;
                result.Phone = user.Phone;
                result.Hobbies = user.Hobbies;
                _context.SaveChanges();
                response["status"] = 1;
                response["message"] = "Data Updated!";
            }

            return Ok(Json(response));
        }
        [HttpGet]
        [ActionName("del/{Userid}/{flag}")]
        public async Task<IActionResult> PostUserdel([FromRoute] int Userid,[FromRoute] int flag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var re = _context.UserProfile.SingleOrDefault(i => i.Userid == Userid);
            re.IsActive = false;
            if (re != null)
            {
                _context.SaveChanges();
            }
            if (flag == 1)
            {
                return Ok(_context.UserProfile.Where(wh => wh.IsActive == true));
            }
            else
            {
                return Ok(_context.UserProfile);
            }
        }


    }
}