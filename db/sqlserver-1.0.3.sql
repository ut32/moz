/*
 Navicat Premium Data Transfer

 Source Server         : sql2014
 Source Server Type    : SQL Server
 Source Server Version : 12002000
 Source Host           : .\sql2014:1433
 Source Catalog        : moz
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 12002000
 File Encoding         : 65001

 Date: 12/02/2020 13:11:44
*/


-- ----------------------------
-- Table structure for tab_ad
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_ad]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_ad]
GO

CREATE TABLE [dbo].[tab_ad] (
  [id] bigint  IDENTITY(7,1) NOT NULL,
  [ad_place_id] bigint  NOT NULL,
  [title] nvarchar(300) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [image_path] nvarchar(300) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [target_url] nvarchar(400) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [order_index] int  NOT NULL,
  [is_show] binary(1)  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_ad] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_ad]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_ad] ON
GO

INSERT INTO [dbo].[tab_ad] ([id], [ad_place_id], [title], [image_path], [target_url], [order_index], [is_show]) VALUES (N'6', N'2', N'课程', N'/upload/72ffc987f89f4611b72556963ec92aa1.jpg', N'/course/index/3', N'0', 0x01)
GO

SET IDENTITY_INSERT [dbo].[tab_ad] OFF
GO


-- ----------------------------
-- Table structure for tab_ad_place
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_ad_place]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_ad_place]
GO

CREATE TABLE [dbo].[tab_ad_place] (
  [id] bigint  IDENTITY(3,1) NOT NULL,
  [title] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [code] nvarchar(100) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [desc] nvarchar(255) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [addtime] datetime2  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_ad_place] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_ad_place]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_ad_place] ON
GO

INSERT INTO [dbo].[tab_ad_place] ([id], [title], [code], [desc], [addtime]) VALUES (N'2', N'顶部', N'index.top', N'宽高 750x250', N'2019-07-01 09:43:19')
GO

SET IDENTITY_INSERT [dbo].[tab_ad_place] OFF
GO


-- ----------------------------
-- Table structure for tab_admin_menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_admin_menu]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_admin_menu]
GO

CREATE TABLE [dbo].[tab_admin_menu] (
  [id] bigint  IDENTITY(14,1) NOT NULL,
  [name] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [parent_id] bigint DEFAULT NULL NULL,
  [link] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [order_index] int  NOT NULL,
  [icon] nvarchar(40) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [is_system] binary(1) DEFAULT (0x00) NOT NULL
)
GO

ALTER TABLE [dbo].[tab_admin_menu] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_admin_menu]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_admin_menu] ON
GO

INSERT INTO [dbo].[tab_admin_menu] ([id], [name], [parent_id], [link], [order_index], [icon], [is_system]) VALUES (N'1', N'内容管理', NULL, N'#', N'0', N'layui-icon layui-icon-component', 0x00)
GO

INSERT INTO [dbo].[tab_admin_menu] ([id], [name], [parent_id], [link], [order_index], [icon], [is_system]) VALUES (N'11', N'菜单2', N'1', N'qscourse/index', N'10', N'layui-icon layui-icon-component', 0x00)
GO

INSERT INTO [dbo].[tab_admin_menu] ([id], [name], [parent_id], [link], [order_index], [icon], [is_system]) VALUES (N'13', N'菜单1', N'1', N'menu/index', N'0', N'layui-icon layui-icon-component', 0x00)
GO

SET IDENTITY_INSERT [dbo].[tab_admin_menu] OFF
GO


-- ----------------------------
-- Table structure for tab_article
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_article]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_article]
GO

CREATE TABLE [dbo].[tab_article] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [article_type_id] bigint  NOT NULL,
  [category_id] bigint DEFAULT NULL NULL,
  [title] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [sub_title] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [title_color] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [title_bold] binary(1) DEFAULT NULL NULL,
  [summary] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [content] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [tags] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [thumb_image] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [video] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [source] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [author] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [hits] int DEFAULT NULL NULL,
  [addtime] datetime2 DEFAULT NULL NULL,
  [order_index] int DEFAULT NULL NULL,
  [is_top] binary(1) DEFAULT NULL NULL,
  [is_recommend] binary(1) DEFAULT NULL NULL,
  [seo_title] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [seo_keyword] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [seo_description] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [string1] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [string2] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [string3] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [string4] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [int1] int DEFAULT NULL NULL,
  [int2] int DEFAULT NULL NULL,
  [int3] int DEFAULT NULL NULL,
  [int4] int DEFAULT NULL NULL,
  [decimal1] decimal(18,2) DEFAULT NULL NULL,
  [decimal2] decimal(18,2) DEFAULT NULL NULL,
  [decimal3] decimal(18,2) DEFAULT NULL NULL,
  [decimal4] decimal(18,2) DEFAULT NULL NULL,
  [datetime1] datetime2 DEFAULT NULL NULL,
  [datetime2] datetime2 DEFAULT NULL NULL,
  [datetime3] datetime2 DEFAULT NULL NULL,
  [datetime4] datetime2 DEFAULT NULL NULL,
  [bool1] binary(1) DEFAULT NULL NULL,
  [bool2] binary(1) DEFAULT NULL NULL,
  [bool3] binary(1) DEFAULT NULL NULL,
  [bool4] binary(1) DEFAULT NULL NULL,
  [text1] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [text2] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [text3] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [text4] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[tab_article] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_article_model
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_article_model]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_article_model]
GO

CREATE TABLE [dbo].[tab_article_model] (
  [id] bigint  IDENTITY(11,1) NOT NULL,
  [name] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [configuration] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [category_id] bigint DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_article_model] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_article_model]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_article_model] ON
GO

