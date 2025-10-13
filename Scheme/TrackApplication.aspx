<%@ Page Title="Track Application" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TrackApplication.aspx.cs" Inherits="DirectorOfScheme.Scheme.TrackApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">
    <div class="row justify-content-center">
        <div class="col-md-6 text-center">
            <h4>Track Your Application</h4>
            <p>Enter your Application ID to view the status of your submission.</p>

            <asp:TextBox ID="txtTrackID" runat="server" CssClass="form-control mb-2" Placeholder="Enter Application ID"></asp:TextBox>
            <asp:Button ID="btnTrack" runat="server" CssClass="btn btn-primary mb-2" Text="Track Application" OnClick="btnTrack_Click" />

            <asp:Label ID="lblTrackMessage" runat="server" CssClass="d-block mt-2"></asp:Label>

            <asp:GridView ID="gvTrackDetails" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mt-2">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Student Name" />
                    <asp:BoundField DataField="Standard" HeaderText="Standard" />
                    <asp:BoundField DataField="AccidentType" HeaderText="Accident Type" />
                    <asp:BoundField DataField="ApplicationID" HeaderText="Application ID" />
                    <asp:BoundField DataField="AccidentDate" HeaderText="Date Submitted" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Status" HeaderText="Application Status" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

</asp:Content>
