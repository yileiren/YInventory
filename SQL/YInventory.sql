/* Create Tables */

-- 系统组织机构表，记录系统中的所有机构名称。
CREATE TABLE ORG_ORGANIZATION
(
	-- 组织机构id，使用自增列作为主键。
	ID INT NOT NULL IDENTITY (1, 1),
	NAME NVARCHAR(50) NOT NULL,
	-- 当前组织机构的父机构id，为NULL时标识该机构为顶级机构。
	PARENTID INT DEFAULT NULL,
	CREATETIME DATETIME DEFAULT GETDATE() NOT NULL,
	-- 标识该机构是否已经删除，系统中的机构不允许进行物理删除。
	ISDELETE NCHAR DEFAULT 'N' NOT NULL,
	-- 排序序号，控制显示顺序。
	[ORDER] INT,
	PRIMARY KEY (ID),
	--自连接，标记父id
	FOREIGN KEY (PARENTID) REFERENCES ORG_ORGANIZATION (ID)
);
GO

--创建用户表
CREATE TABLE ORG_USER
(
	-- 用户id，使用自增列作为主键。
	ID INT NOT NULL IDENTITY ,
	-- 用户登陆名称，使用唯一索引不允许重复。
	LOGNAME NVARCHAR(20) NOT NULL UNIQUE,
	-- 用户登陆密码，使用MD5加密算法加密存储。
	LOGPASSWORD NVARCHAR(40) NOT NULL,
	-- 用户姓名
	NAME NVARCHAR(20) NOT NULL,
	-- 用户所在组织机构的id，为空表示用户为顶级用户，不属于任何组织机构。
	ORGANIZATIONID INT,
	-- 用户是否被删除。和组织机构一样，用户不允许物理删除。
	ISDELETE NCHAR DEFAULT 'N',
	-- 排序序号，用来控制显示顺序。
	[ORDER] INT,
	PRIMARY KEY (ID),
	--用户表外键，关联组织机构id
	FOREIGN KEY (ORGANIZATIONID) REFERENCES ORG_ORGANIZATION (ID)
);
GO

--插入用户表基本数据。
INSERT INTO org_user (logName, logPassword, name) VALUES ('root', 'b9be11166d72e9e3ae7fd407165e4bd2', '超级用户');
INSERT INTO org_user (logName, logPassword, name) VALUES ('admin', 'c3284d0f94606de1fd2af172aba15bf3', '超级管理员');
GO


--创建系统菜单表。
CREATE TABLE SYS_MENUS
(
	-- 菜单id，使用自增列做主键。
	ID INT NOT NULL IDENTITY ,
	-- 菜单名称，记录菜单显示的名称。
	NAME NVARCHAR(20) NOT NULL,
	-- 菜单地址，根据实际情况设置相对路径和绝对路径。
	URL NVARCHAR(200),
	-- 父菜单ID，为NULL表示顶级菜单。
	PARENTID INT,
	-- 菜单前面显示的图标，使用JQuery EasyUI中的图标字符串类型。
	ICON NVARCHAR(20),
	-- 桌面图标，记录图片相对路径。
	DESKTOPICON NVARCHAR(100),
	-- 用来控制菜单的显示顺序。
	[ORDER] INT DEFAULT 0 NOT NULL,
	PRIMARY KEY (ID),
	--自连接，标记父菜单。
	FOREIGN KEY (PARENTID) REFERENCES SYS_MENUS (ID)
);
GO

--插入系统菜单表基础数据。
INSERT INTO sys_menus (name,icon) VALUES ('系统设置','icon-systemSetting')
INSERT INTO sys_menus (name,icon) VALUES ('系统管理','icon-system');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('系统菜单', 'sys/menu/menu_list.aspx', 1, 'icon-systemMenu');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('角色管理', 'sys/role/role_list.aspx', 2,'icon-role');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('组织机构管理', 'sys/organization/organization_list.aspx', 2, 'icon-organization');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('数据字典', 'sys/dataDictionary/dataDictionary_list.aspx', 1, 'icon-dictionary');
INSERT INTO sys_menus (name, icon) VALUES ('库存管理', 'icon-inventory');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('仓库管理', 'inventory/warehouse/warehouse_list.aspx', 7, 'icon-warehouse');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('客户和供应商管理', 'inventory/supplierAndClient/supplierAndClient_list.aspx', 7, 'icon-supplier');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('货物管理', 'inventory/goods/goods_list.aspx', 7, 'icon-goods');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('入库单', 'inventory/putInStorage/putInStorage_list.aspx', 7, 'icon-putInStorage');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('出库单', 'inventory/pushOutStorage/pushOutStorage_list.aspx', 7, 'icon-pushOutStorage');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('商品零售', 'inventory/retailStorage/retailStorage_list.aspx', 7, 'icon-retail');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('退货供应商', 'inventory/retrnOfGoods/retrnOfGoods_list.aspx', 7, 'icon-retrnOfGoods');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('退库单', 'inventory/retrnToStorage/retrnToStorage_list.aspx', 7, 'icon-retrnToStorage');
--INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('零售退货', 'inventory/retailReturn/retailReturn_list.aspx', 7, 'icon-retailReturn');
GO

