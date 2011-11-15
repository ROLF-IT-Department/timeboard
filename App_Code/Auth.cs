using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Класс аутентификации и авторизации пользователей
/// </summary>
public class Auth
{
    private SAPConnection con;         // структура для соединения с SAP
    private sap.SAPProxy sap_proxy;      // коннектор SAP
    private sap.BAPIRET2Table ErrorMessage;
    private sap.ZFEATURE_STRTable RoleTable;

    // настриваем соединение с SAP
    public Auth() 
    {
        this.con = new SAPConnection();
        this.sap_proxy = new sap.SAPProxy(con.SAPDestination.ConnectionString);
        this.ErrorMessage = null;
        this.RoleTable = null;
    }

    // авторизация пользователя windows и forms
    public string Authentication(string netname, string login, string password)
    {
        //netname = "ASODNOBLYUDOVA";
        this.ErrorMessage = new sap.BAPIRET2Table();
        string tab_num = null;

        this.sap_proxy.Zitc_Rfc_Get_Pernr(netname, login, password, out tab_num, ref this.ErrorMessage);

       // if (ErrorMessage.Count != 0)
        //    return null;
        //if (ErrorMessage.Count != 0)
        //    for (int i = 0; i < ErrorMessage.Count; i++)
        //        tab_num += ErrorMessage[i].Message + " - " + ErrorMessage[i].Message_V1 + " - " + ErrorMessage[i].Message_V2 + " - " + ErrorMessage[i].Message_V3 + " - " + ErrorMessage[i].Message_V4 + "<br>";

        return tab_num;
    }

    // получение списка ролей пользователя
    public List<Role> getRoles(string tab_num)
    {
        List<Role> roles = new List<Role>();
        this.ErrorMessage = new sap.BAPIRET2Table();
        this.RoleTable = new sap.ZFEATURE_STRTable();
        this.sap_proxy.Zitc_Rfc_Get_Feature(tab_num, ref this.ErrorMessage, ref this.RoleTable);

        if (ErrorMessage.Count != 0)
            return null;

        int count = this.RoleTable.Count;
        int i;

        for (i = 0; i < count; i++)
        {
            Role r = new Role(this.RoleTable[i].Feature, this.RoleTable[i].Feature_Text);
            roles.Add(r);
        }

        return roles;
    }

}
