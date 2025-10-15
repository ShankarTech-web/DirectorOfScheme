<%@ Page Title="Application Form" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RajivGandhiAccSch.aspx.cs" Inherits="DirectorOfScheme.Scheme.RajivGandhiAccSch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid mt-2">

    <!-- School Details -->
    <div class="row text-center">
        <div class="col-md-6">
            <label class="fw-bold">School Udise Number:</label>
            <asp:Label ID="lblUdise" runat="server" CssClass="form-control-plaintext text-danger"></asp:Label>
        </div>
        <div class="col-md-6">
            <label class="fw-bold">School Name:</label>
            <asp:Label ID="lblSchoolName" runat="server" CssClass="form-control-plaintext text-danger"></asp:Label>
        </div>
    </div>

    <!-- Application ID -->
    <div class="row mt-2 text-center">
        <div class="col-md-12">
            <label class="fw-bold">Application ID:</label>
            <asp:Label ID="lblApplicationID" runat="server" CssClass="form-control-plaintext text-success"></asp:Label>
        </div>
    </div>

    <!-- Form Header -->
    <div class="row mt-2">
        <div class="col-md-12 text-center">
            <h4>Student Accident Application Form</h4>
        </div>
    </div>
    <hr />

    <!-- Student Details -->
    <div class="row">
        <div class="col-md-4">
            <label class="fw-bold">Student Full Name:</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" Placeholder="Full Name"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvName" runat="server"  ControlToValidate="txtFullName" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>

        <div class="col-md-4">
            <label class="fw-bold">Student Age:</label>
            <asp:TextBox ID="txtAge" runat="server" CssClass="form-control" Placeholder="Age"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAge" runat="server" ControlToValidate="txtAge" ErrorMessage="* Required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>

        <div class="col-md-4">
            <label class="fw-bold">Student Standard:</label>
            <asp:DropDownList ID="ddlStandard" runat="server" CssClass="form-select">
                <asp:ListItem Text="Select Standard" Value="0"></asp:ListItem>
                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="11" Value="11"></asp:ListItem>
                <asp:ListItem Text="12" Value="12"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvStandard" runat="server" ControlToValidate="ddlStandard" InitialValue="0" ErrorMessage="Standard is Required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
    </div>
    <hr />

    <!-- District / Taluka -->
    <div class="row">
        <div class="col-md-5">
            <label class="fw-bold">Student District:</label>
             <asp:DropDownList ID="ddldist" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddldist_SelectedIndexChanged"></asp:DropDownList>
           <asp:RequiredFieldValidator ID="rfvDDLDist" runat="server" ControlToValidate="ddldist" ForeColor="Red" ErrorMessage="* Required" Display="Dynamic" InitialValue="-1"></asp:RequiredFieldValidator>
        </div>
        <div class="col-md-5">
            <label class="fw-bold">Student Taluka:</label>
             <asp:DropDownList ID="ddltaluka" runat="server" CssClass="form-control"></asp:DropDownList>
            
        </div>
    </div>
    <hr />

    <!-- Accident Type -->
    <div class="row">
        <div class="col-md-5 mx-auto">
            <asp:DropDownList ID="ddlAccidentType" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlAccidentType_SelectedIndexChanged">
                <asp:ListItem Text="-- Select Accident Type --" Value=""></asp:ListItem>
                <asp:ListItem Text="Accidental Death" Value="Death"></asp:ListItem>
                <asp:ListItem Text="Permanent Disability - Two Organs/Eyes" Value="DisabilityFull"></asp:ListItem>
                <asp:ListItem Text="Permanent Disability - One Organ/Eye" Value="DisabilityPartial"></asp:ListItem>
                <asp:ListItem Text="Surgery Due to Accident" Value="Surgery"></asp:ListItem>
                <asp:ListItem Text="Injury (Sports, Fire, Electric Shock etc)" Value="Injury"></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvAccidentType" runat="server" ControlToValidate="ddlAccidentType"
                InitialValue="" ErrorMessage="* Please select Accident Type" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
    </div>

    <!-- Required Documents -->
    <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mt-2">
        <Columns>
            <asp:BoundField DataField="DocumentName" HeaderText="Required Document" />
            <asp:TemplateField HeaderText="Upload">
                <ItemTemplate>
                    <asp:FileUpload ID="fuDoc" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <!-- Submit / Back -->
    <asp:Button ID="btnSubmit" runat="server" Text="Submit Application" CssClass="btn btn-success mt-2" OnClick="btnSubmit_Click" />
    <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-2"></asp:Label>
    <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard" CssClass="btn btn-danger mt-2" OnClick="btnBack_Click" CausesValidation="false" />

    <!-- Application Tracking -->
    <hr />
    

</div>
</asp:Content>
