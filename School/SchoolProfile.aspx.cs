using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DirectorOfScheme.School
{
    public partial class SchoolProfile : System.Web.UI.Page
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
                lbSchoolUdise.Text = Session["SchoolCode"].ToString();
                lbSchoolName.Text = Session["schoolName"].ToString();
            }
            LoadProfileStatus();
        }

        private void LoadProfileStatus()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT ProfileStatus FROM SchoolInformation WHERE SchoolCode=@SchoolCode";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SchoolCode", Session["SchoolCode"].ToString());

                con.Open();
                object status = cmd.ExecuteScalar();
                con.Close();

                if (status != null)
                {
                    lbProfileStatus.Text = status.ToString();
                    pnlPrincipalInfo.Visible = status.ToString().Equals("Incomplete", StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        private void LoadPrincipalInfo()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"SELECT TOP 1 PrincipalName, PrincipalMobile, PrincipalEmail, 
                                 PrincipalAddress, PrincipalDOB, PrincipalQualification, 
                                 StartDate, EndDate
                                 FROM SchoolPrincipal WHERE SchoolCode=@SchoolCode 
                                 ORDER BY PrincipalID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SchoolCode", Session["SchoolCode"].ToString());

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    txtPrincipalName.Text = dr["PrincipalName"].ToString();
                    txtPrincipalMobile.Text = dr["PrincipalMobile"].ToString();
                    txtPrincipalEmail.Text = dr["PrincipalEmail"].ToString();
                    txtPrincipalAddress.Text = dr["PrincipalAddress"].ToString();
                    txtPrincipalQualification.Text = dr["PrincipalQualification"].ToString();

                    if (dr["PrincipalDOB"] != DBNull.Value)
                        txtPrincipalDOB.Text = Convert.ToDateTime(dr["PrincipalDOB"]).ToString("yyyy-MM-dd");

                    if (dr["StartDate"] != DBNull.Value)
                        txtStartDate.Text = Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd");

                    if (dr["EndDate"] != DBNull.Value)
                        txtEndDate.Text = Convert.ToDateTime(dr["EndDate"]).ToString("yyyy-MM-dd");
                }
                con.Close();
            }
        }

        protected void btnSavePrincipal_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"INSERT INTO SchoolPrincipal 
                                (SchoolCode, PrincipalName, PrincipalMobile, PrincipalEmail, 
                                 PrincipalAddress, PrincipalDOB, PrincipalQualification, StartDate, EndDate)
                                VALUES (@SchoolCode, @PrincipalName, @PrincipalMobile, @PrincipalEmail, 
                                        @PrincipalAddress, @PrincipalDOB, @PrincipalQualification, @StartDate, @EndDate)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SchoolCode", Session["SchoolCode"].ToString());
                cmd.Parameters.AddWithValue("@PrincipalName", txtPrincipalName.Text.Trim());
                cmd.Parameters.AddWithValue("@PrincipalMobile", txtPrincipalMobile.Text.Trim());
                cmd.Parameters.AddWithValue("@PrincipalEmail", txtPrincipalEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@PrincipalAddress", txtPrincipalAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@PrincipalDOB", txtPrincipalDOB.Text.Trim());
                cmd.Parameters.AddWithValue("@PrincipalQualification", txtPrincipalQualification.Text.Trim());
                cmd.Parameters.AddWithValue("@StartDate", txtStartDate.Text.Trim());
                cmd.Parameters.AddWithValue("@EndDate", txtEndDate.Text.Trim());

                con.Open();
                cmd.ExecuteNonQuery();

                // Update profile status to complete
                SqlCommand cmdUpdate = new SqlCommand("UPDATE SchoolInformation SET ProfileStatus='COMPLETE' WHERE SchoolCode=@SchoolCode", con);
                cmdUpdate.Parameters.AddWithValue("@SchoolCode", Session["SchoolCode"].ToString());
                cmdUpdate.ExecuteNonQuery();

                con.Close();
            }

            // Refresh page to reflect status
            LoadProfileStatus();
            pnlPrincipalInfo.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
            {
                Response.Redirect("SchoolLogin.aspx");
                return;
            }

            string token = Session["AuthToken"].ToString();

            // ✅ pass token via query string
            Response.Redirect("SchoolDashboard.aspx?token=" + token);


        }
    }
}
