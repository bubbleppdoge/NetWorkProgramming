# -*- coding: utf-8  -*-
DBCTRL_MANAGER_NAME = "dbctrl"
DATABASE_NAME = "db_demo"


TABLE_GLOBAL = """
CREATE TABLE tbl_global
(
    rl_sName varchar(30) NOT NULL COMMENT '名字',
    rl_dmSaveTime datetime NOT NULL COMMENT '存档时间',
    rl_sData MEDIUMBLOB NOT NULL COMMENT '数据块',
    PRIMARY KEY (rl_sName)
)ENGINE=InnoDB default charset=utf8 comment='全局数据'
"""

TABLE_RANKINGLIST = """
CREATE TABLE tbl_rankinglist
(
    rl_pName varchar(30) NOT NULL COMMENT '名字',
    rl_pScore varchar(30) NOT NULL COMMENT '分数'
)ENGINE=InnoDB default charset=utf8 comment='排行榜数据'
"""

TABLE_LOGIN = """
CREATE TABLE tbl_login
(
    rl_id varchar(30) NOT NULL COMMENT '账号',
    rl_password varchar(30) NOT NULL COMMENT '密码',
    PRIMARY KEY (rl_id)
)ENGINE=InnoDB default charset=utf8 comment='用户信息'
"""

TABLE_ALL = {
    "tbl_global":TABLE_GLOBAL,
    "tbl_rankinglist":TABLE_RANKINGLIST,
    "tbl_login":TABLE_LOGIN
}
