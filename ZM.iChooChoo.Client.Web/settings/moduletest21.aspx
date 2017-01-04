<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="moduletest21.aspx.cs" Inherits="ZM.iChooChoo.Client.Web.settings.moduletest21" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

		<article class="article page">
			<h1><asp:Literal runat="server" ID="litTitle" /></h1>
			<p>&nbsp;</p>
			<p>This page lets you test the module via basic commands.</p>
            <asp:Literal runat="server" ID="litErrorGeneric" Visible="false"><p class="invalid"><strong>Error:</strong> An error occured when applying changes. Please try again or contact support if the error persists.</p></asp:Literal>

            <table>
				<tr>
					<th>Output</th>
					<th class="statuscol">Status</th>
					<th>&nbsp;</th>
				</tr>
            <asp:Repeater runat="server" ID="rpt" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand">
                <ItemTemplate>
                <tr>
                    <td><asp:Literal runat="server" ID="litOutID" /></td>
                    <td class="statuscol"><asp:Label runat="server" ID="lblStatus" /></td>
                    <td><asp:Button runat="server" ID="btnToggle" /></td>
                </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater runat="server" ID="rptDim" OnItemDataBound="rptDim_ItemDataBound" OnItemCommand="rptDim_ItemCommand">
                <ItemTemplate>
                <asp:Literal runat="server" ID="litScript" />
				<input type="hidden" id="value<%# string.Format("{0:X}", Container.DataItem) %>" name="value<%# string.Format("{0:X}", Container.DataItem) %>">
                <tr>
                    <td><asp:Literal runat="server" ID="litOutID" /></td>
					<td class="statuscol">
						<div id="slider<%# string.Format("{0:X}", Container.DataItem) %>"><div id="custom-handle<%# string.Format("{0:X}", Container.DataItem) %>" class="custom-handle ui-slider-handle"></div></div>
					</td>
                    <td><asp:Button runat="server" ID="btnToggle" Text='<%# string.Format("Set value {0:X}", Container.DataItem) %>' /></td>
                </tr>
                </ItemTemplate>
            </asp:Repeater>
            </table>

		</article>

</asp:Content>
