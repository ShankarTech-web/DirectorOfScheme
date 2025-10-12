<%@ Page Title="School Profile" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SchoolProfile.aspx.cs" Inherits="DirectorOfScheme.School.SchoolProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid mt-2">
        <div class="row">
            <div class="col-md-12">
                <h2>School Profile</h2>
                <hr />
            </div>
        </div>

        <!-- School Info -->
        <div class="row mb-3">
            <div class="col-md-4">
                <label class="fw-bold">School UDISE:</label>
                <asp:Label ID="lbSchoolUdise" runat="server" CssClass="form-control-plaintext"></asp:Label>
            </div>
            <div class="col-md-4">
                <label class="fw-bold">School Name:</label>
                <asp:Label ID="lbSchoolName" runat="server" CssClass="form-control-plaintext"></asp:Label>
            </div>
            <div class="col-md-4">
                <label class="fw-bold">Profile Status:</label>
                <br />
                <asp:Label ID="lbProfileStatus" runat="server" CssClass="badge bg-warning text-dark"></asp:Label>
            </div>
        </div>

        <!-- Principal Info Section -->
        <asp:Panel ID="pnlPrincipalInfo" runat="server" Visible="false" CssClass="card p-3 shadow-sm">
            <h5>Principal Information (Required)</h5>

            <div class="row mb-3">
                <div class="col-md-4">
                    <label>Principal Name:</label>
                    <asp:TextBox ID="txtPrincipalName" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrincipalName" runat="server" ControlToValidate="txtPrincipalName"
                        ErrorMessage="Principal Name is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>

                <div class="col-md-4">
                    <label>Mobile:</label>
                    <asp:TextBox ID="txtPrincipalMobile" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrincipalMobile" runat="server" ControlToValidate="txtPrincipalMobile"
                        ErrorMessage="Mobile number is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtPrincipalMobile"
                        ValidationExpression="^[6-9]\d{9}$" ErrorMessage="Enter valid 10-digit mobile number" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>

                <div class="col-md-4">
                    <label>Email:</label>
                    <asp:TextBox ID="txtPrincipalEmail" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPrincipalEmail" runat="server" ControlToValidate="txtPrincipalEmail"
                        ErrorMessage="Email is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtPrincipalEmail"
                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter valid email address" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label>Qualification:</label>
                    <asp:TextBox ID="txtPrincipalQualification" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvQualification" runat="server" ControlToValidate="txtPrincipalQualification"
                        ErrorMessage="Qualification is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>

                <div class="col-md-6">
                    <label>Date of Birth:</label>
                    <asp:TextBox ID="txtPrincipalDOB" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="txtPrincipalDOB"
                        ErrorMessage="Date of Birth is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label>Start Date:</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txtStartDate"
                        ErrorMessage="Start Date is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>

                <div class="col-md-6">
                    <label>End Date:</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="txtEndDate"
                        ErrorMessage="End Date is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-12">
                    <label>Address:</label>
                    <asp:TextBox ID="txtPrincipalAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtPrincipalAddress"
                        ErrorMessage="Address is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="text-end">
                <asp:Button ID="btnSavePrincipal" runat="server" Text="Save Principal Info"
                    CssClass="btn btn-primary" OnClick="btnSavePrincipal_Click" />
                </div>
        </asp:Panel>
        <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard"
    CssClass="btn btn-primary ms-2" OnClick="btnBack_Click" CausesValidation="False" />
            
    </div>
</asp:Content>
