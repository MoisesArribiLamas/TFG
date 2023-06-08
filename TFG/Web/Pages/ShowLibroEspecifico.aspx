<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowLibroEspecifico.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.ShowLibroEspecifico" MasterPageFile="~/PracticaMaD.Master"%> %>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server" Visible="true">
    <form runat="server">




        
        <div id="infoGeneral">
            <div >
             <span class="labelTittle">
                    <asp:Localize ID="Localize1" runat="server"  Text="Producto: " />
                 </span>
                <span class="label">
                    <asp:TextBox ID="boxNombreProd" runat="server" >    </asp:TextBox>  

                </span>
            </div>
            <div >
             <span class="labelTittle">
                    <asp:Localize ID="Localize3" runat="server"  Text="Precio: " />
                 </span>
                <span class="label">
                    <asp:TextBox ID="boxPrecio" runat="server" 
            ></asp:TextBox>  
                 </span>
            </div>

              </div>
           

          
        
        <div id="infoEspecifica" runat="server"> 
 
        </div>
      
             
      <div class="button">
            <asp:Button ID="btModificar" runat="server" OnClick="btModificar_Click" Text="Modificar"  OnClientClick="BtModificarOnClientClick"   /> 
          </div>
         
        
    <br />



             </form>

</asp:Content>