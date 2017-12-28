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
            $("#main_panel").html(data); $("#loading").hide();$("#main_panel").show();
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

