<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="WebApplication1.search" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
   <title>
       <%# TextBox1.Text%>
       WikiSearch</title>
   <link href='http://fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css' />
   <link href="http://fonts.googleapis.com/css?family=Ubuntu" rel="stylesheet" type="text/css" />
   <link href='css/bootstrap.min.css' rel="Stylesheet" type='text/css' />
   <link href='css/bootstrap-theme.min.css' rel="Stylesheet" type='text/css' />

   <style type="text/css">
       .Header
       {
           text-shadow: 1px 0px 5px #aaa;
       }
       .linkstyle
       {
           text-decoration: none;
       }
       .linkstyle:hover
       {
           text-decoration: underline;
       }
       .linkDiv
       {
           /*display: inline-block;*/
           max-width:70%;
       }
       .linkDiv:hover
       {
           /*background: linear-gradient(to right,#e6e6e6, white); */
           background:#f0f0f0;
           cursor: pointer;
           border-radius:5px;
       }
       .linkDivChild
       {
           padding: 10px 10px 10px 5px;
       }
       ::selection {color:white;background:gray;}
       ::-moz-selection {color:white;background:gray;}
   </style>
</head>
<body>
   
   <form id="form1" runat="server" style="height: 39px; width: 907px;">
   <div style="float: left; margin-left: 50px">
       <br />
       <asp:Label ID="Label1" runat="server" CssClass="Header" Font-Names="Open Sans" Font-Size="X-Large" Font-Bold="true"></asp:Label>
       <br />
       <br />
   </div>
   <div style="margin-left: 50px; float: left; width: 100%;margin-right:auto">
       <div class="col-lg-6" style="padding-left:0;">
           <div class="input-group" style="width:100%">
               <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Font-Names="Open Sans"></asp:TextBox>
               <span class="input-group-btn">
                   <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btn btn-default" Font-Names="Open Sans" BackColor="#ebebebe" Font-Bold="true" Text="GO" ForeColor="#7d7d7d"/>
               </span>
           </div>
           <!-- /input-group -->
       </div>
   <!-- /.col-lg-6 -->
       <div style="padding-top:40px;padding-bottom:10px">
           <asp:Label ID="Label2" runat="server" Font-Names="Ubuntu" Font-Bold="false"></asp:Label>
       </div>
   </div>
   <br />
   <br />
   <div style="margin-left: 45px; float: left; width: 747px;">
       <%--<asp:Label ID="Label3" Text='test me' Font-Bold='True' runat="server"
              Font-Names="Ubuntu"></asp:Label><br />--%>
       <asp:Repeater ID="repLinks" runat="server">
           <ItemTemplate>
               <div class='linkDiv' onclick="window.open('<%# ((KeyValuePair<string,string>)Container.DataItem).Value %>','_blank');">
                   <div class='linkDivChild'>
                       <strong><asp:HyperLink ID="HyperLink1" runat="server"  NavigateUrl="<%# ((KeyValuePair<string,string>)Container.DataItem).Value %>" Text="<%# ((KeyValuePair<string,string>)Container.DataItem).Key %>"
                           Font-Bold='true' Font-Size='Large' ForeColor='Blue' Font-Names="Open Sans" CssClass="linkstyle" /></strong>
                       <br />
                       <asp:Label ID="LinkLabel" runat="server" Text="<%# ((KeyValuePair<string,string>)Container.DataItem).Value %>"
                           Font-Size='small' ForeColor='ForestGreen' Font-Names="Open Sans"></asp:Label>
                   </div>
               </div>
           </ItemTemplate>
       </asp:Repeater>
       <br />
       <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" CssClass="linkstyle"
           Font-Names="Open Sans, sans serif" Font-Size="Medium" ForeColor="#0066FF" Font-Underline="False"
           OnClick="LinkButton1_Click">&nbsp;&laquo; Prev&nbsp;&nbsp;&nbsp;&nbsp;</asp:LinkButton>
       <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Open Sans, sans serif"
           Font-Size='Medium' ForeColor="#0066FF" Font-Underline="False" OnClick="LinkButton2_Click">&nbsp;Next &raquo;</asp:LinkButton>
       <br />
       <br />
       <br />
   </div>
   </form>
</body>
</html>