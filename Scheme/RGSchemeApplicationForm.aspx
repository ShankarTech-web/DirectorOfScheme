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
                <asp:TextBox ID="txtStandard" runat="server" CssClass="foem-control" ToolTip="अर्जदाराची इयत्ता" placeholder="अर्जदाराची इयत्ता"></asp:TextBox>
            </div>
        </div>
        <!--Row Four-->
        </div>

     
    
</asp:Content>
