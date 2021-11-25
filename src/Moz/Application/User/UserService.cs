using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Members;
using Moz.Bus.Services;
using Moz.Common;
using Moz.DataBase;
using Moz.Dto.User;
using Moz.Events;
using Moz.Model;
using Moz.Service.Security;
using SqlSugar;

namespace Moz.Application.User
{
    public class UserService :BaseService, IUserService
    {

        private readonly IDistributedCache _distributedCache;
        private readonly IEncryptionService _encryptionService;
        private readonly IEventPublisher _eventPublisher;

        public UserService(
            IDistributedCache distributedCache,
            IEventPublisher eventPublisher,
            IEncryptionService encryptionService)
        {
            _distributedCache = distributedCache;
            _eventPublisher = eventPublisher;
            _encryptionService = encryptionService;
        }

        #region Utils
        
        /// <summary>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static IEnumerable<Role> GetRolesByUserId(long userId)
        {
            var dt = DateTime.Now;
            using (var client = DbFactory.CreateClient())
            {
                return client.Queryable<UserRole, Role>((ur, r) => new object[]
                    {
                        JoinType.Left, ur.RoleId == r.Id
                    })
                    .Where((ur, r) => ur.UserId == userId 
                                      && (ur.ExpireDate == null || ur.ExpireDate != null && ur.ExpireDate > dt)
                                      && r.IsActive)
                    .Select((mr, r) => new 
                    {
                        r.Id,
                        r.Code,
                        r.IsActive,
                        r.Name,
                        r.IsAdmin,
                        mr.ExpireDate
                    })
                    .ToList()
                    .Select(it=> new Role()
                    {
                        Code = it.Code,
                        Id = it.Id,
                        Name = it.Name,
                        IsActive = it.IsActive,
                        IsAdmin = it.IsAdmin
                    });
            }
        }
        
        #endregion
        
        #region Utils
        
