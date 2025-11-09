using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.Scheme
{
    public partial class RGSchemeApplicationForm : System.Web.UI.Page
    {
        // ✅ Make static so it can be accessed in WebMethod
        public static readonly string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;
        private string applicationID;

        protected void Page_Load(object sender, EventArgs e)
        {

            string path = Request.Url.AbsolutePath ?? string.Empty;
            if (path.EndsWith("/RGSchemeApplicationForm.aspx/GetHospitalName", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith("/RGSchemeApplicationForm.aspx/GetHospitalNames", StringComparison.OrdinalIgnoreCase)
                || path.IndexOf("/GetHospitalName", StringComparison.OrdinalIgnoreCase) >= 0
                || path.IndexOf("/GetHospitalNames", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                // short-circuit - do not perform normal Page_Load work for AJAX calls
                return;
            }
            if (IsPostBack || HttpContext.Current.Request.Path.Contains("GetHospitalName"))
                return;

            if (!IsPostBack)
            {
                applicationID = GetNewApplicationID();
                lblApplicationID.Text = applicationID;
                Session["ApplicationId"] = applicationID;

                LoadDistricts();
                BindHospitalRecords();
            }
        }

        private string GetNewApplicationID()
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                int currentYear = DateTime.Now.Year;
                int nextNumber = 1;
                string prefix = $"DES-MH{currentYear}";

                string query = @"SELECT TOP 1 ApplicationID 
                                 FROM AccidentDetails 
                                 WHERE ApplicationID LIKE @prefix + '%' 
                                 ORDER BY ApplicationID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@prefix", prefix);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    string lastId = result.ToString();
                    string lastNumPart = lastId.Substring(prefix.Length);
                    if (int.TryParse(lastNumPart, out int lastNumber))
                        nextNumber = lastNumber + 1;
                }
                return $"{prefix}{nextNumber:D4}";
            }
        }

        protected void txtDOB_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtDOB.Text, out DateTime dob))
            {
                if (dob > DateTime.Today)
                {
                    txtAge.Text = "Date of birth cannot be in the future.";
                    txtAge.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                DateTime today = DateTime.Today;
                int age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age)) age--;

                txtAge.Text = $"{age} years";
                txtAge.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                txtAge.Text = "Please enter a valid date.";
                txtAge.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void LoadDistricts()
        {
            DataSet dsDistrict = new DataSet();
            dsDistrict.ReadXml(Server.MapPath("~/App_Data/Districts.xml"));
            ddldist.DataSource = dsDistrict;
            ddldist.DataTextField = "DistrictName";
            ddldist.DataValueField = "DistrictId";
            ddldist.DataBind();
            ddldist.Items.Insert(0, new ListItem("---Select District---", "-1"));
        }

        protected void ddldist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDistrictId = ddldist.SelectedValue;

            if (!string.IsNullOrEmpty(selectedDistrictId) && selectedDistrictId != "-1")
            {
                DataSet dsTaluka = new DataSet();
                dsTaluka.ReadXml(Server.MapPath("~/App_Data/Taluka.xml"));

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

        protected void btnAddHospital_Click(object sender, EventArgs e)
        {
            string hospitalName = txtHospitalName.Text.Trim();

            // Validate dates
            if (!DateTime.TryParse(txtAdmittedDate.Text, out DateTime admittedDate) ||
                !DateTime.TryParse(txtDischargeDate.Text, out DateTime dischargeDate) ||
                !DateTime.TryParse(txtAccidentDate.Text, out DateTime accidentDate))
            {
                lblMessage.Text = "Please enter valid dates.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (admittedDate > dischargeDate)
            {
                lblMessage.Text = "Discharge date cannot be earlier than admitted date.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (accidentDate > admittedDate || accidentDate > dischargeDate)
            {
                lblMessage.Text = "Accident date must be before admitted and discharge dates.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Validate expenses
            if (!decimal.TryParse(txtExpenses.Text, out decimal totalExpenses))
            {
                lblMessage.Text = "Invalid expense amount entered.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // Check session
            if (Session["ApplicationId"] == null)
            {
                lblMessage.Text = "Session expired. Please reload the page.";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                if (fuHospitalCertMulti.HasFiles)
                {
                    // === START: NEW VALIDATION SECTION ===
                    long maxSizeInBytes = 5 * 1024 * 1024; // 5 MB

                    foreach (HttpPostedFile uploadedFile in fuHospitalCertMulti.PostedFiles)
                    {
                        if (uploadedFile.ContentLength > 0)
                        {
                            // 1. Check File Size
                            if (uploadedFile.ContentLength > maxSizeInBytes)
                            {
                                // File is too large
                                string errorMsg = $"File '{uploadedFile.FileName}' ({(uploadedFile.ContentLength / 1024.0 / 1024.0):F2} MB) exceeds the 5MB limit.";
                                lblMessage.Text = errorMsg;
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return; // Stop the entire operation
                            }

                            // 2. (Recommended) Check File Type
                            string fileExtension = Path.GetExtension(uploadedFile.FileName);
                            if (!string.Equals(fileExtension, ".pdf", StringComparison.OrdinalIgnoreCase))
                            {
                                lblMessage.Text = $"File '{uploadedFile.FileName}' is not a PDF. Please upload only PDF files.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return; // Stop the entire operation
                            }
                        }
                    }
                    // === END: NEW VALIDATION SECTION ===

                    // If we are here, all files have passed validation. Now we can process them.
                    string baseName = lblApplicationID.Text;
                    string savePathDir = Server.MapPath("~/MultiHospitalsDoc/");

                    // Ensure directory exists
                    if (!Directory.Exists(savePathDir))
                        Directory.CreateDirectory(savePathDir);

                    // Find existing files to continue numbering
                    string[] existingFiles = Directory.GetFiles(savePathDir, baseName + "-*" + ".pdf");
                    int counter = existingFiles.Length + 1;

                    foreach (HttpPostedFile uploadedFile in fuHospitalCertMulti.PostedFiles)
                    {
                        if (uploadedFile.ContentLength > 0)
                        {
                            string fileExtension = Path.GetExtension(uploadedFile.FileName);
                            string newFileName = $"{baseName}-{counter}{fileExtension}";
                            string fullSavePath = Path.Combine(savePathDir, newFileName);

                            uploadedFile.SaveAs(fullSavePath);

                            string relativePath = $"~/MultiHospitalsDoc/{newFileName}";

                            // Insert into database
                            using (SqlConnection con = new SqlConnection(conStr))
                            {
                                string insertQuery = @"
                INSERT INTO HospitalRecords 
                (ApplicationId, HospitalName, AdmittedDate, DischargeDate, TotalExpenses, MultiHospitalsDoc, IPAddress)
                VALUES (@ApplicationId, @HospitalName, @AdmittedDate, @DischargeDate, @TotalExpenses, @MultiHospitalsDoc, @IPAddress)";

                                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                {
                                    cmd.Parameters.AddWithValue("@ApplicationId", Session["ApplicationId"].ToString());
                                    cmd.Parameters.AddWithValue("@HospitalName", hospitalName);
                                    cmd.Parameters.AddWithValue("@AdmittedDate", admittedDate);
                                    cmd.Parameters.AddWithValue("@DischargeDate", dischargeDate);
                                    cmd.Parameters.AddWithValue("@TotalExpenses", totalExpenses);
                                    cmd.Parameters.AddWithValue("@MultiHospitalsDoc", relativePath);
                                    cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            counter++; // Increment for next file
                        }
                    }

                    lblMessage.Text = "Hospital details and Documents added successfully!";
                    lblMessage.ForeColor = System.Drawing.Color.Green;

                    // Clear inputs
                    txtHospitalName.Text = "";
                    txtAdmittedDate.Text = "";
                    txtDischargeDate.Text = "";
                    txtExpenses.Text = "";

                    // Refresh GridView
                    BindHospitalRecords();
                }
                else
                {
                    lblMessage.Text = "Please select at least one PDF to upload.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
        private void BindHospitalRecords()
        {
            if (Session["ApplicationId"] == null)
                return;

            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT RecordId, HospitalName, AdmittedDate, DischargeDate, TotalExpenses, MultiHospitalsDoc FROM HospitalRecords",
                    con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvHospitals.DataSource = dt;
                gvHospitals.DataBind();
            }
        }

        protected void gvHospitals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewPDF")
            {
                string relativePath = e.CommandArgument.ToString();

                if (string.IsNullOrEmpty(relativePath))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No PDF found for this record.');", true);
                    return;
                }

                // ✅ Correct folder name
                string filePath = Server.MapPath(relativePath);

                if (System.IO.File.Exists(filePath))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "inline;filename=" + Path.GetFileName(filePath));
                    Response.TransmitFile(filePath);
                    Response.Flush();
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // ✅ safer than Response.End()
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('File not found on server.');", true);
                }
            }

            else if (e.CommandName == "DeleteItem")
            {
                try
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    int recordId = Convert.ToInt32(gvHospitals.DataKeys[rowIndex].Value);

                    if (Session["ApplicationId"] == null)
                    {
                        lblMessage.Text = "Error: Session expired. Please log in again.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM HospitalRecords WHERE RecordId=@RecordId AND ApplicationId=@ApplicationId", con))
                        {
                            cmd.Parameters.AddWithValue("@RecordId", recordId);
                            cmd.Parameters.AddWithValue("@ApplicationId", Session["ApplicationId"].ToString());

                            con.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                lblMessage.Text = "Record deleted successfully!";
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                            }
                            else
                            {
                                lblMessage.Text = "Error: Record not found or could not be deleted.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }

                    BindHospitalRecords();
                }
                catch
                {
                    lblMessage.Text = "An error occurred while deleting the record.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }


        // ✅ WebMethod for autocomplete suggestions
        [System.Web.Services.WebMethod]
        public static List<string> GetHospitalName(string hospitalName)
        {
            List<string> result = new List<string>();
            string filePath = HttpContext.Current.Server.MapPath("~/App_Data/HospitalsList.xml");

            if (System.IO.File.Exists(filePath))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(filePath);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string name = row["HospitalName"].ToString();
                        if (name.StartsWith(hospitalName, StringComparison.OrdinalIgnoreCase))
                            result.Add(name);
                    }
                }
            }
            return result;
        }

        protected void gvHospitals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteButton = e.Row.Cells
                    .Cast<TableCell>()
                    .SelectMany(c => c.Controls.OfType<LinkButton>())
                    .FirstOrDefault(btn => btn.CommandName == "Delete");

                if (deleteButton != null)
                    deleteButton.Attributes["onclick"] = "return confirm('Are you sure you want to delete this record?');";
            }
        }


        protected void gvHospitals_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int recordId = Convert.ToInt32(gvHospitals.DataKeys[e.RowIndex].Value);
                string pdfFolder = Server.MapPath("~/MultiHospitalsDoc/");  // adjust if folder name is different

                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // 1️⃣ Fetch file name before deleting record
                    string getFileQuery = "SELECT MultiHospitalsDoc FROM HospitalRecords WHERE RecordId = @RecordId";
                    string filePath = null;

                    using (SqlCommand cmdGet = new SqlCommand(getFileQuery, con))
                    {
                        cmdGet.Parameters.AddWithValue("@RecordId", recordId);
                        object result = cmdGet.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            filePath = result.ToString().Trim();

                            // Full physical path
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                string fullFilePath = Server.MapPath(filePath);

                                // Delete file if it exists
                                if (File.Exists(fullFilePath))
                                {
                                    File.Delete(fullFilePath);
                                }
                            }
                        }
                    }

                    // 2️⃣ Delete record from DB
                    string deleteQuery = "DELETE FROM HospitalRecords WHERE RecordId = @RecordId";
                    using (SqlCommand cmdDel = new SqlCommand(deleteQuery, con))
                    {
                        cmdDel.Parameters.AddWithValue("@RecordId", recordId);
                        int rows = cmdDel.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblMessage.Text = "✅ Record and attached file deleted successfully.";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblMessage.Text = "⚠️ Record not found.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }

                    con.Close();
                }

                // 3️⃣ Refresh the grid
                BindHospitalRecords();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error while deleting: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }


        // 📑 Populate required documents based on Accident Type
        //protected void ddlAccidentType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    using (SqlConnection con = new SqlConnection(conStr))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(
        //            "SELECT DocumentID, DocumentName FROM RequiredDocuments WHERE AccidentType=@AccidentType", con);
        //        da.SelectCommand.Parameters.AddWithValue("@AccidentType", ddlAccidentType.SelectedValue);

        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        gvDocuments.DataSource = dt;
        //        gvDocuments.DataKeyNames = new[] { "DocumentID" };
        //        gvDocuments.DataBind();
        //    }
        //}
        protected void btnFinalSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["ApplicationId"] == null)
                {
                    lblMessage.Text = "Session expired. Please reload the page.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                string appId = Session["ApplicationId"].ToString();

                // 🟢 Check if hospital data exists
                bool hasMultiple = HasMultipleHospitals(appId);

                // If no hospital records exist, insert the single hospital data
                if (!hasMultiple)
                {
                    string hospitalName = txtHospitalName.Text.Trim();
                    if (string.IsNullOrEmpty(hospitalName))
                    {
                        lblMessage.Text = "Please enter hospital name before final submit.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    if (!DateTime.TryParse(txtAdmittedDate.Text, out DateTime admittedDate) ||
                        !DateTime.TryParse(txtDischargeDate.Text, out DateTime dischargeDate) ||
                        !DateTime.TryParse(txtAccidentDate.Text, out DateTime accidentDate))
                    {
                        lblMessage.Text = "Please enter valid hospital dates.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    if (!decimal.TryParse(txtExpenses.Text, out decimal totalExpenses))
                    {
                        lblMessage.Text = "Invalid expense amount.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    if (!fuHospitalCertMulti.HasFile)
                    {
                        lblMessage.Text = "Please upload the hospital certificate PDF.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    string fileExt = Path.GetExtension(fuHospitalCertMulti.FileName);
                    if (!string.Equals(fileExt, ".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        lblMessage.Text = "Only PDF hospital certificate allowed.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    string saveDir = Server.MapPath("~/MultiHospitalsDoc/");
                    if (!Directory.Exists(saveDir))
                        Directory.CreateDirectory(saveDir);

                    string fileName = $"{appId}-1{fileExt}";
                    string fullSavePath = Path.Combine(saveDir, fileName);
                    fuHospitalCertMulti.SaveAs(fullSavePath);

                    string relativePath = $"~/MultiHospitalsDoc/{fileName}";

                    // Insert single hospital record
                    using (SqlConnection con = new SqlConnection(conStr))
                    {
                        string insertQuery = @"INSERT INTO HospitalRecords 
                    (ApplicationId, HospitalName, AdmittedDate, DischargeDate, TotalExpenses, MultiHospitalsDoc, IPAddress)
                    VALUES (@ApplicationId, @HospitalName, @AdmittedDate, @DischargeDate, @TotalExpenses, @MultiHospitalsDoc, @IPAddress)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@ApplicationId", appId);
                            cmd.Parameters.AddWithValue("@HospitalName", hospitalName);
                            cmd.Parameters.AddWithValue("@AdmittedDate", admittedDate);
                            cmd.Parameters.AddWithValue("@DischargeDate", dischargeDate);
                            cmd.Parameters.AddWithValue("@TotalExpenses", totalExpenses);
                            cmd.Parameters.AddWithValue("@MultiHospitalsDoc", relativePath);
                            cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // 🟡 After hospital logic, continue uploading required documents
                string docFolder = Server.MapPath("~/AccidentDocuments/");
                if (!Directory.Exists(docFolder))
                    Directory.CreateDirectory(docFolder);

                int uploadedCount = 0;
                List<string> missingDocs = new List<string>();

                foreach (GridViewRow row in gvDocuments.Rows)
                {
                    string documentID = ((DataBoundLiteralControl)row.Cells[0].Controls[0]).Text.Trim();
                    string documentName = row.Cells[1].Text.Trim();
                    FileUpload fu = (FileUpload)row.FindControl("fuDocument");

                    if (fu != null && fu.HasFile)
                    {
                        string ext = Path.GetExtension(fu.FileName);
                        if (!string.Equals(ext, ".pdf", StringComparison.OrdinalIgnoreCase))
                        {
                            lblMessage.Text = $"Only PDF files are allowed for {documentName}.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        string cleanName = documentName.Replace(" ", "").Replace("/", "").Replace("\\", "");
                        string fileName = $"{appId}-{cleanName}{ext}";
                        string fullSavePath = Path.Combine(docFolder, fileName);
                        fu.SaveAs(fullSavePath);

                        string relativePath = $"~/AccidentDocuments/{fileName}";

                        using (SqlConnection con = new SqlConnection(conStr))
                        {
                            string query = @"INSERT INTO UploadedDocuments 
                        (ApplicationId, DocumentID, DocumentName, FilePath, UploadedOn, IPAddress)
                        VALUES (@ApplicationId, @DocumentID, @DocumentName, @FilePath, GETDATE(), @IPAddress)";
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Parameters.AddWithValue("@ApplicationId", appId);
                                cmd.Parameters.AddWithValue("@DocumentID", documentID);
                                cmd.Parameters.AddWithValue("@DocumentName", documentName);
                                cmd.Parameters.AddWithValue("@FilePath", relativePath);
                                cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                        uploadedCount++;
                    }
                    else
                    {
                        missingDocs.Add(documentName);
                    }
                }

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = hasMultiple
                    ? $"✅ Multiple hospital records already added. {uploadedCount} documents uploaded successfully."
                    : $"✅ Single hospital details inserted and {uploadedCount} documents uploaded successfully.";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "❌ Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }



        private bool HasMultipleHospitals(string appId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = "SELECT COUNT(*) FROM HospitalRecords WHERE ApplicationId = @ApplicationId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ApplicationId", appId);
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }
        protected void btnNextPart_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure Session ApplicationID exists
                string appId = Session["ApplicationId"]?.ToString();
                if (string.IsNullOrEmpty(appId))
                {
                    lblMessage.Text = "Session expired. Please reload the page.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Basic validation for applicant name
                if (string.IsNullOrWhiteSpace(txtApplicantFullName.Text))
                {
                    lblMessage.Text = "Please enter applicant full name.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Insert Accident & Applicant details into a main table (example)
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    string query = @"INSERT INTO AccidentDetails
                            (ApplicationId, ApplicantName, DOB, Age, DistrictId, TalukaId, AccidentDate, CreatedOn, IPAddress)
                            VALUES (@ApplicationId, @ApplicantName, @DOB, @Age, @DistrictId, @TalukaId, @AccidentDate,  GETDATE(), @IPAddress)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ApplicationId", appId);
                        cmd.Parameters.AddWithValue("@ApplicantName", txtApplicantFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@DOB", string.IsNullOrEmpty(txtDOB.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtDOB.Text));
                        cmd.Parameters.AddWithValue("@Age", txtAge.Text.Replace(" years", "").Trim());
                        cmd.Parameters.AddWithValue("@DistrictId", ddldist.SelectedValue);
                        cmd.Parameters.AddWithValue("@TalukaId", ddltaluka.SelectedValue);
                        cmd.Parameters.AddWithValue("@AccidentDate", string.IsNullOrEmpty(txtAccidentDate.Text) ? (object)DBNull.Value : Convert.ToDateTime(txtAccidentDate.Text));
                        //cmd.Parameters.AddWithValue("@AccidentType", ddlAccidentType.SelectedValue);
                        cmd.Parameters.AddWithValue("@IPAddress", HttpContext.Current.Request.UserHostAddress);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Redirect to next part for document upload
                Response.Redirect("~/Scheme/RGSchemeAppPartTwo.aspx", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }



    }
}
