using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.Scheme
{
    public partial class RajivGandhiAccSch : System.Web.UI.Page
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
                        Response.Redirect("../School/SchoolLogin.aspx");
                        return;
                    }
                lblUdise.Text = Session["SchoolCode"].ToString();
                lblSchoolName.Text = Session["schoolName"].ToString();
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
                    cmd.Parameters.AddWithValue("@Standard", int.Parse(txtStandard.Text));
                    cmd.Parameters.AddWithValue("@SchoolName", txtSchool.Text);
                    cmd.Parameters.AddWithValue("@District", txtDistrict.Text);

                    studentID = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insert Accident
                    SqlCommand cmdAccident = new SqlCommand(
                        "INSERT INTO AccidentDetails (StudentID, AccidentDate, AccidentType, HospitalName, ExpenseAmount, IsEligible) " +
                        "VALUES (@StudentID,@AccidentDate,@AccidentType,@HospitalName,@Expense,1); SELECT SCOPE_IDENTITY();", con);

                    cmdAccident.Parameters.AddWithValue("@StudentID", studentID);
                    cmdAccident.Parameters.AddWithValue("@AccidentDate", DateTime.Now);
                    cmdAccident.Parameters.AddWithValue("@AccidentType", ddlAccidentType.SelectedValue);
                    cmdAccident.Parameters.AddWithValue("@HospitalName", "NA");
                    cmdAccident.Parameters.AddWithValue("@Expense", 0);

                    accidentID = Convert.ToInt32(cmdAccident.ExecuteScalar());

                    // Save Uploaded Docs
                    foreach (GridViewRow row in gvDocuments.Rows)
                    {
                        FileUpload fu = (FileUpload)row.FindControl("fuDoc");
                        if (fu != null && fu.HasFile)
                        {
                            string filePath = Server.MapPath("~/Uploads/") + fu.FileName;
                            fu.SaveAs(filePath);

                            SqlCommand cmdDoc = new SqlCommand(
                                "INSERT INTO UploadedDocuments (AccidentID, DocumentID, FilePath) VALUES (@AccidentID,@DocumentID,@FilePath)", con);

                            cmdDoc.Parameters.AddWithValue("@AccidentID", accidentID);
                            cmdDoc.Parameters.AddWithValue("@DocumentID", gvDocuments.DataKeys[row.RowIndex].Value);
                            cmdDoc.Parameters.AddWithValue("@FilePath", "~/Uploads/" + fu.FileName);

                            cmdDoc.ExecuteNonQuery();
                        }
                    }
                }

                lblMessage.Text = "✅ Application Submitted Successfully!";
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["SchoolCode"] == null || Session["AuthToken"] == null)
            {
                Response.Redirect("../School/SchoolLogin.aspx");
                return;
            }
            string token = Session["AuthToken"].ToString();
            // ✅ pass token via query string
            Response.Redirect("../School/SchoolDashboard.aspx?token=" + token);
        }
    }
}

        
