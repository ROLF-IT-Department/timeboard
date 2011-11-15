<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PageLoader.aspx.cs" Inherits="PageLoader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Идет загрузка страницы</title>
        <link type="text/css" href="MasterPage.css" rel="Stylesheet" />
        <link type="text/css" href="MainForm.css" rel="Stylesheet" />
        <script type="text/javascript" src="JS/TimeTable.js"></script>
        <script type="text/javascript">
        var iLoopCounter = 1;
        var iMaxLoop = 6;
        var iIntervalId;
        function BeginPageLoad()
        {
            location.href = "<%=PageToLoad %>";
            iIntervalId = window.setInterval("iLoopCounter=UpdateProgressMeter(iLoopCounter, iMaxLoop)", 500);
        }
        function UpdateProgressMeter(iCurrentLoopCounter, iMaximumLoops)
        {
            iCurrentLoopCounter += 1;
            if (iCurrentLoopCounter <= iMaximumLoops)
            {
                //ProgressMeter.innerText += ".";
                return iCurrentLoopCounter;
            }
            else
            {
                //ProgressMeter.innerText = "";
                return 1;
            }
        }
        function EndPageLoad()
        {
            window.clearInterval(iIntervalId);
            ProgressMeter.innerText = "Страница загружена. Идет перенаправление"
        }
    </script>
</head>
<body onload="javascript:BeginPageLoad()" onunload="javascript:EndPageLoad()">

    <table class="header_table" id="header_table" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td class="logo" colspan="5"><img alt="" src="App_Resources/logo2.bmp" class="rolf_logo" />&nbsp;</td>
    </tr>
    <!-- Таблица главного меню-->
    <tr>
        <td>
            <asp:Label ID="MenuBar" runat="server" Text=""></asp:Label>
            
        </td>
    </tr>
    <tr>
        <td class="menu_bottom" colspan="5">&nbsp;</td>
    </tr>

    <tr style="width: auto;">
        <td colspan="5">
            <form id="form1" runat="server" >
            <div style="width: 100%; height: 550px; text-align:center;">
                 <div style="width: 150px; height: 80px; position:absolute; left: 600px; top: 300px;"><img src="App_Resources/loading.gif" alt=""><br><span id="Message" style='font-family: Trebuchet MS, Times New Roman; font-size: 12px; font-weight:bold; text-align:center;'>Подождите, пожалуйста! Идет загрузка!</span><span id="ProgressMeter"></span></div>

            </div>
            </form>
        </td>
   </tr>
   
    <tr>
        <td class="bottom" colspan="5">&nbsp;</td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr class="bottom_table">
                    <td  class="copyright" align="center" valign="top"><img alt="" src="App_Resources/copyright.bmp" /> <span style="font-family: Times New Roman">2008</span>, ЗАО РОЛЬФ Холдинг </td>
                    <td  valign="top" colspan="3">&nbsp;</td>
                    <td  class="contact" align="center" valign="top">По вопросам обращаться<br/> в отдел поддержки ИС</td>
                </tr>
            </table>
        </td>
    </tr>
</table>    
    


</body>
</html>
