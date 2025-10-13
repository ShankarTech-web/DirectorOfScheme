<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="DirectorOfScheme.Homepage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<!-- ================== PAGE STYLES ================== -->
<style>
  /* --- Full-width Announcement Bar --- */
  .announcement-bar {
      position: relative;
      width: 100vw; /* Full viewport width */
      margin-left: calc(-50vw + 50%); /* Cancels container padding */
      background: linear-gradient(90deg, #a2f0df, #cae1df);
      color: #212529;
      font-weight: 600;
      padding: 10px 0;
      font-size: 1rem;
      text-align: center;
      border-bottom: 2px solid #7ee9da;
      z-index: 1000;
  }

  .announcement-bar marquee {
      font-weight: 600;
      color: #000000;
  }

  /* --- Rounded Image --- */
  .rounded-image {
      width: 250px;
      height: 250px;
      border-radius: 50%;
      object-fit: cover;
      border: 3px solid #ddd;
      box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }

  /* --- Hero Section --- */
  .hero {
      padding: 2.5rem 1rem;
      display: flex;
      flex-wrap: wrap;
      align-items: center;
      justify-content: space-between;
      gap: 1.5rem;
  }

  .hero-left {
      flex: 1 1 360px;
  }

  .hero-right {
      flex: 1 1 320px;
      text-align: center;
  }

  h1 {
      margin: 0 0 .5rem;
      font-size: 1.9rem;
      font-weight: 700;
      color: #0d6efd;
  }

  p.lead {
      margin: 0 0 1rem;
      color: #444;
      font-size: 1.1rem;
  }

  .btn-primary {
      display: inline-flex;
      align-items: center;
      gap: .6rem;
      padding: .55rem .9rem;
      border-radius: 8px;
      background: #0d6efd;
      color: #fff;
      text-decoration: none;
      border: none;
      cursor: pointer;
      transition: background 0.3s ease;
  }

  .btn-primary:hover {
      background: #0b5ed7;
  }

  footer {
      margin-top: 2rem;
      padding: 1rem 0;
      color: #666;
      font-size: .9rem;
      text-align: center;
  }

  @media (max-width:640px) {
      h1 { font-size: 1.5rem; }
      .hero { padding: 1.2rem; text-align: center; }
      .hero-left, .hero-right { flex: 1 1 100%; }
  }

  .list-group-item img {
      margin-left: 8px;
      height: 18px;
  }
</style>

<!-- ================== ANNOUNCEMENT (FULL WIDTH) ================== -->
<div class="announcement-bar mt-2">
  <marquee behavior="scroll" direction="left" scrollamount="5" onmouseover="this.stop();" onmouseout="this.start();">
    📢 <strong>Announcements:</strong> राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना अर्ज सुरू आहे. | 🎓 राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना अर्ज ३१.१०.२०२५ ला समाप्त होणार आहे.  | 📝 राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना. 
  </marquee>
</div>

<!-- ================== LIVE UPDATES ================== -->
<div class="card shadow-sm mt-2">
  <div class="card-body">
    <div class="container-fluid">
      <div class="row">
        <div class="col-md-12">
          <h4 class="fw-bold text-primary">
            <img src="Images/Homepage/LIVE.svg" height="40" alt="Live Icon" /> Live Updates
          </h4>
          <hr class="border border-primary border-1 opacity-25" style="width: 10%">
          <ul class="list-group list-group-flush">
            <li class="list-group-item">🗓️ राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना अर्ज ३१.१०.२०२५ ला समाप्त होणार आहे.<img src="Images/Homepage/blinking_new.gif" alt="new" /></li>
            <li class="list-group-item">📢 राजीव गांधी विद्यार्थी अपघात सानुग्रह अनुदान योजना अर्ज सुरू आहे.<img src="Images/Homepage/blinking_new.gif" alt="new" /></li>
            <%--<li class="list-group-item">💼 Parent login access is now enabled for class-wise result view.<img src="Images/Homepage/blinking_new.gif" alt="new" /></li>--%>
          </ul>
        </div>
      </div>
    </div>
  </div>
</div>

<!-- ================== HOME ICON SECTION ================== -->
<%--<div class="container-fluid mt-4">
  <div class="row text-center gy-3">
    <div class="col-md-4">
      <img src="Images/HomePage/BONE.svg" alt="Content Image One" height="150" width="150" />
    </div>
    <div class="col-md-4">
      <img src="Images/HomePage/BTWO.svg" alt="Content Image Two" height="150" width="150" />
    </div>
    <div class="col-md-4">
      <img src="Images/HomePage/BTHREE.svg" alt="Content Image Three" height="150" width="150" />
    </div>
  </div>
    </div>--%>

  <div class="row text-center mt-3">
    <div class="col-md-10 mx-auto">
      <div class="fs-5 fw-bold text-dark">
        <span class="fw-bolder">“Schemes for Students, Strength for the Nation.”</span>
      </div>
    </div>
  </div>


<!-- ================== HERO SECTION ================== -->
<div class="container mt-4">
  <section class="hero" aria-labelledby="hero-title">
    <div class="hero-left">
      <h1 id="hero-title">Welcome to Director of Education (Scheme) Portal</h1>
    <%--  <p class="lead">Centralized dashboard for scheme management. Access login below.</p>

      <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/School/SchoolLogin.aspx" CssClass="btn-primary">
        <svg width="18" height="18" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path d="M10 17l5-5-5-5v10zM4 19h2V5H4v14z" fill="currentColor" />
        </svg>
        Login
      </asp:HyperLink>--%>
    </div>

    <div class="hero-right">
      <img class="rounded-image" src="Images/honDirector.jpeg" alt="Director of Education (Scheme)" />
      <div style="margin-top: .8rem; line-height: 1.6;">
        <b>श्री. कृष्णकुमार पाटील</b><br />
        संचालक<br />
        शिक्षण संचालनालय (योजना),<br />
        महाराष्ट्र राज्य.
      </div>
    </div>
  </section>

  <footer>
    <small>Beta Version 0.9.7 | &copy; Director of Education (Scheme)</small>
  </footer>
</div>

<!-- ================== CLOCK SCRIPT ================== -->
<script>
    function tickClock() {
        const el = document.getElementById('clock');
        if (!el) return;
        const now = new Date();
        el.textContent = now.toLocaleTimeString();
    }
    setInterval(tickClock, 1000);
    tickClock();
</script>

</asp:Content>
