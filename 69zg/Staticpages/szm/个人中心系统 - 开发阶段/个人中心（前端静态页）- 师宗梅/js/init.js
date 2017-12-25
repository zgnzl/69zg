// JavaScript Document
/* 
  个人中心 20160510
  说明：是所有页面的功能集合，一部分是公用功能，一部分是各个栏目单独的功能，可根据每个栏目需要自行调用  
*/

$(function(){

	//检索输入框的鼠标事件
	$.inputDefault(".search-input");


	//检索
	$.BindSearch(".search-form",".search-input",'.searchbtn');


	//左侧导航
	$.sideNav(".user",".nav-list","cur");
	
	/*工具栏--排序*/
	$.SortBY('.sort','.sort-default','select');
	
			
	//基本资料→教育信息
    $.infoEducation();
	
	//成果推送的切换
	$.Switches('.push-achievement','.submenu','.database-type','active');
	
	//推送条件
	$.pushTerm();

	//定制检索式/引文跟踪の邮箱
	$.inputDefault(".useremail");

	//引文跟踪
	$.citationTrack();
	
	//检索历史
	$.searchHistory();
	
	//收藏按钮变色
	$.collectIcon();
	
	//我的云端
	$.myCloud();

    //侧边栏，判断初始时是否显示滑动条
	$.initScrollBar();

    //“复选框”的全选、清除
	$.checkBoxFn(".js-checkboxlist");

	//侧边栏 点击进行筛选
	$.groupFilter();

	//删除确认框
	$(".js-sureDel").on("click",function(event){//确认删除弹出框
		event.preventDefault();
		$.delConfirm();//调用删除确认弹出框
	});


});



