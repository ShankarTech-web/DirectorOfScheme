using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.Scheme
{
    public partial class RGSchemeAppPartTwo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ApplicationID"] == null)
                {
                    Response.Redirect("~/Scheme/RGSchemeApplicationForm.aspx");
                }
                else
                {
                    lblAppID.Text = "Application ID: " + Session["ApplicationID"].ToString();
                    LoadDocumentList();
                }
            }
        }

        private void LoadDocumentList()
        {
            // Example static data; replace with DB fetch
            DataTable dt = new DataTable();
            dt.Columns.Add("DocumentID");
            dt.Columns.Add("DocumentName");
            dt.Rows.Add("1", "Hospital Bill");
            dt.Rows.Add("2", "Doctor Certificate");
            dt.Rows.Add("3", "Discharge Summary");
            gvDocuments.DataSource = dt;
            gvDocuments.DataBind();
        }

        protected void btnUploadAll_Click(object sender, EventArgs e)
        {
            string appID = Session["ApplicationID"].ToString();
            string folderPath = Server.MapPath("~/Uploads/" + appID + "/");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Upload medical certificate
            if (fuMedicalCert.HasFile)
            {
                string filePath = Path.Combine(folderPath, "MedicalCertificate.pdf");
                fuMedicalCert.SaveAs(filePath);
            }

            // Upload grid files
            foreach (GridViewRow row in gvDocuments.Rows)
            {
                FileUpload fu = (FileUpload)row.FindControl("fuDocument");
                if (fu != null && fu.HasFile)
                {
                    string docName = row.Cells[1].Text.Replace(" ", "_");
                    string filePath = Path.Combine(folderPath, docName + ".pdf");
                    fu.SaveAs(filePath);
                }
            }

            lblMsg.Text = "All documents uploaded successfully.";
        }
    }
}
