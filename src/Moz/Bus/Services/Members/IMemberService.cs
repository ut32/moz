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
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult ResetPassword(ServRequest<ResetPasswordDto> request);

        /// <summary>
        /// 获取用户详细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns> 
        ServResult<GetMemberDetailApo> GetMemberDetail(ServRequest<GetMemberDetailDto> request);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<CreateMemberApo> CreateMember(ServRequest<CreateMemberDto> request);
        
        /// <summary> 
        /// 更新用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult UpdateMember(ServRequest<UpdateMemberDto> request);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult ChangePassword(ServRequest<ChangePasswordDto> request);

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ServResult<PagedList<QueryMemberItem>> PagedQueryMembers(ServRequest<PagedQueryMemberDto> request);
    }
}