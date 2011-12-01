// JScript File


// PROGRESS BAR
//var progress_bar = new Control.ProgressBar('progress_bar',{ interval: 0.15 });

var iIntervalId;
var iLoopCounter = 1;
var iMaxLoop = 6;
var DepartmentID;

// Сворачиваем, разворачиваем список сотрудников по отделу
function HideEmployeeList(dep_id)
{
    //var hidden = document.getElementById("DepartmentID");
    
    //alert(hidden.value);
    
    var rows = dep_id.split('|');
    var DepID = rows[1];  

    var imgID = "square" + DepID;
    var img = document.getElementById(imgID);
    var div = document.getElementById(DepID);
    
    if (img.alt == 'Свернуть')
    {
        img.src = 'App_Resources/plus.bmp';
        img.alt = 'Развернуть';
        div.style.display = 'none';
    }
    else
    {
        //var dep = DepID;
        if (div.childNodes.length == 0) 
        {
            var ddd = document.getElementById("data");
            var loading = document.getElementById("loading");
            ddd.style.display = "none";
            loading.style.display = "block";
            FillEmployeesCallback(dep_id, "");
            DepartmentID = DepID;
            var id = "ProgressMeterText" + DepartmentID;
            //var span = document.getElementById(id);
            //span.innerText = "Идет загрузка ";
            //span.style.color = "#666666";
            //span.style.fontSize = 14;
            iIntervalId = window.setInterval("iLoopCounter=UpdateProgressMeter(iLoopCounter, iMaxLoop)", 500);
        }
        img.src = 'App_Resources/minus.bmp';
        img.alt = 'Свернуть';
        div.style.display = 'block';
		updateTables();
    }    
}

function UpdateProgressMeter(iCurrentLoopCounter, iMaximumLoops)
{
            var id = "ProgressMeter" + DepartmentID;
            //var span = document.getElementById(id);
            //span.style.color = "#666666";
            //span.style.fontSize = 14;
            iCurrentLoopCounter += 1;
            if (iCurrentLoopCounter <= iMaximumLoops)
            {
                //span.innerText += ".";
                return iCurrentLoopCounter;
            }
            else
            {
                //span.innerText = "";
                return 1;
            }
}



function ClientCallback(result, context)
{
        var rows = result.split('||');
        var DepID = rows[0];
        var html = rows[1];
        var help = rows[2];
                
        var div = document.getElementById(DepID);
        div.innerHTML = html;
        
        var divHelp = document.getElementById("helpsap");
        divHelp.innerHTML = help;
        
        window.clearInterval(iIntervalId);
        
        var id = "ProgressMeterText" + DepartmentID;
        var span = document.getElementById(id);
        span.innerText = "";
        
         var id = "ProgressMeter" + DepartmentID;
         var span1 = document.getElementById(id);
         span1.innerText = "";
         
         var ddd = document.getElementById("data");
         ddd.style.display = "block";
         
         var loading = document.getElementById("loading");
         loading.style.display = "none";
        //alert(DepID);
        //alert(html);
		updateTables();
}


function onRefreshClick()
{
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];
    
    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year;
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");

}

function onSaveClick()
{
    HideBody();
    
    var inp = document.getElementById("sec_dep_value");
    
    var str = "";
    $("#departments input.timekeeper").each(
       function() 
       {
            str += $(this).attr("name") + "=" + $(this).attr("value") + ";";
       } 
    );
    
    ClearDepartments();

    var argument = "save_refresh_" + inp.value + "|" + str;
    RefreshCallback(argument, "");
}


function onClosePeriodClick()
{
    HideBody();
    
    var inp = document.getElementById("sec_dep_value");
    
    var str = "";
    $("#departments input.checkbox_close").each(
       function() 
       {
            if ($(this).attr("checked"))
                str += $(this).attr("name") + ";";
       } 
    );
    
    var all = "";
    
    if (str.length == 0) 
        all = "all_";
    
    ClearDepartments();
    
    var argument = "close_period_" + all + "refresh_" + inp.value + "|" + str;
    //alert(argument);
    RefreshCallback(argument, "");
}


function ClientRefreshCallback(result, context)
{
    
    var rows = result.split('||');
    var html = rows[0];
    var msg = rows[1];
    
    if (msg.length > 0)
        alert(msg);     
    
    ShowBody();

    var dep = document.getElementById("departments");
    dep.innerHTML = html;
}

