<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuccesfulOperation.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.SuccesfulOperation" MasterPageFile="~/TFG.Master" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <br />
    <asp:Localize ID="lclSucccessSuccessfullOperation" runat="server" Text="Operación realizada con éxito" meta:resourcekey="lclSucccessSuccessfullOperationResource1" />
    <br />
    <br />
    <br />

    <div class="container">
        <div style="text-align:center;">
            <asp:Image ID="IS" runat="server" height="33%" width="12%" ImageUrl= "~/Img/successfull.jpg" />
        </div>

    </div>
    <br />
    <br />
    <br />
</asp:Content>