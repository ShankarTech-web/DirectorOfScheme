using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.Scheme
{
    public partial class TrackApplication : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Optional: check if school session exists
                if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
                {
                    Response.Redirect("../School/SchoolLogin.aspx");
                }
            }
        }

        protected void btnTrack_Click(object sender, EventArgs e)
        {
            string appID = txtTrackID.Text.Trim();
            if (string.IsNullOrEmpty(appID))
            {
                lblTrackMessage.Text = "⚠️ Please enter Application ID.";
                lblTrackMessage.ForeColor = System.Drawing.Color.Red;
                gvTrackDetails.DataSource = null;
                gvTrackDetails.DataBind();
                return;
            }

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
                    SELECT s.FullName, s.Standard, a.AccidentType, a.ApplicationID, a.AccidentDate,
                           CASE WHEN a.IsEligible = 1 THEN 'Approved' ELSE 'Pending' END AS Status
                    FROM Students s
                    INNER JOIN AccidentDetails a ON s.StudentID = a.StudentID
                    WHERE a.ApplicationID = @ApplicationID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ApplicationID", appID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    gvTrackDetails.DataSource = dt;
                    gvTrackDetails.DataBind();
                    lblTrackMessage.Text = $"🔍 Details for Application ID: {appID}";
                    lblTrackMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    gvTrackDetails.DataSource = null;
                    gvTrackDetails.DataBind();
                    lblTrackMessage.Text = $"❌ No application found with ID: {appID}";
                    lblTrackMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}