INSERT INTO [dbo].[tab_article_model] ([id], [name], [configuration], [category_id]) VALUES (N'10', N'新闻中心', N'[{"FiledName":"Title","DisplayName":"标题","IsEnable":true,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"SubTitle","DisplayName":"副标题","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"TitleColor","DisplayName":"颜色","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"TitleBold","DisplayName":"标题粗体","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Summary","DisplayName":"摘要","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Content","DisplayName":"内容","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Tags","DisplayName":"标签","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":100},{"FiledName":"ThumbImage","DisplayName":"缩略图","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Video","DisplayName":"视频","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Source","DisplayName":"来源","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Author","DisplayName":"作者","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Hits","DisplayName":"点击次数","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Addtime","DisplayName":"添加时间","IsEnable":false,"DisplayType":8,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"OrderIndex","DisplayName":"排序","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"IsTop","DisplayName":"是否置顶","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"IsRecommend","DisplayName":"是否推荐","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"SeoTitle","DisplayName":"SeoTitle","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"SeoKeyword","DisplayName":"SeoKeyword","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"SeoDescription","DisplayName":"SeoDescription","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"String1","DisplayName":"String1","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"String2","DisplayName":"String2","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"String3","DisplayName":"String3","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"String4","DisplayName":"String4","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Int1","DisplayName":"Int1","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Int2","DisplayName":"Int2","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Int3","DisplayName":"Int3","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Int4","DisplayName":"Int4","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Long1","DisplayName":"Long1","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":100},{"FiledName":"Long2","DisplayName":"Long2","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":100},{"FiledName":"Long3","DisplayName":"Long3","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":100},{"FiledName":"Long4","DisplayName":"Long4","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":100},{"FiledName":"Decimal1","DisplayName":"Decimal1","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Decimal2","DisplayName":"Decimal2","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Decimal3","DisplayName":"Decimal3","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Decimal4","DisplayName":"Decimal4","IsEnable":false,"DisplayType":1,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Datetime1","DisplayName":"DateTime1","IsEnable":false,"DisplayType":8,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Datetime2","DisplayName":"DateTime2","IsEnable":false,"DisplayType":8,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Datetime3","DisplayName":"DateTime3","IsEnable":false,"DisplayType":8,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Datetime4","DisplayName":"DateTime4","IsEnable":false,"DisplayType":8,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Bool1","DisplayName":"Bool1","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Bool2","DisplayName":"Bool2","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Bool3","DisplayName":"Bool3","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Bool4","DisplayName":"Bool4","IsEnable":false,"DisplayType":16,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Text1","DisplayName":"Text1","IsEnable":false,"DisplayType":64,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Text2","DisplayName":"Text2","IsEnable":false,"DisplayType":64,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Text3","DisplayName":"Text3","IsEnable":false,"DisplayType":64,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0},{"FiledName":"Text4","DisplayName":"Text4","IsEnable":false,"DisplayType":64,"Options":null,"Description":null,"DefaultValue":null,"IsMultiLanguage":false,"IsShowedInList":false,"IsEnableSearch":false,"IsRequired":false,"OrderIndex":0}]', NULL)
GO

SET IDENTITY_INSERT [dbo].[tab_article_model] OFF
GO


-- ----------------------------
-- Table structure for tab_category
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_category]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_category]
GO

CREATE TABLE [dbo].[tab_category] (
  [id] bigint  IDENTITY(57,1) NOT NULL,
  [name] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [alias] nvarchar(100) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [description] nvarchar(2400) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [order_index] int  NOT NULL,
  [parent_id] bigint DEFAULT NULL NULL,
  [path] nvarchar(300) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_category] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_category]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_category] ON
GO

INSERT INTO [dbo].[tab_category] ([id], [name], [alias], [description], [order_index], [parent_id], [path]) VALUES (N'17', N'产品分类', N'product', N'1118', N'0', NULL, N'17')
GO

INSERT INTO [dbo].[tab_category] ([id], [name], [alias], [description], [order_index], [parent_id], [path]) VALUES (N'56', N'新闻分类', N'news', NULL, N'0', NULL, N'56')
GO

SET IDENTITY_INSERT [dbo].[tab_category] OFF
GO


-- ----------------------------
-- Table structure for tab_external_authentication
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_external_authentication]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_external_authentication]
GO

CREATE TABLE [dbo].[tab_external_authentication] (
  [id] bigint  IDENTITY(936,1) NOT NULL,
  [member_id] bigint  NOT NULL,
  [provider] smallint  NOT NULL,
  [openid] nvarchar(120) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [access_token] nvarchar(200) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [refresh_token] nvarchar(200) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [expire_dt] datetime2 DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_external_authentication] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_external_authentication]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_external_authentication] ON
GO

INSERT INTO [dbo].[tab_external_authentication] ([id], [member_id], [provider], [openid], [access_token], [refresh_token], [expire_dt]) VALUES (N'935', N'1593', N'4', N'o6BAG1YUe5WZxJV7pu1MeqmwqWO0', NULL, NULL, N'2020-03-08 14:24:26')
GO

SET IDENTITY_INSERT [dbo].[tab_external_authentication] OFF
GO


-- ----------------------------
-- Table structure for tab_generic_attribute
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_generic_attribute]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_generic_attribute]
GO

CREATE TABLE [dbo].[tab_generic_attribute] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [entity_id] bigint  NOT NULL,
  [key_group] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [key] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [value] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_generic_attribute] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_identify
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_identify]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_identify]
GO

CREATE TABLE [dbo].[tab_identify] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [rel] binary(1) DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_identify] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_language
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_language]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_language]
GO

CREATE TABLE [dbo].[tab_language] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [name] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [language_culture] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [published] smallint  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_language] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_locale_string_resource
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_locale_string_resource]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_locale_string_resource]
GO

CREATE TABLE [dbo].[tab_locale_string_resource] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [language_id] bigint  NOT NULL,
  [name] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [value] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_locale_string_resource] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_localized_property
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_localized_property]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_localized_property]
GO

CREATE TABLE [dbo].[tab_localized_property] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [entity_id] bigint  NOT NULL,
  [language_id] bigint  NOT NULL,
  [locale_key_group] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [locale_key] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [locale_value] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_localized_property] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_member
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_member]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_member]
GO