--创建角色表。
CREATE TABLE AUT_ROLE
(
	ID INT NOT NULL IDENTITY ,
	NAME NVARCHAR(20) NOT NULL,
	EXPLAIN NVARCHAR(50),
	PRIMARY KEY (ID)
);
GO

--创建角色菜单关联表。
CREATE TABLE AUT_ROLE_MENU
(
	ROLEID INT NOT NULL,
	-- 菜单id，使用自增列做主键。
	MENUID INT NOT NULL,
	PRIMARY KEY (ROLEID, MENUID),
	--创建角色表外键，关联角色id
	FOREIGN KEY (ROLEID) REFERENCES AUT_ROLE (ID),
	--创建菜单表外键，关联菜单id
	FOREIGN KEY (MENUID) REFERENCES SYS_MENUS (ID)
);
GO

--创建用户角色关联表。
CREATE TABLE AUT_USER_ROLE
(
	-- 用户id，使用自增列作为主键。
	USERID INT NOT NULL,
	ROLEID INT NOT NULL,
	PRIMARY KEY (USERID, ROLEID),
	--创建用户表外键，关联用户id
	FOREIGN KEY (USERID) REFERENCES ORG_USER (ID),
	--创建角色表外键，关联角色id
	FOREIGN KEY (ROLEID) REFERENCES AUT_ROLE (ID)
);
GO

--系统数据字典表
CREATE TABLE SYS_DATADICTIONARY
(
	--字典项id
	ID INT NOT NULL IDENTITY ,
	--父id
	PARENTID INT,
	--字典项名称
	NAME NVARCHAR(50) NOT NULL,
	--字典项对应的数值
	VALUE INT NOT NULL,
	--字典项对应的代码
	CODE NVARCHAR(50) NOT NULL,
	--字典项排序序号
	[ORDER] INT,
	PRIMARY KEY (ID),
	--自连接，标记父字典项
	FOREIGN KEY (PARENTID) REFERENCES SYS_DATADICTIONARY (ID)
);
GO

-- 数据字典表初始数据
INSERT INTO sys_dataDictionary (name, value, code) VALUES ('颜色', '0', '颜色');
INSERT INTO sys_dataDictionary (name, value, code) VALUES ('尺码', '0', '');
GO

-- 库存系统仓库表，存储仓库信息。
CREATE TABLE INV_WAREHOUSE
(
	-- 仓库id，使用自增列作为主键。
	ID INT NOT NULL IDENTITY ,
	-- 父仓库id，为空表示顶级仓库。
	PARENTID INT,
	NAME NVARCHAR(50) NOT NULL,
	EXPLAIN NVARCHAR(200),
	[ORDER] INT DEFAULT 0 NOT NULL,
	PRIMARY KEY (ID),
	--自连接，关联父id
	FOREIGN KEY (PARENTID) REFERENCES INV_WAREHOUSE (ID)
);
GO

-- 仓库用户关联表。
CREATE TABLE INV_WAREHOUSE_USER
(
	-- 仓库id，使用自增列作为主键。
	WAREHOUSEID INT NOT NULL,
	-- 用户id，使用自增列作为主键。
	USERID INT NOT NULL,
	PRIMARY KEY (WAREHOUSEID, USERID),
	--增加仓库表外键，关联仓库id
	FOREIGN KEY (WAREHOUSEID) REFERENCES INV_WAREHOUSE (ID),
	--增加用户表外键，关联用户id
	FOREIGN KEY (USERID) REFERENCES ORG_USER (ID)
);
GO


