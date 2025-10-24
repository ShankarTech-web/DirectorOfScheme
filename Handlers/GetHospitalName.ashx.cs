using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;

namespace DirectorOfScheme.Scheme
{
    public class GetHospitalName : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            string term = context.Request["term"];
            List<string> hospitalNames = new List<string>();

            if (!string.IsNullOrEmpty(term))
            {
                // Path to your XML file in App_Data
                string filePath = context.Server.MapPath("~/App_Data/HospitalList.xml");

                if (System.IO.File.Exists(filePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(filePath);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string name = row["HospitalName"].ToString(); // Make sure your XML node is <HospitalName>
                            if (name.StartsWith(term, StringComparison.OrdinalIgnoreCase))
                                hospitalNames.Add(name);
                        }
                    }
                }
            }

            // Convert list to JSON and return
            JavaScriptSerializer js = new JavaScriptSerializer();
            string json = js.Serialize(hospitalNames);
            context.Response.Write(json);
        }

        public bool IsReusable => false;
    }
}
