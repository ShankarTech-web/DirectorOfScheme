<%@ Page Title="Login Page" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="DirectorOfScheme.LoginPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-4">
        <div class="row">
                <div class="col-md-10 mx-auto">
                        <p class="fw-bold fs-4 text-center" style="font-family: 'Poor Richard'">Login Page</p>
                <hr /> 
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-5">
                        <asp:Label ID="unamelbl" runat="server" CssClass="text-black fw-bold" Text="Username" onkeyup="capitalizeInput(this)"></asp:Label><span style="color: red">*</span>
                        <asp:TextBox ID="txtUserId" runat="server" CssClass="form-control" placeholder="Username" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="vlaidUserId" runat="server" ControlToValidate="txtUserId" ErrorMessage="Username is Required!" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-5">
                        <asp:Label ID="passlbl" runat="server" CssClass="text-black fw-bold" Text=" Password:"></asp:Label><span style="color: red">*</span>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Password" TextMode="Password" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is Required!" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-1"></div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-6 mx-auto">
                        <center>
                            <label class="fw-bold">Captcha Code:</label><span style="color: red">*</span>
                            <asp:Label ID="captchaCodeLabel" BorderStyle="Groove" BorderWidth="1" BackColor="WhiteSmoke" onkeydown="disableCopy(event)" oncut="return false" oncopy="return false" onpaste="return false" runat="server" CssClass="fw-bold text-center fs-3 form-label"></asp:Label>
                            <span onclick="refreshPage()" class="" onclientclick="refreshCaptcha(); return false;"><i class="fa-solid fa-rotate" style="color: blue"></i></span>
                        </center>
                        <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control" placeholder="Type Captcha" onkeyup="capitalizeInput(this)"></asp:TextBox>
                        <br />
                        <center>
                    </div>
                </div>
                <div class="row text-center">
                    <center>
                        <asp:Button ID="btniologin" runat="server" CssClass="btn btn-primary w-25" Text="Login" OnClick="btniologin_Click" />
                        <br />
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="fw-bold text-primary" CausesValidation="false" OnClick="btnBack_Click">Back to Home</asp:LinkButton>
                        <br/>
                    <asp:LinkButton ID="btnForgetPassword" runat="server" CssClass="fw-bold text-primary" CausesValidation="false" OnClick="btnForgetPassword_Click"><i class="fa-solid fa-lock"></i>Forgot Password</asp:LinkButton>
                </div>
            </div>

</asp:Content>
