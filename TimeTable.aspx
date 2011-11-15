<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="TimeTable.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<!--<asp:ListItem Text="ИЮНЬ 2008" Value="1" ></asp:ListItem>
                 <asp:ListItem Text="ИЮЛЬ 2008" Value="2" Selected="True"></asp:ListItem>
                 <asp:ListItem Text="СЕНТЯБРЬ 2008" Value="3" ></asp:ListItem>-->

<div id="filter_status" class="filter_div_status"  style="display: none;"><b>Статус:</b><br />
<table>
    <tr>
        <td><div style="width: 50px; height: 20px;"><img alt="Закрыт" src="App_Resources/red.bmp" style="cursor: pointer; cursor: hand;" onclick="onFilterStatusClick(2)" /></div></td>
    </tr>
    <tr>
        <td><div style="width: 50px; height: 20px;"><img alt="Блокирован" src="App_Resources/yellow.bmp" style="cursor: pointer; cursor: hand;" onclick="onFilterStatusClick(1)" /></div></td>
    </tr>
    <tr>
        <td><div style="width: 50px; height: 20px;"><img alt="Открыт" src="App_Resources/green.bmp" style="cursor: pointer; cursor: hand;" onclick="onFilterStatusClick(0)" /></div></td>
    </tr>
</table>
</div>
<div id="filter_post" class="filter_div"  style="display: none;"><b>Фильтр по должности:</b>
<asp:Label ID="lbPost" runat="server" Text=""></asp:Label>    
</div>
<div id="filter_tab" class="filter_div"  style="display: none;"><b>Введите табельный номер:</b><input id="filter_tab_text" style="width:110px; margin:5px;" type="text" /><input id="bt_filter_tab" style=" margin: 5px" type="button" value="Найти" onclick="onFilterTabClick()" /></div>
<div id="filter_fio" class="filter_div"  style="display: none;"><b>Введите фамилию:</b><input id="filter_fio_text" style="width:110px; margin:5px;" type="text" /><input id="bt_filter_fio" style=" margin: 5px" type="button" value="Найти" onclick="onFilterFioClick()" /></div>


<div id="helpsap" class="help_sap"  style="display: none;"></div>


<center>

<div id="loading" style="display: none; width: 150px; height: 120px; padding-top: 200px; text-align:center;"><img src="App_Resources/progress_bar.gif" alt="" style="padding-bottom: 10px;"><br><span style='font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold; text-align:center;'>Подождите, пожалуйста! Идет обработка!</span></div>

