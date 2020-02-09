/*
 Navicat Premium Data Transfer

 Source Server         : 139.196.228.135
 Source Server Type    : MySQL
 Source Server Version : 80018
 Source Host           : 139.196.228.135:3306
 Source Schema         : moz

 Target Server Type    : MySQL
 Target Server Version : 80018
 File Encoding         : 65001

 Date: 09/02/2020 19:46:51
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for tab_ad
-- ----------------------------
DROP TABLE IF EXISTS `tab_ad`;
CREATE TABLE `tab_ad` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ad_place_id` bigint(20) NOT NULL,
  `title` varchar(300) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `image_path` varchar(300) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `target_url` varchar(400) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `order_index` int(11) NOT NULL,
  `is_show` bit(1) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `fk_ad_place_id` (`ad_place_id`) USING BTREE,
  CONSTRAINT `fk_ad_place_id` FOREIGN KEY (`ad_place_id`) REFERENCES `tab_ad_place` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_ad
-- ----------------------------
BEGIN;
INSERT INTO `tab_ad` VALUES (6, 2, '课程', '/upload/72ffc987f89f4611b72556963ec92aa1.jpg', '/course/index/3', 0, b'1');
COMMIT;

-- ----------------------------
-- Table structure for tab_ad_place
-- ----------------------------
DROP TABLE IF EXISTS `tab_ad_place`;
CREATE TABLE `tab_ad_place` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `title` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `code` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `desc` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `addtime` datetime NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `idx_code` (`code`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_ad_place
-- ----------------------------
BEGIN;
INSERT INTO `tab_ad_place` VALUES (2, '顶部', 'index.top', '宽高 750x250', '2019-07-01 09:43:19');
COMMIT;

-- ----------------------------
-- Table structure for tab_admin_menu
-- ----------------------------
DROP TABLE IF EXISTS `tab_admin_menu`;
CREATE TABLE `tab_admin_menu` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `link` varchar(500) NOT NULL,
  `order_index` int(11) NOT NULL,
  `icon` varchar(40) DEFAULT NULL,
  `is_system` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`) USING BTREE,
  KEY `parent_id_self_index` (`parent_id`) USING BTREE,
  CONSTRAINT `admin_menu_parent_id_self_fk` FOREIGN KEY (`parent_id`) REFERENCES `tab_admin_menu` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_admin_menu
-- ----------------------------
BEGIN;
INSERT INTO `tab_admin_menu` VALUES (1, '内容管理', NULL, '#', 0, 'layui-icon layui-icon-component', b'0');
INSERT INTO `tab_admin_menu` VALUES (11, '菜单2', 1, 'qscourse/index', 10, 'layui-icon layui-icon-component', b'0');
INSERT INTO `tab_admin_menu` VALUES (13, '菜单1', 1, 'menu/index', 0, 'layui-icon layui-icon-component', b'0');
COMMIT;

-- ----------------------------
-- Table structure for tab_article
-- ----------------------------
DROP TABLE IF EXISTS `tab_article`;
CREATE TABLE `tab_article` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `article_type_id` bigint(20) NOT NULL,
  `category_id` bigint(20) DEFAULT NULL,
  `title` varchar(1000) DEFAULT NULL,
  `sub_title` varchar(1000) DEFAULT NULL,
  `title_color` varchar(1000) DEFAULT NULL,
  `title_bold` bit(1) DEFAULT NULL,
  `summary` varchar(1000) DEFAULT NULL,
  `content` text,
  `tags` varchar(1000) DEFAULT NULL,
  `thumb_image` varchar(1000) DEFAULT NULL,
  `video` varchar(1000) DEFAULT NULL,
  `source` varchar(1000) DEFAULT NULL,
  `author` varchar(1000) DEFAULT NULL,
  `hits` int(11) DEFAULT NULL,
  `addtime` datetime DEFAULT NULL,
  `order_index` int(11) DEFAULT NULL,
  `is_top` bit(1) DEFAULT NULL,
  `is_recommend` bit(1) DEFAULT NULL,
  `seo_title` varchar(1000) DEFAULT NULL,
  `seo_keyword` varchar(1000) DEFAULT NULL,
  `seo_description` varchar(1000) DEFAULT NULL,
  `string1` varchar(1000) DEFAULT NULL,
  `string2` varchar(1000) DEFAULT NULL,
  `string3` varchar(1000) DEFAULT NULL,
  `string4` varchar(1000) DEFAULT NULL,
  `int1` int(11) DEFAULT NULL,
  `int2` int(11) DEFAULT NULL,
  `int3` int(11) DEFAULT NULL,
  `int4` int(11) DEFAULT NULL,
  `decimal1` decimal(18,2) DEFAULT NULL,
  `decimal2` decimal(18,2) DEFAULT NULL,
  `decimal3` decimal(18,2) DEFAULT NULL,
  `decimal4` decimal(18,2) DEFAULT NULL,
  `datetime1` datetime DEFAULT NULL,
  `datetime2` datetime DEFAULT NULL,
  `datetime3` datetime DEFAULT NULL,
  `datetime4` datetime DEFAULT NULL,
  `bool1` bit(1) DEFAULT NULL,
  `bool2` bit(1) DEFAULT NULL,
  `bool3` bit(1) DEFAULT NULL,
  `bool4` bit(1) DEFAULT NULL,
  `text1` text,
  `text2` text,
  `text3` text,
  `text4` text,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `article_type_id` (`article_type_id`) USING BTREE,
  KEY `fk_article_category_id` (`category_id`),
  CONSTRAINT `fk_aritle_type_id` FOREIGN KEY (`article_type_id`) REFERENCES `tab_article_model` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_article_category_id` FOREIGN KEY (`category_id`) REFERENCES `tab_category` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_article_model
-- ----------------------------
DROP TABLE IF EXISTS `tab_article_model`;
CREATE TABLE `tab_article_model` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `configuration` text NOT NULL,
  `category_id` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_article_model
-- ----------------------------
BEGIN;
INSERT INTO `tab_article_model` VALUES (10, '新闻中心', '[{\"FiledName\":\"Title\",\"DisplayName\":\"标题\",\"IsEnable\":true,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"SubTitle\",\"DisplayName\":\"副标题\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"TitleColor\",\"DisplayName\":\"颜色\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"TitleBold\",\"DisplayName\":\"标题粗体\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Summary\",\"DisplayName\":\"摘要\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Content\",\"DisplayName\":\"内容\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Tags\",\"DisplayName\":\"标签\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":100},{\"FiledName\":\"ThumbImage\",\"DisplayName\":\"缩略图\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Video\",\"DisplayName\":\"视频\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Source\",\"DisplayName\":\"来源\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Author\",\"DisplayName\":\"作者\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Hits\",\"DisplayName\":\"点击次数\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Addtime\",\"DisplayName\":\"添加时间\",\"IsEnable\":false,\"DisplayType\":8,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"OrderIndex\",\"DisplayName\":\"排序\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"IsTop\",\"DisplayName\":\"是否置顶\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"IsRecommend\",\"DisplayName\":\"是否推荐\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"SeoTitle\",\"DisplayName\":\"SeoTitle\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"SeoKeyword\",\"DisplayName\":\"SeoKeyword\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"SeoDescription\",\"DisplayName\":\"SeoDescription\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"String1\",\"DisplayName\":\"String1\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"String2\",\"DisplayName\":\"String2\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"String3\",\"DisplayName\":\"String3\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"String4\",\"DisplayName\":\"String4\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Int1\",\"DisplayName\":\"Int1\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Int2\",\"DisplayName\":\"Int2\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Int3\",\"DisplayName\":\"Int3\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Int4\",\"DisplayName\":\"Int4\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Long1\",\"DisplayName\":\"Long1\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":100},{\"FiledName\":\"Long2\",\"DisplayName\":\"Long2\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":100},{\"FiledName\":\"Long3\",\"DisplayName\":\"Long3\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":100},{\"FiledName\":\"Long4\",\"DisplayName\":\"Long4\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":100},{\"FiledName\":\"Decimal1\",\"DisplayName\":\"Decimal1\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Decimal2\",\"DisplayName\":\"Decimal2\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Decimal3\",\"DisplayName\":\"Decimal3\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Decimal4\",\"DisplayName\":\"Decimal4\",\"IsEnable\":false,\"DisplayType\":1,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Datetime1\",\"DisplayName\":\"DateTime1\",\"IsEnable\":false,\"DisplayType\":8,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Datetime2\",\"DisplayName\":\"DateTime2\",\"IsEnable\":false,\"DisplayType\":8,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Datetime3\",\"DisplayName\":\"DateTime3\",\"IsEnable\":false,\"DisplayType\":8,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Datetime4\",\"DisplayName\":\"DateTime4\",\"IsEnable\":false,\"DisplayType\":8,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Bool1\",\"DisplayName\":\"Bool1\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Bool2\",\"DisplayName\":\"Bool2\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Bool3\",\"DisplayName\":\"Bool3\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Bool4\",\"DisplayName\":\"Bool4\",\"IsEnable\":false,\"DisplayType\":16,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Text1\",\"DisplayName\":\"Text1\",\"IsEnable\":false,\"DisplayType\":64,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Text2\",\"DisplayName\":\"Text2\",\"IsEnable\":false,\"DisplayType\":64,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Text3\",\"DisplayName\":\"Text3\",\"IsEnable\":false,\"DisplayType\":64,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0},{\"FiledName\":\"Text4\",\"DisplayName\":\"Text4\",\"IsEnable\":false,\"DisplayType\":64,\"Options\":null,\"Description\":null,\"DefaultValue\":null,\"IsMultiLanguage\":false,\"IsShowedInList\":false,\"IsEnableSearch\":false,\"IsRequired\":false,\"OrderIndex\":0}]', NULL);
COMMIT;

-- ----------------------------
-- Table structure for tab_category
-- ----------------------------
DROP TABLE IF EXISTS `tab_category`;
CREATE TABLE `tab_category` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `alias` varchar(100) DEFAULT NULL,
  `description` varchar(2400) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `order_index` int(255) NOT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `path` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `category_fk_pid` (`parent_id`) USING BTREE,
  CONSTRAINT `category_fk_pid` FOREIGN KEY (`parent_id`) REFERENCES `tab_category` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_category
-- ----------------------------
BEGIN;
INSERT INTO `tab_category` VALUES (17, '产品分类', 'product', '1118', 0, NULL, '17');
INSERT INTO `tab_category` VALUES (56, '新闻分类', 'news', NULL, 0, NULL, '56');
COMMIT;

-- ----------------------------
-- Table structure for tab_external_authentication
-- ----------------------------
DROP TABLE IF EXISTS `tab_external_authentication`;
CREATE TABLE `tab_external_authentication` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `member_id` bigint(20) NOT NULL,
  `provider` tinyint(4) NOT NULL,
  `openid` varchar(120) NOT NULL,
  `access_token` varchar(200) DEFAULT NULL,
  `refresh_token` varchar(200) DEFAULT NULL,
  `expire_dt` datetime DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `idx_ea_member_id` (`provider`,`openid`) USING BTREE,
  KEY `idx_ea_unq_provider_openid` (`member_id`) USING BTREE,
  CONSTRAINT `fk_ea_member_id` FOREIGN KEY (`member_id`) REFERENCES `tab_member` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=936 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_external_authentication
-- ----------------------------
BEGIN;
INSERT INTO `tab_external_authentication` VALUES (935, 1593, 4, 'o6BAG1YUe5WZxJV7pu1MeqmwqWO0', NULL, NULL, '2020-03-08 14:24:26');
COMMIT;

-- ----------------------------
-- Table structure for tab_generic_attribute
-- ----------------------------
DROP TABLE IF EXISTS `tab_generic_attribute`;
CREATE TABLE `tab_generic_attribute` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `entity_id` bigint(20) NOT NULL,
  `key_group` varchar(200) NOT NULL,
  `key` varchar(500) NOT NULL,
  `value` longtext NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_identify
-- ----------------------------
DROP TABLE IF EXISTS `tab_identify`;
CREATE TABLE `tab_identify` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `rel` bit(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1454 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_language
-- ----------------------------
DROP TABLE IF EXISTS `tab_language`;
CREATE TABLE `tab_language` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) NOT NULL,
  `language_culture` varchar(20) NOT NULL,
  `published` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_locale_string_resource
-- ----------------------------
DROP TABLE IF EXISTS `tab_locale_string_resource`;
CREATE TABLE `tab_locale_string_resource` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `language_id` bigint(20) NOT NULL,
  `name` varchar(200) NOT NULL,
  `value` varchar(2000) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `FK_Reference_14` (`language_id`) USING BTREE,
  CONSTRAINT `FK_Reference_14` FOREIGN KEY (`language_id`) REFERENCES `tab_language` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_localized_property
-- ----------------------------
DROP TABLE IF EXISTS `tab_localized_property`;
CREATE TABLE `tab_localized_property` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `entity_id` bigint(20) NOT NULL,
  `language_id` bigint(20) NOT NULL,
  `locale_key_group` varchar(200) NOT NULL,
  `locale_key` varchar(200) NOT NULL,
  `locale_value` varchar(2000) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `FK_Reference_15` (`language_id`) USING BTREE,
  CONSTRAINT `FK_Reference_15` FOREIGN KEY (`language_id`) REFERENCES `tab_language` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_member
-- ----------------------------
DROP TABLE IF EXISTS `tab_member`;
CREATE TABLE `tab_member` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `uid` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `username` varchar(64) NOT NULL,
  `password` varchar(128) NOT NULL,
  `password_salt` char(8) NOT NULL,
  `email` varchar(100) DEFAULT NULL,
  `mobile` varchar(64) DEFAULT NULL,
  `avatar` varchar(250) DEFAULT NULL,
  `gender` int(11) DEFAULT NULL,
  `birthday` date DEFAULT NULL,
  `register_ip` varchar(39) DEFAULT NULL,
  `register_datetime` datetime NOT NULL,
  `login_count` int(11) NOT NULL,
  `last_login_ip` varchar(39) DEFAULT NULL,
  `last_login_datetime` datetime NOT NULL,
  `cannot_login_until_date` datetime DEFAULT NULL,
  `last_active_datetime` datetime NOT NULL,
  `failed_login_attempts` int(11) NOT NULL,
  `online_time_count` int(11) NOT NULL,
  `address` varchar(250) DEFAULT NULL,
  `region_code` varchar(100) DEFAULT NULL,
  `lng` decimal(30,18) DEFAULT NULL,
  `lat` decimal(30,18) DEFAULT NULL,
  `geohash` varchar(10) DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL,
  `is_delete` tinyint(1) NOT NULL,
  `is_email_valid` tinyint(1) NOT NULL,
  `is_mobile_valid` tinyint(1) NOT NULL,
  `nickname` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `unq_member_username` (`username`) USING BTREE,
  UNIQUE KEY `unq_member_uid` (`uid`) USING BTREE,
  UNIQUE KEY `unq_member_email` (`email`) USING BTREE,
  UNIQUE KEY `unq_member_mobile` (`mobile`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1595 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_member
-- ----------------------------
BEGIN;
INSERT INTO `tab_member` VALUES (1, '4fe33034296311ea86e500163e00111a', 'admin', '7D3465CD1DAD77A3BC211380CC40D13CAEC1ACB30E5437D83C86326682BA41BBC4E2D569B6A4E70549EDE76854452443E286043A375C1FBFAB2C47AEBEA8D9A4', '8H/d9FrN', 'zdh082@qq.com', NULL, 'https://wx.qlogo.cn/mmopen/vi_32/NSWdycs0F7TvIug7AhAn5TcZsjB7ribrX4yw2YTvMwuApoH5OCVFJ9pDicUbAaASqwmTufpSAFxicXBs9WRVQiaiaNA/132', 1, '1990-01-01', '192.168.3.114', '2018-01-09 09:32:21', 0, '192.168.3.114', '2018-01-09 09:32:21', NULL, '2018-01-09 09:32:21', 0, 0, '', '', 104.063615882866000000, 30.672543514548500000, 'wm6n2pbbd', 1, 0, 0, 0, NULL);
INSERT INTO `tab_member` VALUES (1552, '96991deb4fbe40959ba75ab38f5d040b', 'test', '554CDD69FFBA03B1C8E4E88D9CE2746AB503DD523E9A4A7F6025D089F7F5C6C99D32944C4E90F34BD7E2077AB4A080488FD2FBA6DEB6671EA562BAD171018567', 'MMbLVw4=', NULL, NULL, 'http://thirdwx.qlogo.cn/mmopen/vi_32/gdiajvBXJtMibRoqchJTVKTrkXSicPuKAp86oM5dv12YzLS0ZCTnFI19ibmfdVjHnGjM6AIU6W6TlQsoVFSKSOEXTQ/132', 0, NULL, NULL, '2020-01-09 14:31:53', 0, NULL, '2020-01-09 14:31:53', NULL, '2020-01-09 14:31:53', 0, 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, 0, 'the7');
INSERT INTO `tab_member` VALUES (1593, '34a7c9fbe6d14dea92b7a5c0d4f1af40', 'mp_1453', '942C8D0513010DD2712F264EFF2F8E255C6778110FC2AC08DF185C2141AA2A9C18194A042E585ACEA505E8A6418F152EC2BEB35A83162FB330AC38BC68BD55A7', 'ZlPFJPfn', NULL, NULL, NULL, 0, NULL, NULL, '2020-02-08 06:24:26', 0, NULL, '2020-02-08 06:24:26', NULL, '2020-02-08 06:24:26', 0, 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, 0, 'mp');
INSERT INTO `tab_member` VALUES (1594, '1f5653659466419eabd888dbc728f02a', 'fffff', 'F5DC2E97BB5F9A62808BC5036998802C5BBDBC93E97581B758CB22258DD35BEDE64BE0A7DC8025E6DE409ADC0136E1F8C617EAA562ABC0D4381B946DF184A701', 'LP5IbwkQ', NULL, NULL, NULL, 1, NULL, NULL, '2020-02-09 17:38:56', 0, NULL, '2020-02-09 17:38:56', NULL, '2020-02-09 17:38:56', 0, 0, NULL, NULL, NULL, NULL, NULL, 1, 0, 0, 0, NULL);
COMMIT;

-- ----------------------------
-- Table structure for tab_member_role
-- ----------------------------
DROP TABLE IF EXISTS `tab_member_role`;
CREATE TABLE `tab_member_role` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `member_id` bigint(20) NOT NULL,
  `role_id` bigint(20) NOT NULL,
  `expire_date` date DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `UNI_MEMBERID_ROLEID` (`member_id`,`role_id`) USING BTREE,
  KEY `FK_Reference_12` (`member_id`) USING BTREE,
  KEY `FK_Reference_13` (`role_id`) USING BTREE,
  CONSTRAINT `FK_Reference_12` FOREIGN KEY (`member_id`) REFERENCES `tab_member` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Reference_13` FOREIGN KEY (`role_id`) REFERENCES `tab_role` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_member_role
-- ----------------------------
BEGIN;
INSERT INTO `tab_member_role` VALUES (10, 1, 1, NULL);
INSERT INTO `tab_member_role` VALUES (11, 1, 2, NULL);
INSERT INTO `tab_member_role` VALUES (12, 1, 3, NULL);
INSERT INTO `tab_member_role` VALUES (17, 1552, 2, NULL);
INSERT INTO `tab_member_role` VALUES (18, 1593, 3, NULL);
INSERT INTO `tab_member_role` VALUES (19, 1594, 3, NULL);
COMMIT;

-- ----------------------------
-- Table structure for tab_message
-- ----------------------------
DROP TABLE IF EXISTS `tab_message`;
CREATE TABLE `tab_message` (
  `id` bigint(20) NOT NULL,
  `msg_type` int(11) NOT NULL,
  `content` varchar(2000) NOT NULL,
  `dest_member_id` bigint(20) NOT NULL,
  `my_member_id` bigint(20) NOT NULL,
  `add_time` datetime NOT NULL,
  `is_read` bit(1) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `FK_Reference_19` (`dest_member_id`) USING BTREE,
  KEY `FK_Reference_20` (`my_member_id`) USING BTREE,
  CONSTRAINT `FK_Reference_19` FOREIGN KEY (`dest_member_id`) REFERENCES `tab_member` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Reference_20` FOREIGN KEY (`my_member_id`) REFERENCES `tab_member` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_moz_engine
-- ----------------------------
DROP TABLE IF EXISTS `tab_moz_engine`;
CREATE TABLE `tab_moz_engine` (
  `id` bigint(20) NOT NULL,
  `version` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `code` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of tab_moz_engine
-- ----------------------------
BEGIN;
INSERT INTO `tab_moz_engine` VALUES (1, '1.0.3', 103);
COMMIT;

-- ----------------------------
-- Table structure for tab_permission
-- ----------------------------
DROP TABLE IF EXISTS `tab_permission`;
CREATE TABLE `tab_permission` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(20) NOT NULL,
  `code` varchar(100) NOT NULL,
  `is_active` bit(1) NOT NULL,
  `parent_id` bigint(20) DEFAULT NULL,
  `order_index` int(8) NOT NULL DEFAULT '0',
  `is_system` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `idx_permission_id_self_parent_id` (`code`) USING BTREE,
  KEY `fk_permission_id_self_parent_id` (`parent_id`),
  CONSTRAINT `fk_permission_id_self_parent_id` FOREIGN KEY (`parent_id`) REFERENCES `tab_permission` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_permission
-- ----------------------------
BEGIN;
INSERT INTO `tab_permission` VALUES (1, '后台管理', 'admin.access', b'1', NULL, 9, b'1');
INSERT INTO `tab_permission` VALUES (2, '文章管理', 'UPLOAD_IMAGE', b'1', 1, 50, b'1');
INSERT INTO `tab_permission` VALUES (4, '模型管理', 'admin.articlemodel.index', b'1', 2, 0, b'1');
INSERT INTO `tab_permission` VALUES (7, '角色管理', 'admin.role', b'1', 1, 0, b'1');
INSERT INTO `tab_permission` VALUES (8, '添加角色', 'admin.role.crate', b'1', 7, 20, b'1');
INSERT INTO `tab_permission` VALUES (9, '修改角色', 'admin.role.update', b'1', 7, 10, b'1');
INSERT INTO `tab_permission` VALUES (12, '删除角色', 'admin.role.delete', b'1', 7, 40, b'1');
INSERT INTO `tab_permission` VALUES (14, '开启/关闭角色', 'admin.role.setisactive', b'1', 7, 50, b'1');
INSERT INTO `tab_permission` VALUES (15, '设置成管理组', 'admin.role.setisadmin', b'1', 7, 60, b'1');
INSERT INTO `tab_permission` VALUES (16, '配置权限', 'admin.role.configpermission', b'1', 7, 70, b'1');
INSERT INTO `tab_permission` VALUES (17, '配置管理菜单', 'admin.role.configmenu', b'1', 7, 80, b'1');
INSERT INTO `tab_permission` VALUES (19, '前台会员中心', 'member.access', b'1', NULL, 0, b'1');
INSERT INTO `tab_permission` VALUES (20, '个人资料', 'member.profile', b'1', 19, 0, b'0');
INSERT INTO `tab_permission` VALUES (21, '修改资料', 'member.profile.update', b'1', 20, 80, b'0');
INSERT INTO `tab_permission` VALUES (22, '上传头像', 'member.profile.uploadavatar', b'1', 20, 30, b'0');
INSERT INTO `tab_permission` VALUES (23, '权限管理', 'admin.permission', b'1', 1, 10, b'1');
INSERT INTO `tab_permission` VALUES (24, '添加权限', 'admin.permission.create', b'1', 23, 20, b'1');
INSERT INTO `tab_permission` VALUES (25, '编辑权限', 'admin.permission.update', b'1', 23, 30, b'1');
INSERT INTO `tab_permission` VALUES (26, '删除权限', 'admin.permission.delete', b'1', 23, 40, b'1');
INSERT INTO `tab_permission` VALUES (27, '设置开启', 'admin.permission.isactive', b'1', 23, 50, b'1');
INSERT INTO `tab_permission` VALUES (28, '列表展示', 'admin.permission.index', b'1', 23, 10, b'1');
INSERT INTO `tab_permission` VALUES (29, '列表展示', 'admin.role.index', b'1', 7, 10, b'1');
INSERT INTO `tab_permission` VALUES (30, '菜单管理', 'admin.menu', b'1', 1, 201, b'1');
INSERT INTO `tab_permission` VALUES (31, '列表展示', 'admin.menu.index', b'1', 30, 10, b'1');
INSERT INTO `tab_permission` VALUES (32, '添加菜单', 'admin.menu.create', b'1', 30, 20, b'1');
INSERT INTO `tab_permission` VALUES (33, '修改菜单', 'admin.menu.update', b'1', 30, 30, b'1');
INSERT INTO `tab_permission` VALUES (34, '删除菜单', 'admin.menu.delete', b'1', 30, 40, b'1');
INSERT INTO `tab_permission` VALUES (35, '文章模型管理', 'admin.article.model', b'1', 1, 0, b'1');
INSERT INTO `tab_permission` VALUES (36, '列表展示', 'admin.article.model.index', b'1', 35, 10, b'1');
INSERT INTO `tab_permission` VALUES (37, '添加模型', 'admin.article.model.create', b'1', 35, 20, b'1');
INSERT INTO `tab_permission` VALUES (38, '编辑模型', 'admin.article.model.update', b'1', 35, 30, b'1');
INSERT INTO `tab_permission` VALUES (39, '删除模型', 'admin.article.model.delete', b'1', 35, 40, b'1');
INSERT INTO `tab_permission` VALUES (40, '定时任务', 'admin.scheduleTask', b'1', 1, 0, b'1');
INSERT INTO `tab_permission` VALUES (41, '列表', 'admin.scheduleTask.index', b'1', 40, 0, b'1');
INSERT INTO `tab_permission` VALUES (42, '添加', 'admin.scheduleTask.create', b'1', 40, 0, b'1');
INSERT INTO `tab_permission` VALUES (43, '编辑', 'admin.scheduleTask.update', b'1', 40, 0, b'1');
INSERT INTO `tab_permission` VALUES (44, '删除', 'admin.scheduleTask.delete', b'1', 40, 0, b'1');
INSERT INTO `tab_permission` VALUES (45, '开启/关闭', 'admin.scheduleTask.setisenable', b'1', 40, 0, b'1');
INSERT INTO `tab_permission` VALUES (46, '立即执行一次', 'admin.scheduleTask.execute', b'1', 40, 0, b'1');
INSERT INTO `tab_permission` VALUES (47, '分类管理', 'admin.category', b'1', 1, 0, b'1');
INSERT INTO `tab_permission` VALUES (48, '列表页', 'admin.category.index', b'1', 47, 0, b'1');
INSERT INTO `tab_permission` VALUES (49, '添加', 'admin.category.create', b'1', 47, 0, b'1');
INSERT INTO `tab_permission` VALUES (50, '修改', 'admin.category.update', b'1', 47, 0, b'1');
INSERT INTO `tab_permission` VALUES (51, '删除', 'admin.category.delete', b'1', 47, 0, b'1');
INSERT INTO `tab_permission` VALUES (52, '设置排序', 'admin.category.setOrderIndex', b'1', 47, 0, b'1');
INSERT INTO `tab_permission` VALUES (55, '修改密码', 'member.profile.editpassword', b'1', 20, 0, b'0');
COMMIT;

-- ----------------------------
-- Table structure for tab_profile
-- ----------------------------
DROP TABLE IF EXISTS `tab_profile`;
CREATE TABLE `tab_profile` (
  `id` bigint(20) NOT NULL,
  `header_bg_id` bigint(20) NOT NULL,
  `intro` varchar(0) DEFAULT NULL,
  `like_gender` tinyint(4) NOT NULL,
  `like_age` varchar(20) NOT NULL,
  `wechat` varchar(50) DEFAULT NULL,
  `qq` varchar(50) DEFAULT NULL,
  `twitter` varchar(50) DEFAULT NULL,
  `weight` int(11) NOT NULL,
  `height` int(11) NOT NULL,
  `income` tinyint(4) NOT NULL,
  `religion` tinyint(4) NOT NULL,
  `i_viewed` int(11) NOT NULL,
  `i_liked` int(11) NOT NULL,
  `i_gift_presented` int(11) NOT NULL,
  `viewed_me` int(11) NOT NULL,
  `liked_me` int(11) NOT NULL,
  `gift_prensented_to_me` int(11) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `FK_Reference_22` (`header_bg_id`) USING BTREE,
  CONSTRAINT `FK_Reference_2` FOREIGN KEY (`id`) REFERENCES `tab_member` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_Reference_22` FOREIGN KEY (`header_bg_id`) REFERENCES `tab_resource` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_resource
-- ----------------------------
DROP TABLE IF EXISTS `tab_resource`;
CREATE TABLE `tab_resource` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `server` varchar(200) NOT NULL,
  `relative_path` varchar(500) NOT NULL,
  `content_type` varchar(20) NOT NULL,
  `properies` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_resource
-- ----------------------------
BEGIN;
INSERT INTO `tab_resource` VALUES (1, 'tdating1.oss-cn-hongkong.aliyuncs.com', '/avatar/avatar_default.png', 'image/png', NULL);
INSERT INTO `tab_resource` VALUES (2, 'tdating1.oss-cn-hongkong.aliyuncs.com', '/top/timg%20%281%29.jpeg', 'image/jpeg', NULL);
COMMIT;

-- ----------------------------
-- Table structure for tab_reward_points_history
-- ----------------------------
DROP TABLE IF EXISTS `tab_reward_points_history`;
CREATE TABLE `tab_reward_points_history` (
  `id` bigint(20) NOT NULL,
  `member_id` bigint(20) NOT NULL,
  `points` int(11) NOT NULL,
  `message` varchar(120) NOT NULL,
  `points_balance` int(11) NOT NULL,
  `add_time` datetime NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  KEY `FK_Reference_18` (`member_id`) USING BTREE,
  CONSTRAINT `FK_Reference_18` FOREIGN KEY (`member_id`) REFERENCES `tab_member` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_role
-- ----------------------------
DROP TABLE IF EXISTS `tab_role`;
CREATE TABLE `tab_role` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `is_active` bit(1) NOT NULL,
  `code` varchar(50) NOT NULL,
  `is_admin` bit(1) NOT NULL DEFAULT b'0',
  `is_system` bit(1) NOT NULL DEFAULT b'0',
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `idx_role_name` (`name`) USING BTREE,
  UNIQUE KEY `idx_role_code` (`code`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_role
-- ----------------------------
BEGIN;
INSERT INTO `tab_role` VALUES (1, '超级管理员', b'1', 'Administrator', b'1', b'1');
INSERT INTO `tab_role` VALUES (2, '一般管理员', b'1', 'Manager', b'1', b'1');
INSERT INTO `tab_role` VALUES (3, '普通用户', b'1', 'User', b'0', b'1');
INSERT INTO `tab_role` VALUES (4, '投稿员', b'1', 'Publisher', b'0', b'0');
COMMIT;

-- ----------------------------
-- Table structure for tab_role_menu
-- ----------------------------
DROP TABLE IF EXISTS `tab_role_menu`;
CREATE TABLE `tab_role_menu` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `role_id` bigint(20) NOT NULL,
  `menu_id` bigint(20) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `idx_rolemenu_uniq_role_menu` (`role_id`,`menu_id`) USING BTREE,
  KEY `idx_rolemenu_menu_id` (`menu_id`) USING BTREE,
  CONSTRAINT `fk_rolemenu_menu_id` FOREIGN KEY (`menu_id`) REFERENCES `tab_admin_menu` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_rolemenu_role_id` FOREIGN KEY (`role_id`) REFERENCES `tab_role` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_role_menu
-- ----------------------------
BEGIN;
INSERT INTO `tab_role_menu` VALUES (1, 1, 1);
INSERT INTO `tab_role_menu` VALUES (4, 1, 11);
INSERT INTO `tab_role_menu` VALUES (12, 2, 1);
INSERT INTO `tab_role_menu` VALUES (13, 2, 13);
INSERT INTO `tab_role_menu` VALUES (15, 3, 1);
INSERT INTO `tab_role_menu` VALUES (16, 3, 11);
INSERT INTO `tab_role_menu` VALUES (17, 3, 13);
COMMIT;

-- ----------------------------
-- Table structure for tab_role_permission
-- ----------------------------
DROP TABLE IF EXISTS `tab_role_permission`;
CREATE TABLE `tab_role_permission` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `role_id` bigint(20) NOT NULL,
  `permission_id` bigint(20) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `idx_uniq_roleid_permissionid` (`permission_id`,`role_id`) USING BTREE,
  KEY `idx_permission_id` (`role_id`) USING BTREE,
  KEY `idx_role_id` (`role_id`) USING BTREE,
  CONSTRAINT `fk_permission_id` FOREIGN KEY (`permission_id`) REFERENCES `tab_permission` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_role_id` FOREIGN KEY (`role_id`) REFERENCES `tab_role` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=296 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_schedule_task
-- ----------------------------
DROP TABLE IF EXISTS `tab_schedule_task`;
CREATE TABLE `tab_schedule_task` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `status` int(11) NOT NULL,
  `status_desc` varchar(10000) DEFAULT NULL,
  `job_key` varchar(32) NOT NULL,
  `job_group` varchar(32) NOT NULL,
  `trigger_key` varchar(32) NOT NULL,
  `trigger_group` varchar(32) NOT NULL,
  `is_enable` bit(1) NOT NULL,
  `type` varchar(255) NOT NULL,
  `cron` varchar(40) DEFAULT NULL,
  `interval` int(11) DEFAULT NULL,
  `last_start_time` datetime DEFAULT NULL,
  `last_end_time` datetime DEFAULT NULL,
  `last_success_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_schedule_task
-- ----------------------------
BEGIN;
INSERT INTO `tab_schedule_task` VALUES (5, '测试任务', 1, '', '', '', '', '', b'0', 'Moz.TaskSchedule.Jobs.MozTestJob,Moz, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null', '0/8 * * * * ? ', NULL, '2020-02-05 19:50:24', '2020-02-05 19:50:24', '2020-02-05 19:50:24');
COMMIT;

-- ----------------------------
-- Table structure for tab_service_performance
-- ----------------------------
DROP TABLE IF EXISTS `tab_service_performance`;
CREATE TABLE `tab_service_performance` (
  `id` bigint(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(1000) NOT NULL,
  `elapsed_ms` int(11) NOT NULL DEFAULT '0',
  `http_request_id` varchar(45) DEFAULT NULL,
  `add_time` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5139 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for tab_setting
-- ----------------------------
DROP TABLE IF EXISTS `tab_setting`;
CREATE TABLE `tab_setting` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `name` varchar(120) NOT NULL,
  `value` varchar(21000) NOT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `AK_Key_2` (`name`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of tab_setting
-- ----------------------------
BEGIN;
INSERT INTO `tab_setting` VALUES (1, 'MemberSettings.HashedPasswordFormat', 'SHA512');
INSERT INTO `tab_setting` VALUES (2, 'CommonSettings.Uploader', '4');
INSERT INTO `tab_setting` VALUES (3, 'CommonSettings.AliyunOSSServer', 'tdating1.oss-cn-hongkong.aliyuncs.com');
INSERT INTO `tab_setting` VALUES (4, 'CommonSettings.AliyunOSSBucket', 'tdating1');
INSERT INTO `tab_setting` VALUES (5, 'CommonSettings.AliyunOSSEndpoint', 'oss-cn-hongkong.aliyuncs.com');
INSERT INTO `tab_setting` VALUES (6, 'CommonSettings.AliyunOSSAccessKeyId', 'LTAI9o4Zuap3y2Wn');
INSERT INTO `tab_setting` VALUES (7, 'CommonSettings.AliyunOSSAccessKeySecret', '9ER7IwNXTW5yoTuxLkkDlUDDVCM6iv');
COMMIT;

SET FOREIGN_KEY_CHECKS = 1;
