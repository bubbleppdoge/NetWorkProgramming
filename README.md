# NetWorkProgramming


实现内容：

1.增加音效相关内容，游戏时增加背景音乐，击中敌人时有击中音效

2.游戏改成主角在2分钟内需要从地图中A地点去到B地点，去到B地点则游戏结束。

3.在A到B的路径中有多个敌人按固定线路走动，当主角靠近到一定范围时会自动追踪主角。

4.服务端保存分数排行数据

5.实现简单登录界面（任意输入账号与密码，当已经有此账号时直接登录到该账号，如果没有此账号则直接创建一个账号并登录，行为如游客账号登录）

6.登录时自动同步服务端数据到本地显示并开始游戏



使用：

先在server文件夹下开启服务端脚本

然后执行unity客户端

登录账号（有账号检测是否密码正确，无账号直接登录并记录账号信息）

显示排行榜数据

开始后有背景音乐，攻击到敌人会有音效

结束后显示排行榜信息



ps:

因为地图限制，游戏内容改为收集四个方块后结束

分数 = 时间剩余 + 击中敌人（10） + 方块（100）

因为时间有点仓促，测试可能不太足够，但目前并未发现太大bug



服务端代码serverinit中地址更改：
23 10.32.12.103 -> 127.0.0.1



表的信息：
CREATE TABLE tbl_rankinglist
(
    rl_pName varchar(30) NOT NULL COMMENT '名字',
    rl_pScore integer NOT NULL COMMENT '分数'
)ENGINE=InnoDB default charset=utf8 comment='排行榜数据'


CREATE TABLE tbl_login
(
    rl_id varchar(30) NOT NULL COMMENT '账号',
    rl_password varchar(30) NOT NULL COMMENT '密码',
    PRIMARY KEY (rl_id)
)ENGINE=InnoDB default charset=utf8 comment='用户信息'


可用于测试排行榜的数据：
insert into tbl_rankinglist values("p1", "100");
insert into tbl_rankinglist values("p1", "200");
insert into tbl_rankinglist values("p1", "300");
insert into tbl_rankinglist values("p2", "120");
insert into tbl_rankinglist values("p3", "220");
