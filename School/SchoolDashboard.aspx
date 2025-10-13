<%@ Page Title="School Dashboard" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SchoolDashboard.aspx.cs" Inherits="DirectorOfScheme.School.SchoolDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <div class="container-fluid mt-3">
           <div class="row text-center">
               <div class="col-md-4">
                   <label class="badge text-bg-success">UDISE:</label>
                   <asp:Label ID="lbUdise" runat="server" CssClass="badge text-bg-success"></asp:Label>
               </div>
                <div class="col-md-4">
     <label class="badge text-bg-success">School Name:</label>
                    <asp:Label ID="lbSchoolName" runat="server" CssClass="badge text-bg-success"></asp:Label>
                    </div>
               <div class="col-md-4">
                   <asp:LinkButton ID="lnkSchoolProfile" runat="server" CssClass="badge text-bg-primary" OnClick="lnkSchoolProfile_Click">School Profile</asp:LinkButton> 
                   <asp:LinkButton ID="lnkLogout" runat="server" CssClass="badge text-bg-danger" OnClick="lnkLogout_Click">Logout</asp:LinkButton>
                   </div>
           </div>
            <hr class="border border-danger border-1 opacity-50">
        <!-- Dashboard Links -->
        <div class="row text-center mb-4">
                        
            <div class="col-md-4">
                <div class="card shadow-sm p-3">
                   <asp:LinkButton ID="lbSchoolProfile" runat="server" CssClass="text-decoration-none" OnClick="lbSchoolProfile_Click">
                        <h5 class="fw-bold"> School Profile</h5>
                        <p>शाळा प्रोफाइल व्यवस्थापन</p>
                        
             </asp:LinkButton>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card shadow-sm p-3">
                    <asp:LinkButton ID="lbRGScheme" runat="server" CssClass="text-decoration-none" OnClick="lbRGScheme_Click">
                        <h5 class="fw-bold">Apply Now</h5>
 <p>राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना</p>
                    </asp:LinkButton>
                  
                        
                  
                </div>
            </div>
                <div class="col-md-4">
    <div class="card shadow-sm p-3">
                           <asp:LinkButton ID="lbApplTrack" runat="server" CssClass="text-decoration-none" OnClick="lbApplTrack_Click">
                       <h5 class="fw-bold">Track Application's</h5>
<p>राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना अर्ज ट्रॅकिंग</p>
        </asp:LinkButton>
        </div>
                    </div>
          </div>
           </div>
    
</asp:Content>