function ClearDepartments()
{
    var dep = document.getElementById("departments");
    while (dep.childNodes.length >= 1)
    {
         var node = dep.lastChild;
         dep.removeChild(node);
    }
}

function HideBody()
{
    var ddd = document.getElementById("data");
    var loading = document.getElementById("loading");
    ddd.style.display = "none";
    loading.style.display = "block";
    //progress_bar.start.bind(progress_bar));

}

function ShowBody()
{
    var ddd = document.getElementById("data");
    ddd.style.display = "block";
        
    var loading = document.getElementById("loading");
    loading.style.display = "none";
}


function onChangeRole(url)
{
    HideBody();

    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "")
}

function onChangePeriod()
{
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];
    
    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year;
    
    //window.navigate(url);
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");
}

function onChangePost()
{
    var filter_div = document.getElementById("filter_post");
    filter_div.style.display = "none";
    
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];
    
    var ddl_post = document.getElementById("post");
    var selected_post = ddl_post.options[ddl_post.selectedIndex].value;
    
    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year + "&post=" + selected_post;
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");
}

function onCheckClick()
{
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];
    
    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year + "&check=1";
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");
}

function onCloseClick()
{
    ClearDepartments();
    HideBody();
    
    var inp = document.getElementById("sec_dep_value");
    
    var argument = "close_refresh" + inp.value;
    
    RefreshCallback(argument, "");
}

function ClientNavigateCallback(result, context)
{
    var url = result;
    window.location.href = url;
}

function onFilterStatusClick(status)
{
    var filter_div = document.getElementById("filter_status");
    filter_div.style.display = "none";
    
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];

    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year + "&status=" + status;
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");
}

function onFilterTabClick()
{
    var tab_input = document.getElementById("filter_tab_text");
    var tab_num = tab_input.value;
    
    var filter_div = document.getElementById("filter_tab");
    filter_div.style.display = "none";
    
    if (!CheckDigits(tab_num))
    {
        alert("Недопустимый символ в табельном номере!");
        tab_input.value = "";
        return;
    }
    
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];

    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year + "&tab=" + tab_num;
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");
    
}

function onFilterFioClick()
{
    var fio_input = document.getElementById("filter_fio_text");
    var fio = fio_input.value;
    
    var filter_div = document.getElementById("filter_fio");
    filter_div.style.display = "none";
        
    var period = document.getElementById("period");
    var selected_period = period.options[period.selectedIndex].value;
    
    var per = selected_period.split('|');
    var month = per[0];
    var year = per[1];
    var role = per[2];

    HideBody();
    
    var url = "TimeTable.aspx?role=" +  role + "&month=" + month + "&year=" + year + "&fio=" + fio;
    
    var argument = "navigate" + '|' + url;
    NavigateCallback(argument, "");
    
}

function ShowSectors()
{
    ClearDepartments();
    HideBody();
    
    var inp = document.getElementById("sec_dep_value");
    var msg = document.getElementById("sector_msg");
    if (inp.value == "department")
    {
        inp.value = "sector";
        msg.innerText = "Просмотр по отделам";
    }
    else
    {
        inp.value = "department";
        msg.innerText = "Просмотр по участкам";
    }
    
    var argument = "refresh_" + inp.value;
    
    RefreshCallback(argument, "");
}

// разбираем введенные часы
function CheckHourType(node)
{
    var hour = node.value;
    if (hour.length == 0) return;
    
    // разбираем строку
    var alphabet = "АаБбВвГгДдЕеЁёЖжЗзИиЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЪъЫыЬьЭэЮюЯя";
    var isText = true;

    for (i=0; i<hour.length; i++)
    {
        if (alphabet.indexOf(hour.charAt(i)) < 0)
            isText = false;
    }
    
    if (isText == true) return;
    
    // разбираем число
    if (hour.indexOf(',') < 0)
    {
        if ((parseInt(hour) > 24) || (parseInt(hour) < 0))
        { 
            alert("Время должно быть числом от 0 до 24!");    
            node.value = "";
            return;  
        } 
        
        if (!CheckDigits(hour))
        {
            alert("Недопустимый символ! Время должно быть числом от 0 до 24!");
            node.value = "";
            return;
        }

        return;  
        
    }
    else 
    {
        // берем целую часть десятичного числа
        var int_part = hour.substring(0, hour.indexOf(','));
        //alert(real);
        
        if (!CheckDigits(int_part))
        {
            alert("Недопустимый символ! Время должно быть десятичным числом от 0 до 24,00!");
            node.value = "";
            return;
        }
       
        if ((parseInt(int_part) > 24) || (parseInt(int_part) < 0))
        {
            alert("Время должно быть десятичным числом от 0 до 24,00!");
            node.value = "";
            return;
        }
        
        var real_part = hour.substring(hour.indexOf(',') + 1, hour.length);
        //alert(real_part);
        
        if (!CheckDigits(real_part))
        {
            alert("Недопустимый символ! Время должно быть десятичным числом от 0 до 24,00!");
            node.value = "";
            return;
        }
        
        if (real_part.length > 2)
        {
            alert("Недопустимый символ! Время должно быть десятичным числом от 0 до 24,00!");
            node.value = "";
            return;
        }
             
        
    }

}

