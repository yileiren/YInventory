/* Create Tables */

-- ϵͳ��֯��������¼ϵͳ�е����л������ơ�
CREATE TABLE ORG_ORGANIZATION
(
	-- ��֯����id��ʹ����������Ϊ������
	ID INT NOT NULL IDENTITY (1, 1),
	NAME NVARCHAR(50) NOT NULL,
	-- ��ǰ��֯�����ĸ�����id��ΪNULLʱ��ʶ�û���Ϊ����������
	PARENTID INT DEFAULT NULL,
	CREATETIME DATETIME DEFAULT GETDATE() NOT NULL,
	-- ��ʶ�û����Ƿ��Ѿ�ɾ����ϵͳ�еĻ����������������ɾ����
	ISDELETE NCHAR DEFAULT 'N' NOT NULL,
	-- ������ţ�������ʾ˳��
	[ORDER] INT,
	PRIMARY KEY (ID),
	--�����ӣ���Ǹ�id
	FOREIGN KEY (PARENTID) REFERENCES ORG_ORGANIZATION (ID)
);
GO

--�����û���
CREATE TABLE ORG_USER
(
	-- �û�id��ʹ����������Ϊ������
	ID INT NOT NULL IDENTITY ,
	-- �û���½���ƣ�ʹ��Ψһ�����������ظ���
	LOGNAME NVARCHAR(20) NOT NULL UNIQUE,
	-- �û���½���룬ʹ��MD5�����㷨���ܴ洢��
	LOGPASSWORD NVARCHAR(40) NOT NULL,
	-- �û�����
	NAME NVARCHAR(20) NOT NULL,
	-- �û�������֯������id��Ϊ�ձ�ʾ�û�Ϊ�����û����������κ���֯������
	ORGANIZATIONID INT,
	-- �û��Ƿ�ɾ��������֯����һ�����û�����������ɾ����
	ISDELETE NCHAR DEFAULT 'N',
	-- ������ţ�����������ʾ˳��
	[ORDER] INT,
	PRIMARY KEY (ID),
	--�û��������������֯����id
	FOREIGN KEY (ORGANIZATIONID) REFERENCES ORG_ORGANIZATION (ID)
);
GO

--�����û���������ݡ�
INSERT INTO org_user (logName, logPassword, name) VALUES ('root', 'b9be11166d72e9e3ae7fd407165e4bd2', '�����û�');
INSERT INTO org_user (logName, logPassword, name) VALUES ('admin', 'c3284d0f94606de1fd2af172aba15bf3', '��������Ա');
GO


--����ϵͳ�˵���
CREATE TABLE SYS_MENUS
(
	-- �˵�id��ʹ����������������
	ID INT NOT NULL IDENTITY ,
	-- �˵����ƣ���¼�˵���ʾ�����ơ�
	NAME NVARCHAR(20) NOT NULL,
	-- �˵���ַ������ʵ������������·���;���·����
	URL NVARCHAR(200),
	-- ���˵�ID��ΪNULL��ʾ�����˵���
	PARENTID INT,
	-- �˵�ǰ����ʾ��ͼ�꣬ʹ��JQuery EasyUI�е�ͼ���ַ������͡�
	ICON NVARCHAR(20),
	-- ����ͼ�꣬��¼ͼƬ���·����
	DESKTOPICON NVARCHAR(100),
	-- �������Ʋ˵�����ʾ˳��
	[ORDER] INT DEFAULT 0 NOT NULL,
	PRIMARY KEY (ID),
	--�����ӣ���Ǹ��˵���
	FOREIGN KEY (PARENTID) REFERENCES SYS_MENUS (ID)
);
GO

--����ϵͳ�˵���������ݡ�
INSERT INTO sys_menus (name,icon) VALUES ('ϵͳ����','icon-systemSetting')
INSERT INTO sys_menus (name,icon) VALUES ('ϵͳ����','icon-system');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('ϵͳ�˵�', 'sys/menu/menu_list.aspx', 1, 'icon-systemMenu');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('��ɫ����', 'sys/role/role_list.aspx', 2,'icon-role');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('��֯��������', 'sys/organization/organization_list.aspx', 2, 'icon-organization');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�����ֵ�', 'sys/dataDictionary/dataDictionary_list.aspx', 1, 'icon-dictionary');
INSERT INTO sys_menus (name, icon) VALUES ('������', 'icon-inventory');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�ֿ����', 'inventory/warehouse/warehouse_list.aspx', 7, 'icon-warehouse');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�ͻ��͹�Ӧ�̹���', 'inventory/supplierAndClient/supplierAndClient_list.aspx', 7, 'icon-supplier');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�������', 'inventory/goods/goods_list.aspx', 7, 'icon-goods');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('��ⵥ', 'inventory/putInStorage/putInStorage_list.aspx', 7, 'icon-putInStorage');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('���ⵥ', 'inventory/pushOutStorage/pushOutStorage_list.aspx', 7, 'icon-pushOutStorage');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('��Ʒ����', 'inventory/retailStorage/retailStorage_list.aspx', 7, 'icon-retail');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�˻���Ӧ��', 'inventory/retrnOfGoods/retrnOfGoods_list.aspx', 7, 'icon-retrnOfGoods');
INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�˿ⵥ', 'inventory/retrnToStorage/retrnToStorage_list.aspx', 7, 'icon-retrnToStorage');
--INSERT INTO sys_menus (name, url, parentID, icon) VALUES ('�����˻�', 'inventory/retailReturn/retailReturn_list.aspx', 7, 'icon-retailReturn');
GO

