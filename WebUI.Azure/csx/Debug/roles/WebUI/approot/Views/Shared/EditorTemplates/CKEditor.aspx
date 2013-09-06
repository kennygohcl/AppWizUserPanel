<%@ Page Language="C#" MasterPageFile="Template.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Data" runat="server">
    <%= Html.TextArea("", Model) %>
    <script type="text/javascript">
        $(function () {
            if (CKEDITOR.instances["<%=ViewData.TemplateInfo.GetFullHtmlFieldId(string.Empty)%>"]) {
                CKEDITOR.remove(CKEDITOR.instances["<%=ViewData.TemplateInfo.GetFullHtmlFieldId(string.Empty)%>"]);
            }
            $("#<%=ViewData.TemplateInfo.GetFullHtmlFieldId(string.Empty)%>").closest('form').submit(CKupdate);
            CKEDITOR.replace("<%=ViewData.TemplateInfo.GetFullHtmlFieldId(string.Empty)%>");
        });
	</script>
</asp:Content>