// JavaScript Document
/*
  定制检索式 20161019
  说明：该栏目交互较多，所以把代码从init.js中单独分离出来
*/

function myCustomFn(){
	this.custom = $(".mycustom");
	this.menu = this.custom.find(".menu");
	this.result = this.custom.find(".result");
	this.tabList = this.custom.find(".tab-list");
	this.setPopBtn = this.custom.find(".addcustom-btn");
	this.emailPush = this.custom.find(".email-push");
	this.userEmail = this.custom.find(".useremail");

	this.pop = $(".custom-popup");
	this.searchContainer = this.pop.find(".search-condition");
	this.searchItems = this.pop.find(".item");
	this.searchAddBtn = this.pop.find(".add-group");
	this.itemsLen = this.searchItems.length;
	this.init();
}
myCustomFn.prototype = {

	init:function () {
		var that = this;
		that.pop.hide();

		that.bindEditSwitch();//menu菜单→编辑
		that.editEmail();//邮箱编辑事件
		that.bindPushEmail();//邮箱推送开关
		that.bindTableListDel();//表格→删除
		that.bindTableReturn();//表格→完成/取消

		that.bindPopup();//触发弹出框事件
		that.addSearchCondition();//添加检索条件
		that.delSearchCondition();//删除检索条件
	},

	bindEditSwitch: function () {  //menu菜单→编辑
		var that = this;
		that.menu.on("click",".edit",function () {
			that.result.hide();
			that.tabList.show();

		});
	},

	editEmail: function () {//编辑邮箱
		var that = this;
		that.userEmail.each(function () {
			_this = $(this);
			if (_this.val() == "添加邮箱，及时推送更新") {
				_this.parent().addClass("onediting");
				_this.prop("readonly", false);
			}else {
				_this.parent().removeClass("onediting");
				// _this.prop("readonly", true);
				_this.siblings(".edit").click(function(){
					$(this).parent().addClass("onediting");
					_this.prop("readonly",false);
					_this.css("color","#333");
				});
			}
		});
	},

	bindPushEmail : function () {//邮箱推送开关
		var that = this;
		that.emailPush.find(".icon-btn").click(function(){
			$.on_off($(this));
		});
	},

	bindTableListDel:function () {//表格列表→删除
		var that = this;
		that.tabList.find(".del a").click(function(){

			/*$(this).parents("tr").remove();*/
		});
	},

	bindTableReturn:function () {//表格列表→点击“完成”/“取消”
		var that = this;

		that.tabList.find(".btn a").click(function(event){
			that.result.show();
			that.tabList.hide();

			if( event.target.className == "save" ){//完成

			}

		});
	},

	bindPopup : function () {
		var that = this;
		that.setPopBtn.click(function(event) {
			event.preventDefault();

			$(".custom-popup").popCenter({//添加检索式的弹出框
				clearPop: false,//关闭时是否需要清除此弹出框，清除true，隐藏false
				btns: [".add"]//弹出框上的按钮“添加”
			});

		});

	},

	addSearchCondition: function () {//添加检索条件
		var that = this;
		if(!that._isShowAdd(that.itemsLen)){
			return;
		}else{
			that.searchAddBtn.on("click",function(){
				$newItem = $('<li></li>')
					.addClass('new-item')
					.html('<select>'+
							'<option value="and" selected="">并且</option>'+
							'<option value="or">或者</option>'+
							'<option value="not">不含</option>'+
						'</select> '+
						'<select class="item-type">'+
							'<option value="主题" selected="">主题</option>'+
							'<option value="全文">全文</option>'+
							'<option value="篇名">篇名</option>'+
							'<option value="作者">作者</option>'+
							'<option value="单位">单位</option>'+
							'<option value="关键词">关键词</option>'+
							'<option value="摘要">摘要</option>'+
							'<option value="参考文献">参考文献</option>'+
							'<option value="中图分类号">中图分类号</option>'+
							'<option value="文献来源">文献来源</option>'+
						'</select> '+
						'<input class="data" type="text" maxlength="50" />'+
						'<a class="icon-btn del-group"></a>');

				that.searchContainer.append($newItem);
				// $newItem.find(".item-type option").eq(that.itemsLen).attr("selected",true);
				that.itemsLen++;
				that._isShowAdd(that.itemsLen);
			});
		}
	},

	delSearchCondition: function () {//删除检索条件
		var that = this;
		that.searchContainer.on("click",".del-group",function(){
			$(this).parent("li").remove();
			that.itemsLen--;
			that._isShowAdd(that.itemsLen);
		});
	},


	_isShowAdd :function (length) {//判断是否显示“添加”按钮---设置3条时隐藏“添加”按钮，少于3条则显示
		var that = this;
		if(length >= 3){
			that.searchAddBtn.hide();
			return false;
		}
		that.searchAddBtn.show();
		return true;
	}

};


$(function(){
	new myCustomFn();//调用

});