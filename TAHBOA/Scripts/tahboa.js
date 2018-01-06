var currentactionname = "";
var currentcontroller = "";
//菜单选择
function menuselect(obj) {
    $("#main_panel").hide();
    $("#loading").show(); 
    $("#panal").html($(obj).html());
    currentactionname = $(obj).attr("actionname");
    currentcontroller= $(obj).attr("controllername");
    $.ajax({
        url: app + "/" + currentcontroller + "/" + currentactionname,
        type: "get",
        success: function (data) {
            $("#loading").hide(); $("#main_panel").show();
            $("#main_panel").html(data); 
        },
        error: function () {
            $("#main_panel").html($(obj).html() + "•功能待完善……");
        }
    });
}

//对话框
function dialog(title, message) {
    $("#dialog_title").html("" + title);
    $("#dialog_message").html("" + message);
    $("#dialog_show").click();
}
function modal(title, message, modal_close, modal_save, submiturl) {
    $("#modal_title").html("" + title); 
    $("#modal_body").html($("#" + message).html()); 
    $("#modal_close").html("" + modal_close);
    $("#modal_save").html("" + modal_save);
    $("#modal_save").attr("submiturl", submiturl);
    $("#modal").show();
}
function modal_update(id, title, modal_close, modal_save, updateurl) {
    updateurl = "/" + updateurl + "/";
    $.ajax({
        url: app + updateurl + id,
        type: 'get',
        async: false,
        success: function (data) {
            $("#modal_title").html(title);
            $("#modal_body").html(data);
            $("#modal_close").html(modal_close);
            $("#modal_save").html(modal_save);
            $("#modal_save").attr("submiturl", updateurl);
            $("#modal").show();
        },
        error: function () {
            dialog("失败", "本次修改失败")
        }
    });
}
function modal_add( title, modal_close, modal_save, addurl) {
    updateurl = "/" + addurl + "/";
    $.ajax({
        url: app + updateurl,
        type: 'get',
        async: false,
        success: function (data) {
            $("#modal_title").html(title);
            $("#modal_body").html(data);
            $("#modal_close").html(modal_close);
            $("#modal_save").html(modal_save);
            $("#modal_save").attr("submiturl", addurl);
            $("#modal").show();
        },
        error: function () {
            dialog("失败", "本次新增失败")
        }
    });
}

function modal_save()
{
    $("#modal_form").validate();
   
    var submiurl = $("#modal_save").attr("submiturl")
    try {
        if (submiurl.indexOf("Contract") >= 0) {
            $("#modal_body #合同内容").val(encodeURIComponent(CKEDITOR.instances.合同内容.getData()));
        }
    }
        catch(e)
        {
        }
    $.ajax({
        url: app + "/" + submiurl,
        data: $("#modal_form").serialize(),
        type: 'post',
        success: function (data)
        {
            dialog(data.title, data.message);
        }
    });
    $.ajax({
        url: app + "/" + currentcontroller + "/" + currentactionname,
        type: "get",
        success: function (data) {
            $("#loading").hide(); $("#main_panel").show();
            $("#main_panel").html(data);
        },
        error: function () {
        }
    });
}

//翻页
function pages(number) {
    var actionname = $("#actionname").val();
    var controllername = $("#controllername").val();
    var pagesize = parseInt($("#pagesize").val());
    var currentpage = parseInt($("#currentpage").val());
    var keyword = "";
    if ($("#keyword").val() != "undefined")
    {
        keyword = $("#keyword").val();
    }
    switch (number) {
        case "p": currentpage = currentpage - 1; break;
        case "n": currentpage = currentpage + 1; break;
        default:
            currentpage = parseInt(number);
            break;
    }
    $.ajax({
        url: app + "/" + controllername + "/" + actionname,
        data: { pagesize: pagesize,pageindex: currentpage,keyword:keyword },
        type: "post",
        success: function (data) {
            $("#main_panel").html(data); $("#loading").hide(); $("#main_panel").show();
        }
    });
}
//省市县
function selectchanger(obj, changeid) {
    var regioncode = $(obj).find("option:selected").attr("regioncode");
    var selectcode = $("#selectcode").val();
    $.ajax({
        url: app + "/" + currentcontroller + "/GetArea",
        data: { "parentcode": regioncode, "selectcode": selectcode },
        type: "post",
        success: function (data) {
            $("#" + changeid).html(data);
        }
    });
}