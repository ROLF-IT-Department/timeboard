<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CheckEmployees.aspx.cs" Inherits="CheckEmployees" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Проверка сотрудников</title>
    <link type="text/css" href="Default.css" rel="Stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;<center>
        <h1>Вход под другим пользователем</h1>
            <table cellpadding="0" cellspacing="0" border="0" class="main_table">
                <tr>
                    <td align="left" valign="top">
                        <div class="main_div">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr class="col_size">
                                    <td align="right"><span class="caption">Логин Windows:</span>&nbsp;</td>
                                    <td align="left"><asp:TextBox ID="Netname" CssClass="inputtext" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="col_size">
                                    <td align="right"><span class="caption">Логин УД:</span>&nbsp;</td>
                                    <td align="left"><asp:TextBox ID="Login" CssClass="inputtext" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="col_size">
                                    <td align="right"><span class="caption">Пароль:</span>&nbsp;</td>
                                    <td align="left"><asp:TextBox ID="Password" TextMode="Password" CssClass="inputtext" runat="server"></asp:TextBox></td>
                                </tr>
                                <tr class="col_size">
                                    <td colspan="2" align="center"><asp:Button ID="submit" runat="server" Text="Войти" CssClass="bt_enter" OnClick="submit_Click" /></td>
                                 
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
