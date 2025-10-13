using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.Scheme
{
    public partial class RajivGandhiAccSch : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;
        private string applicationID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sessionToken = Session["AuthToken"] as string;
                string queryToken = Request.QueryString["token"];

                if (string.IsNullOrEmpty(sessionToken) || queryToken != sessionToken)
                {
                    Response.Redirect("../School/SchoolLogin.aspx");
                    return;
                }

                lblUdise.Text = Session["SchoolCode"].ToString();
                lblSchoolName.Text = Session["schoolName"].ToString();

                // Generate Application ID on page load
                applicationID = GenerateApplicationID();
                lblApplicationID.Text = applicationID;
            }
            else
            {
                applicationID = lblApplicationID.Text;
            }
        }

        protected void ddlAccidentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT DocumentID, DocumentName FROM RequiredDocuments WHERE AccidentType=@AccidentType", con);
                da.SelectCommand.Parameters.AddWithValue("@AccidentType", ddlAccidentType.SelectedValue);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvDocuments.DataSource = dt;
                gvDocuments.DataKeyNames = new string[] { "DocumentID" };
                gvDocuments.DataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Validate document uploads
            foreach (GridViewRow row in gvDocuments.Rows)
            {
                FileUpload fu = (FileUpload)row.FindControl("fuDoc");
                if (fu == null || !fu.HasFile)
                {
                    lblMessage.Text = "⚠️ Please upload all required documents before submitting.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }
            }

            int studentID, accidentID;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                // Insert Student
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Students (FullName, Age, Standard, SchoolName, District) " +
                    "VALUES (@FullName,@Age,@Standard,@SchoolName,@District); SELECT SCOPE_IDENTITY();", con);
                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@Age", int.Parse(txtAge.Text));
                cmd.Parameters.AddWithValue("@Standard", int.Parse(ddlStandard.SelectedValue));
                cmd.Parameters.AddWithValue("@SchoolName", txtSchool.Text);
                cmd.Parameters.AddWithValue("@District", txtDistrict.Text);
                studentID = Convert.ToInt32(cmd.ExecuteScalar());

                // Insert Accident
                SqlCommand cmdAccident = new SqlCommand(
                    "INSERT INTO AccidentDetails (StudentID, AccidentDate, AccidentType, HospitalName, ExpenseAmount, IsEligible, ApplicationID) " +
                    "VALUES (@StudentID,@AccidentDate,@AccidentType,@HospitalName,@Expense,1,@ApplicationID); SELECT SCOPE_IDENTITY();", con);
                cmdAccident.Parameters.AddWithValue("@StudentID", studentID);
                cmdAccident.Parameters.AddWithValue("@AccidentDate", DateTime.Now);
                cmdAccident.Parameters.AddWithValue("@AccidentType", ddlAccidentType.SelectedValue);
                cmdAccident.Parameters.AddWithValue("@HospitalName", "NA");
                cmdAccident.Parameters.AddWithValue("@Expense", 0);
                cmdAccident.Parameters.AddWithValue("@ApplicationID", applicationID);
                accidentID = Convert.ToInt32(cmdAccident.ExecuteScalar());

                // Save uploaded documents with ApplicationID
                foreach (GridViewRow row in gvDocuments.Rows)
                {
                    FileUpload fu = (FileUpload)row.FindControl("fuDoc");
                    if (fu != null && fu.HasFile)
                    {
                        string ext = System.IO.Path.GetExtension(fu.FileName);
                        string fileName = applicationID + fu.ID.ToLower() + ext;
                        string filePath = Server.MapPath("~/Uploads/") + fileName;
                        fu.SaveAs(filePath);

                        SqlCommand cmdDoc = new SqlCommand(
                            "INSERT INTO UploadedDocuments (AccidentID, DocumentID, FilePath) VALUES (@AccidentID,@DocumentID,@FilePath)", con);
                        cmdDoc.Parameters.AddWithValue("@AccidentID", accidentID);
                        cmdDoc.Parameters.AddWithValue("@DocumentID", gvDocuments.DataKeys[row.RowIndex].Value);
                        cmdDoc.Parameters.AddWithValue("@FilePath", "~/Uploads/" + fileName);
                        cmdDoc.ExecuteNonQuery();
                    }
                }
            }

            lblMessage.Text = $"✅ Application Submitted Successfully!<br/>📄 Your Application ID: <b>{applicationID}</b>";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }

        private string GenerateApplicationID()
        {
            Random rnd = new Random();
            string randomNum = rnd.Next(1000, 9999).ToString();
            return "RG" + DateTime.Now.ToString("yyyyMMdd") + randomNum;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
            {
                Response.Redirect("../School/SchoolLogin.aspx");
                return;
            }
            string token = Session["AuthToken"].ToString();
            Response.Redirect("../School/SchoolDashboard.aspx?token=" + token);
        }

        // Application Tracking
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
