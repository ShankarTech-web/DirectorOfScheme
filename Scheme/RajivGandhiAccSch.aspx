<%@ Page Title="Application Form" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RajivGandhiAccSch.aspx.cs" Inherits="DirectorOfScheme.Scheme.RajivGandhiAccSch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="container-fluid mt-2">
        <div class="row text-center">
            <div class="col-md-6">
                <label class="fw-bold">School Udise Number:</label>
                <asp:Label ID="lblUdise" runat="server" CssClass="form-control-plaintext text-danger"></asp:Label><br />
            </div>
            <div class="col-md-6">
                <label class="fw-bold">School Name:</label>
                <asp:Label ID="lblSchoolName" runat="server" CssClass="form-control-plaintext text-danger"></asp:Label><br />
                </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
                <h4>Student Accident Application Form</h4>

            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-4">
                <label class="fw-bold">Student Full Name:</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Placeholder="Full Name"></asp:TextBox><br />

            </div>
            <div class="col-md-4">
                 <label class="fw-bold">Student Age:</label>
                <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Placeholder="Age"></asp:TextBox><br />

      </div>
            <div class="col-md-4">
                   <label class="fw-bold">Student Standard:</label>
                <asp:TextBox ID="txtStandard" runat="server" CssClass="form-control" Placeholder="Standard (1-12)"></asp:TextBox><br />

            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-md-1"></div>
            <div class="col-md-5">
                  <label class="fw-bold">Student District:</label>
                <asp:TextBox ID="txtSchool" runat="server" CssClass="form-control" Placeholder="District Name"></asp:TextBox><br />

            </div>
            <div class="col-md-5">
                 <label class="fw-bold">Student Taulka:</label>
                <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control" Placeholder="Taluka"></asp:TextBox><br />

            </div>
            <div class="col-md-1"></div>
        </div>

        <div class="row">
            <div class="col-md-5 mx-auto">
                <label class="fw-bold">Application Type:</label>
              <asp:DropDownList ID="ddlAccidentType" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlAccidentType_SelectedIndexChanged">
    <asp:ListItem Text="-- Select Accident Type --" Value=""></asp:ListItem>
    <asp:ListItem Text="Accidental Death" Value="Death"></asp:ListItem>
    <asp:ListItem Text="Permanent Disability - Two Organs/Eyes" Value="DisabilityFull"></asp:ListItem>
    <asp:ListItem Text="Permanent Disability - One Organ/Eye" Value="DisabilityPartial"></asp:ListItem>
    <asp:ListItem Text="Surgery Due to Accident" Value="Surgery"></asp:ListItem>
    <asp:ListItem Text="Injury (Sports, Fire, Electric Shock etc)" Value="Injury"></asp:ListItem>
</asp:DropDownList><br /><br />
            </div>
        </div>
        </div>







<asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
    <Columns>
        <asp:BoundField DataField="DocumentName" HeaderText="Required Document" />
        <asp:TemplateField HeaderText="Upload">
            <ItemTemplate>
                <asp:FileUpload ID="fuDoc" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:Button ID="btnSubmit" runat="server" Text="Submit Application" CssClass="btn btn-success" OnClick="btnSubmit_Click" />
<asp:Label ID="lblMessage" runat="server" CssClass="mt-2 d-block"></asp:Label>
<asp:Button ID="btnBack" runat="server" Text="Back to Dashboard" CssClass="btn btn-danger mt-2" OnClick="btnBack_Click" />
</asp:Content>