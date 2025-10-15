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
    public partial class RGSchemeApplicationForm : System.Web.UI.Page
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;
        private string applicationID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 🧾 Generate Application ID
                applicationID = GetNewApplicationID();
                lblApplicationID.Text = applicationID;
                LoadDistricts();

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

        protected void txtDOB_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtDOB.Text, out DateTime dob))
            {
                // Ensure the selected date is not in the future
                if (dob > DateTime.Today)
                {
                    txtAge.Text = "Date of birth cannot be in the future.";
                    txtAge.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Get today's date
                DateTime today = DateTime.Today;

                // Calculate the initial age
                int age = today.Year - dob.Year;

                // Adjust the age if the birthday hasn't happened yet this year
                if (dob.Date > today.AddYears(-age))
                {
                    age--;
                }

                // Display the final age in the label
                txtAge.Text = $"{age} years";
                txtAge.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                // If the input is empty or invalid, clear the label or show a message
                if (string.IsNullOrWhiteSpace(txtDOB.Text))
                {
                    txtAge.Text = ""; // Clear if user erases the date
                }
                else
                {
                    txtAge.Text = "Please enter a valid date.";
                    txtAge.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private void LoadDistricts()
        {
            DataSet dsDistrict = new DataSet();
            dsDistrict.ReadXml(Server.MapPath("Districts.xml"));

            ddldist.DataSource = dsDistrict;
            ddldist.DataTextField = "DistrictName";
            ddldist.DataValueField = "DistrictId";
            ddldist.DataBind();

            ddldist.Items.Insert(0, new ListItem("---Select District---", "-1"));
           // ddltaluka.Items.Insert(0, new ListItem("---Select Taluka---", "-1"));
        }
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