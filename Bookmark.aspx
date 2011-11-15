<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bookmark.aspx.cs" Inherits="Bookmark" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Заметки</title>
    <link type="text/css" href="Bookmark.css" rel="Stylesheet" />
    <script type="text/javascript" src="JS/TimeTable.js"></script>
    <script type="text/javascript" src="JS/calendar/src/js/jscal2.js"></script>
    <script type="text/javascript" src="JS/calendar/src/js/lang/ru.js"></script>
    <link type="text/css" href="JS/calendar/src/css/border-radius.css" rel="Stylesheet" />
    <link type="text/css" href="JS/calendar/src/css/jscal2.css" rel="Stylesheet" />
    <link type="text/css" href="JS/calendar/src/css/reduce-spacing.css" rel="Stylesheet" />
</head>
<body>

	<form id="form1" runat="server">
	        <div class="task">
				<div class="task-label">Заметки по сотруднику</div>		
			    <div class="task-description"><asp:Label ID="lbTitle" runat="server" Text=""></asp:Label></div>
			</div>
			<div class="notes">
                <asp:Label ID="lbNotes" runat="server" Text=""></asp:Label>
			</div>
			<div class="newnote">
				<table width="100%">
				    <tr>
				        <td width="70%"><div class="newnote-label">Новая заметка </div></td>
				        <td width="30%"><b>Срок:</b>&nbsp;<input id="input_date" type="text" readonly style="width: 70px;" onfocus="ShowCalendar(this)" /></td>
				    </tr>
                </table>
				<div class="newnote-field-text">
                    <textarea id="tbBookmarkText" ></textarea>
                </div>
				<div class="newnote-submit">
                    <input id="btSave" type="button" value="Сохранить" onclick="SaveBookmark('tbBookmarkText')" />
                </div>
			</div>
	</form>

</body>
</html>
