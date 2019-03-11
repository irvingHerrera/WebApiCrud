using Crud.Business.ContractBusiness;
using Crud.Data.RepositoryContract;
using Curd.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Curd.Common.Enum;
using Crud.Data.Entities;
using Curd.Common.Helper;

namespace Crud.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUnityOfWork unityOfWork;


        #region Metodos Privados

        private User GetModel(UserViewModel viewModel)
        {
            Security.CrearPasswordHash(viewModel.Password, out byte[] passwordHash, out byte[] passwordSalt);

            return new User
            {
                Id = viewModel.Id,
                UserSystem = viewModel.User,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreationDate = DateTime.Now,
                Email = viewModel.Email,
                Gender = viewModel.Gender == Gender.Male ? true : false,
                Status = viewModel.Status == Status.Active ? true : false,
            };
        }

        private async Task<bool> ValidateUser(UserViewModel viewModel)
        {
            var lstUser = await unityOfWork.User.GetAllAsync();
            lstUser = lstUser.Where(u => (u.Status ? Status.Active : Status.Active) == Status.Active);
            return lstUser.Any(u => u.UserSystem.Equals(viewModel.User) || u.Email.Equals(viewModel.Email));
        }

        private UserViewModel GetViewMode(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                User = user.UserSystem,
                CreationDate = user.CreationDate,
                Email = user.Email,
                Gender = user.Gender ? Gender.Male : Gender.Female,
                Status = user.Status ? Status.Active : Status.Inactive,
            };
        }

        #endregion

        public UserBusiness(IUnityOfWork unityOfWork)
        {
            this.unityOfWork = unityOfWork;
        }

        public async Task<ResponseViewModel<UserViewModel>> Add(UserViewModel user)
        {
            var result = new ResponseViewModel<UserViewModel>();

            try
            {
                if (await ValidateUser(user))
                {
                    result.Message = "Ya se encuentra un usuario registrado con los mismos datos";
                    result.Success = false;
                    result.Object = user;
                    return result;
                }

                var userModel = GetModel(user);
                userModel.Status = true;
                unityOfWork.User.Add(userModel);

                if (await unityOfWork.SaveChangesAsync() == 1)
                {
                    result.Message = "Se ha registrado exitosamente";
                    result.Success = true;
                    result.Object = user;
                }
                else
                {
                    result.Message = "Ocurrió un error al registrar el usuario";
                    result.Success = false;
                    result.Object = user;
                }

            }
            catch (Exception ex)
            {
                result.Message = "Ocurrió un error al registrar el usuario";
                result.TechnicalError = ex.Message;
                result.Success = false;
                result.Object = user;
            }

            return result;

        }

        public async Task<ResponseViewModel<UserViewModel>> Delete(int id)
        {
            var result = new ResponseViewModel<UserViewModel>();

            try
            {
                var user = await unityOfWork.User.GetAsync(id);
                user.Status = false;
                unityOfWork.User.Update(user);

                if (await unityOfWork.SaveChangesAsync() == 1)
                {
                    result.Message = "El usuario fue eliminado exitosamente";
                    result.Success = true;
                }
                else
                {
                    result.Message = "Ocurrió un problema al eliminar el usuario";
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = "Ocurrió un problema al eliminar el usuario";
                result.TechnicalError = ex.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<ResponseViewModel<UserViewModel>> Get(int id)
        {
            var result = new ResponseViewModel<UserViewModel>();
            try
            {
                var user = await unityOfWork.User.GetAsync(id);

                if (user != null)
                {
                    var userViewModel = GetViewMode(user);

                    result.Success = true;
                    result.Object = userViewModel;
                }
                else
                {
                    result.Message = "Ocurrió un problema al consultar el usuario";
                    result.Success = false;
                }


            }
            catch (Exception ex)
            {
                result.Message = "Ocurrió un problema al consultar el usuario";
                result.TechnicalError = ex.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<ResponseViewModel<List<UserViewModel>>> GetAll()
        {
            var listUser = await unityOfWork.User.GetAllAsync();
            listUser = listUser.Where(u => u.Status == true);
            var result = new ResponseViewModel<List<UserViewModel>>();
            try
            {
                result.Success = true;
                result.Object = listUser.Select(u => GetViewMode(u)).ToList();
            }
            catch (Exception ex)
            {
                result.Message = "Ocurrió un error al obtener la lista de usuarios";
                result.TechnicalError = ex.Message;
                result.Success = false;
            }

            return result;
        }

        public ResponseViewModel<UserViewModel> Login(UserViewModel user)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseViewModel<UserViewModel>> Update(UserViewModel user)
        {
            var userViewModel = GetModel(user);
            var result = new ResponseViewModel<UserViewModel>();
            unityOfWork.User.Update(userViewModel);

            try
            {
                if (await unityOfWork.SaveChangesAsync() == 1)
                {
                    result.Message = "El usuario ha sido modificado exitosamente";
                    result.Success = true;
                }
                else
                {
                    result.Message = "Ocurrió un problema al actualizar el usuario";
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                result.Message = "Ocurrió un problema al actualizar el usuario";
                result.TechnicalError = ex.Message;
                result.Success = false;
            }
            return result;
        }
    }
}
