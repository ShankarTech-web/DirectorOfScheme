using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DirectorOfScheme
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UpdateLoginLogoutLink();
            }
        }
        private void UpdateLoginLogoutLink()
        {
            // Check if user is logged in (example: store username in Session["User"])
            if (Session["User"] != null)
            {
                lnkLoginLogout.Text = "&#160लॉगआउट";
                lnkLoginLogout.CommandArgument = "Logout";
            }
            else
            {
                lnkLoginLogout.Text = "&#160लॉगिन";
                lnkLoginLogout.CommandArgument = "Login";
            }
        }

        protected void lnkLoginLogout_Click(object sender, EventArgs e)
        {
            string action = ((System.Web.UI.WebControls.LinkButton)sender).CommandArgument;

            if (action == "Logout")
            {
                // Clear session and redirect to login page
                Session.Clear();
                Session.Abandon();
                Response.Redirect("/School/SchoolLogin.aspx");
            }
            else
            {
                // Redirect to login page
                Response.Redirect("/School/SchoolLogin.aspx");
            }
        }
    }
}