-- 供应商可客户信息表。
CREATE TABLE INV_SUPPLIERANDCLIENT
(
	ID INT NOT NULL IDENTITY ,
	NAME NVARCHAR(50) NOT NULL,
	-- 供应商或客户编号。
	NUMBER NVARCHAR(30),
	PRIMARY KEY (ID)
);
GO

-- 库存单据主表。
CREATE TABLE INV_INVENTORYMASTER
(
	ID INT NOT NULL IDENTITY ,
	NUMBER NVARCHAR(50) NOT NULL,
	-- 关联到供货商和客户表。
	SUPPLIERORCLIENTID INT NOT NULL,
	-- 记录单据所属仓库。
	WAREHOUSEID INT NOT NULL,
	-- 单据类型。
	-- 1是入库单。
	-- 2是出库单。
	-- 3是退货单，把单子推给供货商。
	-- 4是退库单，把客户的单子退到仓库。
	TYPE INT NOT NULL,
	-- 单据状态。
	-- 1是创建，创建的单据保存单不启用。
	-- 2是执行，执行后开始改变库存。
	-- 3是作废。
	STATE INT NOT NULL,
	CREATETIME DATETIME DEFAULT GETDATE() NOT NULL,
	-- 创建表单的用户。
	CREATEUSER INT NOT NULL,
	-- 单据的执行时间，执行时记录，为null表示未执行。
	EXECUTETIME DATETIME,
	-- 执行单据的用户，为null表示未执行。
	EXECUTEUSER INT,
	PRIMARY KEY (ID),
	--设置供应商或客户表外键，关联id
	FOREIGN KEY (SUPPLIERORCLIENTID) REFERENCES INV_SUPPLIERANDCLIENT (ID),
	--设置仓库表外键，关联仓库id
	FOREIGN KEY (WAREHOUSEID) REFERENCES INV_WAREHOUSE (ID),
	--设置创建人外键，关联用户id
	FOREIGN KEY (CREATEUSER) REFERENCES ORG_USER (ID),
	--设置执行人外键，关联用户id
	FOREIGN KEY (EXECUTEUSER) REFERENCES ORG_USER (ID)
);
GO

-- 货物定义表。
CREATE TABLE INV_GOODS
(
	ID INT NOT NULL IDENTITY ,
	-- 货物名称
	NAME NVARCHAR(50) NOT NULL,
	-- 货物编号。
	NUMBER NVARCHAR(50) NOT NULL,
	PRIMARY KEY (ID)
);
GO

-- 库存明细表。
CREATE TABLE INV_INVENTORYDETAIL
(
	-- 明细id
	ID INT NOT NULL IDENTITY ,
	-- 记录与该明细关联的明细id。
	DETAILID INT,
	-- 单据主表id
	MASTERID INT NOT NULL,
	-- 货物id
	GOODSID INT NOT NULL,
	-- 颜色id。
	COLORID INT NOT NULL,
	-- 尺码id
	SIZEID INT NOT NULL,
	-- 货品数量。
	COUNT INT DEFAULT 1 NOT NULL,
	-- 货物单价。
	UNITPRICE DECIMAL(18,4) DEFAULT 0 NOT NULL,
	--设置自关联，关联明细id
	FOREIGN KEY (DETAILID) REFERENCES INV_INVENTORYDETAIL (ID),
	--设置主表外键，关联主表id
	FOREIGN KEY (MASTERID) REFERENCES INV_INVENTORYMASTER (ID),
	PRIMARY KEY (ID),
	--设置货物外键，关联货物表id
	FOREIGN KEY (GOODSID) REFERENCES INV_GOODS (ID),
	--设置颜色外键，关联字典表id
	FOREIGN KEY (COLORID) REFERENCES SYS_DATADICTIONARY (ID),
	--设置尺码外键，关联字典表id
	FOREIGN KEY (SIZEID) REFERENCES SYS_DATADICTIONARY (ID)
);
GO

-- 库存数量表，记录每个入库单的剩余库存。
CREATE TABLE INV_INVENTORYCOUNT
(
	ID INT NOT NULL IDENTITY ,
	-- 入库明细id。
	DETAILID INT NOT NULL,
	-- 库存的剩余数量。
	COUNT INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (DETAILID) REFERENCES INV_INVENTORYDETAIL (ID)
);
GO