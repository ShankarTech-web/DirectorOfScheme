<%@ Page Title="DocumentsSection" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RGSchemeAppPartTwo.aspx.cs" Inherits="DirectorOfScheme.Scheme.RGSchemeAppPartTwo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-3">
    <h4 class="text-center fw-bold">भाग २ - आवश्यक कागदपत्रे</h4>
    <hr />

    <asp:Label ID="lblAppID" runat="server" CssClass="text-danger fw-bold"></asp:Label>
    <br /><br />

    <div class="row">
        <div class="col-md-6">
            <label class="fw-bold">वैद्यकीय प्रमाणपत्र (PDF, Max 5 MB)</label>
            <asp:FileUpload ID="fuMedicalCert" runat="server" CssClass="form-control" />
        </div>
        <div class="col-md-6">
            <label class="fw-bold">इतर आवश्यक कागदपत्रे:</label>
            <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="DocumentID" HeaderText="Doc ID" Visible="false" />
                    <asp:BoundField DataField="DocumentName" HeaderText="Document" />
                    <asp:TemplateField HeaderText="Upload File">
                        <ItemTemplate>
                            <asp:FileUpload ID="fuDocument" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <br />
    <div class="text-center">
        <asp:Button ID="btnUploadAll" runat="server" CssClass="btn btn-success" Text="Upload Documents"
            OnClick="btnUploadAll_Click" />
    </div>

    <br />
    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger fw-bold"></asp:Label>
</div>
</asp:Content>
