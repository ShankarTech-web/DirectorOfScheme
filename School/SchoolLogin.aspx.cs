using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DirectorOfScheme.School
{
    public partial class SchoolLogin : System.Web.UI.Page
    {
        string conStr = ConfigurationManager.ConnectionStrings["connectionDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateAndDisplayCaptcha();
            }


        }
        //Capchta Generate Methods
        private void GenerateAndDisplayCaptcha()
        {
            string code = GenerateCaptchaCode();
            captchaCodeLabel.Text = code;
            Session["CaptchaCode"] = code;
        }
        //Custom Characters
        private string GenerateCaptchaCode()
        {

            string characters = "ABCDEFGHJKMNOPQRSTUVWXYZ0123456789";
            string code = "";
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                code += characters[random.Next(characters.Length)];
            }

            return code;
        }
        //Valid Captcha
        private bool ValidateCaptcha(string userInput)
        {

            string storedCode = (string)Session["CaptchaCode"];
            return string.Equals(userInput, storedCode, StringComparison.OrdinalIgnoreCase);
        }

        protected void btniologin_Click(object sender, EventArgs e)
        {
            string username = txtUserId.Text.Trim();
            string password = txtPassword.Text.Trim();
            string captchaCode = txtCaptcha.Text.Trim();

            if (ValidateCaptcha(captchaCode))
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();

                    // ✅ Fetch DeptName also
                    SqlCommand cmd = new SqlCommand("SELECT SchoolCode, PassCode,SchoolName FROM SchoolInformation WHERE SchoolCode=@username AND PassCode=@password", con);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();


                        string schoolName = reader["SchoolName"].ToString();

                        string SchoolCode = reader["SchoolCode"].ToString();
                        Session["SchoolName"] = schoolName;
                        Session["SchoolCode"] = SchoolCode;

                        Session["AuthToken"] = Guid.NewGuid().ToString();

                        reader.Close();
                        con.Close();

                        // Redirect with token
                        Response.Redirect($"SchoolDashboard.aspx?token={Session["AuthToken"]}");
                    }

                    else
                    {
                        reader.Close();
                        con.Close();
                        Response.Write("<script>alert('Invalid username or password.');</script>");
                    }
                }
            }
            else
            {
                Response.Write("<script>alert('Invalid CAPTCHA code. Please try again.');</script>");
                txtCaptcha.Text = string.Empty;
                GenerateAndDisplayCaptcha();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("../HomePage.aspx");
        }

        protected void btnForgetPassword_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("ForgotPassword.aspx");
        }
    }
}