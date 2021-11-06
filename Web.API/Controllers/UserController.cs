using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using ViewModels;
using Web.API.Filters;
using System.Web.Http;
using System.Data.Entity;

namespace Web.API.Controllers
{
    public class UserController : ApiController
    {
        IUnitOfWork UnitOfWork { get; set; }
        IModelRepository<User> UserRepo;
        IModelRepository<Token> TokenRepo;
        ResultViewModel Result = new ResultViewModel();

        public UserController(IUnitOfWork _UnitOfWork)
        {
            UnitOfWork = _UnitOfWork;
            UserRepo = UnitOfWork.GetUserRepo();
            TokenRepo = UnitOfWork.GetTokenRepo();
        }

        private IEnumerable<UserViewModel> GetUsers()
        {
            return
                UserRepo.Get().Select(i => new UserViewModel
                {
                    ID = i.ID,
                    UserName = i.UserName,
                    Mobile = i.Mobile,
                    Address = i.Address
                }).ToList();
        }



        [HttpGet]
        //[CheckUserIdentity]
        public ResultViewModel Get()
        {
            Result.Data
                = GetUsers();
            Result.Message = "List of Users";
            return Result;
        }

        [HttpGet]
        //[CheckUserIdentity]
        public ResultViewModel Get(int id)
        {
            Result.Data
                = UserRepo.Get(id);
            Result.Message = "User of Entered ID";
            return Result;
        }





        [HttpPost]
        //[CheckUserIdentity]
        public ResultViewModel Add(UserEditViewModel User)
        {

            if (ModelState.IsValid)
            {
                User Temp = User.ToModel();
                UserRepo.Add(Temp);

                TokenRepo.Add(new Token()
                {
                    UserID = Temp.ID,
                    Code = Guid.NewGuid().ToString()
                });

                UnitOfWork.Save();

                ModelState.Clear();

                Result.Data = Temp;
                Result.Message = "user Added Successfully.";
                return Result;
            }
            Result.Data = "";
            Result.Message = "User didn't add";
            return Result;
        }

       

        [HttpPut]
        //[CheckUserIdentity]
        public ResultViewModel Edit(UserEditViewModel User)
        {
            if (ModelState.IsValid)
            {
                User Temp = User.ToModel();
                UserRepo.Edit(Temp);
                UnitOfWork.Save();
                Result.Data = Temp;
                Result.Message = "User updated successfully";
                return Result;
            }
            Result.Data = "";
            Result.Message = "User didn't update";
            return Result;
        }


        [HttpDelete]
        //[CheckUserIdentity]
        public ResultViewModel Delete(int? id)
        {

            if (id == null || id <= 0)
            {
                Result.Data = "";
                Result.Message = "User didn't delete";
                return Result;
            }


            User Temp = new User { ID = id.Value };
            UserRepo.Remove(Temp);  

            UnitOfWork.Save();

            Result.Data = Temp;
            Result.Message = "User deleted successfully";
            return Result;
        }
    }
}