CREATE TABLE [dbo].[tab_member] (
  [id] bigint  IDENTITY(1595,1) NOT NULL,
  [uid] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [username] nvarchar(64) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [password] nvarchar(128) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [password_salt] nchar(8) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [email] nvarchar(100) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [mobile] nvarchar(64) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [avatar] nvarchar(250) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [gender] int DEFAULT NULL NULL,
  [birthday] date DEFAULT NULL NULL,
  [register_ip] nvarchar(39) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [register_datetime] datetime2  NOT NULL,
  [login_count] int  NOT NULL,
  [last_login_ip] nvarchar(39) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [last_login_datetime] datetime2  NOT NULL,
  [cannot_login_until_date] datetime2 DEFAULT NULL NULL,
  [last_active_datetime] datetime2  NOT NULL,
  [failed_login_attempts] int  NOT NULL,
  [online_time_count] int  NOT NULL,
  [address] nvarchar(250) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [region_code] nvarchar(100) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [lng] decimal(30,18) DEFAULT NULL NULL,
  [lat] decimal(30,18) DEFAULT NULL NULL,
  [geohash] nvarchar(10) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [is_active] smallint  NOT NULL,
  [is_delete] smallint  NOT NULL,
  [is_email_valid] smallint  NOT NULL,
  [is_mobile_valid] smallint  NOT NULL,
  [nickname] nvarchar(20) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_member] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_member]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_member] ON
GO

INSERT INTO [dbo].[tab_member] ([id], [uid], [username], [password], [password_salt], [email], [mobile], [avatar], [gender], [birthday], [register_ip], [register_datetime], [login_count], [last_login_ip], [last_login_datetime], [cannot_login_until_date], [last_active_datetime], [failed_login_attempts], [online_time_count], [address], [region_code], [lng], [lat], [geohash], [is_active], [is_delete], [is_email_valid], [is_mobile_valid], [nickname]) VALUES (N'1', N'4fe33034296311ea86e500163e00111a', N'admin', N'7D3465CD1DAD77A3BC211380CC40D13CAEC1ACB30E5437D83C86326682BA41BBC4E2D569B6A4E70549EDE76854452443E286043A375C1FBFAB2C47AEBEA8D9A4', N'8H/d9FrN', N'zdh082@qq.com', NULL, N'https://wx.qlogo.cn/mmopen/vi_32/NSWdycs0F7TvIug7AhAn5TcZsjB7ribrX4yw2YTvMwuApoH5OCVFJ9pDicUbAaASqwmTufpSAFxicXBs9WRVQiaiaNA/132', N'1', N'1990-01-01', N'192.168.3.114', N'2018-01-09 09:32:21', N'0', N'192.168.3.114', N'2018-01-09 09:32:21', NULL, N'2018-01-09 09:32:21', N'0', N'0', N'', N'', N'104.063615882866000000', N'30.672543514548500000', N'wm6n2pbbd', N'1', N'0', N'0', N'0', NULL)
GO

INSERT INTO [dbo].[tab_member] ([id], [uid], [username], [password], [password_salt], [email], [mobile], [avatar], [gender], [birthday], [register_ip], [register_datetime], [login_count], [last_login_ip], [last_login_datetime], [cannot_login_until_date], [last_active_datetime], [failed_login_attempts], [online_time_count], [address], [region_code], [lng], [lat], [geohash], [is_active], [is_delete], [is_email_valid], [is_mobile_valid], [nickname]) VALUES (N'1552', N'96991deb4fbe40959ba75ab38f5d040b', N'test', N'554CDD69FFBA03B1C8E4E88D9CE2746AB503DD523E9A4A7F6025D089F7F5C6C99D32944C4E90F34BD7E2077AB4A080488FD2FBA6DEB6671EA562BAD171018567', N'MMbLVw4=', NULL, NULL, N'http://thirdwx.qlogo.cn/mmopen/vi_32/gdiajvBXJtMibRoqchJTVKTrkXSicPuKAp86oM5dv12YzLS0ZCTnFI19ibmfdVjHnGjM6AIU6W6TlQsoVFSKSOEXTQ/132', N'0', NULL, NULL, N'2020-01-09 14:31:53', N'0', NULL, N'2020-01-09 14:31:53', NULL, N'2020-01-09 14:31:53', N'0', N'0', NULL, NULL, NULL, NULL, NULL, N'1', N'0', N'0', N'0', N'the7')
GO

INSERT INTO [dbo].[tab_member] ([id], [uid], [username], [password], [password_salt], [email], [mobile], [avatar], [gender], [birthday], [register_ip], [register_datetime], [login_count], [last_login_ip], [last_login_datetime], [cannot_login_until_date], [last_active_datetime], [failed_login_attempts], [online_time_count], [address], [region_code], [lng], [lat], [geohash], [is_active], [is_delete], [is_email_valid], [is_mobile_valid], [nickname]) VALUES (N'1593', N'34a7c9fbe6d14dea92b7a5c0d4f1af40', N'mp_1453', N'942C8D0513010DD2712F264EFF2F8E255C6778110FC2AC08DF185C2141AA2A9C18194A042E585ACEA505E8A6418F152EC2BEB35A83162FB330AC38BC68BD55A7', N'ZlPFJPfn', NULL, NULL, NULL, N'0', NULL, NULL, N'2020-02-08 06:24:26', N'0', NULL, N'2020-02-08 06:24:26', NULL, N'2020-02-08 06:24:26', N'0', N'0', NULL, NULL, NULL, NULL, NULL, N'1', N'0', N'0', N'0', N'mp')
GO

