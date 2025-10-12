using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.School
{
    public partial class SchoolDashboard : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sessionToken = Session["AuthToken"] as string;
                string queryToken = Request.QueryString["token"];

                // ❌ If no token or mismatch → force login
                if (string.IsNullOrEmpty(sessionToken) || queryToken != sessionToken)
                {
                    Response.Redirect("SchoolLogin.aspx");
                    return;
                }

                // ✅ Now safe to load data
                lbUdise.Text = Session["SchoolCode"].ToString();
                lbSchoolName.Text = Session["schoolName"].ToString();
            }
        }


        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("SchoolLogin.aspx");
        }

        protected void lnkSchoolProfile_Click(object sender, EventArgs e)
        {
            if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
            {
                Response.Redirect("SchoolLogin.aspx");
                return;
            }

            string token = Session["AuthToken"].ToString();

            // ✅ pass token via query string
            Response.Redirect("SchoolProfile.aspx?token=" + token);

        }

        protected void lbRGScheme_Click(object sender, EventArgs e)
        {
            if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
            {
                Response.Redirect("SchoolLogin.aspx");
                return;
            }
            string token = Session["AuthToken"].ToString();
            // ✅ pass token via query string
            Response.Redirect("../Scheme/RajivGandhiAccSch.aspx?token=" + token);
        }

        protected void lbSchoolProfile_Click(object sender, EventArgs e)
        {
            if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
            {
                Response.Redirect("SchoolLogin.aspx");
                return;
            }

            string token = Session["AuthToken"].ToString();

            // ✅ pass token via query string
            Response.Redirect("SchoolProfile.aspx?token=" + token);
        }
    }
}