<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Card.aspx.cs" Inherits="Card" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Табель сотрудника</title>
    <link type="text/css" href="Card.css" rel="Stylesheet" />
    <script type="text/javascript" src="JS/TimeTable.js"></script>
</head>
<body>
    <center>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="Title" runat="server" CssClass="title" Text=""></asp:Label>
    <br>
    <br>
    <table cellpadding="0" cellspacing="0" border=0 width="100%">
        <tr>
            <td rowspan="7" colspan="2" width="10%" align="center"><img src="App_Resources/profile.bmp" alt="" /><!--<img src="App_Resources/userpic.bmp" alt="" />--></td>

        </tr>
        <tr>
            <td align="left" width="20%"><span class="personal">Сотрудник: &nbsp</span></td>
            <td align="left" width="60%"><span class="personal_info"><asp:Label ID="Employee" runat="server" Text=""></asp:Label></span></td>
            <td rowspan="4" width="10%">&nbsp</td>
        </tr>
        <tr>
            <td align="left" width="20%"><span class="personal">Должность: &nbsp</span></td>
            <td align="left" width="60%"><span class="personal_info"><asp:Label ID="Post" runat="server" Text=""></asp:Label></span></td>
        </tr>
        <tr>
            <td align="left" width="20%"><span class="personal">Отдел: &nbsp</span></td>
            <td align="left" width="60%"><span class="personal_info"><asp:Label ID="Department" runat="server" Text=""></asp:Label></span></td>
        </tr>
        <tr>
            <td align="left" width="20%"><span class="personal">Табельный номер: &nbsp</span></td>
            <td align="left" width="60%"><span class="personal_info"><asp:Label ID="Tab" runat="server" Text=""></asp:Label></span></td>
        </tr>
        <tr>
            <td align="left" width="20%"><span class="personal">Недельный график: &nbsp</span></td>
            <td align="left" width="60%"><span class="personal_info"><asp:Label ID="WeekSchedule" runat="server" Text=""></asp:Label></span></td>
        </tr>
        <tr>
            <td align="left" width="20%"><span class="personal">Статус периода: &nbsp</span></td>
            <td align="left" width="60%"><span class="personal_info"><asp:Label ID="Status" runat="server" Text=""></asp:Label></span></td>
        </tr>
    </table>
    <br>
    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td><asp:ImageButton ID="Save" runat="server" ImageUrl="App_Resources/button_save.bmp" onmouseover="src='App_Resources/button_save_yellow.bmp'" onmouseout="src='App_Resources/button_save.bmp'" style="cursor: hand; cursor: pointer; padding-bottom:5px" OnClick="Save_Click"    /></td>
            <td>&nbsp</td>
            <td><asp:ImageButton ID="Refresh" runat="server" ImageUrl="App_Resources/button_refresh.bmp" onmouseover="src='App_Resources/button_refresh_yellow.bmp'" onmouseout="src='App_Resources/button_refresh.bmp'" style="cursor: hand; cursor: pointer; padding-bottom:5px" OnClick="Refresh_Click" /></td>
            <td>&nbsp</td>
            <td><asp:ImageButton ID="Check" runat="server" ImageUrl="App_Resources/button_check.bmp" onmouseover="src='App_Resources/button_check_yellow.bmp'" onmouseout="src='App_Resources/button_check.bmp'" style="cursor: hand; cursor: pointer; display:block; padding-bottom:5px" OnClick="Check_Click" /></td>
            <td>&nbsp</td>
            <td><asp:ImageButton ID="ClosePeriod" runat="server" ImageUrl="App_Resources/button_close.bmp" onmouseover="src='App_Resources/button_close_yellow.bmp'" onmouseout="src='App_Resources/button_close.bmp'" style="cursor: hand; cursor: pointer; display:block; padding-bottom:5px" OnClick="ClosePeriod_Click" /></td>
            <td width="50%">&nbsp</td>
        </tr>
    </table>
    <br>
   <table cellpadding="0" cellspacing="0" class="main_table" border="0" width="100%">
        <tr>
            <td class="card_columns" style="border-top: 2px solid #999999" align="left">Дни в месяце</td>
            <td rowspan="13" style="border-top: 2px solid #999999; border-bottom: 2px solid #999999;">
                <asp:Panel ID="Panel1" runat="server" Height="100%" Width="100%">
                </asp:Panel>
                </td>
        </tr>
        <tr >
            <td class="card_columns" align="left">Данные табельщика</td>
        </tr>
        <tr>
            <td class="card_columns" align="left">Данные HR</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">Подтвержденные переработки</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">ДНЕВНЫЕ переработки</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">НОЧНЫЕ переработки</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">Время работы С</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">Время работы ПО</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">Количество часов</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">Однодневный график</td>
        </tr>
        <tr>
            <td class="card_columns"  align="left">Вариант однодн. графика</td>
        </tr>
        <tr>
            <td class="card_columns" align="left">Тип отклонения от графика</td>
        </tr>
        <tr>
            <td class="card_columns" style="border-bottom: 2px solid #999999;" align="left">Часы отклонения</td>
        </tr>

    </table>
    
    
    </div>
    </form>
    </center>
</body>
</html>
