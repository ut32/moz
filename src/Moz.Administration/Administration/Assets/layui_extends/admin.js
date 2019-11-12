layui.define(['layer', 'form','element','jquery'], function(exports){
    var layer = layui.layer
        ,form = layui.form
        ,element = layui.element
        ,$ = layui.jquery;


    var AjaxManager = function () {
        var _this = this;
        _this.ajax = function (url,method,data,sucFunc,failFunc) {
            var options = {
                url:url,
                type:method,
                cache:false,
                data:data,
                timeout:10000,
                success:function (result,status,xhr) {
                    if (status!=="success"){
                        if($.isFunction(failFunc)){
                            failFunc("json",{ Code:888, Message:"发生未知错误，请联系管理员"});
                        }else{
                            layer.alert("发生未知错误，请联系管理员", {icon: 5});
                        }
                        return;
                    }
                    var ct = xhr.getResponseHeader("content-type") || "";
                    if (ct.indexOf('html') > -1) {
                        if ($.isFunction(sucFunc)){
                            sucFunc("html",result);
                        }
                    }else if (ct.indexOf('json') > -1) {
                        if (result.Code>0){
                            if (result.Code===401) {
                                location.href = "/manage";
                            }else if(result.Code===403){
                                layer.alert("权限不足", {icon: 5});
                            }else {
                                if($.isFunction(failFunc)){
                                    failFunc("json",{ Code:result.Code, Message:result.Message});
                                }else{
                                    layer.alert(result.Message, {icon: 5});
                                }
                            }
                        }else{
                            if ($.isFunction(sucFunc)) {
                                sucFunc("json", result);
                            }
                        }
                    }
                },
                error:function (xhr,status,error) {
                    if($.isFunction(failFunc)){
                        failFunc("json",{ Code:xhr.status||500, Message:error || "XMLHttpRequest Error"});
                    }else{
                        if (xhr.status===403){
                            layer.alert("没有相关权限", {icon: 5});
                        } else{
                            layer.alert(error, {icon: 5});
                        }
                    }
                }
            };
            $.ajax(options);
        };
    };
    var NavTabManager = function(){
        var _this = this;
        _this.currentLayId = "";
        _this.clearTimer = 0;
        _this.left = 0;

        _this.refresh = function (layId) {
            if (layId){
                getContent(layId,function (html) {
                    var $item = $("#zui-admin-content .item[page-id='"+layId+"']");
                    if ($item.size()===1){
                        $item.remove();
                        var $html = $("<div class='item' page-id='"+layId+"'>"+html+"</div>");
                        $html.appendTo($("#zui-admin-content"));
                        initUI($html);
                    }
                });
            }
        };
        _this.create = function(layId,title){
            if (layId){
                getContent(layId,function (html) {
                    if ($("#zui-admin-content .item[page-id='"+layId+"']").size()===0){
                        $("#zui-admin-content .item").hide();
                        var $html = $("<div class='item' page-id='"+layId+"'>"+html+"</div>");
                        $html.appendTo($("#zui-admin-content"));
                        initUI($html);
                    }
                    element.tabAdd('zui-admin-header-tabs', {
                        title: title
                        ,content: ''
                        ,id: layId
                    });
                    element.tabChange('zui-admin-header-tabs', layId);
                    _this.ajdustTabLeft();
                });
            }
        };
        _this.closeCurrentTab = function(){
            if(_this.currentLayId==="home/welcome") return;
            if (_this.currentLayId) {
                element.tabDelete('zui-admin-header-tabs', _this.currentLayId);
                _this.clear();
                _this.ajdustTabLeft();
            }
        };
        _this.closeOtherTabs = function(){
            var curLayId = _this.currentLayId;
            var allTabs = $("#zui-admin-header-tabs .layui-tab-title li[lay-id]");
            allTabs.each(function (i, v) {
                var $tab = $(v);
                var layId = $tab.attr("lay-id");
                if (layId && layId!=="home/welcome" && layId!==curLayId) {
                    element.tabDelete('zui-admin-header-tabs', layId);
                }
            });
            _this.clear();
            _this.ajdustTabLeft();
        };
        _this.closeAllTabs = function(){
            var allTabs = $("#zui-admin-header-tabs .layui-tab-title li[lay-id]");
            allTabs.each(function (i, v) {
                var $tab = $(v);
                var layId = $tab.attr("lay-id");
                if (layId && layId!=="home/welcome") {
                    element.tabDelete('zui-admin-header-tabs', layId);
                }
            });
            _this.clear();
            _this.ajdustTabLeft();
        };
        _this.refreshCurrentTab = function(){
            _this.refresh(_this.currentLayId);
        };
        _this.select = function (layId) {
            if (layId){
                _this.currentLayId = layId;
                $("#zui-admin-leftside .layui-nav dd").removeClass("layui-this");
                $("#zui-admin-leftside .layui-nav dd a[zui-href='"+_this.currentLayId+"']").parent().addClass("layui-this");
                $("#zui-admin-content .item").hide();
                $("#zui-admin-content .item[page-id='"+_this.currentLayId+"']").show();
            }
            //_this.clear();
        };
        _this.clear = function () {
            if (_this.clearTimer){
                window.clearTimeout(_this.clearTimer);
            }
            _this.clearTimer = setTimeout(function () {
                var allTabs = $("#zui-admin-header-tabs .layui-tab-title li[lay-id]");
                var allContents = $("#zui-admin-content .item");
                allContents.each(function (i,v) {
                    var $content = $(v);
                    var isMatch = allTabs.filter("[lay-id='"+$content.attr("page-id")+"']").size()>0;
                    if (!isMatch){
                        $content.remove();
                    }
                });
                window.clearTimeout(_this.clearTimer);
            },500);
        };
        _this.gotoLeft = function () {
            var $ul = $("#zui-admin-header-tabs .layui-tab ul.layui-tab-title");
            var tabWidth = getTabWidth();
            var ulWidth = getUlWidth();
            if (ulWidth>tabWidth){
                _this.left = _this.left +300;
                if (_this.left>0) _this.left=0;
                $ul.css({"left":_this.left});
            }else{
                _this.left = 0;
                $ul.css({"left":_this.left});
            }
        };
        _this.gotoRight = function () {
            var $ul = $("#zui-admin-header-tabs .layui-tab ul.layui-tab-title");
            var tabWidth = getTabWidth();
            var ulWidth = getUlWidth();
            if (ulWidth>tabWidth){
                _this.left = _this.left - 300;
                if (_this.left < (tabWidth - ulWidth)) {
                    _this.left = tabWidth - ulWidth;
                }
                $ul.css({"left":_this.left});
            }
        };
        var getTabWidth = function () {
            return $("#zui-admin-header-tabs .layui-tab").width()|0;
        };
        var getUlWidth = function () {
            var $lis = $("#zui-admin-header-tabs .layui-tab ul.layui-tab-title li");
            return eval($lis.toArray().map(function (v) {
                return $(v).outerWidth() | 0;
            }).join('+'));
        };
        _this.ajdustTabLeft = function () {
            var $ul = $("#zui-admin-header-tabs .layui-tab ul.layui-tab-title");
            var tabWidth = getTabWidth();
            var ulWidth = getUlWidth();
            if (tabWidth>ulWidth) {
                _this.left = 0;
                $ul.css({"left":_this.left});
            }else{
                _this.left = tabWidth - ulWidth;
                $ul.css({"left":_this.left});
            }
        };
        var getContent = function (layId,func) {
            ajaxManager.ajax(layId,"GET",{},function (type, result) {
                if ($.isFunction(func)){
                    if (type==="html"){
                        func(result);
                    }
                }
            });
        };
    };
    var DialogManager = function () {
        var _this = this;
        var _zindex = 1000;
        _this.create = function(url,title,width,height){
            if (url){
                var newTitle = title || "标题";
                _zindex = _zindex + 5;
                ajaxManager.ajax(url,"GET",{},function (type, result) {
                    if (type==="html"){
                        layer.open({
                            type: 1,
                            title:newTitle,
                            area: [width, height],
                            maxmin: true,
                            zIndex :_zindex,
                            content: result,
                            success: function(layero, index){
                                initUI(layero);
                            }
                        });
                    }
                });
            }
        };
    };
    var initUI = function($p){
        $("textarea.editor", $p).each(function () {
            if (CKEDITOR)
                CKEDITOR.replace( this );
        });
    };

    var navTabManager = new NavTabManager();
    var ajaxManager = new AjaxManager();
    var dialogManager = new DialogManager();
    
    var isFullScreen = false;
    
    var obj = {
        init:function () {
            //ajax 相关
            $(document).ajaxStart(function(){
                NProgress.start();
            }).ajaxStop(function(){
                NProgress.done();
            });

            /**
             * -----------------
             * 全局监听
             * -----------------
             **/

            $(document).on("click","[zui-type]",function (event) {

                event.preventDefault();

                var $this = $(this);
                var type = $this.attr("zui-type");
                var href = $this.attr("zui-href");
                var title = $this.attr("title") || $this.text() || "无标题";
                if (type === "dialog"){
                    var width = $this.attr("zui-width") || "800px";
                    var height = $this.attr("zui-height") || "500px";
                    if (href){
                        dialogManager.create(href,title,width,height);
                    }
                    return false;
                }else if (type==="tab"){
                    if (href) {
                        var count = $("#zui-admin-header-tabs [lay-id='"+href+"']").size();
                        if (count>0){
                            element.tabChange('zui-admin-header-tabs', href);
                        }else{
                            navTabManager.create(href,title);
                        }
                    }
                    return false;
                }else if(type==="ajax"){
                    var fun = function () {
                        ajaxManager.ajax(href,"POST",{},function (type, result) {
                            navTabManager.refreshCurrentTab();
                        });
                    };
                    var confirm = $this.attr("zui-confirm");
                    if(confirm){
                        layer.confirm(confirm, {icon: 3, title:'提示'}, function(index){
                            fun();
                            layer.close(index);
                        });
                    }else{
                        fun();
                    }

                }
                return true;
            });


            $(document).on("submit","form",function (event) {

                event.preventDefault();

                var $_form = $(this);
                var url = $_form.attr("action");
                if (url){
                    var next = function ($form) {
                        if(CKEDITOR){
                            for ( instance in CKEDITOR.instances )
                            {
                                CKEDITOR.instances[instance].updateElement();
                            }
                        }

                        $form.find("button[lay-submit]").addClass("loading");

                        ajaxManager.ajax(url,"POST",$form.serializeArray(),function (type,result) {
                            $form.find("button[lay-submit]").removeClass("loading");
                            if (type==="json"){
                                if (result.Code === 0){
                                    layer.alert(result.Message || "操作成功", {icon: 1});
                                }
                                var onSuccessCallBack = $form.attr("onSuccessCallBack");
                                if (onSuccessCallBack) {
                                    if($.isFunction(window[onSuccessCallBack]))
                                        window[onSuccessCallBack](result);
                                }else{
                                    navTabManager.refreshCurrentTab();
                                }
                            }
                        },function (type,result) {
                            $form.find("button[lay-submit]").removeClass("loading");
                            layer.alert(result.Message || "操作失败", {icon: 5});
                        });
                    };

                    var onBeforePost = $_form.attr("onBeforePost");
                    if (onBeforePost){
                        window[onBeforePost](next,$_form);
                    }else{
                        next($_form);
                    }
                }
            });
            

            /**
             * -----------------
             * tab模块监听
             * -----------------
             **/

            //tab标签按钮监听
            element.on("tab(zui-admin-header-tabs)",function (data) {
                var layId = $(this).attr('lay-id');
                if (layId){
                    navTabManager.select(layId);
                }
            });
            element.on('tabDelete(zui-admin-header-tabs)', function(data){
                var layId = $(this).parent().attr("lay-id");
                if (layId){
                    $("#zui-admin-content .item[page-id='"+layId+"']").remove();
                    navTabManager.ajdustTabLeft();
                }
            });

            //tab刷新
            $(document).on("click","a[zui-event='refresh']",function (event) {
                var $ls = $(this);
                if($ls.hasClass("refreshtag")){
                    $ls.removeClass("refreshtag");
                    $(this).find(".refreshicon").css({'transform': 'rotate(0deg)'});
                }else{
                    $ls.addClass("refreshtag");
                    $(this).find(".refreshicon").css({'transform': 'rotate(360deg)'});
                }
                navTabManager.refreshCurrentTab();
            });

            
            $(document).on("click","a[zui-event='fullscreen']",function (event) {
                if(isFullScreen){
                    isFullScreen = false;
                    if (document.exitFullscreen) {
                        document.exitFullscreen();
                    } else if (document.msExitFullscreen) {
                        document.msExitFullscreen();
                    } else if (document.mozCancelFullScreen) {
                        document.mozCancelFullScreen();
                    } else if (document.webkitExitFullscreen) {
                        document.webkitExitFullscreen();
                    }
                }else{
                    isFullScreen = true;
                    var element = document.documentElement;
                    if (element.requestFullscreen) {
                        element.requestFullscreen();
                    } else if (element.msRequestFullscreen) {
                        element.msRequestFullscreen();
                    } else if (element.mozRequestFullScreen) {
                        element.mozRequestFullScreen();
                    } else if (element.webkitRequestFullscreen) {
                        element.webkitRequestFullscreen();
                    }
                }
            });

            //关闭当前tab监听
            $(document).on("click","#zui-admin-header-tabs dd[zui-event='closeThisTab']",function (event) {
                navTabManager.closeCurrentTab();
            });
            $(document).on("click","#zui-admin-header-tabs dd[zui-event='closeOtherTabs']",function (event) {
                navTabManager.closeOtherTabs();
            });
            $(document).on("click","#zui-admin-header-tabs dd[zui-event='closeAllTabs']",function (event) {
                navTabManager.closeAllTabs();
            });

            //tab滑动 leftPage
            $(document).on("click","#zui-admin-header-tabs div[zui-event='leftPage']",function (event) {
                navTabManager.gotoLeft();
            });
            $(document).on("click","#zui-admin-header-tabs div[zui-event='rightPage']",function (event) {
                navTabManager.gotoRight();
            });


            /**
             * -----------------
             * 左侧收缩栏监听
             * -----------------
             **/
            //侧边伸缩监听
            $(document).on("click","a[zui-event='flexible']",function (event) {
                var $ls = $("#zui-admin-leftside");
                if($ls.hasClass("flexible")){
                    $ls.removeClass("flexible");
                    $(this).find(".flexibleicon").css({'transform': 'rotate(0deg)'});
                }else{
                    $ls.addClass("flexible");
                    $(this).find(".flexibleicon").css({'transform': 'rotate(90deg)'});
                }
            });
        },
        
        ajax:function (url,data,sucFunc,failFunc) {
            ajaxManager.ajax(url,"POST",data,function (type, result) {
                if (type==="json"){
                    if($.isFunction(sucFunc)){
                        sucFunc(result)
                    }
                }
            },function (type, result) {
                if($.isFunction(failFunc)){
                    failFunc(result)
                }
            });
        },
        
        onSwitch:function (url,data,obj,form,filter,sucFunc,failFunc) {
            var callback = function(data) {
                var result = obj.elem.checked;
                if (data.Code !== 0) {
                    if ($.isFunction(failFunc)){
                        failFunc(data);
                    }else{
                        if (data.Code === 403) {
                            layer.alert("没有相关权限", {icon: 5});
                        } else {
                            layer.alert(data.Message, {icon: 5});
                        }
                        result = !result;
                        $(obj.othis).parent().find("input").prop("checked", result);
                        form.render(null, filter);
                    }
                } else {
                    if ($.isFunction(sucFunc)) {
                        sucFunc(data);
                    }else{
                        layer.msg("设置成功");
                    }
                }
            };
            ajaxManager.ajax(url, "POST", data, function (type, result) {
                if (type === "json"){
                    callback(result);
                }else{
                    layer.alert("解析 json 出错", {icon: 5});
                }
            },function (type, result) {
                callback(result);
            });
        },
        
        onCellEdit:function (url,data,obj,sucFunc,failFunc) {
            var callback = function(data) {
                if (data.Code !== 0) {
                    if ($.isFunction(failFunc)){
                        failFunc(data);
                    }else{
                        if (data.Code === 403) {
                            layer.alert("没有相关权限", {icon: 5});
                        } else {
                            layer.alert(data.Message, {icon: 5});
                        }
                    }
                } else {
                    if ($.isFunction(sucFunc)) {
                        sucFunc(data);
                    }else{
                        layer.msg("编辑成功");
                    }
                }
            };
            ajaxManager.ajax(url, "POST", data, function (type, result) {
                if (type === "json"){
                    callback(result);
                }else{
                    layer.alert("解析 json 出错", {icon: 5});
                }
            },function (type, result) {
                callback(result);
            });
        },
        
        formatDt:function () {
            
        }
    };
    
    exports('admin', obj);
});  