INSERT INTO [dbo].[tab_member] ([id], [uid], [username], [password], [password_salt], [email], [mobile], [avatar], [gender], [birthday], [register_ip], [register_datetime], [login_count], [last_login_ip], [last_login_datetime], [cannot_login_until_date], [last_active_datetime], [failed_login_attempts], [online_time_count], [address], [region_code], [lng], [lat], [geohash], [is_active], [is_delete], [is_email_valid], [is_mobile_valid], [nickname]) VALUES (N'1594', N'1f5653659466419eabd888dbc728f02a', N'fffff', N'F5DC2E97BB5F9A62808BC5036998802C5BBDBC93E97581B758CB22258DD35BEDE64BE0A7DC8025E6DE409ADC0136E1F8C617EAA562ABC0D4381B946DF184A701', N'LP5IbwkQ', NULL, NULL, NULL, N'1', NULL, NULL, N'2020-02-09 17:38:56', N'0', NULL, N'2020-02-09 17:38:56', NULL, N'2020-02-09 17:38:56', N'0', N'0', NULL, NULL, NULL, NULL, NULL, N'1', N'0', N'0', N'0', NULL)
GO

SET IDENTITY_INSERT [dbo].[tab_member] OFF
GO


-- ----------------------------
-- Table structure for tab_member_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_member_role]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_member_role]
GO

CREATE TABLE [dbo].[tab_member_role] (
  [id] bigint  IDENTITY(20,1) NOT NULL,
  [member_id] bigint  NOT NULL,
  [role_id] bigint  NOT NULL,
  [expire_date] date DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_member_role] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_member_role]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_member_role] ON
GO

INSERT INTO [dbo].[tab_member_role] ([id], [member_id], [role_id], [expire_date]) VALUES (N'10', N'1', N'1', NULL)
GO

INSERT INTO [dbo].[tab_member_role] ([id], [member_id], [role_id], [expire_date]) VALUES (N'11', N'1', N'2', NULL)
GO

INSERT INTO [dbo].[tab_member_role] ([id], [member_id], [role_id], [expire_date]) VALUES (N'12', N'1', N'3', NULL)
GO

INSERT INTO [dbo].[tab_member_role] ([id], [member_id], [role_id], [expire_date]) VALUES (N'17', N'1552', N'2', NULL)
GO

INSERT INTO [dbo].[tab_member_role] ([id], [member_id], [role_id], [expire_date]) VALUES (N'18', N'1593', N'3', NULL)
GO

INSERT INTO [dbo].[tab_member_role] ([id], [member_id], [role_id], [expire_date]) VALUES (N'19', N'1594', N'3', NULL)
GO

SET IDENTITY_INSERT [dbo].[tab_member_role] OFF
GO


-- ----------------------------
-- Table structure for tab_message
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_message]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_message]
GO

CREATE TABLE [dbo].[tab_message] (
  [id] bigint  NOT NULL,
  [msg_type] int  NOT NULL,
  [content] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [dest_member_id] bigint  NOT NULL,
  [my_member_id] bigint  NOT NULL,
  [add_time] datetime2  NOT NULL,
  [is_read] binary(1)  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_message] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_moz_engine
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_moz_engine]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_moz_engine]
GO

CREATE TABLE [dbo].[tab_moz_engine] (
  [id] bigint  NOT NULL,
  [version] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [code] int  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_moz_engine] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_moz_engine]
-- ----------------------------
INSERT INTO [dbo].[tab_moz_engine]  VALUES (N'1', N'1.0.3', N'103')
GO


-- ----------------------------
-- Table structure for tab_permission
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_permission]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_permission]
GO

CREATE TABLE [dbo].[tab_permission] (
  [id] bigint  IDENTITY(56,1) NOT NULL,
  [name] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [code] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [is_active] binary(1)  NOT NULL,
  [parent_id] bigint DEFAULT NULL NULL,
  [order_index] int DEFAULT ((0)) NOT NULL,
  [is_system] binary(1) DEFAULT (0x00) NOT NULL
)
GO

ALTER TABLE [dbo].[tab_permission] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_permission]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_permission] ON
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'1', N'后台管理', N'admin.access', 0x01, NULL, N'9', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'2', N'文章管理', N'UPLOAD_IMAGE', 0x01, N'1', N'50', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'4', N'模型管理', N'admin.articlemodel.index', 0x01, N'2', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'7', N'角色管理', N'admin.role', 0x01, N'1', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'8', N'添加角色', N'admin.role.crate', 0x01, N'7', N'20', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'9', N'修改角色', N'admin.role.update', 0x01, N'7', N'10', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'12', N'删除角色', N'admin.role.delete', 0x01, N'7', N'40', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'14', N'开启/关闭角色', N'admin.role.setisactive', 0x01, N'7', N'50', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'15', N'设置成管理组', N'admin.role.setisadmin', 0x01, N'7', N'60', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'16', N'配置权限', N'admin.role.configpermission', 0x01, N'7', N'70', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'17', N'配置管理菜单', N'admin.role.configmenu', 0x01, N'7', N'80', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'19', N'前台会员中心', N'member.access', 0x01, NULL, N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'20', N'个人资料', N'member.profile', 0x01, N'19', N'0', 0x00)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'21', N'修改资料', N'member.profile.update', 0x01, N'20', N'80', 0x00)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'22', N'上传头像', N'member.profile.uploadavatar', 0x01, N'20', N'30', 0x00)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'23', N'权限管理', N'admin.permission', 0x01, N'1', N'10', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'24', N'添加权限', N'admin.permission.create', 0x01, N'23', N'20', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'25', N'编辑权限', N'admin.permission.update', 0x01, N'23', N'30', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'26', N'删除权限', N'admin.permission.delete', 0x01, N'23', N'40', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'27', N'设置开启', N'admin.permission.isactive', 0x01, N'23', N'50', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'28', N'列表展示', N'admin.permission.index', 0x01, N'23', N'10', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'29', N'列表展示', N'admin.role.index', 0x01, N'7', N'10', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'30', N'菜单管理', N'admin.menu', 0x01, N'1', N'201', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'31', N'列表展示', N'admin.menu.index', 0x01, N'30', N'10', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'32', N'添加菜单', N'admin.menu.create', 0x01, N'30', N'20', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'33', N'修改菜单', N'admin.menu.update', 0x01, N'30', N'30', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'34', N'删除菜单', N'admin.menu.delete', 0x01, N'30', N'40', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'35', N'文章模型管理', N'admin.article.model', 0x01, N'1', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'36', N'列表展示', N'admin.article.model.index', 0x01, N'35', N'10', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'37', N'添加模型', N'admin.article.model.create', 0x01, N'35', N'20', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'38', N'编辑模型', N'admin.article.model.update', 0x01, N'35', N'30', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'39', N'删除模型', N'admin.article.model.delete', 0x01, N'35', N'40', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'40', N'定时任务', N'admin.scheduleTask', 0x01, N'1', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'41', N'列表', N'admin.scheduleTask.index', 0x01, N'40', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'42', N'添加', N'admin.scheduleTask.create', 0x01, N'40', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'43', N'编辑', N'admin.scheduleTask.update', 0x01, N'40', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'44', N'删除', N'admin.scheduleTask.delete', 0x01, N'40', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'45', N'开启/关闭', N'admin.scheduleTask.setisenable', 0x01, N'40', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'46', N'立即执行一次', N'admin.scheduleTask.execute', 0x01, N'40', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'47', N'分类管理', N'admin.category', 0x01, N'1', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'48', N'列表页', N'admin.category.index', 0x01, N'47', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'49', N'添加', N'admin.category.create', 0x01, N'47', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'50', N'修改', N'admin.category.update', 0x01, N'47', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'51', N'删除', N'admin.category.delete', 0x01, N'47', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'52', N'设置排序', N'admin.category.setOrderIndex', 0x01, N'47', N'0', 0x01)
GO

