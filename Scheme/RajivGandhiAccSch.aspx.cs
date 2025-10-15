using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.Scheme
{
    public partial class RajivGandhiAccSch : System.Web.UI.Page
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;
        private string applicationID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 🔒 Validate token
                string sessionToken = Session["AuthToken"] as string;
                string queryToken = Request.QueryString["token"];

                if (string.IsNullOrEmpty(sessionToken) || queryToken != sessionToken)
                {
                    Response.Redirect("../School/SchoolLogin.aspx");
                    return;
                }

                // 🏫 Display school info
                lblUdise.Text = Session["SchoolCode"]?.ToString();
                lblSchoolName.Text = Session["schoolName"]?.ToString();

                // 🧾 Generate Application ID
                applicationID = GetNewApplicationID();
                lblApplicationID.Text = applicationID;

                // Load district dropdown
                LoadDistricts();
            }
            else
            {
                applicationID = lblApplicationID.Text;
            }
        }

        // 🏙 Load Districts from XML
        private void LoadDistricts()
        {
            DataSet dsDistrict = new DataSet();
            dsDistrict.ReadXml(Server.MapPath("Districts.xml"));

            ddldist.DataSource = dsDistrict;
            ddldist.DataTextField = "DistrictName";
            ddldist.DataValueField = "DistrictId";
            ddldist.DataBind();

            ddldist.Items.Insert(0, new ListItem("---Select District---", "-1"));
            ddltaluka.Items.Insert(0, new ListItem("---Select Taluka---", "-1"));
        }

        // 📑 Populate required documents based on Accident Type
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
                gvDocuments.DataKeyNames = new[] { "DocumentID" };
                gvDocuments.DataBind();
            }
        }

        // 🧾 Generate New Application ID (Format: DES-MHYYYY0001)
        private string GetNewApplicationID()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();

                int currentYear = DateTime.Now.Year;
                int nextNumber = 1;
                string prefix = $"DES-MH{currentYear}";

                // Get last ApplicationID of current year
                string query = @"
                    SELECT TOP 1 ApplicationID 
                    FROM AccidentDetails 
                    WHERE ApplicationID LIKE @prefix + '%' 
                    ORDER BY ApplicationID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@prefix", prefix);

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string lastId = result.ToString(); // e.g. DES-MH20250012
                    string lastNumPart = lastId.Substring(prefix.Length); // e.g. "0012"

                    if (int.TryParse(lastNumPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                // Return new ApplicationID (e.g. DES-MH20250013)
                return $"{prefix}{nextNumber:D4}";
            }
        }

        // 💾 Submit Application
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // 🧾 Get fresh Application ID to ensure uniqueness
            applicationID = GetNewApplicationID();

            // ⚠️ Validate uploads
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

                // 🔹 Insert Student Info
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Students (FullName, Age, Standard, SchoolName, District) 
                      VALUES (@FullName, @Age, @Standard, @SchoolName, @District); 
                      SELECT SCOPE_IDENTITY();", con);

                cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                cmd.Parameters.AddWithValue("@Age", int.Parse(txtAge.Text));
                cmd.Parameters.AddWithValue("@Standard", int.Parse(ddlStandard.SelectedValue));
                cmd.Parameters.AddWithValue("@SchoolName", lblSchoolName.Text.Trim());
                cmd.Parameters.AddWithValue("@District", ddldist.SelectedItem.Text.Trim());

                studentID = Convert.ToInt32(cmd.ExecuteScalar());

                // 🔹 Insert Accident Details
                SqlCommand cmdAccident = new SqlCommand(
                    @"INSERT INTO AccidentDetails 
                      (StudentID, AccidentDate, AccidentType, HospitalName, ExpenseAmount, IsEligible, ApplicationID)
                      VALUES (@StudentID, @AccidentDate, @AccidentType, @HospitalName, @ExpenseAmount, 1, @ApplicationID);
                      SELECT SCOPE_IDENTITY();", con);

                cmdAccident.Parameters.AddWithValue("@StudentID", studentID);
                cmdAccident.Parameters.AddWithValue("@AccidentDate", DateTime.Now);
                cmdAccident.Parameters.AddWithValue("@AccidentType", ddlAccidentType.SelectedValue);
                cmdAccident.Parameters.AddWithValue("@HospitalName", "NA");
                cmdAccident.Parameters.AddWithValue("@ExpenseAmount", 0);
                cmdAccident.Parameters.AddWithValue("@ApplicationID", applicationID);

                accidentID = Convert.ToInt32(cmdAccident.ExecuteScalar());

                // 🔹 Save Uploaded Documents
                foreach (GridViewRow row in gvDocuments.Rows)
                {
                    FileUpload fu = (FileUpload)row.FindControl("fuDoc");
                    if (fu != null && fu.HasFile)
                    {
                        string ext = System.IO.Path.GetExtension(fu.FileName);
                        string fileName = $"{applicationID}_{gvDocuments.DataKeys[row.RowIndex].Value}{ext}";
                        string filePath = Server.MapPath("~/Uploads/") + fileName;
                        fu.SaveAs(filePath);

                        SqlCommand cmdDoc = new SqlCommand(
                            @"INSERT INTO UploadedDocuments (AccidentID, DocumentID, FilePath) 
                              VALUES (@AccidentID, @DocumentID, @FilePath)", con);

                        cmdDoc.Parameters.AddWithValue("@AccidentID", accidentID);
                        cmdDoc.Parameters.AddWithValue("@DocumentID", gvDocuments.DataKeys[row.RowIndex].Value);
                        cmdDoc.Parameters.AddWithValue("@FilePath", "~/Uploads/" + fileName);
                        cmdDoc.ExecuteNonQuery();
                    }
                }
            }

            // ✅ Success Message
            lblMessage.Text = $"✅ Application Submitted Successfully!<br/>📄 Your Application ID: <b>{applicationID}</b>";
            lblMessage.ForeColor = System.Drawing.Color.Green;
        }

        // ↩️ Back Button
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

        // 🌐 Load Talukas based on District
        protected void ddldist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDistrictId = ddldist.SelectedValue;

            if (!string.IsNullOrEmpty(selectedDistrictId) && selectedDistrictId != "-1")
            {
                DataSet dsTaluka = new DataSet();
                dsTaluka.ReadXml(Server.MapPath("Taluka.xml"));

                DataTable talukaTable = dsTaluka.Tables["Taluka"];
                DataTable filteredTalukas = talukaTable.Clone();

                foreach (DataRow row in talukaTable.Rows)
                {
                    if (row["DistrictId"].ToString() == selectedDistrictId)
                        filteredTalukas.ImportRow(row);
                }

                ddltaluka.DataSource = filteredTalukas;
                ddltaluka.DataTextField = "TalukaName";
                ddltaluka.DataValueField = "TalukaId";
                ddltaluka.DataBind();
                ddltaluka.Items.Insert(0, new ListItem("---Select Taluka---", "-1"));
            }
            else
            {
                ddltaluka.Items.Clear();
                ddltaluka.Items.Insert(0, new ListItem("---Select Taluka---", "-1"));
            }
        }
    }
}
