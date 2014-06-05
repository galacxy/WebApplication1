<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="WebApplication1.search"  %>

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
       
        .full
        {
            margin:0;
            padding:0; 
            display:block;
            top:0;
            left:0;
            width:100%;
            height:100%;
            background:#fff;
            position:absolute;
        }
        
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
<body class="full">
   
   <form id="form1" runat="server" style="height: 100%; width: 100%;">
   <div style="float: left; margin-left: 50px">
       <br />
       <asp:Label ID="Label1" runat="server" CssClass="Header" Font-Names="Open Sans" Font-Size="X-Large" Font-Bold="true"></asp:Label>
       <br />
       <br />
   </div>
   <div style="margin-left: 50px; float: left; width: 600px;clear:both">
       <div class="col-lg-6" style="padding-left:0;width:100%">
           <div class="input-group" style="width:100%">
               <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Font-Names="Open Sans"></asp:TextBox>
               <span class="input-group-btn">
                   <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btn btn-default" Font-Names="Open Sans" BackColor="#ebebebe" Font-Bold="true" Text="GO" ForeColor="#7d7d7d"/>
               </span>
           </div>
           <!-- /input-group -->
       </div>
       <div style="padding-top:10px;padding-bottom:10px;clear:both">
                <asp:Label ID="Label3" runat="server" Font-Names="Ubuntu" ForeColor="Gray">Did you mean: </asp:Label>
                <asp:LinkButton ID="LinkButton4" runat="server" Font-Bold="true" Font-Italic="true" Font-Underline="true" Font-Names="Open Sans" ForeColor="DarkBlue" OnClick="LinkButton4_Click"></asp:LinkButton>
           <asp:Label ID="Label2" runat="server" Font-Names="Ubuntu" Font-Bold="false"></asp:Label>
       </div>
   </div>
<%--   <br />
   <br />--%>
   <div style="margin-left: 45px; float: left; width: 747px;">
       <%--<asp:Label ID="Label3" Text='test me' Font-Bold='True' runat="server"
              Font-Names="Ubuntu"></asp:Label><br />--%>
       <asp:Repeater ID="repLinks" runat="server">
           <ItemTemplate>
               <div class='linkDiv' onclick="window.open('<%# ((KeyValuePair<string,string>)Container.DataItem).Value %>','_blank');">
                   <div class='linkDivChild'>
                       <strong><asp:HyperLink ID="HyperLink1" runat="server"  Target="_blank" NavigateUrl="<%# ((KeyValuePair<string,string>)Container.DataItem).Value %>" Text="<%# ((KeyValuePair<string,string>)Container.DataItem).Key %>"
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