INSERT INTO [dbo].[tab_permission] ([id], [name], [code], [is_active], [parent_id], [order_index], [is_system]) VALUES (N'55', N'修改密码', N'member.profile.editpassword', 0x01, N'20', N'0', 0x00)
GO

SET IDENTITY_INSERT [dbo].[tab_permission] OFF
GO


-- ----------------------------
-- Table structure for tab_profile
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_profile]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_profile]
GO

CREATE TABLE [dbo].[tab_profile] (
  [id] bigint  NOT NULL,
  [header_bg_id] bigint  NOT NULL,
  [intro] nvarchar(1) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [like_gender] smallint  NOT NULL,
  [like_age] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [wechat] nvarchar(50) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [qq] nvarchar(50) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [twitter] nvarchar(50) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [weight] int  NOT NULL,
  [height] int  NOT NULL,
  [income] smallint  NOT NULL,
  [religion] smallint  NOT NULL,
  [i_viewed] int  NOT NULL,
  [i_liked] int  NOT NULL,
  [i_gift_presented] int  NOT NULL,
  [viewed_me] int  NOT NULL,
  [liked_me] int  NOT NULL,
  [gift_prensented_to_me] int  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_profile] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_resource
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_resource]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_resource]
GO

CREATE TABLE [dbo].[tab_resource] (
  [id] bigint  IDENTITY(3,1) NOT NULL,
  [server] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [relative_path] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [content_type] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [properies] nvarchar(1000) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_resource] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_resource]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_resource] ON
GO

INSERT INTO [dbo].[tab_resource] ([id], [server], [relative_path], [content_type], [properies]) VALUES (N'1', N'tdating1.oss-cn-hongkong.aliyuncs.com', N'/avatar/avatar_default.png', N'image/png', NULL)
GO

INSERT INTO [dbo].[tab_resource] ([id], [server], [relative_path], [content_type], [properies]) VALUES (N'2', N'tdating1.oss-cn-hongkong.aliyuncs.com', N'/top/timg%20%281%29.jpeg', N'image/jpeg', NULL)
GO

SET IDENTITY_INSERT [dbo].[tab_resource] OFF
GO


-- ----------------------------
-- Table structure for tab_reward_points_history
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_reward_points_history]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_reward_points_history]
GO

CREATE TABLE [dbo].[tab_reward_points_history] (
  [id] bigint  NOT NULL,
  [member_id] bigint  NOT NULL,
  [points] int  NOT NULL,
  [message] nvarchar(120) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [points_balance] int  NOT NULL,
  [add_time] datetime2  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_reward_points_history] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_role
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_role]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_role]
GO

CREATE TABLE [dbo].[tab_role] (
  [id] bigint  IDENTITY(5,1) NOT NULL,
  [name] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [is_active] binary(1)  NOT NULL,
  [code] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [is_admin] binary(1) DEFAULT (0x00) NOT NULL,
  [is_system] binary(1) DEFAULT (0x00) NOT NULL
)
GO

ALTER TABLE [dbo].[tab_role] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_role]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_role] ON
GO

INSERT INTO [dbo].[tab_role] ([id], [name], [is_active], [code], [is_admin], [is_system]) VALUES (N'1', N'超级管理员', 0x01, N'Administrator', 0x01, 0x01)
GO

INSERT INTO [dbo].[tab_role] ([id], [name], [is_active], [code], [is_admin], [is_system]) VALUES (N'2', N'一般管理员', 0x01, N'Manager', 0x01, 0x01)
GO

INSERT INTO [dbo].[tab_role] ([id], [name], [is_active], [code], [is_admin], [is_system]) VALUES (N'3', N'普通用户', 0x01, N'User', 0x00, 0x01)
GO

INSERT INTO [dbo].[tab_role] ([id], [name], [is_active], [code], [is_admin], [is_system]) VALUES (N'4', N'投稿员', 0x01, N'Publisher', 0x00, 0x00)
GO

SET IDENTITY_INSERT [dbo].[tab_role] OFF
GO


-- ----------------------------
-- Table structure for tab_role_menu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_role_menu]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_role_menu]
GO

CREATE TABLE [dbo].[tab_role_menu] (
  [id] bigint  IDENTITY(18,1) NOT NULL,
  [role_id] bigint  NOT NULL,
  [menu_id] bigint  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_role_menu] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_role_menu]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_role_menu] ON
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'1', N'1', N'1')
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'4', N'1', N'11')
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'12', N'2', N'1')
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'13', N'2', N'13')
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'15', N'3', N'1')
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'16', N'3', N'11')
GO