(function ($) {
    
	$.extend({

		//检索输入框的鼠标事件
		inputDefault:function(input){

			$(input).focus(function(){
				defaultValue = $(this).attr("placeholder");
				if($(this).val()==defaultValue){
					$(this).val('');
					$(this).css("color","#333");
				}
			}).blur(function(){
				defaultValue = $(this).attr("placeholder");
				if($(this).val()==''){
					$(this).val(defaultValue);
					$(this).css("color","#999");
				}
			});
		},



		//检索
		BindSearch:function(secSelector,inputSelector,submitSelector){
			secBox = $(".header").find(secSelector);
			inputBox = secBox.find(inputSelector);

			var defaultValue = inputBox.attr("placeholder");
			//检索按钮
			secBox.on("click", submitSelector, function () {
				var keyword = $(this).siblings(inputSelector).val();
				if (keyword == null || $.trim(keyword) == '' || keyword == defaultValue) {
					alert('请输入有效检索词！');
					return false;
				}
				return true;
			});

			//绑定回车检索
			inputBox.keydown(function (event) {
				var event = arguments[0] || window.event;
				if (event.keyCode == 13) {
					$(this).siblings(submitSelector).trigger('click');
				}
			});

		},


		//左侧导航
		sideNav:function( userSelector,navSelector,curClass){
			$(navSelector).find("a").click(function(){
				_index = $(navSelector).find("a").index($(this));
				if( $(this).siblings("dl").length > 0 ){
					_index++;
				}
				$(navSelector).find("a").removeClass(curClass).eq(_index).addClass(curClass);
			});
			
			$(userSelector).find("p a").click(function(){
				$(navSelector).find("a").removeClass(curClass);
			});
			
		},	
		
		
		/*工具栏--排序*/
		SortBY:function(Selector,Default,curClass){
			
			$(Selector).mouseenter(function(){
				$(this).find("ul").show();
			}).mouseleave(function(){
				$(this).find("ul").hide();
			});
			
			var click_i = 0;
			$(Selector).find("a").click(function(){
				$(this).parent().addClass(curClass).siblings().removeClass(curClass);
				
				//升降序
				$sortDefault = $(Selector).find(Default);
				$sortList = $(Selector).find("ul");
				_index = $(Selector).find("a").index($(this));
				
				if( click_i == _index){
					$(this).find("i").toggleClass("ascend descend");
				}
				
				click_i = _index;
				$sortDefault.find("span").html($(this).html());
				$sortList.hide();

            });
		},
		
		
		/****判断"侧边栏滑动条"是否显示****/
		initScrollBar:function(){
			$(".scroll-list").each(function(){
				if( $(this).find("dd").length > 4){
					$(this).parent().addClass("isScroll");
				}else{
					$(this).parent().removeClass("isScroll");
				}
			});
		},
		
		
		//基本资料→教育信息
		infoEducation:function(){
			
			//编辑
			$(".info-education").on("click",".edit",function(){
				$(this).parent().addClass("onediting");
			});
			
			//保存
			$(".info-education").on("click",".save",function(){
				$(this).parent().removeClass("onediting");
			});

			
		    //添加select组
			$(".info-education").find(".add-group").click(function(){
				len = $(this).parent().find("dd").length+1;
				var $ddList = $( '<dd>'+
								'<em>'+ len +'</em>'+
								'<p class="list-1">【专辑】 '+
								  '<label>不限</label>'+
								  '<select class="zj">'+
									'<option value="不限">不限</option>'+
									'<option value="基础科学">基础科学</option>'+
									'<option value="工程科技 I 辑">工程科技 I 辑</option>'+
									'<option value="工程科技 II 辑">工程科技 II 辑</option>'+
									'<option value="农业科技">农业科技</option>'+
									'<option value="医药卫生科技">医药卫生科技</option>'+
									'<option value="哲学与人文科学">哲学与人文科学</option>'+
									'<option value="社会科学 I 辑">社会科学 I 辑</option>'+
									'<option value="社会科学 II 辑">社会科学 II 辑</option>'+
									'<option value="信息科技"> 信息科技</option>'+
									'<option value="经济与管理科学">经济与管理科学</option>'+
								  '</select>'+
								'</p>'+
								'<p class="list-2">【专题】 '+
								  '<label>不限</label>'+
								  '<select class="zt">'+
									'<option value="不限">不限</option>'+
									'<option value="无线电电子学">无线电电子学</option>'+
									'<option value="电信技术">电信技术</option>'+
									'<option value="计算机硬件技术">计算机硬件技术</option>'+
									'<option value="计算机软件及计算机应用">计算机软件及计算机应用</option>'+
									'<option value="互联网技术">互联网技术</option>'+
									'<option value="自动化技术">自动化技术</option>'+
									'<option value="新闻与传媒">新闻与传媒</option>'+
									'<option value="出版">出版</option>'+
									'<option value="图书情报与数字图书馆"> 图书情报与数字图书馆</option>'+
									'<option value="档案及博物馆">档案及博物馆</option>'+
								  '</select>'+
							   ' </p>'+
							   ' <a class="icon-btn del-group"></a>'+
							  '</dd>');
				$(this).parent().append($ddList);
				$(this).parent().find("select").trigger("change");	   
			});
			
			//删除“专辑、专题”组
			$(".info-education").on("click",".del-group",function(){
				$(this).parent().remove();
				
				//序号重新赋值
				$(".info-education").find("dd").each(function(){
					_index = $(".info-education").find("dd").index($(this));
					$(this).find("em").html(_index+1);
				});
			});
			
			
			//select选择
			$(".info-education").on("change","select",function(event){
				newValue = $(this).val();
				$(this).siblings("label").html(newValue);
			});
			
		},
		
		
		//检索历史
		searchHistory:function(){
			//检索历史设置
			$(".setup-list").find("a.icon-btn").click(function(){
				$.on_off($(this));
			});
			
			
		},
		
	
		//“我的成果”→成果推送切换、手动添加切换
		Switches:function(switchSelector, btnSelector, nr, curClass){
			$(btnSelector).find("li").click(function(){
				var _index = $(this).parent().find("li").index($(this));
				
				$(this).addClass(curClass).siblings().removeClass(curClass);
				$(this).parents(switchSelector).find(nr).hide().eq(_index).show();
			});
		},
				
		
		//推送条件
		pushTerm:function(){
			//编辑
			$(".push-term").find(".edit").click(function(){
				$(".push-term").addClass("onediting");
				$(".push-term").find(".term").prop("readonly",false);
			});
			
			//文献类型
			$(".push-term").find(".typelist a").click(function(){
				$(this).parent().remove();
			});
			
			//发表日期
			$(".simulate").on('change',"select",function(event){
				newValue = $(this).val();
				$(this).siblings().html(newValue);
			});
			
			//保存
			$(".push-term").find(".save").click(function(){
				$(".push-term").removeClass("onediting");
				$(".push-term").find(".term").prop("readonly",true);
			});
			
			//刷新
			
			//取消
			
		},
		
		//引文跟踪
		citationTrack:function(){	
			
			//编辑
			$(".citation-tracking").find(".edit").click(function(){
				$(this).parents(".set").addClass("onediting");
				$(this).parents(".set").find(".useremail").prop("readonly",false);
			});

			//保存
			$(".citation-tracking .set").find(".save").click(function(){
				var $setContainer = $(this).parents(".set");
				
				//保存select设置
				$setContainer.find("label").each(function(){
					val = $(this).siblings("select").val();
					$(this).html(val);
				});
				
				$setContainer.removeClass("onediting");
				$setContainer.find(".useremail").prop("readonly",true);
			});

			//跟踪状态
			$(".citation-tracking").on('click',".icon-btn",function(){
				$.on_off($(this));
			});
			
		},
		
		
		//on off开关
		on_off:function(obj){
			obj.toggleClass("switch-on switch-off");
			
			if(obj.hasClass("switch-on")){
				obj.text("开启");
			}else{
				obj.text("关闭");
			}
		},
		
		
		//收藏按钮变色
		collectIcon:function(){
			$(".funcs-btn").find(".collect a").click(function(){
				$(this).toggleClass("on");
				if($(this).hasClass("on")){
					$(this).attr("title","取消收藏");
				}else{
					$(this).attr("title","收藏");
				}
			});
		},
		
		
		
		//我的云端
		myCloud:function(){
			
			//升降排序
			$(".tabfile").find("th a").click(function(){
				if( $(this).hasClass("ascend") || $(this).hasClass('') ){
					$(".tabfile").find("th a").removeClass();
					$(this).addClass("descend");
					return false;
				}else {
					$(this).removeClass().addClass("ascend");
					return false;
				}
	
			});
			
			
			
		},
		
		
		//工具栏→复选框
		//NodeObj： 与工具栏.toolbar 并列的列表盒子
		checkBoxFn:function(NodeObj){
			
			//全选/全不选
			$(NodeObj).siblings(".toolbar").on("click",".checkall",function(event){
				event.preventDefault;
				$tablist = $(this).parents(".toolbar").siblings(NodeObj);
				$inputlist = $tablist.find("input[type='checkbox']");
				
				if( $(this).prop("checked") ){
					$inputlist.prop("checked",true);
					len = $inputlist.length;
					$(this).siblings("label").find("em").html(len);
				}else{
					$inputlist.prop("checked",false);
					$(this).siblings("label").find("em").html(0);
				}
			});
			
			//清除
			$(NodeObj).siblings(".toolbar").on("click",".checkclear",function(event){
				event.preventDefault;
				$tablist = $(this).parents(".toolbar").siblings(NodeObj);
				$inputlist = $tablist.find("input[type='checkbox']");
				$inputlist.prop("checked",false);
				//清除全选按钮状态
				$(this).siblings(".checkall").prop("checked",false);
				
				$(this).siblings("label").find("em").html(0);
			});
			
			//子选择框
			$(NodeObj).find("input[type='checkbox']").on("click",function(event){
				event.preventDefault;
				$toolbar = $(this).parents(NodeObj).siblings(".toolbar");
				$inputlist = $(this).parents(NodeObj).find("input[type='checkbox']");
				len = $inputlist.length;
				count = parseInt($toolbar.find("label em").html());

				if($(this).prop("checked")){
					count++;
					if(count==len){
						$toolbar.find(".checkall").prop("checked",true);//选中全选按钮
					}
				}else{
					count--;
					$toolbar.find(".checkall").prop("checked",false);//清除全选按钮状态
				}
				$toolbar.find("label em").html(count);

			});
		},

		groupFilter:function () {//侧边栏 点击进行筛选

			$(".sidetags-list").find("dd a").click(function () {
				sideTagsList = $(this).parents(".side-b").find(".sidetags-list");
				sideTagsList.find("dd a").removeClass("cur");
				$(this).addClass("cur");
			});
		},


		delConfirm:function () {//删除确认提示框

			$delTemplet = $('<div class="del-pop"></div>')
						.html('<a class="close" title="关闭"><i class="icon iconfont"></i></a>'+
							'<p>您确定要删除此项？</p>'+
							'<div class="del-btns">'+
								'<a class="sure">是</a>'+
								'<a class="no">否</a>'+
							'</div>'
			);
			$("body").append($delTemplet);

			$delTemplet.popCenter({
				clearPop : true,//关闭时是否需要清除popup框
				btns : [".sure",".no"]//弹出框上的两个按钮“是”，“否”
			});

		}
			


	});
		
})(jQuery);

