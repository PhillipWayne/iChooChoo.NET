<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ZM.iChooChoo.Client.Web.settings.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
		<article class="article page">
            <asp:Literal runat="server" ID="litErrorAddressType" Visible="false"><p class="invalid"><strong>Error:</strong> Please select an address other than 0x77 and a valid type!</p></asp:Literal>
            <asp:Literal runat="server" ID="litErrorDescription" Visible="false"><p class="invalid"><strong>Error:</strong> Please enter a valid description (at least 1 and at most 14 characters, no white space).</p></asp:Literal>
            <asp:Literal runat="server" ID="litErrorGeneric" Visible="false"><p class="invalid"><strong>Error:</strong> An error occured when applying changes. Please try again or contact support if the error persists.</p></asp:Literal>

			<p>This page lists all modules detected by the iChooChoo server. You can change description of modules and define address and type of new
			module. You can also soft-reset or hard-reset a module.</p>
			<p>Use rescan button at the bottom if you think this list is not up-to-date.</p>
			<p><strong>Caution:</strong> don't hard-reset a module if there is already a new module detected (address 0x77). This may cause two modules have
			the same 0x77 address and you won't be able to configure it unless you physically unconnect the module from the bus.</p>

            <table>
				<tr>
					<th>Address</th>
					<th>Version</th>
					<th>Type</th>
					<th>Description</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
				</tr>
        <asp:Repeater runat="server" ID="rpt" OnItemDataBound="rpt_ItemDataBound" OnItemCommand="rpt_ItemCommand">
            <ItemTemplate>
                <tr runat="server" id="tr">
                    <td><%# Eval("ID", "0x{0:X2}") %></td>
                    <td><asp:literal runat="server" ID="litVersion" Text='<%# Eval("Version") %>' /></td>
                    <td><asp:literal runat="server" ID="litType" Text='<%# Eval("TypeFullDescription") %>' /></td>
                    <td><asp:TextBox runat="server" ID="txtDescription" Text='<%# Eval("Description") %>' MaxLength="14" /></td>
                    <td><asp:Button runat="server" ID="btnSave" Text="Save" CommandName="SV" CommandArgument='<%# Eval("ID") %>' /></td>
                    <td><asp:HyperLink runat="server" ID="hypTest" CssClass="button buttongreen" Text="Test" NavigateUrl='<%# string.Format("~/settings/moduletest{0:X2}.aspx?addr={1}", Eval("Type"), Eval("ID")) %>' /></td>
                    <td><asp:Button runat="server" ID="btnSoftReset" Text="Soft-Reset" CommandName="SR" CommandArgument='<%# Eval("ID") %>' /></td>
                    <td><asp:Button runat="server" ID="btnHardReset" Text="Hard-Reset" CommandName="HR" CommandArgument='<%# Eval("ID") %>' /></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:PlaceHolder runat="server" ID="plhNewModule" Visible="false">
                <tr>
                    <td><asp:DropDownList runat="server" ID="lstNewAddress" /></td>
                    <td><asp:literal runat="server" ID="litNewVersion" /></td>
                    <td><asp:DropDownList runat="server" ID="lstNewType" /></td>
                    <td><%= ZM.iChooChoo.Library.ICCConstants.GetTypeDescription(ZM.iChooChoo.Library.ICCConstants.BICCP_GRP_NEW) %></td>
                    <td><asp:Button runat="server" ID="btnNewSave" Text="Save" OnClick="btnNewSave_Click" /></td>
                    <td></td>
                    <td><asp:Button runat="server" ID="btnNewSoftReset" Text="Soft-Reset" OnClick="btnNewSoftReset_Click" /></td>
                    <td><asp:Button runat="server" ID="btnNewHardReset" Text="Hard-Reset" OnClick="btnNewHardReset_Click" /></td>
                </tr>
        </asp:PlaceHolder>
            </table>
            <asp:Literal runat="server" ID="lit" />

			<p class="center"><asp:Button runat="server" ID="btnRescan" Text="Rescan the bus" OnClick="btnRescan_Click" /></p>
		</article>

</asp:Content>
