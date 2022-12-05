
using System;
using BLL.Base;
using CyberneticCode;
using Model;
using Model.ViewModels.AspNetUsers2;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BLAspNetUsers2 : BLBase
    {
        public VmAspNetUsers2 GetAspNetUsers2(int id)
        {
            try
            {
                var aspNetUsers2Repository = UnitOfWork.GetRepository<AspNetUsers2Repository>();

                var aspNetUsers2 = aspNetUsers2Repository.GetAspNetUsers2ById(id);

                var vmAspNetUsers2 = new VmAspNetUsers2
                {
                    Id = aspNetUsers2.Id,
                    UserId = aspNetUsers2.UserId,
                    Password = DataCoderHandler.Decrypt(aspNetUsers2.Password),
                };

                return vmAspNetUsers2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public VmAspNetUsers2 GetAspNetUsers2ByUserId(string userId)
        {
            try
            {
                var aspNetUsers2Repository = UnitOfWork.GetRepository<AspNetUsers2Repository>();

                var aspNetUsers2 = aspNetUsers2Repository.GetAspNetUsers2ByUserId(userId);
                if (aspNetUsers2 != null)
                {
                    var vmAspNetUsers2 = new VmAspNetUsers2
                    {
                        Id = aspNetUsers2.Id,
                        UserId = aspNetUsers2.UserId,
                        Password = DataCoderHandler.Decrypt(aspNetUsers2.Password),
                    };

                    return vmAspNetUsers2;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IEnumerable<VmAspNetUsers2> GetAllAspNetUsers2s()
        {
            try
            {
                var aspNetUsers2Repository = UnitOfWork.GetRepository<AspNetUsers2Repository>();

                var aspNetUsers2List = aspNetUsers2Repository.GetAllAspNetUsers2();

                var vmAspNetUsers2List = from si in aspNetUsers2List
                                         select new VmAspNetUsers2
                                         {
                                             Id = si.Id,
                                             UserId = si.UserId,
                                             Password = DataCoderHandler.Decrypt(si.Password),
                                         };

                return vmAspNetUsers2List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateAspNetUsers2(VmAspNetUsers2 vmAspNetUsers2)
        {
            try
            {
                var aspNetUsers2Repository = UnitOfWork.GetRepository<AspNetUsers2Repository>();

                var existsUser = GetAspNetUsers2ByUserId(vmAspNetUsers2.UserId);

                if (existsUser == null)
                {
                    aspNetUsers2Repository.CreateAspNetUsers2(
                        new AspNetUsers2
                        {
                            UserId = vmAspNetUsers2.UserId,
                            Password = DataCoderHandler.Encrypt(vmAspNetUsers2.Password),
                        });

                    return UnitOfWork.Commit();
                }

                aspNetUsers2Repository.UpdateAspNetUsers2(new AspNetUsers2
                {
                    Id = existsUser.Id,
                    UserId = existsUser.UserId,
                    Password = DataCoderHandler.Encrypt(vmAspNetUsers2.Password),
                });

                var aspNetUsersRepository = UnitOfWork.GetRepository<AspNetUserRepository>();
                aspNetUsersRepository.UpdateAspNetUser(new AspNetUser
                {
                    Id = existsUser.UserId,
                    PasswordHash = vmAspNetUsers2.PasswordHash,
                    Email = vmAspNetUsers2.Email,
                    UserName = vmAspNetUsers2.Email,
                 
                });


                return UnitOfWork.Commit();

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateAspNetUsers2(VmAspNetUsers2 vmAspNetUsers2)
        {
            try
            {
                var aspNetUsers2Repository = UnitOfWork.GetRepository<AspNetUsers2Repository>();

                var aspNetUsers2 = new AspNetUsers2
                {
                    Id = vmAspNetUsers2.Id,
                    UserId = vmAspNetUsers2.UserId,
                    Password = DataCoderHandler.Encrypt(vmAspNetUsers2.Password),
                };

                aspNetUsers2Repository.UpdateAspNetUsers2(aspNetUsers2);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteAspNetUsers2(int id)
        {
            try
            {
                var aspNetUsers2Repository = UnitOfWork.GetRepository<AspNetUsers2Repository>();
                aspNetUsers2Repository.DeleteAspNetUsers2(id);

                UnitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}