INSERT INTO [dbo].[tab_role_menu] ([id], [role_id], [menu_id]) VALUES (N'17', N'3', N'13')
GO

SET IDENTITY_INSERT [dbo].[tab_role_menu] OFF
GO


-- ----------------------------
-- Table structure for tab_role_permission
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_role_permission]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_role_permission]
GO

CREATE TABLE [dbo].[tab_role_permission] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [role_id] bigint  NOT NULL,
  [permission_id] bigint  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_role_permission] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_schedule_task
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_schedule_task]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_schedule_task]
GO

CREATE TABLE [dbo].[tab_schedule_task] (
  [id] bigint  IDENTITY(6,1) NOT NULL,
  [name] nvarchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [status] int  NOT NULL,
  [status_desc] nvarchar(max) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [job_key] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [job_group] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [trigger_key] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [trigger_group] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [is_enable] binary(1)  NOT NULL,
  [type] nvarchar(255) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [cron] nvarchar(40) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [interval] int DEFAULT NULL NULL,
  [last_start_time] datetime2 DEFAULT NULL NULL,
  [last_end_time] datetime2 DEFAULT NULL NULL,
  [last_success_time] datetime2 DEFAULT NULL NULL
)
GO

ALTER TABLE [dbo].[tab_schedule_task] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_schedule_task]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_schedule_task] ON
GO

INSERT INTO [dbo].[tab_schedule_task] ([id], [name], [status], [status_desc], [job_key], [job_group], [trigger_key], [trigger_group], [is_enable], [type], [cron], [interval], [last_start_time], [last_end_time], [last_success_time]) VALUES (N'5', N'测试任务', N'1', N'', N'', N'', N'', N'', 0x00, N'Moz.TaskSchedule.Jobs.MozTestJob,Moz, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null', N'0/8 * * * * ? ', NULL, N'2020-02-05 19:50:24', N'2020-02-05 19:50:24', N'2020-02-05 19:50:24')
GO

SET IDENTITY_INSERT [dbo].[tab_schedule_task] OFF
GO


-- ----------------------------
-- Table structure for tab_service_performance
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_service_performance]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_service_performance]
GO

CREATE TABLE [dbo].[tab_service_performance] (
  [id] bigint  IDENTITY(1,1) NOT NULL,
  [name] nvarchar(1000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [elapsed_ms] int DEFAULT ((0)) NOT NULL,
  [http_request_id] nvarchar(45) COLLATE Chinese_PRC_CI_AS DEFAULT NULL NULL,
  [add_time] datetime2  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_service_performance] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for tab_setting
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[tab_setting]') AND type IN ('U'))
	DROP TABLE [dbo].[tab_setting]
GO

CREATE TABLE [dbo].[tab_setting] (
  [id] bigint  IDENTITY(8,1) NOT NULL,
  [name] nvarchar(120) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [value] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[tab_setting] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Records of [tab_setting]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[tab_setting] ON
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'1', N'MemberSettings.HashedPasswordFormat', N'SHA512')
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'2', N'CommonSettings.Uploader', N'4')
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'3', N'CommonSettings.AliyunOSSServer', N'tdating1.oss-cn-hongkong.aliyuncs.com')
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'4', N'CommonSettings.AliyunOSSBucket', N'tdating1')
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'5', N'CommonSettings.AliyunOSSEndpoint', N'oss-cn-hongkong.aliyuncs.com')
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'6', N'CommonSettings.AliyunOSSAccessKeyId', N'LTAI9o4Zuap3y2Wn')
GO

INSERT INTO [dbo].[tab_setting] ([id], [name], [value]) VALUES (N'7', N'CommonSettings.AliyunOSSAccessKeySecret', N'9ER7IwNXTW5yoTuxLkkDlUDDVCM6iv')
GO

SET IDENTITY_INSERT [dbo].[tab_setting] OFF
GO


