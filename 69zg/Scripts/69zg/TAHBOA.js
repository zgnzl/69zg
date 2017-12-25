$(function () {
    setInterval(function () {
        var date = new Date();
        date.toLocaleTimeString();
        $("#currenttime").html(date.toLocaleTimeString());
    }, 1000);
});
function zhedie(obj)
{
    $("#leftnavi").toggleClass("zhedie");
    $(obj).show();
    if ($("#leftnavi").hasClass("zhedie")) {
        $(obj).html(">");
    } else
    {
        $(obj).html("<");
    }
}
function ulfirst(obj) {
    $("#leftnavi>div[class!='first']").hide();
    $("#leftnavi>div[class='first']").css("background-color", "#f5f5f5")
    $(obj).next("div").toggle(); $(obj).css("background-color", "darkgray");
};
function liselelct(obj) {
    $("#leftcontent").html($(obj).html() +"•努力加载……");
    $("#2017005015").attr("onclick", "liselelct(this)").removeAttr("style").removeAttr("id");
    $(obj).css("color", "#999").removeAttr("onclick").css("cursor", "auto").attr("id","2017005015");
    var actionname = $(obj).attr("actionname");
    var controllername = $(obj).attr("controllername");
    $.ajax({
        url: app + "/" + controllername + "/" + actionname,
        type: "get",
        success: function (data) {
            $("#leftcontent").html(data);
            $("#leftnavi").height($("#leftcontent").height()+50);
        },
        error: function () {
            $("#leftcontent").html($(obj).html() + "•功能待完善……");
        }
    });
}

function linkliselelct(actionname)
{
    $("#2017005015").attr("onclick", "liselelct(this)").removeAttr("style").removeAttr("id");
    $("#leftnavi").find("span[actionname='" + actionname + "']").click().css("color", "#999").removeAttr("onclick").css("cursor", "auto").attr("id", "2017005015");;
}

function ContractOpt(id,controllername,actionname,bh)
{
    if (actionname.substr(0, 3) == "Del")
    {
        if (!DelData(bh))
        {
            return false;
        }
    }

    $.ajax({
        url: app + "/" + controllername + "/" + actionname + "/" + id,
        type: "get",
        success: function (data) {
            $("#leftcontent").html(data);
            $("#leftnavi").height($("#leftcontent").height() + 50);
        },
        error: function () {
            $("#leftcontent").html( "•功能待完善……");
        }
    });
}

function Delfunctioninfo(id)
{
    if (!DelData()) {
            return false;
        }
    var controllername = $("#controllername").val();
    $.ajax({
        url: app + "/" + controllername + "/DelC_functioninfo/" + id,
        type: "get",
        success: function (data)
        {
            if (data["result"] == "SUCCESS")
            {
                $.alert("删除<span style='color:red'>" + data["name"] + "</span>成功！");
                $("#" + id).remove();
            }
            else
                {
                $.alert(data["name"]);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown)
        {
            $.alert(textStatus);
        }
    });
}
function Updfunctioninfo(id)
{
    var controllername = $("#controllername").val();
    $.ajax({
        url: app + "/" + controllername + "/EditC_functioninfo/" + id,
        type: "get",
        data: { displayname: $("#" + id + "_displayname").val(), description: $("#" + id + "_description").val(), actionname: $("#" + id + "_actionname").val(), fatherid: $("#" + id + "_fatherid").val() },
        success: function (data) {
            if (data["result"] == "SUCCESS") {
                $.alert("更新<span style='color:red'>" + $("#" + id + "_displayname").val() + "</span>成功！");
            }
            else {
                $.alert(data["name"]);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.alert(textStatus);
        }
    });
}
function Getlistcontract() {
    $.ajax({
        url: app + "/" + controllername + "/UpdateC_contract/",
        type: "post",
        data: { type:'list',contract_type: $("#contract_type").val(), contract_area: $("#contract_area").val(), contract_id: $("#contract_id").val(), contract_yf: $("#contract_yf").val()  },
        success: function (data)
        {
            $("#contract_list").html(data);
        },
        error: function(XMLHttpRequest, textStatus, errorThrown){
            $.alert(textStatus);
        }
    });
}
function DelData(name) {
    if (!confirm("数据无价，确定要删除" + name + "吗?")) {
        return false;
    } else
    {
        return true;
    }
}