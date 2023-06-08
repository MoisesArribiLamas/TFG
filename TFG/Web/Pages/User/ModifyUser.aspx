<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyUser.aspx.cs" Inherits="Es.Udc.DotNet.TFG.Web.Pages.User.ModifyUser" MasterPageFile="~/TFG.Master" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="contentModify" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <div id="form" style="height: 290px">
        <div>
            <span>
                <asp:HyperLink ID="lnkChangePasswordModifyUser" runat="server"
                    NavigateUrl="~/Pages/User/ChangePassword.aspx"
                    Text="Cambiar contraseña" meta:resourcekey="lnkChangePasswordModifyUserResource1" />

            </span>
        </div>
        <div>
            <span>
                <asp:HyperLink ID="HyperLink2ModifyUser" runat="server"
                    NavigateUrl="~/Pages/User/VisorTarjetas.aspx"
                    Text="Gestionar tarjetas" meta:resourcekey="HyperLink2ModifyUserResource1" />
            </span>
        </div>

        <form id="form1" runat="server">
            <div class="field">
                <span class="label">
                    <asp:Localize ID="Localize1ModifyUser" runat="server" Text="Mail" meta:resourcekey="Localize1ModifyUserResource1"></asp:Localize>
                </span>
                <asp:TextBox ID="BoxUserMailModifyUser" runat="server" meta:resourcekey="BoxUserMailModifyUserResource1"></asp:TextBox>

            </div>

            <div class="field">
                <span class="label">

                    <asp:Localize ID="Localize3ModifyUser" runat="server" Text="Nombre" meta:resourcekey="Localize3ModifyUserResource1"></asp:Localize>


                </span>
                <asp:TextBox ID="BoxNombreModModifyUser" runat="server" meta:resourcekey="BoxNombreModModifyUserResource1"></asp:TextBox>

            </div>


            <div class="field">
                <span class="label">

                    <asp:Localize ID="Localize4ModifyUser" runat="server" Text="Apellidos" meta:resourcekey="Localize4ModifyUserResource1"></asp:Localize>




                </span>
                <asp:TextBox ID="BoxApellidosModModifyUser" runat="server" meta:resourcekey="BoxApellidosModModifyUserResource1"></asp:TextBox>

            </div>

            <div class="field">
                <span class="label">

                    <asp:Localize ID="Localize5ModifyUser" runat="server" Text="Codigo Postal" meta:resourcekey="Localize5ModifyUserResource1"></asp:Localize>


                </span>

                <asp:TextBox ID="BoxCPModModifyUser" runat="server" meta:resourcekey="BoxCPModModifyUserResource1"></asp:TextBox>
            
            </div>
            <div class="field">
                <span class="label">

                    <asp:Localize ID="Localize6ModifyUser" runat="server" Text="Pais" meta:resourcekey="Localize6ModifyUserResource1"></asp:Localize>


                </span>
                <asp:DropDownList ID="ListaPaisesModModifyUser" runat="server" AutoPostBack="True"
                    Width="100px" OnSelectedIndexChanged="ListaPaises_SelectedIndexChanged" meta:resourcekey="ListaPaisesModModifyUserResource1">
                </asp:DropDownList>

            </div>
            <div class="field">
                <span class="label">

                    <asp:Localize ID="Localize7ModifyUser" runat="server" Text="Idioma" meta:resourcekey="Localize7ModifyUserResource1"></asp:Localize>


                </span>
                <asp:DropDownList ID="ListaIdiomasModModifyUser" runat="server" AutoPostBack="True"
                    Width="100px" OnSelectedIndexChanged="ListaIdiomas_SelectedIndexChanged" meta:resourcekey="ListaIdiomasModModifyUserResource1">
                </asp:DropDownList>

            </div>

            <div class="button">



                <asp:Button ID="btModificarModifyUser" runat="server" Text="Modificar" OnClick="btModificar_Click" meta:resourcekey="btModificarModifyUserResource1" />



            </div>
        </form>




    </div>


</asp:Content>