-- ----------------------------
-- Indexes structure for table tab_ad
-- ----------------------------
CREATE NONCLUSTERED INDEX [fk_ad_place_id]
ON [dbo].[tab_ad] (
  [ad_place_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_ad
-- ----------------------------
ALTER TABLE [dbo].[tab_ad] ADD CONSTRAINT [PK_tab_ad_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Uniques structure for table tab_ad_place
-- ----------------------------
ALTER TABLE [dbo].[tab_ad_place] ADD CONSTRAINT [tab_ad_place$idx_code] UNIQUE NONCLUSTERED ([code] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_ad_place
-- ----------------------------
ALTER TABLE [dbo].[tab_ad_place] ADD CONSTRAINT [PK_tab_ad_place_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_admin_menu
-- ----------------------------
CREATE NONCLUSTERED INDEX [parent_id]
ON [dbo].[tab_admin_menu] (
  [parent_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_admin_menu
-- ----------------------------
ALTER TABLE [dbo].[tab_admin_menu] ADD CONSTRAINT [PK_tab_admin_menu_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_article
-- ----------------------------
CREATE NONCLUSTERED INDEX [article_type_id]
ON [dbo].[tab_article] (
  [article_type_id] ASC
)
GO

CREATE NONCLUSTERED INDEX [fk_article_category_id]
ON [dbo].[tab_article] (
  [category_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_article
-- ----------------------------
ALTER TABLE [dbo].[tab_article] ADD CONSTRAINT [PK_tab_article_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_article_model
-- ----------------------------
ALTER TABLE [dbo].[tab_article_model] ADD CONSTRAINT [PK_tab_article_model_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_category
-- ----------------------------
CREATE NONCLUSTERED INDEX [category_fk_pid]
ON [dbo].[tab_category] (
  [parent_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_category
-- ----------------------------
ALTER TABLE [dbo].[tab_category] ADD CONSTRAINT [PK_tab_category_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_external_authentication
-- ----------------------------
CREATE NONCLUSTERED INDEX [idx_ea_unq_provider_openid]
ON [dbo].[tab_external_authentication] (
  [member_id] ASC
)
GO


-- ----------------------------
-- Uniques structure for table tab_external_authentication
-- ----------------------------
ALTER TABLE [dbo].[tab_external_authentication] ADD CONSTRAINT [tab_external_authentication$idx_ea_member_id] UNIQUE NONCLUSTERED ([provider] ASC, [openid] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_external_authentication
-- ----------------------------
ALTER TABLE [dbo].[tab_external_authentication] ADD CONSTRAINT [PK_tab_external_authentication_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_generic_attribute
-- ----------------------------
ALTER TABLE [dbo].[tab_generic_attribute] ADD CONSTRAINT [PK_tab_generic_attribute_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_identify
-- ----------------------------
ALTER TABLE [dbo].[tab_identify] ADD CONSTRAINT [PK_tab_identify_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_language
-- ----------------------------
ALTER TABLE [dbo].[tab_language] ADD CONSTRAINT [PK_tab_language_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_locale_string_resource
-- ----------------------------
CREATE NONCLUSTERED INDEX [FK_Reference_14]
ON [dbo].[tab_locale_string_resource] (
  [language_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_locale_string_resource
-- ----------------------------
ALTER TABLE [dbo].[tab_locale_string_resource] ADD CONSTRAINT [PK_tab_locale_string_resource_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_localized_property
-- ----------------------------
CREATE NONCLUSTERED INDEX [FK_Reference_15]
ON [dbo].[tab_localized_property] (
  [language_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_localized_property
-- ----------------------------
ALTER TABLE [dbo].[tab_localized_property] ADD CONSTRAINT [PK_tab_localized_property_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Uniques structure for table tab_member
-- ----------------------------
ALTER TABLE [dbo].[tab_member] ADD CONSTRAINT [tab_member$unq_member_uid] UNIQUE NONCLUSTERED ([uid] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tab_member] ADD CONSTRAINT [tab_member$unq_member_username] UNIQUE NONCLUSTERED ([username] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_member
-- ----------------------------
ALTER TABLE [dbo].[tab_member] ADD CONSTRAINT [PK_tab_member_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_member_role
-- ----------------------------
CREATE NONCLUSTERED INDEX [FK_Reference_12]
ON [dbo].[tab_member_role] (
  [member_id] ASC
)
GO

CREATE NONCLUSTERED INDEX [FK_Reference_13]
ON [dbo].[tab_member_role] (
  [role_id] ASC
)
GO


-- ----------------------------
-- Uniques structure for table tab_member_role
-- ----------------------------
ALTER TABLE [dbo].[tab_member_role] ADD CONSTRAINT [tab_member_role$UNI_MEMBERID_ROLEID] UNIQUE NONCLUSTERED ([member_id] ASC, [role_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_member_role
-- ----------------------------
ALTER TABLE [dbo].[tab_member_role] ADD CONSTRAINT [PK_tab_member_role_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_message
-- ----------------------------
CREATE NONCLUSTERED INDEX [FK_Reference_19]
ON [dbo].[tab_message] (
  [dest_member_id] ASC
)
GO

CREATE NONCLUSTERED INDEX [FK_Reference_20]
ON [dbo].[tab_message] (
  [my_member_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_message
-- ----------------------------
ALTER TABLE [dbo].[tab_message] ADD CONSTRAINT [PK_tab_message_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_moz_engine
-- ----------------------------
ALTER TABLE [dbo].[tab_moz_engine] ADD CONSTRAINT [PK_tab_moz_engine_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_permission
-- ----------------------------
CREATE NONCLUSTERED INDEX [fk_permission_id_self_parent_id]
ON [dbo].[tab_permission] (
  [parent_id] ASC
)
GO


-- ----------------------------
-- Uniques structure for table tab_permission
-- ----------------------------
ALTER TABLE [dbo].[tab_permission] ADD CONSTRAINT [tab_permission$idx_permission_id_self_parent_id] UNIQUE NONCLUSTERED ([code] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_permission
-- ----------------------------
ALTER TABLE [dbo].[tab_permission] ADD CONSTRAINT [PK_tab_permission_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_profile
-- ----------------------------
CREATE NONCLUSTERED INDEX [FK_Reference_22]
ON [dbo].[tab_profile] (
  [header_bg_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_profile
-- ----------------------------
ALTER TABLE [dbo].[tab_profile] ADD CONSTRAINT [PK_tab_profile_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_resource
-- ----------------------------
ALTER TABLE [dbo].[tab_resource] ADD CONSTRAINT [PK_tab_resource_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_reward_points_history
-- ----------------------------
CREATE NONCLUSTERED INDEX [FK_Reference_18]
ON [dbo].[tab_reward_points_history] (
  [member_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table tab_reward_points_history
-- ----------------------------
ALTER TABLE [dbo].[tab_reward_points_history] ADD CONSTRAINT [PK_tab_reward_points_history_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Uniques structure for table tab_role
-- ----------------------------
ALTER TABLE [dbo].[tab_role] ADD CONSTRAINT [tab_role$idx_role_code] UNIQUE NONCLUSTERED ([code] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

ALTER TABLE [dbo].[tab_role] ADD CONSTRAINT [tab_role$idx_role_name] UNIQUE NONCLUSTERED ([name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_role
-- ----------------------------
ALTER TABLE [dbo].[tab_role] ADD CONSTRAINT [PK_tab_role_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_role_menu
-- ----------------------------
CREATE NONCLUSTERED INDEX [idx_rolemenu_menu_id]
ON [dbo].[tab_role_menu] (
  [menu_id] ASC
)
GO


-- ----------------------------
-- Uniques structure for table tab_role_menu
-- ----------------------------
ALTER TABLE [dbo].[tab_role_menu] ADD CONSTRAINT [tab_role_menu$idx_rolemenu_uniq_role_menu] UNIQUE NONCLUSTERED ([role_id] ASC, [menu_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_role_menu
-- ----------------------------
ALTER TABLE [dbo].[tab_role_menu] ADD CONSTRAINT [PK_tab_role_menu_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table tab_role_permission
-- ----------------------------
CREATE NONCLUSTERED INDEX [idx_permission_id]
ON [dbo].[tab_role_permission] (
  [role_id] ASC
)
GO

CREATE NONCLUSTERED INDEX [idx_role_id]
ON [dbo].[tab_role_permission] (
  [role_id] ASC
)
GO


-- ----------------------------
-- Uniques structure for table tab_role_permission
-- ----------------------------
ALTER TABLE [dbo].[tab_role_permission] ADD CONSTRAINT [tab_role_permission$idx_uniq_roleid_permissionid] UNIQUE NONCLUSTERED ([permission_id] ASC, [role_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_role_permission
-- ----------------------------
ALTER TABLE [dbo].[tab_role_permission] ADD CONSTRAINT [PK_tab_role_permission_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_schedule_task
-- ----------------------------
ALTER TABLE [dbo].[tab_schedule_task] ADD CONSTRAINT [PK_tab_schedule_task_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_service_performance
-- ----------------------------
ALTER TABLE [dbo].[tab_service_performance] ADD CONSTRAINT [PK_tab_service_performance_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Uniques structure for table tab_setting
-- ----------------------------
ALTER TABLE [dbo].[tab_setting] ADD CONSTRAINT [tab_setting$AK_Key_2] UNIQUE NONCLUSTERED ([name] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table tab_setting
-- ----------------------------
ALTER TABLE [dbo].[tab_setting] ADD CONSTRAINT [PK_tab_setting_id] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table tab_ad
-- ----------------------------
ALTER TABLE [dbo].[tab_ad] ADD CONSTRAINT [tab_ad$fk_ad_place_id] FOREIGN KEY ([ad_place_id]) REFERENCES [tab_ad_place] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_admin_menu
-- ----------------------------
ALTER TABLE [dbo].[tab_admin_menu] ADD CONSTRAINT [tab_admin_menu$fk_admin_menu_parent_id_self_id] FOREIGN KEY ([parent_id]) REFERENCES [tab_admin_menu] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table tab_article
-- ----------------------------
ALTER TABLE [dbo].[tab_article] ADD CONSTRAINT [tab_article$fk_aritle_type_id] FOREIGN KEY ([article_type_id]) REFERENCES [tab_article_model] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[tab_article] ADD CONSTRAINT [tab_article$fk_article_category_id] FOREIGN KEY ([category_id]) REFERENCES [tab_category] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_category
-- ----------------------------
ALTER TABLE [dbo].[tab_category] ADD CONSTRAINT [tab_category$category_fk_pid] FOREIGN KEY ([parent_id]) REFERENCES [tab_category] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table tab_external_authentication
-- ----------------------------
ALTER TABLE [dbo].[tab_external_authentication] ADD CONSTRAINT [tab_external_authentication$fk_ea_member_id] FOREIGN KEY ([member_id]) REFERENCES [tab_member] ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table tab_locale_string_resource
-- ----------------------------
ALTER TABLE [dbo].[tab_locale_string_resource] ADD CONSTRAINT [tab_locale_string_resource$FK_Reference_14] FOREIGN KEY ([language_id]) REFERENCES [tab_language] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_localized_property
-- ----------------------------
ALTER TABLE [dbo].[tab_localized_property] ADD CONSTRAINT [tab_localized_property$FK_Reference_15] FOREIGN KEY ([language_id]) REFERENCES [tab_language] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_member_role
-- ----------------------------
ALTER TABLE [dbo].[tab_member_role] ADD CONSTRAINT [tab_member_role$FK_Reference_12] FOREIGN KEY ([member_id]) REFERENCES [tab_member] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[tab_member_role] ADD CONSTRAINT [tab_member_role$FK_Reference_13] FOREIGN KEY ([role_id]) REFERENCES [tab_role] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_message
-- ----------------------------
ALTER TABLE [dbo].[tab_message] ADD CONSTRAINT [tab_message$FK_Reference_19] FOREIGN KEY ([dest_member_id]) REFERENCES [tab_member] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[tab_message] ADD CONSTRAINT [tab_message$FK_Reference_20] FOREIGN KEY ([my_member_id]) REFERENCES [tab_member] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table tab_permission
-- ----------------------------
ALTER TABLE [dbo].[tab_permission] ADD CONSTRAINT [tab_permission$fk_permission_id_self_parent_id] FOREIGN KEY ([parent_id]) REFERENCES [tab_permission] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table tab_profile
-- ----------------------------
ALTER TABLE [dbo].[tab_profile] ADD CONSTRAINT [tab_profile$FK_Reference_2] FOREIGN KEY ([id]) REFERENCES [tab_member] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[tab_profile] ADD CONSTRAINT [tab_profile$FK_Reference_22] FOREIGN KEY ([header_bg_id]) REFERENCES [tab_resource] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO


-- ----------------------------
-- Foreign Keys structure for table tab_reward_points_history
-- ----------------------------
ALTER TABLE [dbo].[tab_reward_points_history] ADD CONSTRAINT [tab_reward_points_history$FK_Reference_18] FOREIGN KEY ([member_id]) REFERENCES [tab_member] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_role_menu
-- ----------------------------
ALTER TABLE [dbo].[tab_role_menu] ADD CONSTRAINT [tab_role_menu$fk_rolemenu_menu_id] FOREIGN KEY ([menu_id]) REFERENCES [tab_admin_menu] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[tab_role_menu] ADD CONSTRAINT [tab_role_menu$fk_rolemenu_role_id] FOREIGN KEY ([role_id]) REFERENCES [tab_role] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO


-- ----------------------------
-- Foreign Keys structure for table tab_role_permission
-- ----------------------------
ALTER TABLE [dbo].[tab_role_permission] ADD CONSTRAINT [tab_role_permission$fk_permission_id] FOREIGN KEY ([permission_id]) REFERENCES [tab_permission] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[tab_role_permission] ADD CONSTRAINT [tab_role_permission$fk_role_id] FOREIGN KEY ([role_id]) REFERENCES [tab_role] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

