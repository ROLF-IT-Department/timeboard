<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tabel.aspx.cs" Inherits="Tabel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Табель</title>
    <link type="text/css" href="Tabel.css" rel="Stylesheet" />
    <script type="text/javascript" src="JS/TimeTable.js"></script>
    
</head>

<body>
    <form id="form1" runat="server">
    <div>
    
    <div id="filter_post" class="filter_div"  style="display: none;"><b>Фильтр по должности:</b>
    <asp:DropDownList ID="ddl_post" runat="server" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddl_post_SelectedIndexChanged" >
        <asp:ListItem Text="Выберите должность" Value=""></asp:ListItem>
    </asp:DropDownList>
    
</div>
<div id="filter_tab" class="filter_div"  style="display: none;"><b>Введите табельный номер:</b><asp:TextBox ID="filter_tab_text" Width="110px" style=" margin: 5px" runat="server"></asp:TextBox><asp:Button ID="bt_filter_tab" style=" margin: 5px" runat="server" Text="Найти" OnClick="bt_filter_tab_Click" /></div>
<div id="filter_fio" class="filter_div"  style="display: none;"><b>Введите фамилию:</b><asp:TextBox ID="filter_fio_text" Width="110px" style=" margin: 5px" runat="server"></asp:TextBox><asp:Button ID="bt_filter_fio" style=" margin: 5px" runat="server" Text="Найти" OnClick="bt_filter_fio_Click" /></div>

<div id="helpsap" class="help_sap"  style="display: none;"></div>


    <center>
    
    <table class="main_table" cellpadding="0" cellspacing="0" >
        <tr class="period_header">
            <td width="300px" align="center" style="border-bottom: 1px solid #000000"><asp:Label ID="Traffic" runat="server" Text=""></asp:Label>&nbsp;
                    <span class="period">Отчетный период - </span>
                    <asp:DropDownList ID="period" runat="server" CssClass="ddl_period" AutoPostBack="true" OnSelectedIndexChanged="period_SelectedIndexChanged"   ></asp:DropDownList> 
            </td>
            <td style="border-bottom: 1px solid #000000">&nbsp;<span class="period">Статус: <asp:Label ID="lbStatus" runat="server" Text="открыт"></asp:Label></span ></td>
        </tr>
        <tr class="main_menu">         
                <td width="300px" class="main_info" style="border-right: 4px solid #666666">
                     <table cellpadding="0" cellspacing="0" border="0" width="300px" >
                          <tr>
                               <td width="150px" align="center"><asp:ImageButton ID="Save" runat="server" ImageUrl="App_Resources/button_save.bmp" onmouseover="src='App_Resources/button_save_yellow.bmp'" onmouseout="src='App_Resources/button_save.bmp'" style="cursor: hand; cursor: pointer; padding-bottom:5px" OnClick="Save_Click"    /><asp:ImageButton ID="Check" runat="server" ImageUrl="App_Resources/button_check.bmp" onmouseover="src='App_Resources/button_check_yellow.bmp'" onmouseout="src='App_Resources/button_check.bmp'" style="cursor: hand; cursor: pointer; display:block;" OnClick="Check_Click" /></td>
                               <td width="150px" align="center"><asp:ImageButton ID="Refresh" runat="server" ImageUrl="App_Resources/button_refresh.bmp" onmouseover="src='App_Resources/button_refresh_yellow.bmp'" onmouseout="src='App_Resources/button_refresh.bmp'" style="cursor: hand; cursor: pointer; padding-bottom:5px" OnClick="Refresh_Click" /><asp:ImageButton ID="ClosePeriod" runat="server" ImageUrl="App_Resources/button_close.bmp" onmouseover="src='App_Resources/button_close_yellow.bmp'" onmouseout="src='App_Resources/button_close.bmp'" style="cursor: hand; cursor: pointer; display:block;" OnClick="ClosePeriod_Click" /></td>
                          </tr>
                     </table>
                </td>
                <td class="main_info">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="height: 100%">
                         <tr>
                             <td>&nbsp;</td>
                             <td valign="middle" class="square_column"><img alt="" src="App_Resources/square_grey.bmp" /></td>
                             <td class="square_info">Данные табельщика</td>
                             <td>&nbsp;</td>
                             <td valign="middle" class="square_column"><img alt="" src="App_Resources/square_yellow.bmp" /></td>
                             <td class="square_info">Данные HR</td>
                             <td>&nbsp;</td>
                             <td valign="middle" class="square_column"><img alt="" src="App_Resources/square_green.bmp" /></td>
                             <td class="square_info">Подтвержденные переработки</td>
                             <td>&nbsp;</td>
                         </tr>
                    </table>     
                </td>
        </tr>
        <tr class="columns">
            <td width="300px" style="border-right: 4px solid #666666">
                 <table cellpadding="0" cellspacing="0" border="0" width="100%">    
                       <tr class="columns">
                          <td class="post" align="center">Должность &nbsp <img src="App_Resources/filter.gif" alt="Фильтровать по должности" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_post',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                          <td class="tab_num" align="center">Таб. № &nbsp <img src="App_Resources/filter.gif" alt="Фильтровать по таб. номеру" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_tab',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                          <td class="fio" align="center">ФИО &nbsp <img src="App_Resources/filter.gif" alt="Фильтровать по фамилии" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_fio',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                       </tr>     
                 </table>   
            </td>
            <td class="days" align="center" valign="middle">
                <asp:Panel ID="days" runat="server" CssClass="days_panel" >
                </asp:Panel>
            </td>
        </tr>
         <tr >
                <td colspan="2" ><asp:Panel runat="server" ID="content" CssClass="panel" /></td>   
         </tr>
    </table>
    
    
    </center>
    </div>
    </form>
</body>
</html>
