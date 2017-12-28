//菜单选择
function menuselect(obj) {
    $("#main_panel").hide();
    $("#loading").show();
    var actionname = $(obj).attr("actionname");
    var controllername = $(obj).attr("controllername");
    $.ajax({
        url: app + "/" + controllername + "/" + actionname,
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
    $("#modal_title").html("" + title); $("#modal_body").html("");
    $("#modal_body").append($("#" + message).html()); 
    $("#modal_close").html("" + modal_close);
    $("#modal_save").html("" + modal_save);
    $("#modal_save").attr("submiturl", submiturl);
    $("#modal").show();
}

function modal_save()
{
    $("#modal_form").validate();
    $.ajax({
        url: app + "/" + $("#modal_save").attr("submiturl"),
        data: $("#modal_form").serialize(),
        type: 'post',
        success: function (data)
        {
            dialog(data.title, data.message);
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