// JavaScript Document
/*我的收藏-论文*/
/*
  说明：该栏目交互较多，采用了面向对象的写法，将单独的功能集合成一个文件
  注： “创建新标签”后或“删除侧边栏标签”后，判断滚动条是否需要显示隐藏
*/

$(function() {

	/*调用*/
	new myPaperFn();

});

function myPaperFn() {
	this.myTags = $(".mytags");
	this.addTagGroup = $(".addtag-group");

	this.tagsPopup({  //主体内容 “+”事件
		allTagsArray: ['科学', '图书情报', '检索性能分析', '数字图书馆', '评价指标', '管理学研究', '科学1', '图书情报1', '检索性能分析1', '数字图书馆1', '评价指标1', '管理学研究1', '科学2', '图书情报2']
	});

	this.popCreat();//主体内容中“创建新标签”
	this.sidePopEdit();//侧边栏 标签“编辑”事件
	this.sidePopCreat();//侧边栏“创建新标签+”
	this.addBtns();//侧边栏 鼠标移入添加按钮事件
	this.delSurePop();//侧边栏标签 删除确认弹出框
}



myPaperFn.prototype = {
	sidePopEdit: function () { //侧边栏 标签“编辑”事件
		var that = this;
		that.myTags.on("click", ".edit", function () {
			html = $(this).parent().siblings("a").html();
			that._addPopup({
				title: "编辑标签",
				inputVal: html
			});
		});
	},

	sidePopCreat: function () { //侧边栏“创建新标签+”
		var that = this;
		that.myTags.on("click", ".creat-new", function () {
			that._addPopup({
				title: "创建新标签",
				inputVal: ""
			});

		});
	},
	popCreat: function () {//主体内容 “创建新标签”
		var that = this;
		that.addTagGroup.on("click", ".creat-new", function () {
			that._addPopup({
				title: "创建新标签",
				inputVal: ""
			});

		});
	},

	_addPopup: function (options) { //创建新标签、编辑标签弹出层事件
		var params = $.extend({
			title: "创建新标签",
			inputVal: ""
		}, options);

		$templet = $('<div class="creat-pop"></div>')
			.html('<a class="close" title="关闭"><i class="icon iconfont">&#xe62a;</i></a>' +
				'<h6><i class="icon iconfont">&#xe616;</i>' + params.title + '</h6>' +
				'<input type="text" maxLength="40" value="' + params.inputVal + '"/>' +
				'<input class="creat-btn" type="submit" value="创建" />'
			);

		$("body").append($templet);

		$templet.popCenter({
			clearPop: true,//清除true,隐藏false;
			btns: [".creat-btn"]//“创建”
		});

		$templet.find("input[type='text']").focus().val(params.inputVal);//移入光标
	},

	addBtns: function () {//侧边栏 鼠标移入添加按钮事件
		var that = this;
		that.myTags.find(".tags").off().on("mouseenter mouseleave", "dd", function (event) {
			event.preventDefault();

			$opts = $('<ul>' +
				'<li class="edit" title="编辑"><i class="icon iconfont">&#xe60e;</i></li>' +
				'<li class="del" title="删除"><i class="icon iconfont">&#xe62a;</i></li>' +
				'</ul>'
			);
			if (event.type == "mouseenter") {
				if ($(this).find("ul").length == 0) {
					$(this).find("a").after($opts);
				} else {
					$(this).find("ul").show();
				}
				return;
			} else if (event.type == "mouseleave") {
				$(this).find("ul").hide();
				return;
			}

		});
	},

	delSurePop :function () {//侧边栏标签 删除确认弹出框
		var that = this;
		that.myTags.find(".tags").on("click",".del",function(event){
			event.preventDefault();
			$.delConfirm();

		});
	},



	/*
	 名称：tagsPopup标签悬浮框
	 范围：“论文”  主体内容（中间）部分
	 功能： 鼠标移入“+”时，显示相应的标签悬浮框,添加/删除标签
	 参数说明： btn：  “+”按钮，触发标签悬浮框事件；
	          allTagsArray： 数组，用户的所有标签集合，即标签库；
	 */
	tagsPopup : function (options) {
		var that= this;
		var params = $.extend({
			btn : ".addtag-btn",
			allTagsArray : []
		},options);


		$(params.btn).mouseover(function(event){
			event.preventDefault();

			var oldTagBox = $('<div></div>')
				.addClass("taglist")
				.html('<h6><i class="icon iconfont">&#xe616;</i>添加标签</h6>'+
					'<ul></ul>'+
					'<input class="creat-new" type="submit" value="创建新标签" />'
				);


			tagsContainer = $(this).parent(); //$(".addtag-group")
			existTags = tagsContainer.find(".tagitem");
			$pop = $(this).siblings(".tagpopup");

			if($pop.length){
				$pop.show();

			}else{//新建标签弹出框
				$(this).after('<div class="tagpopup"></div>');
				$newPop = $(this).siblings(".tagpopup");
				$newPop.append(oldTagBox);


				for(var test in params.allTagsArray){

					//将获得的tag放入<li>内，依次存储到ul下
					var oddTag = $('<li></li>')
						.html(params.allTagsArray[test]);
					$newPop.find("ul").append(oddTag);


					if(existTags.length){
						//如果该标签已被添加，则从当前弹出框的标签库中隐藏
						//防止重复添加
						existTags.each(function(){

							var tagText = $(this).find("b").text();

							if( params.allTagsArray[test] == tagText ){
								$newPop.find("li").eq(test).hide();
								return false;
							}
						});

					}//if end
				}//for end
			}//else end

			//鼠标移开事件
			tagsContainer.mouseleave(function(event){
				event.preventDefault();
				$pop = $(this).find(".tagpopup");
				$pop.hide();
			});

		});	//mouseover end



		//点击主体悬浮框上的某个标签，可在相应条目下添加此“标签”
		that.addTagGroup.on('click','.tagpopup li',function(event){
			event.preventDefault();

			var Text =$(this).html();
			var $Item = $('<span class="tagitem"><b>' + Text + '</b><i class="icon-btn icon-del"></i></span>');

			btn = $(this).parents(".tagpopup").siblings(params.btn);
			btn.before($Item);

			//在弹出框中隐藏该标签
			$(this).hide();
		});


		//点击主体条目下某个标签右上角的X，可删除此标签
		that.addTagGroup.on("click",".icon-del",function(event){
			event.preventDefault();

			//将弹出框标签库中对应的标签显示
			var $Item = $(this).parent();
			var tagText = $Item.find("b").text();
			var $pop =$Item.siblings(".tagpopup");

			$pop.find("li").each(function(){
				if( $(this).html() == tagText ){
					$(this).show();

					return false;
				}
			});

			$Item.remove();
		});
	}

};