--������ɫ��
CREATE TABLE AUT_ROLE
(
	ID INT NOT NULL IDENTITY ,
	NAME NVARCHAR(20) NOT NULL,
	EXPLAIN NVARCHAR(50),
	PRIMARY KEY (ID)
);
GO

--������ɫ�˵�������
CREATE TABLE AUT_ROLE_MENU
(
	ROLEID INT NOT NULL,
	-- �˵�id��ʹ����������������
	MENUID INT NOT NULL,
	PRIMARY KEY (ROLEID, MENUID),
	--������ɫ�������������ɫid
	FOREIGN KEY (ROLEID) REFERENCES AUT_ROLE (ID),
	--�����˵�������������˵�id
	FOREIGN KEY (MENUID) REFERENCES SYS_MENUS (ID)
);
GO

--�����û���ɫ������
CREATE TABLE AUT_USER_ROLE
(
	-- �û�id��ʹ����������Ϊ������
	USERID INT NOT NULL,
	ROLEID INT NOT NULL,
	PRIMARY KEY (USERID, ROLEID),
	--�����û�������������û�id
	FOREIGN KEY (USERID) REFERENCES ORG_USER (ID),
	--������ɫ�������������ɫid
	FOREIGN KEY (ROLEID) REFERENCES AUT_ROLE (ID)
);
GO

--ϵͳ�����ֵ��
CREATE TABLE SYS_DATADICTIONARY
(
	--�ֵ���id
	ID INT NOT NULL IDENTITY ,
	--��id
	PARENTID INT,
	--�ֵ�������
	NAME NVARCHAR(50) NOT NULL,
	--�ֵ����Ӧ����ֵ
	VALUE INT NOT NULL,
	--�ֵ����Ӧ�Ĵ���
	CODE NVARCHAR(50) NOT NULL,
	--�ֵ����������
	[ORDER] INT,
	PRIMARY KEY (ID),
	--�����ӣ���Ǹ��ֵ���
	FOREIGN KEY (PARENTID) REFERENCES SYS_DATADICTIONARY (ID)
);
GO

-- �����ֵ���ʼ����
INSERT INTO sys_dataDictionary (name, value, code) VALUES ('��ɫ', '0', '��ɫ');
INSERT INTO sys_dataDictionary (name, value, code) VALUES ('����', '0', '');
GO

-- ���ϵͳ�ֿ���洢�ֿ���Ϣ��
CREATE TABLE INV_WAREHOUSE
(
	-- �ֿ�id��ʹ����������Ϊ������
	ID INT NOT NULL IDENTITY ,
	-- ���ֿ�id��Ϊ�ձ�ʾ�����ֿ⡣
	PARENTID INT,
	NAME NVARCHAR(50) NOT NULL,
	EXPLAIN NVARCHAR(200),
	[ORDER] INT DEFAULT 0 NOT NULL,
	PRIMARY KEY (ID),
	--�����ӣ�������id
	FOREIGN KEY (PARENTID) REFERENCES INV_WAREHOUSE (ID)
);
GO

-- �ֿ��û�������
CREATE TABLE INV_WAREHOUSE_USER
(
	-- �ֿ�id��ʹ����������Ϊ������
	WAREHOUSEID INT NOT NULL,
	-- �û�id��ʹ����������Ϊ������
	USERID INT NOT NULL,
	PRIMARY KEY (WAREHOUSEID, USERID),
	--���Ӳֿ������������ֿ�id
	FOREIGN KEY (WAREHOUSEID) REFERENCES INV_WAREHOUSE (ID),
	--�����û�������������û�id
	FOREIGN KEY (USERID) REFERENCES ORG_USER (ID)
);
GO


