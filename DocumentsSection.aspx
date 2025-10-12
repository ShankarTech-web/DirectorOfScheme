<%@ Page Title="Rules & Circular's" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DocumentsSection.aspx.cs" Inherits="DirectorOfScheme.DocumentsSection" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="conatiner-fluid mt-2">
        <div class="row">
            <div class="col-md-10 mx-auto">
               <div class="card">
                   <div class="card-body">
                       <p class="fw-bold text-danger fs-5">शासन निर्णय</p>
                       <hr />
                       <a href="GR's/01Oct2013GR.pdf"  class="link-primary link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover"> 
                         इयत्ता १ली ते १२वी पर्यंत शिकणा-या मुला/मुलींना अपघातामुळे क्षतीची नुकसान भरपाई ईत्यादी 
                           देण्याबाबत "राजीव गांधी विदयार्थी अपघात सानुग्रह अनुदान योजना " 
                           सन २०१२-१३ पासून नियमित स्वरुपात राबविण्याबाबत.(दिनांक:०१ ऑक्टोबर, २०१३)
                       </a>
                       <hr class="border border-danger border-1 opacity-50">
                        <a href="GR's/21June2022GR.pdf"  class="link-primary link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover"> 
                       इयत्ता १ ली ते १२ वी पर्यंत शिकणा-या विद्यार्थ्यांसाठीच्या राजीव गांधी विदयार्थी अपघात 
                            सानुग्रह अनुदान योजनेत सुधारणा करणेबाबत.(दिनांक:२१ जून, २०२२)
                        </a>
                        <hr class="border border-danger border-1 opacity-50">
  <a href="GR's/06Feb2024GR.pdf"  class="link-primary link-offset-2 link-underline-opacity-25 link-underline-opacity-100-hover"> 
               इयत्ता १ ली ते इयत्ता १२ वी पर्यंत शिकणाऱ्या विद्यार्थ्यांसाठीच्या राजीव गांधी विद्यार्थी 
      अपघात सानुग्रह अनुदान योजनेत सुधारणा करणेबाबत.(दिनांक:०६ फेब्रुवार, २०२४)
      </a>
                   </div>
               </div>
                </div>
            </div>
        </div>
    <asp:Button ID="btnBack"  runat="server" Text="Back to Home" CssClass="btn btn-primary mt-2" OnClick="btnBack_Click" />
</asp:Content>
