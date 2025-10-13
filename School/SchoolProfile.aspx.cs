using System;
using System.Configuration;
using System.Data; // FIX: Added for CommandType and SqlDbType
using System.Data.SqlClient;
using System.IO;

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

                if (string.IsNullOrEmpty(sessionToken) || queryToken != sessionToken)
                {
                    Response.Redirect("SchoolLogin.aspx");
                    return;
                }

                lbSchoolUdise.Text = Session["SchoolCode"]?.ToString();
                lbSchoolName.Text = Session["schoolName"]?.ToString();

                // FIX: Call LoadProfileStatus first to check if we need to load principal info
                LoadProfileStatus();

                // FIX: Load existing principal info if the profile is incomplete so it can be edited.
                if (pnlPrincipalInfo.Visible)
                {
                    LoadPrincipalInfo();
                }
            }
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

                if (status != null && status != DBNull.Value)
                {
                    lbProfileStatus.Text = status.ToString();
                    // Show the panel if the profile is not yet complete
                    pnlPrincipalInfo.Visible = !status.ToString().Equals("COMPLETE", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    // Default case if status is missing
                    lbProfileStatus.Text = "Incomplete";
                    pnlPrincipalInfo.Visible = true;
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

        // FIX: btnSavePrincipal_Click logic is completely rewritten for correctness and robustness.
        protected void btnSavePrincipal_Click(object sender, EventArgs e)
        {
            string schoolCode = Session["SchoolCode"].ToString();
            string principalPhotoPath = null; // Will hold the path to the photo if uploaded

            try
            {
                // --- 1. Handle File Upload Separately ---
                if (fileuploadPrincipal.HasFile)
                {
                    string fileExtension = Path.GetExtension(fileuploadPrincipal.FileName).ToLower();
                    if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                    {
                        Response.Write("<script>alert('Invalid file type. Only .jpg, .jpeg, or .png are allowed.');</script>");
                        return;
                    }
                    if (fileuploadPrincipal.FileContent.Length > 100 * 1024) // 100 KB
                    {
                        Response.Write("<script>alert('File size exceeds the 100 KB limit.');</script>");
                        return;
                    }

                    // FIX: Use the session SchoolCode for a unique and correct file name
                    string newFileName = schoolCode + fileExtension;
                    string savePath = Server.MapPath("~/PrincipalsPhoto/");
                    string filePath = Path.Combine(savePath, newFileName);

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }

                    fileuploadPrincipal.SaveAs(filePath);
                    principalPhotoPath = "~/PrincipalsPhoto/" + newFileName; // Store relative path for the database
                }

                // --- 2. Save Data to Database (UPDATE or INSERT) ---
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // Check if a principal record already exists for this school
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM SchoolPrincipal WHERE SchoolCode = @SchoolCode", con);
                    checkCmd.Parameters.AddWithValue("@SchoolCode", schoolCode);
                    int existingRecords = (int)checkCmd.ExecuteScalar();

                    string query;
                    if (existingRecords > 0)
                    {
                        // UPDATE existing record
                        query = @"UPDATE SchoolPrincipal SET 
                                    PrincipalName=@PrincipalName, PrincipalMobile=@PrincipalMobile, PrincipalEmail=@PrincipalEmail,
                                    PrincipalAddress=@PrincipalAddress, PrincipalDOB=@PrincipalDOB, 
                                    PrincipalQualification=@PrincipalQualification, StartDate=@StartDate, EndDate=@EndDate
                                    " + (principalPhotoPath != null ? ", PrincipalPhoto=@PrincipalPhoto " : "") + // Only update photo if a new one was uploaded
                                  "WHERE SchoolCode=@SchoolCode";
                    }
                    else
                    {
                        // INSERT new record
                        query = @"INSERT INTO SchoolPrincipal 
                                    (SchoolCode, PrincipalName, PrincipalMobile, PrincipalEmail, PrincipalAddress, 
                                    PrincipalDOB, PrincipalQualification, StartDate, EndDate, FilePath)
                                  VALUES 
                                    (@SchoolCode, @PrincipalName, @PrincipalMobile, @PrincipalEmail, @PrincipalAddress, 
                                    @PrincipalDOB, @PrincipalQualification, @StartDate, @EndDate, @FilePath)";
                    }

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@SchoolCode", schoolCode);
                    cmd.Parameters.AddWithValue("@PrincipalName", txtPrincipalName.Text.Trim());
                    cmd.Parameters.AddWithValue("@PrincipalMobile", txtPrincipalMobile.Text.Trim());
                    cmd.Parameters.AddWithValue("@PrincipalEmail", txtPrincipalEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@PrincipalAddress", txtPrincipalAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@PrincipalQualification", txtPrincipalQualification.Text.Trim());

                    // Handle nullable dates properly
                    cmd.Parameters.AddWithValue("@PrincipalDOB", string.IsNullOrEmpty(txtPrincipalDOB.Text) ? (object)DBNull.Value : txtPrincipalDOB.Text.Trim());
                    cmd.Parameters.AddWithValue("@StartDate", string.IsNullOrEmpty(txtStartDate.Text) ? (object)DBNull.Value : txtStartDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@EndDate", string.IsNullOrEmpty(txtEndDate.Text) ? (object)DBNull.Value : txtEndDate.Text.Trim());

                    // Add photo path parameter
                    if (principalPhotoPath != null)
                    {
                        cmd.Parameters.AddWithValue("@FilePath", principalPhotoPath);
                    }
                    else if (existingRecords == 0)
                    {
                        // If inserting new record and no photo, pass NULL
                        cmd.Parameters.AddWithValue("@FilePath", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();

                    // --- 3. Update Profile Status to COMPLETE ---
                    SqlCommand cmdUpdate = new SqlCommand("UPDATE SchoolInformation SET ProfileStatus='COMPLETE' WHERE SchoolCode=@SchoolCode", con);
                    cmdUpdate.Parameters.AddWithValue("@SchoolCode", schoolCode);
                    cmdUpdate.ExecuteNonQuery();

                    con.Close();
                }

                // --- 4. Refresh UI to Reflect Changes ---
                LoadProfileStatus(); // This will now hide the panel as status is 'COMPLETE'
                Response.Write("<script>alert('Profile saved successfully!');</script>");
            }
            catch (Exception ex)
            {
                // A better practice is to log the error and show a user-friendly message
                // For example: lblErrorMessage.Text = "An error occurred while saving. Please try again.";
                Response.Write("An error occurred: " + ex.Message);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["AuthToken"] == null)
            {
                Response.Redirect("SchoolLogin.aspx");
                return;
            }
            string token = Session["AuthToken"].ToString();
            Response.Redirect("SchoolDashboard.aspx?token=" + token);
        }
    }
}