// проверяем что в числе только цифры
function CheckDigits(number)
{
    var symbols = "1234567890";
    
    for (i=0; i<number.length; i++)
    {
        if (symbols.indexOf(number.charAt(i)) < 0)
        {
            return false;
        }
    }
    
    return true;
}


// показываем фильтр для выбора по должности, окошко выводим рядом со значком фильтра
function ShowFilter(id,posX, posY)
{
    var node = document.getElementById(id);
    node.style.left = posX + 5;
    node.style.top = posY;
    if (node.style.display == "none")
        node.style.display = "block";
    else
        node.style.display = "none";
}

function HideBlock(id)
{
    var node = document.getElementById(id);
    node.style.display = "none";
}


function SortPost(param)
{
    var node = document.getElementById("filter_post");
    node.style.display = "none";
    window.location = "TimeTable.aspx?post=" + param;
    //alert(param);
}

function onButton(node)
{
    node.style.color='#FFFFFF'; 
    if (GetBrowser()) node.style.background="background-image: url('App_Resources/menu_button_yellow.bmp') repeat-x";
    else node.style.backgroundImage="url('App_Resources/menu_button_yellow.bmp')"; 
}
        
function offButton(node)
{
    node.style.color='#FF0000'; 
    if (GetBrowser()) node.style.background="background-image: url('App_Resources/menu_button.bmp') repeat-x";
    else node.style.backgroundImage="url('App_Resources/menu_button.bmp')";
}

function onButtonPost(node)
{
    if (GetBrowser()) node.style.background="background-color: #FFC557";
    else node.style.backgroundColor="#FFC557"; 
}
        
function offButtonPost(node)
{
    if (GetBrowser()) node.style.background="background-color: #FFFFFF";
    else node.style.backgroundColor="#FFFFFF";
}
        
function GetBrowser()
{
    var ua = navigator.userAgent.toLowerCase(); 
    // Internet Explorer 
    if (ua.indexOf("msie") != -1 && ua.indexOf("opera") == -1 && ua.indexOf("webtv") == -1) return 1; 
    // Opera 
    if (ua.indexOf("opera") != -1) return 0; 
    // Gecko = Mozilla + Firefox + Netscape 
    if (ua.indexOf("gecko") != -1) return 0; 
}

function ShowCalendar(node)
{
    Calendar.setup(
        {
          trigger : node.id,
          inputField  : node.id,         // ID of the input field
          dateFormat  : "%d.%m.%Y",    // the date format
          onSelect    : function() { this.hide() },
          electric    : false
        }
    );
}

function SaveBookmark()
{
    var node = document.getElementById("tbBookmarkText");
    var input_date = document.getElementById("input_date");
    
    var expire_date = input_date.value;
    
    if (node.value.length == 0)
    {
        alert("Введите текст комментария!");
        return;
    }
    
    if (expire_date.length < 10)
    {
        alert("Введите срок действия комментария в поле <Срок>!");
        return;
    }
    
    var date_parts = expire_date.split('.');
    var exp_date = new Date(date_parts[2], date_parts[1] - 1, date_parts[0], 0,0,0,0);
    var exp_time = exp_date.getTime();
    
    var currentDate = new Date();
    var now = currentDate.getTime();

    if (now > exp_time)
    {
        alert("Указанный срок действия комментария МЕНЬШЕ текущей даты");
        return;
    }       
        
    var argument = expire_date + node.value;
    BookmarkCallback(argument,"");
    
}

function ClientBookmarkCallback(result, context)
{
    window.location(result);
}

function updateTables()
{
	$('.sch_hr .day_number').mouseover(function(){
		ShowFilter("helpsap", event.clientX + document.body.scrollLeft, event.clientY + document.body.scrollTop);
	}).mouseout(function(){
		HideBlock("helpsap");
	});
}