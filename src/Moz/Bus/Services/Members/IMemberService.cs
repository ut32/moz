using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moz.Bus.Dtos;
using Moz.Bus.Dtos.Members;
using Moz.Bus.Models.Members;
using Moz.Utils.Impl;
using SqlSugar;

namespace Moz.Bus.Services.Members
{
    public interface IMemberService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        SimpleMember GetSimpleMemberByUId(string uid); 

        /// <summary> 
        /// 重置密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult ResetPassword(ResetPasswordDto dto);

        /// <summary>
        /// 获取用户详细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns> 
        PublicResult<GetMemberDetailApo> GetMemberDetail(GetMemberDetailDto dto);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<CreateMemberApo> CreateMember(CreateMemberDto dto);
        
        /// <summary> 
        /// 更新用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateMember(UpdateMemberDto dto);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult ChangePassword(ChangePasswordDto dto);

        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateAvatar(UpdateAvatarDto dto);

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult UpdateUsername(UpdateUsernameDto dto);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        PublicResult<PagedList<QueryMemberItem>> PagedQueryMembers(PagedQueryMemberDto dto);
    }
}