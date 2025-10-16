<%@ Page Title="Application Form" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RGSchemeApplicationForm.aspx.cs" Inherits="DirectorOfScheme.Scheme.RGSchemeApplicationForm" EnableEventValidation="true"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   

    <div class="container-fluid mt-2">
         <!-- Row One-->
        <div class="row">
            <div class="col-md-10 mx-auto text-center">
                <p class="fw-bold" style="font-family:'AMS Calligraphy 8'">
                    शासन निर्णय क्रमांक: संकीर्ण-२०२१/प्र.क्र.१५२/एसडी-६ दिनांक :- २१ जून, २०२२ चे सहपत्र.

                </p>
             <center> <hr style="width: 60%" class="border border-danger border-1 opacity-75"></center>
                <p class="fw-bold" style="font-family:'AMS Calligraphy 9'">
                    विवरण पत्र - अ 
                </p>
                <center><hr style="width: 20%" class="border border-primary border-1 opacity-75"></center>
            </div>
        </div>
        <!-- Row One-->
        <br />
        <!--Row Two-->
        <div class="row">
            <div class="col-md-12 mx-auto text-center">
                <label class="fw-bold">अर्ज क्रमांक:</label>
                 <asp:Label ID="lblApplicationID" runat="server" CssClass="form-control-plaintext text-danger text-decoration-underline fw-bolder"></asp:Label>
            </div>
        </div>
        <!--Row Two-->
        <br />
        <!--Row Three-->
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-5">
                <label class="fw-bold">
                    अर्जदाराचे संपूर्ण नाव:
                </label>
                <asp:TextBox ID="txtApplicantFullName" runat="server" CssClass="form-control" ToolTip=" अर्जदाराचे संपूर्ण नाव" placeholder="अर्जदाराचे संपूर्ण नाव" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvApplName" ControlToValidate="txtApplicantFullName" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-5">
    <label class="fw-bold">अर्जदाराशी असलेले नाते:</label>
    <asp:DropDownList ID="ddlApplicantRelation" runat="server" CssClass="form-select" ToolTip="अर्जदाराशी असलेले नाते">
        <asp:ListItem Text="निवडा" Value="0" />
        <asp:ListItem Text="स्वत:" Value="1" />
        <asp:ListItem Text="आई " Value="2" />
        <asp:ListItem Text="वडील" Value="3" />
        <asp:ListItem Text="भाऊ " Value="4" />
              <asp:ListItem Text="बहीण" Value="5" />
              <asp:ListItem Text="पालक " Value="6" />
    </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvApplRelation" runat="server" ControlToValidate="ddlApplicantRelation" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
</div>
 <div class="col-md-1"></div>
            </div>
            <!--Row Three-->
        <br />
        <!--Row Four-->
        <div class="row">
            <div class="col-md-4">
                <label class="fw-bold">अर्जदाराची जन्म तारीख:</label>
                <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" ToolTip="अर्जदाराचे जन्म तारीख"  TextMode="Date" OnTextChanged="txtDOB_TextChanged" AutoPostBack="true"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDoB" ControlToValidate="txtDOB" runat="server" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            <div class="col-md-4">
                <label class="fw-bold">वय:</label>
                <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Enabled="false" Visible="true" placeholder="वय" ToolTip="वय"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <label class="fw-bold">अर्जदाराची इयत्ता:</label>
               <asp:DropDownList ID="ddlStandard" runat="server" CssClass="form-select">
     <asp:ListItem Text="इयत्ता निवडा " Value="0"></asp:ListItem>
     <asp:ListItem Text="इ.१ ली" Value="1"></asp:ListItem>
     <asp:ListItem Text="इ.२ री" Value="2"></asp:ListItem>
     <asp:ListItem Text="इ.३ री" Value="3"></asp:ListItem>
     <asp:ListItem Text="इ.४ थी" Value="4"></asp:ListItem>
     <asp:ListItem Text="इ.५ वी" Value="5"></asp:ListItem>
     <asp:ListItem Text="इ.६ वी" Value="6"></asp:ListItem>
     <asp:ListItem Text="इ.७ वी" Value="7"></asp:ListItem>
     <asp:ListItem Text="इ.८ वी" Value="8"></asp:ListItem>
     <asp:ListItem Text="इ.९ वी" Value="9"></asp:ListItem>
     <asp:ListItem Text="इ.१० वी" Value="10"></asp:ListItem>
     <asp:ListItem Text="इ.११ वी" Value="11"></asp:ListItem>
     <asp:ListItem Text="इ.१२ वी" Value="12"></asp:ListItem>
 </asp:DropDownList>
 <asp:RequiredFieldValidator ID="rfvStandard" runat="server" ControlToValidate="ddlStandard" InitialValue="0" ErrorMessage="Standard is Required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
        
               
            </div>
        </div>
        
        <!--Row Four-->
        <br />
        <!--Row Five-->
        <div class="row">
            <div class="col-md-4">
                <label class="fw-bold">
                    अर्जदाराचे जिल्हा: 
                </label>
                  <asp:DropDownList ID="ddldist" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddldist_SelectedIndexChanged"></asp:DropDownList>
<asp:RequiredFieldValidator ID="rfvDDLDist" runat="server" ControlToValidate="ddldist" ForeColor="Red" ErrorMessage="* Required" Display="Dynamic" InitialValue="-1"></asp:RequiredFieldValidator>
       
            </div>
            <div class="col-md-4">
                <label class="fw-bold">अर्जदाराचे तालुका:</label>
 <asp:DropDownList ID="ddltaluka" runat="server" CssClass="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvTaluka" runat="server" ControlToValidate="ddltaluka" ForeColor="Red" ErrorMessage="* Required" Display="Dynamic" InitialValue="-1"></asp:RequiredFieldValidator>
            
            </div>
            <div class="col-md-4">
                <label class="fw-bold">अर्जदाराचे गाव:</label>
                <asp:TextBox ID="txtVillage" runat="server" CssClass="form-control" ToolTip="अर्जदाराचे गाव" placeholder="अर्जदाराचे गाव"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvVillage" runat="server" ControlToValidate="txtVillage" ForeColor="Red" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
        </div>
        <!--Row Five-->
        <br />
        <!--Row Six-->
        <div class="row">
            <div class="col-md-6 mx-auto">
                <label class="fw-bold">विद्यार्थ्यांचे शाळेचे नाव किंवा संस्थेचे नाव:</label>
                <asp:TextBox ID="txtSchoolName" runat="server" CssClass="form-control" ToolTip="विद्यार्थ्यांचे शाळेचे नाव किंवा संस्थेचे नाव" placeholder="विद्यार्थ्यांचे शाळेचे नाव किंवा संस्थेचे नाव" Enabled="false" Visible="true"></asp:TextBox>
            </div>
        </div>
        <!--Row Six-->
        <br />
        <!--Row Seven-->
        <div class="row">
            <div class="col-md-4">
                <label class="fw-bold">

                </label>
            </div>
              <div class="col-md-4">
      <label class="fw-bold"></label>
  </div>
              <div class="col-md-4">
      <label class="fw-bold"></label>
  </div>
        </div>
        <!--Row Seven-->

        </div>

     
    
</asp:Content>
