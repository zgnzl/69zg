

/*
  名称：侧边栏 滚动条
  说明：不是所有栏目的页面都有滚动条，因此在文件init.js中调用会有错误提示。暂时放到该文件中调用，可根据每个栏目需要自行调用
*/
$(function(){
	$(".scroll-list").scrollBars();
});



/*
  start: 20160906
  名称：侧边栏滚动条
  有滚动条的栏目：论文、下载历史、浏览历史（文献、出版来源）、定制检索式、我的成果、我的云端
  标签高度超出（104px,即4条)时出现滚动条
  出现导航条的分类有：标签、学科、资源类型、作者、机构、基金
*/

$.fn.scrollBars = function(){
	
	maxListH = $(this).find("dd").height() * 4;

	this.each(function(){
		obj = $(this);
	
		var controller = false;
		var $Tags = obj.find(".tags");
		var $Bar = obj.siblings(".scroll-bar");
		var $Handle =  obj.siblings().find(".handle");		
		
		//滑块移动的最大范围
        maxY = $Bar.height() - $Handle.height();
		
		$Handle.mousedown(function(e) {
			e.preventDefault();
			controller = true;
			
			$(document).mousemove(function(e) {
				e.preventDefault();
				positionY = Calc(parseInt(e.pageY - $Bar.offset().top - $Handle.height()/2));
				
				if (controller) {	
					$Handle.css("top", positionY);
					$Tags.css("top",-positionY / maxY * ( $Tags.height() - maxListH ) );
				}
			});
			 
			$(document).mouseup(function(e) {
				e.preventDefault();
				controller = false;
			});
		});
		
		//计算滑块位置
		function Calc(value) {
			if( value < 0 ){
				return 0;
			}else if( value > maxY ){
				return maxY;
			}else
				return value;
		}	


	});//each end
	
}