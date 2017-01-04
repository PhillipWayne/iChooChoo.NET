<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="logview.aspx.cs" Inherits="ZM.iChooChoo.Client.Web.settings.logview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
		<article class="article page">
            <asp:Repeater runat="server" ID="rpt">
                <ItemTemplate>
                    <span><%# Container.DataItem.ToString() %></span><br />
                </ItemTemplate>
            </asp:Repeater>
        </article>
</asp:Content>