        private bool PasswordsMatch(string passwordInDb, string salt, string enteredPassword)
        {
            if (string.IsNullOrEmpty(passwordInDb) ||
                string.IsNullOrEmpty(salt) ||
                string.IsNullOrEmpty(enteredPassword))
                return false;
            var savedPassword = _encryptionService.CreatePasswordHash(enteredPassword, salt,"SHA512");
            return passwordInDb.Equals(savedPassword, StringComparison.OrdinalIgnoreCase);
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// 获取详细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<GetUserDetailInfo> GetUserDetail(GetUserDetailDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var user = client.Queryable<Model.User>().InSingle(dto.Id);
                if (user == null)
                {
                    return Error("找不着信息");
                }

                var roles = client.Queryable<UserRole>()
                    .Where(it => it.UserId == dto.Id)
                    .Select(it => new {it.RoleId})
                    .ToList()
                    .Select(it => it.RoleId).ToArray();

                var resp = new GetUserDetailInfo
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Gender = user.Gender,
                    Birthday = user.Birthday,
                    RegisterIp = user.RegisterIp,
                    RegisterDatetime = user.RegisterDatetime,
                    LoginCount = user.LoginCount,
                    LastLoginIp = user.LastLoginIp,
                    LastLoginDatetime = user.LastLoginDatetime,
                    CannotLoginUntilDate = user.CannotLoginUntilDatetime,
                    IsActive = user.IsActive,
                    IsDelete = user.IsDelete,
                    Roles = roles
                };
                return resp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public SimpleUser GetSimpleUserByUuid(string uuid)
        {
            SimpleUser GetSimpleUser(string newUuid)   
            {
                using (var client = DbFactory.CreateClient())
                {
                    var user = client.Queryable<Model.User>().Select(t => new
                    { 
                        t.Id,
                        t.Uuid,
                        t.Email,
                        t.Username,
                        t.IsActive,
                        t.IsDelete,
                        t.Avatar,
                        t.CannotLoginUntilDatetime
                    }).Single(t => t.Uuid == newUuid);

                    if (user == null)
                        return null;

                    return new SimpleUser
                    {
                        Id = user.Id,
                        Uuid = user.Uuid,
                        Email = user.Email,
                        Username = user.Username,
                        Avatar = user.Avatar,
                        IsActive = user.IsActive,
                        IsDelete = user.IsDelete,
                        CannotLoginUntilDate = user.CannotLoginUntilDatetime
                    };
                }
            }
            
            var currentMember =  GetSimpleUser(uuid);
            if (currentMember == null) 
                return null;

            currentMember.Roles = GetRolesByUserId(currentMember.Id) ?? new List<Role>();
            currentMember.Permissions = new List<Permission>();
            return currentMember;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ResetPassword(ResetPasswordDto dto)
        {
            using (var db = DbFactory.CreateClient())
            {
                var members = db.Queryable<Model.User>()
                    .Select(it => new Model.User {Id = it.Id, Password = it.Password, PasswordSalt = it.PasswordSalt})
                    .Where(it => dto.MemberIds.Contains(it.Id))
                    .ToList();

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService
                    .CreatePasswordHash(dto.NewPassword, saltKey, "SHA512");

                members.ForEach(it =>
                {
                    it.Password = newPassword;
                    it.PasswordSalt = saltKey;
                });
                db.Updateable(members)
                    .UpdateColumns(it => new
                    {
                        it.Password,
                        it.PasswordSalt
                    })
                    .ExecuteCommand();

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<CreateMemberApo> CreateUser(CreateMemberDto dto)
        {
            var user = new Model.User
            {
                Id = UuidHelper.SnowId,
                Uuid = Guid.NewGuid().ToString("N"),
                Avatar = dto.Avatar,
                Birthday = null,
                CannotLoginUntilDatetime = null,
                Email = dto.Email,
                Gender = dto.Gender,
                IsActive = true,
                IsDelete = false,
                LastLoginDatetime = DateTime.Now,
                LastLoginIp = null,
                LoginCount = 0,
                Password = null,
                PasswordSalt = null,
                RegisterDatetime = DateTime.Now,
                RegisterIp = null,
                Username = dto.Username
            };
            
            using (var db = DbFactory.CreateClient())
            {
                //检查是否存在此用户名
                {
                    var isExist = db.Queryable<Model.User>()
                        .Any(it=>it.Username.Equals(dto.Username));
                    if (isExist)
                    {
                        return Error($"此用户名 {dto.Username} 已存在");
                    }
                }

                //检查是否存在此email
                {
                    var isExist = db.Queryable<Model.User>()
                        .Any(it => it.Email.Equals(dto.Email));
                    if (isExist)
                    {
                        return Error($"此邮箱 {dto.Email} 已存在");
                    }
                }

                //设置密码
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(dto.Password, saltKey, "SHA512");

                    user.Password = newPassword;
                    user.PasswordSalt = saltKey;
                }

                db.Insertable(user).ExecuteCommand();

            }

            return new CreateMemberApo
            {
                Id = user.Id,
                UId = user.Uuid,
                Username = user.Username
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateMember(UpdateMemberDto dto)
        {
            using (var db = DbFactory.CreateClient())
            {
                var member = db.Queryable<Model.User>().InSingle(dto.Id);
                if (member == null)
                {
                    return Error("找不到该用户");
                }

                if (!member.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Model.User>()
                        .Where(it => it.Username.Equals(dto.Username))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此用户名 {dto.Username} 已存在");
                    }

                    member.Username = dto.Username;
                }

                if (!dto.Password.Equals("******"))
                {
                    var saltKey = _encryptionService.CreateSaltKey(6);
                    var newPassword = _encryptionService
                        .CreatePasswordHash(dto.Password, saltKey, "SHA512");

                    member.Password = newPassword;
                    member.PasswordSalt = saltKey;
                }

                if (!dto.Email.IsNullOrEmpty() &&
                    !dto.Email.Equals(member.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var isExist = db.Queryable<Model.User>()
                        .Where(it => it.Email.Equals(dto.Email))
                        .Select(it => new {it.Id})
                        .ToList()
                        .Any();
                    if (isExist)
                    {
                        return Error($"此邮箱 {dto.Email} 已存在");
                    }
                    member.Email = dto.Email;
                }


                member.Avatar = dto.Avatar;
                member.Gender = dto.Gender;
                member.Birthday = dto.BirthDay;
                
                db.UseTran(tran =>
                {
                    tran.Updateable(member).UpdateColumns(it => new
                    {
                        it.Username,
                        it.Password,
                        it.PasswordSalt,
                        it.Email,
                        it.Avatar,
                        it.Gender,
                        it.Birthday
                    }).ExecuteCommand();

                    tran.Deleteable<UserRole>().Where(it => it.UserId == member.Id).ExecuteCommand();

                    if (dto.Roles != null && dto.Roles.Length > 0)
                    {
                        var newRoles = dto.Roles.Select(it => new UserRole
                        {
                            ExpireDate = null,
                            UserId = member.Id,
                            RoleId = it
                        }).ToList();
                        tran.Insertable(newRoles).ExecuteCommand();
                    }

                    return 0;
                });
                
                _distributedCache.Remove($"CACHE_MEMBER_{member.Uuid}");

                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult ChangePassword(ChangePasswordDto dto)
        {
            using (var db = DbFactory.CreateClient())
            {
                var member = db.Queryable<Model.User>().InSingle(dto.MemberId);
                if (member == null)
                {
                    return Error("找不到该用户");
                }
                
                var inputEncryptOldPassword = _encryptionService.CreatePasswordHash(dto.OldPassword, member.PasswordSalt, "SHA512");
                if (!inputEncryptOldPassword.Equals(member.Password, StringComparison.OrdinalIgnoreCase))
                {
                    return Error("原密码不正确");
                }

                var saltKey = _encryptionService.CreateSaltKey(6);
                var newPassword = _encryptionService.CreatePasswordHash(dto.NewPassword, saltKey, "SHA512");

                member.Password = newPassword;
                member.PasswordSalt = saltKey;

                db.Updateable(member).UpdateColumns(it => new
                {
                    it.Password,
                    it.PasswordSalt
                }).ExecuteCommand();
            }

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateAvatar(UpdateAvatarDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var member = client.Queryable<Model.User>()
                    .Select(it => new {it.Id, UId = it.Uuid})
                    .First(it => it.Id == dto.MemberId);
                if (member == null)
                    return Error("没找到对象");
                
                client.Updateable<Model.User>()
                    .SetColumns(it => new Model.User() { Avatar = dto.Avatar })
                    .Where(it=>it.Id==dto.MemberId)
                    .ExecuteCommand();
                
                _distributedCache.Remove($"CACHE_MEMBER_{member.UId}");
            }
            
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult UpdateUsername(UpdateUsernameDto dto)
        {
            using (var client = DbFactory.CreateClient())
            {
                var user = client.Queryable<Model.User>()
                    .Select(it => new {it.Id, UId = it.Uuid, it.Username})
                    .First(it => it.Id == dto.MemberId);
                if (user == null)
                    return Error("没找到对象");

                if (user.Username.Equals(dto.Username, StringComparison.OrdinalIgnoreCase))
                    return Error("用户名未变更");

                var isExist = client.Queryable<Model.User>()
                    .Select(it => new { it.Id,it.Username })
                    .First(it => it.Username.Equals(dto.Username))!=null;
                if (isExist)
                    return Error($"此用户名 {dto.Username} 已存在");

                client.Updateable<Model.User>()
                    .SetColumns(it => new Model.User() { Username = dto.Username })
                    .Where(it => it.Id == dto.MemberId)
                    .ExecuteCommand();

                _distributedCache.Remove($"CACHE_MEMBER_{user.UId}");
            }

            return Ok();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<PagedList<QueryUserItem>> PagedQueryMembers(PagedQueryUserDto dto)
        {
            var page = dto.Page ?? 1;
            var pageSize = dto.PageSize ?? 20;
            using (var client = DbFactory.CreateClient())
            {
                var total = 0;
                var list = client.Queryable<Model.User>()
                    .WhereIF(!dto.Keyword.IsNullOrEmpty(), t => t.Username.Contains(dto.Keyword))
                    .Select(t => new QueryUserItem()
                    {
                        Id = t.Id,
                        Username = t.Username,
                        Email = t.Email,
                        Avatar = t.Avatar,
                        Gender = t.Gender,
                        Birthday = t.Birthday,
                        RegisterIp = t.RegisterIp,
                        RegisterDatetime = t.RegisterDatetime,
                        LoginCount = t.LoginCount,
                        LastLoginIp = t.LastLoginIp,
                        LastLoginDatetime = t.LastLoginDatetime,
                        CannotLoginUntilDate = t.CannotLoginUntilDatetime,
                        IsActive = t.IsActive,
                        IsDelete = t.IsDelete
                    })
                    .OrderBy("id DESC")
                    .ToPageList(page, pageSize, ref total);

                var userIds = list.Select(it => it.Id).ToArray();
                var roles = client.Queryable<UserRole, Role>((mr, r) => new object[]
                    {
                        JoinType.Left, mr.RoleId == r.Id
                    })
                    .Where((mr, r) => userIds.Contains(mr.UserId))
                    .Select((mr, r) => new
                    {
                        mr.UserId,
                        r.Name
                    })
                    .ToList();
                list.ForEach(it => { it.Roles = roles.Where(x => x.UserId == it.Id).Select(x => x.Name).ToArray(); });

                return new PagedList<QueryUserItem>
                {
                    List = list,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total
                };
            }
        }

        /// <summary>
        /// 用户名密码登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<MemberLoginInfo> LoginWithUsernamePassword(LoginWithUsernamePasswordDto dto)
        {
            static LoginWithUsernamePasswordQueryableItem GetMember(string username)
            {
                // ReSharper disable once ConvertToUsingDeclaration
                using (var client = DbFactory.CreateClient())
                {
                    return client.Queryable<Model.User>()
                        .Select(it => new LoginWithUsernamePasswordQueryableItem
                        {
                            Id = it.Id,
                            UId = it.UuId,
                            Username = it.Username,
                            Password = it.Password,
                            PasswordSalt = it.PasswordSalt,
                            IsDelete = it.IsDelete,
                            IsActive = it.IsActive,
                            CannotLoginUntilDate = it.CannotLoginUntilDate
                        })
                        .Single(t => t.Username.Equals(username));
                }
            }

            var member = GetMember(dto.Username);
            if (member == null)
            {
                return Error("用户不存在");
            }

            if (member.UId.IsNullOrEmpty())
            {
                return Error("用户UID为空");
            }

            if (member.IsDelete)
            {
                return Error("用户已删除");
            }

            if (!member.IsActive)
            {
                return Error("用户未激活");
            }

            if (member.CannotLoginUntilDate != null && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
            {
                return Error("用户未解封，请等待");
            }

            if (!PasswordsMatch(member.Password, member.PasswordSalt, dto.Password))
            {
                return Error("密码错误");
            }

            var tokenInfo = _jwtService.GenerateTokenInfo(member.UId);
            return new MemberLoginInfo
            {
                AccessToken = tokenInfo.JwtToken,
                RefreshToken = tokenInfo.RefreshToken,
                ExpireDateTime = tokenInfo.ExpireDateTime
            };
        }

        /// <summary>
        /// 三方平台登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PublicResult<MemberLoginInfo> ExternalAuth(ExternalAuthDto dto)
        {
            ExternalAuthentication externalAuthentication;
            string memberUId;
            using (var db = DbFactory.CreateClient())
            {
                externalAuthentication = db.Queryable<ExternalAuthentication>().Single(t =>
                        t.Openid.Equals(dto.OpenId, StringComparison.OrdinalIgnoreCase) &&
                        t.Provider == dto.Provider);
            }

            if (externalAuthentication == null)
            {
                var registerResult = _registrationService.Register(new ExternalRegistrationDto
                {
                    Provider = dto.Provider,
                    AccessToken = dto.AccessToken,
                    ExpireDt = dto.ExpireDt,
                    OpenId = dto.OpenId,
                    RefreshToken = dto.RefreshToken,
                    UserInfo = dto.UserInfo
                });
                if (registerResult.Code > 0)
                {
                    return Error(registerResult.Message, registerResult.Code);
                }
                memberUId = registerResult.Data.MemberUId;
            }
            else
            {
                var memberId = externalAuthentication.MemberId;
                using (var db = DbFactory.CreateClient())
                {
                    var member = db.Queryable<Model.User>()
                        .Select(it => new {it.Id, UId = it.UuId, it.Avatar,it.IsDelete, it.IsActive, it.CannotLoginUntilDate})
                        .Single(it => it.Id == memberId);
                    
                    if (member == null)
                    {
                        return Error("用户不存在");
                    }

                    if (member.UId.IsNullOrEmpty())
                    {
                        return Error("用户UID为空");
                    }

                    if (member.IsDelete)
                    {
                        return Error("用户已删除");
                    }

                    if (!member.IsActive)
                    {
                        return Error("用户未激活");
                    }

                    if (member.CannotLoginUntilDate != null && member.CannotLoginUntilDate.Value > DateTime.UtcNow)
                    {
                        return Error("用户未解封，请等待");
                    }

                    externalAuthentication.AccessToken = dto.AccessToken;
                    externalAuthentication.ExpireDt = dto.ExpireDt;
                    externalAuthentication.RefreshToken = dto.RefreshToken;

                    db.UseTran(tran =>
                    {
                        tran.Updateable(externalAuthentication).ExecuteCommand();

                        if (member.Avatar.IsNullOrEmpty() &&
                            !(dto.UserInfo?.Avatar?.IsNullOrEmpty() ?? true))
                        {
                            tran.Updateable<Model.User>()
                                .SetColumns(it => new Model.User()
                                {
                                    Avatar = dto.UserInfo.Avatar
                                })
                                .Where(it => it.Id == memberId)
                                .ExecuteCommand();
                        }
                    });

                    memberUId = member.UId;
                }
            }

            var tokenInfo = _jwtService.GenerateTokenInfo(memberUId);
            return new MemberLoginInfo
            {
                AccessToken = tokenInfo.JwtToken,
                RefreshToken = tokenInfo.RefreshToken,
                ExpireDateTime = tokenInfo.ExpireDateTime
            };
        }

        #endregion
    }
}