-- ��Ӧ�̿ɿͻ���Ϣ��
CREATE TABLE INV_SUPPLIERANDCLIENT
(
	ID INT NOT NULL IDENTITY ,
	NAME NVARCHAR(50) NOT NULL,
	-- ��Ӧ�̻�ͻ���š�
	NUMBER NVARCHAR(30),
	PRIMARY KEY (ID)
);
GO

-- ��浥������
CREATE TABLE INV_INVENTORYMASTER
(
	ID INT NOT NULL IDENTITY ,
	NUMBER NVARCHAR(50) NOT NULL,
	-- �����������̺Ϳͻ���
	SUPPLIERORCLIENTID INT NOT NULL,
	-- ��¼���������ֿ⡣
	WAREHOUSEID INT NOT NULL,
	-- �������͡�
	-- 1����ⵥ��
	-- 2�ǳ��ⵥ��
	-- 3���˻������ѵ����Ƹ������̡�
	-- 4���˿ⵥ���ѿͻ��ĵ����˵��ֿ⡣
	TYPE INT NOT NULL,
	-- ����״̬��
	-- 1�Ǵ����������ĵ��ݱ��浥�����á�
	-- 2��ִ�У�ִ�к�ʼ�ı��档
	-- 3�����ϡ�
	STATE INT NOT NULL,
	CREATETIME DATETIME DEFAULT GETDATE() NOT NULL,
	-- ���������û���
	CREATEUSER INT NOT NULL,
	-- ���ݵ�ִ��ʱ�䣬ִ��ʱ��¼��Ϊnull��ʾδִ�С�
	EXECUTETIME DATETIME,
	-- ִ�е��ݵ��û���Ϊnull��ʾδִ�С�
	EXECUTEUSER INT,
	PRIMARY KEY (ID),
	--���ù�Ӧ�̻�ͻ������������id
	FOREIGN KEY (SUPPLIERORCLIENTID) REFERENCES INV_SUPPLIERANDCLIENT (ID),
	--���òֿ������������ֿ�id
	FOREIGN KEY (WAREHOUSEID) REFERENCES INV_WAREHOUSE (ID),
	--���ô���������������û�id
	FOREIGN KEY (CREATEUSER) REFERENCES ORG_USER (ID),
	--����ִ��������������û�id
	FOREIGN KEY (EXECUTEUSER) REFERENCES ORG_USER (ID)
);
GO

-- ���ﶨ���
CREATE TABLE INV_GOODS
(
	ID INT NOT NULL IDENTITY ,
	-- ��������
	NAME NVARCHAR(50) NOT NULL,
	-- �����š�
	NUMBER NVARCHAR(50) NOT NULL,
	PRIMARY KEY (ID)
);
GO

-- �����ϸ��
CREATE TABLE INV_INVENTORYDETAIL
(
	-- ��ϸid
	ID INT NOT NULL IDENTITY ,
	-- ��¼�����ϸ��������ϸid��
	DETAILID INT,
	-- ��������id
	MASTERID INT NOT NULL,
	-- ����id
	GOODSID INT NOT NULL,
	-- ��ɫid��
	COLORID INT NOT NULL,
	-- ����id
	SIZEID INT NOT NULL,
	-- ��Ʒ������
	COUNT INT DEFAULT 1 NOT NULL,
	-- ���ﵥ�ۡ�
	UNITPRICE DECIMAL(18,4) DEFAULT 0 NOT NULL,
	--�����Թ�����������ϸid
	FOREIGN KEY (DETAILID) REFERENCES INV_INVENTORYDETAIL (ID),
	--���������������������id
	FOREIGN KEY (MASTERID) REFERENCES INV_INVENTORYMASTER (ID),
	PRIMARY KEY (ID),
	--���û�����������������id
	FOREIGN KEY (GOODSID) REFERENCES INV_GOODS (ID),
	--������ɫ����������ֵ��id
	FOREIGN KEY (COLORID) REFERENCES SYS_DATADICTIONARY (ID),
	--���ó�������������ֵ��id
	FOREIGN KEY (SIZEID) REFERENCES SYS_DATADICTIONARY (ID)
);
GO

-- �����������¼ÿ����ⵥ��ʣ���档
CREATE TABLE INV_INVENTORYCOUNT
(
	ID INT NOT NULL IDENTITY ,
	-- �����ϸid��
	DETAILID INT NOT NULL,
	-- ����ʣ��������
	COUNT INT NOT NULL,
	PRIMARY KEY (ID),
	FOREIGN KEY (DETAILID) REFERENCES INV_INVENTORYDETAIL (ID)
);
GO