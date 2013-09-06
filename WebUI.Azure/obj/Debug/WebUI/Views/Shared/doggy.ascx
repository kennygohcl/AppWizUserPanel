<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="doggy">
</div>
<div id="tip">
    <div id="tipcontent">
    hi, click on the message to show more tips; click on me to hide
    </div>
</div>
<%
    var c = ViewContext.RouteData.Values["Controller"].ToString().ToLower();
    var a = ViewContext.RouteData.Values["Action"].ToString().ToLower();
%>
<script type="text/javascript">
    jQuery(function () {
        var x = Math.random() * 8000;
        if (getCookie("showdoggy") != "false") {
            setTimeout("showTip()", x + 5000);
        }
        setTimeout("doSomething()", x);

        jQuery('#doggy').draggable({
            drag: function () {
                var dl = parseFloat(jQuery('#doggy').css('left'));
                var dt = parseFloat(jQuery('#doggy').css('top'));
                jQuery('#tip').css('left', (dl - 130) + "px").css('top', (dt - 100) + 'px');
            }
        });
        jQuery('#tip').click(showTip);
        jQuery('#doggy').click(function () {
            if (!jQuery('#tip').is(':visible')) {
                setCookie("showdoggy", true, 10);
                jQuery('#tip').fadeIn();
            }
            else {
                setCookie("showdoggy", false, 10);
                jQuery('#tip').fadeOut();
            }
        });
    });
    function doSomething() {
        if(jQuery('#tip').is(':visible')) showTip();
        setTimeout('doSomething()', Math.random() * 25000 + 5000);
    }
    function showTip() {
        jQuery.post('<%=Url.Action("tell","doggy") %>',
        { c: '<%=c %>', a: '<%=a %>' },
        function (d) {
            jQuery('#tipcontent').html(d.o);
            jQuery('#tip').show();            
        });
    }
</script>

