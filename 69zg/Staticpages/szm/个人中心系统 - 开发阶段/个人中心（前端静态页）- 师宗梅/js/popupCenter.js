/*
  start 20160606
  名称: 弹出框
  功能：出现弹出框，使之垂直左右居中，创建半透明蒙版；
       点击关闭按钮、或弹出框外的空白区域,移除蒙版,移除/隐藏弹出框；
*/
jQuery.fn.popCenter = function(options) {

	var params = $.extend({
		clearPop: true,//是否清除此弹出框，清除true,隐藏false；
		btns :[],//弹出框上（除了关闭按钮）的功能按钮；
	    closeBtn : ".close"//关闭按钮
	},options);

	var obj = this;
	loaded = true;
	
	$mask = $('<div class="pop-mask"></div>');
	obj.after($mask);
	
	_Width = parseInt(obj.outerWidth());
	_Height = parseInt(obj.outerHeight());

	$mask.css({
		'width'  : $(document).width(),
		'height' : $(document.body).outerHeight(true)
	});
	obj.show();

	//计算obj的位置
	_calcPosition();


	function _calcPosition(){
		leftPos= parseInt(($(window).width() / 2) - (_Width / 2));
		topPos = parseInt(($(window).height() / 2) - (_Height / 2));

		if ($(window).width() < _Width) { leftPos = 0 ; }
		if ($(window).height() < _Height) { topPos = 0 ; }

		obj.css({
			'left': leftPos + $(window).scrollLeft()+'px',
			'top' : topPos + $(window).scrollTop()+'px'
		});
	}


	function initPop(obj){//弹出框初始化
		if(params.clearPop){
			obj.remove();
		}else{
			obj.hide();
		}
		$mask.remove();
		loaded = false;
		return false;
	}
	
	
	//点击空白处、添加、关闭按钮，移除弹出窗口
	$(document).click(function(event){
		ev = event||window.event;
		$tar = $(ev.target);
		if( $tar.closest($mask).length == 1 || $tar.closest(params.closeBtn).length == 1){//点击X或弹出框外空白区域

			initPop(obj);//关闭弹出框
		}
		for(var i in params.btns){
			if( $tar.closest(params.btns[i]).length == 1){

				initPop(obj);//关闭弹出框
			}
		}
	});

	$(window).on("resize scroll",function(){//缩放时
		if(loaded){
			//重新计算obj的位置
			_calcPosition();
		}
		return;
	});

    //如果自动获取高度，ie7下,第一次点击获取的高度292,正常
	//第二次点击获取的高度892,多出600px;
	//为兼容ie7，此语句为obj 高度重新赋值；
	// obj.outerHeight(_Height);

}
/*弹出框 end*/