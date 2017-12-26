var fade = true;
$(function () {
    setInterval(function () {
        if (fade) {
            fade = false;
            $("#bannar1").fadeIn("slow");
            $("#bannar2").fadeOut("slow");
        } else {
            fade = true;
            $("#bannar2").fadeIn("slow");
            $("#bannar1").fadeOut("slow");
        }
    }, 88888);

    (function ($) {
        $.extend({
            _isalert: 0,
            alert: function () {
                if (arguments.length) {
                    $._isalert = 1;
                    $.confirm.apply($, arguments);
                }
            },
            confirm: function () {
                var args = arguments;
                if (args.length && (typeof args[0] == 'string') && !$('#alert_overlay').length) {
                    if (!$('#alert_style').length) $('body').append('<style id="alert_style" type="text/css">#alert_overlay{position:fixed;width:99%;height:99%;top:0;left:0;z-index:999;background:#000;filter:alpha(opacity=5);opacity:.05}#alert_msg{position:fixed;width:400px;margin-left:-201px;left:50%;top:20%;z-index:1000;border:1px solid #aaa;box-shadow:0 2px 15px rgba(0,0,0,.3);background:#fff}#alert_content{padding:20px;font-size:14px;text-align:left}#alert_buttons{padding:10px;border-top:1px solid #aaa;text-align:right;box-shadow:0 1px 0 #fff inset;background:#eee;-moz-user-select:none;-webkit-user-select:none;-ms-user-select:none}#alert_buttons .alert_btn{padding:5px 12px;margin:0 2px;border:1px solid #aaa;background:#eee;cursor:pointer;border-radius:2px;font-size:14px;outline:0;-webkit-appearance:none}#alert_buttons .alert_btn:hover{border-color:#bbb;box-shadow:0 1px 2px #aaa;background:#eaeaea}#alert_buttons .alert_btn:active{box-shadow:0 1px 2px #aaa inset;background:#e6e6e6}</style>');
                    var dialog = $('<div id="alert_overlay"></div><div id="alert_msg"><div id="alert_content">' + args[0] + '</div><div id="alert_buttons"><button class="alert_btn alert_btn_ok">确定</button><button class="alert_btn alert_btn_cancel">取消</button></div></div>');
                    if ($._isalert) dialog.find('.alert_btn_cancel').hide();
                    dialog.on('contextmenu', function () {
                        return !1;
                    }).on('click', '.alert_btn_ok', function () {
                        dialog.remove() && (typeof args[1] == 'function') && args[1].call($, !0);
                    }).on('click', '.alert_btn_cancel', function () {
                        dialog.remove() && (typeof args[1] == 'function') && args[1].call($, !1);
                    }).appendTo('body');
                }
                $._isalert = 0;
            }
        });
    })($);
});

function NaviClick(view)
{

    $("#navCenter table").css("background", "rgba(0, 0, 0, 0)");
    $("#" + view).css("background", "#1e50ae none repeat scroll 0 0");
    $.ajax({
        url: app + "/home/NaviClick/" + view,
        type: "Post",
        success: function (data) {
            $("#centerTopForms").html(data);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
                console.error(textStatus + "     " + errorThrown);
        }
    });
    return false;
}
function CultureClick(id) {
    $("div[id^='corporate']").removeClass("formTabButtonHover").children("div").removeClass("formTabMiddleHover");
    $("#" + id).addClass("formTabButtonHover").children("div").addClass("formTabMiddleHover");
    $("div[id^='contentcorporate']").hide();
    $("#content" + id).show();
}
function NewsClick(id) {
    $("div[id^='news']").removeClass("formTabButtonHover").children("div").removeClass("formTabMiddleHover");
    $("#" + id).addClass("formTabButtonHover").children("div").addClass("formTabMiddleHover");
    $("div[id^='contentnews']").hide();
    $("#content" + id).show();
}
function ServiceClick(id)
{
    $("div[id^='service']").removeClass("formTabButtonHover").children("div").removeClass("formTabMiddleHover");
    $("#" + id).addClass("formTabButtonHover").children("div").addClass("formTabMiddleHover");
    $("div[id^='contentservice']").hide();
    $("#content" + id).show();
}
function CommitMessage(num)
{
    if ($("#messagename").val() == "") {
        $.alert("姓名不能为空!");

    } else {
        $.alert("留言提交成功，我们会尽快审核！");
    }
}