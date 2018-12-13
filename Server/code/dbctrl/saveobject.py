# -*- coding: utf-8 -*-

import marshal
import pubglobalmanager

from . import defines

class CSaveData(object):
    def __init__(self):
        self.m_Data = {}
        self.m_Loaded = True

    def Query(self, sAttr, default = 0):
        return self.m_Data.get(sAttr, default)

    def Add(self, sAttr, iVal):
        self.m_Data.setdefault(sAttr, 0)
        self.m_Data[sAttr] += iVal

    def Delete(self, sAttr):
        if sAttr in self.m_Data:
            del self.m_Data[sAttr]

    def GetKey(self):
        raise Exception("未实现Key")

    def GetCreateSql(self):
        sData = marshal.dumps(self.m_Data)
        sSql = "insert into tbl_global(rl_sName, rl_dmSaveTime, rl_sData) values('%s', now(), '%s')" % (self.GetKey(), sData)
        return sSql

    def GetUpdateSql(self):
        sData = marshal.dumps(self.m_Data)
        sSql = "update tbl_global set rl_sData='%s',rl_dmSaveTime=now() where rl_sName='%s'" % (sData, self.GetKey())
        return sSql

    def GetQuerySql(self):
        sSql = "select rl_sData from tbl_global where rl_sName ='%s'" % self.GetKey()
        return sSql

    def Init(self):
        if self.Load():
            return
        self.New()

    def Load(self):
        sSql = self.GetQuerySql()
        resultList = pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "Query", sSql)
        if resultList:
            sData = resultList[0][0]
            self.m_Data = marshal.loads(sData)
            return True
        return False

    def New(self):
        sSql = self.GetCreateSql()
        pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "ExecSql", sSql)

    def Save(self):
        sSql = self.GetUpdateSql()
        pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "ExecSql", sSql)

    def GetRListCreateSql(self, dData):
        sSql = "insert into tbl_rankinglist(rl_pName, rl_pScore) values('%s', '%s')" % (dData["name"], dData["score"])
        return sSql

    def GetRListQuerySql(self):
        sSql = "select rl_pName, rl_pScore from tbl_rankinglist order by rl_pScore desc limit 10"
        return sSql

    def GetLoginCreateSql(self, dData):
        sSql = "insert into tbl_login(rl_id, rl_password) values('%s', '%s')" % (dData["id"], dData["password"])
        return sSql

    def GetLoginQuerySql(self):
        sSql = "select rl_id, rl_password from tbl_login"
        return sSql
    
    def AddRankMsg(self, dData):
        sSql = self.GetRListCreateSql(dData)
        pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "ExecSql", sSql)
              
    def GetRankMsg(self):
        sSql = self.GetRListQuerySql()
        resultList = pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "Query", sSql)

        dataList = []
        for item in resultList:
            data = {}
            data["name"] = item[0]
            data["score"] = item[1]
            dataList.append(data)
        return {"data":dataList}

    def SetLoginMsg(self, dData):
        sSql = self.GetLoginCreateSql(dData)
        pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "ExecSql", sSql)

    def QueryLoginMsg(self, dData):
        sSql = self.GetLoginQuerySql()
        resultList = pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "Query", sSql)
        if resultList:
            for item in resultList:
                if dData["id"] == item[0]:
                    if dData["password"] == item[1]:
                        return {"action":"canLogin"}
                    else:
                        return {"action":"cannotLogin"}
        sSql = self.GetLoginCreateSql(dData)
        pubglobalmanager.CallManagerFunc(defines.DBCTRL_MANAGER_NAME, "ExecSql", sSql)
        return {"action":"newLogin"}

        
