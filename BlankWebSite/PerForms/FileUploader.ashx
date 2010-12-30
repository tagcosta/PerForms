<%@ WebHandler Language="C#" Class="FileUploader" %>

using System;
using System.Web;
using System.IO;
using PerForms.Util;

public class FileUploader : IHttpHandler
{
    private const string path = "PerForms\\Uploads\\";
    
    private string GetNewFileNameKey()
    {
        string fileNameKey = "";
        bool fileExists = true;
        while (fileExists)
        {
            fileNameKey = Guid.NewGuid().ToString("N");
            fileExists = File.Exists(new Paths().GetRootPath() + path + fileNameKey);
        }
        return fileNameKey;
    }


    public void ProcessRequest(HttpContext context)
    {
        string result = "{\"success\":false}";
        string saveLocation = string.Empty;
        string fileName = string.Empty;
        string guid = string.Empty;

        if (new Custom_PerFormsService().IsAuthenticated())
        {
            int length = 1024;
            int bytesRead = 0;
            Byte[] buffer = new Byte[length];

            //This works with Chrome/FF/Safari
            fileName = context.Request.QueryString["qqfile"];
            if (!string.IsNullOrEmpty(fileName))
            {
                guid = GetNewFileNameKey();
                saveLocation = new Paths().GetRootPath() + path + guid;

                using (FileStream fileStream = new FileStream(saveLocation, FileMode.Create))
                {
                    do
                    {
                        bytesRead = context.Request.InputStream.Read(buffer, 0, length);
                        fileStream.Write(buffer, 0, bytesRead);    
                    }
                    while (bytesRead > 0);
                    
                    result = "{\"success\":true, \"guid\":'" + guid + "', \"filename\":'" + fileName + "'}";
                }
            }
            else
            {
                //This works with IE
                try
                {
                    fileName = Path.GetFileName(context.Request.Files[0].FileName);
                    guid = GetNewFileNameKey();
                    saveLocation = new Paths().GetRootPath() + path + guid;
                    context.Request.Files[0].SaveAs(saveLocation);

                    result = "{\"success\":true, \"guid\":'" + guid + "', \"filename\":'" + fileName + "'}";
                }
                catch { }
            }
        }

        context.Response.Write(result);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}