<!--<div id="data" style="cursor:hand; width: 800px; height: 500px; position: relative; z-index: 2;  filter: progid:dximagetransform.microsoft.alpha(opacity=10); -moz-opacity: 0.1; -khtml-opacity: 0.1; opacity: 0.1;">-->
<div id="data" style="display: block;">
    <table  class="mainform" cellpadding="0" id="tab1" cellspacing="0" border="0">
        <tr>
            <td colspan="2" align="left"><div id="sector_department" style="display:block;"><input type="hidden" id="sec_dep_value" value="department" /><img alt="" src="App_Resources/sector.bmp" onclick="ShowSectors()" style="cursor: hand; cursor: pointer; padding-left: 20px; top: 2px; position:relative;"/>&nbsp;<span id="sector_msg" onclick="ShowSectors()" style="cursor: hand; cursor: pointer; font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold;">Просмотр по участкам</span></div></td>
            <td colspan="3" align="left">&nbsp;<asp:Label ID="lbTabel" runat="server" Text=""></asp:Label><asp:Label ID="lbSector" runat="server" Text=""></asp:Label><asp:Label ID="lbReportFactTime" runat="server" Text=""></asp:Label><asp:Label ID="lbReportTimeboard" runat="server" Text=""></asp:Label></td>
        </tr>
        <!-- Серая шапка с выбором периода -->
        <tr class="period_header">
            <td class="top_period_line"><img alt="" src="App_Resources/left_top_corner.bmp" /></td>
            <td class="top_period_menu">
                  <span class="period">Отчетный период - </span>
                <asp:Label ID="lbPeriod" runat="server" Text="Label"></asp:Label>                       
            </td>
            <td class="grid_center">&nbsp;</td>
            <td >&nbsp;<span class="period">Роль: <asp:Label ID="lbRole" runat="server"></asp:Label></span ></td>
            <td class="top_period_line"><img alt="" src="App_Resources/right_top_corner.bmp" /></td>
        </tr>
        <!-- Строка с кнопками Сохранить/Обновить -->
         <tr class="main_menu">
            <td class="main_info_left">&nbsp;</td>                 
            <td class="main_info_buttons">
                 <table cellpadding="0" cellspacing="0" border="0" width="350px" >
                      <tr class="main_menu">
                           <td width="175px" align="center"><img alt="" id="btSave" src="App_Resources/button_save.bmp" onclick="onSaveClick()" onmouseover="src='App_Resources/button_save_yellow.bmp'" onmouseout="src='App_Resources/button_save.bmp'" style="cursor: hand; cursor: pointer; display:block;  padding-bottom:5px" /><img alt="" id="btCheck" src="App_Resources/button_check.bmp" onclick="onCheckClick()" onmouseover="src='App_Resources/button_check_yellow.bmp'" onmouseout="src='App_Resources/button_check.bmp'" style="cursor: hand; cursor: pointer; display:block;" /></td>
                           <td width="175px" align="center"><img alt="" id="btRefresh" src="App_Resources/button_refresh.bmp" onclick="onRefreshClick()" onmouseover="src='App_Resources/button_refresh_yellow.bmp'" onmouseout="src='App_Resources/button_refresh.bmp'" style="cursor: hand; cursor: pointer; display:block; padding-bottom:5px" /><asp:Label ID="lbCloseButton" runat="server" Text=""></asp:Label></td>
                      </tr>
                 </table>
            </td>
            <td class="grid_line_center">&nbsp;</td>
            <td class="main_info_squares">
                <table cellpadding="0" cellspacing="0" border="0" width="100%" style="height:100%">
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
            <td class="main_info_right">&nbsp;</td>
        </tr>
        <!-- Строка с полями дни -->
        <tr class="columns">
            <td class="columns_left">&nbsp;</td>
            <td class="top_period_menu">
                 <table cellpadding="0" cellspacing="0" border="0" width="350px">    
                       <tr class="columns">
                          <td class="status" align="center">&nbsp;<img src="App_Resources/neutral.gif" alt="Фильтровать по статусу" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_status',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                          <td class="post" align="center">Должность &nbsp <img src="App_Resources/filter.gif" alt="Фильтровать по должности" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_post',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                          <td class="tab_num" align="center">Таб. № &nbsp <img src="App_Resources/filter.gif" alt="Фильтровать по таб. номеру" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_tab',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                          <td class="fio" align="center">ФИО &nbsp <img src="App_Resources/filter.gif" alt="Фильтровать по фамилии" style="cursor: pointer; cursor: hand;" onclick="ShowFilter('filter_fio',event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);"></td>
                       </tr>     
                 </table>   
            </td>
            <td class="grid_line_center">&nbsp;</td>
            <td class="days" align="center" valign="middle">
                <asp:Panel ID="days" runat="server" CssClass="days_panel" >
                </asp:Panel>
               </td>
            <td class="columns_right">&nbsp;</td>

        </tr>
        
        <tr class="main_panel">
             <td colspan="5"><div style='width:100%;height:400px; overflow: scroll; border-left: 1px solid #C0C0C0; border-right: 1px solid #C0C0C0;'><asp:Panel runat="server" ID="content" CssClass="panel"   ScrollBars="None" /></div></td>   
        </tr>
        
        <tr class="bottom_row">    
             <td class="bottom_line" valign="top"><img alt="" src="App_Resources/left_corner_bottom.bmp"  /></td>
             <td colspan="3" class="bottom_line" valign="top">&nbsp;</td>   
             <td class="bottom_line" valign="top"><img alt="" src="App_Resources/right_corner_bottom.bmp" /></td>
        </tr>
    </table>
</div>
</center>


</asp:Content>

