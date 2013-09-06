<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (HttpContext.Current.User.Identity.IsAuthenticated)
   { %>
    <%=Html.ActionLink(Mui.LogOut, "SignOff","account") %>
<%}
   else
   {%>
<%=Html.ActionLink(Mui.LogIn, "SignIn","account") %>
<%}%>
