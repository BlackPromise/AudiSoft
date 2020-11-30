using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;

namespace Server.Services
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("corsAllowPolicy")]
    [Info()]
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration _config;
        public string SessionValue = "";
        public string connection = "";

        public void LoadInfo()
        {
            SessionValue = Request?.Headers?["origin"];
            if (string.IsNullOrEmpty(SessionValue))
            {
                SessionValue = Request?.Headers["Referer,"];
            }
            SessionValue = SessionValue == "::1" ? "127.0.0.1" : SessionValue;
        }

        public BaseController(IConfiguration config)
        {
            _config = config;
            connection = _config.GetValue<string>("Connection");
        }

        public void Log(Exception ex, string MethodName = null)
        {
            if (MethodName != null)
            {
                MethodName = new StackTrace(StackTrace.METHODS_TO_SKIP + 1).GetFrame(0).GetMethod().Name.ToString();
                Log("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" + Environment.NewLine + "-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                Log(">>" + DateTime.Now.ToString() + " - " + MethodName + "");
            }
            Log(string.Format("General: [Detail:{0}], [EX:{1}] ,[InnerException:{2}] ", ex.Message, ex.ToString(), (ex.InnerException != null ? "yes." : "No.")));
            if (ex.InnerException != null)
            {
                Log(">>InnerException");
                Log(ex.InnerException, MethodName);
            }
        }
        private void Log(string Text)
        {
            try
            {
                string strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                bool IsNew = false;
                string text = "";
                string Name = "Log" + DateTime.Now.ToString("_dd_MM_yyyy_hh") + ".txt";
                string FILE_NAME = strPath + @"\" + Name;
                if (FILE_NAME[0].Equals('f'))
                {
                    FILE_NAME = FILE_NAME.Substring(6);
                }
                if (System.IO.File.Exists(FILE_NAME) == false)
                {
                    FileStream fs = System.IO.File.Create(FILE_NAME);
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                    IsNew = true;
                }
                else
                {
                    using (System.IO.StreamReader objReader = new System.IO.StreamReader(FILE_NAME))
                    {
                        text = objReader.ReadToEnd();
                        objReader.Close();
                        objReader.Dispose();
                    }
                }

                using (StreamWriter sw = new StreamWriter(FILE_NAME))
                {
                    if (IsNew)
                    {
                        sw.WriteLine("# APP");
                        sw.WriteLine("# © Copy " + DateTime.Now.Year.ToString());
                        sw.WriteLine("# Events: ");
                    }
                    else
                    {
                        sw.WriteLine(text);
                    }
                    sw.WriteLine(Text);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }

            }
            catch (Exception ex)
            {
            }
        }
    }


}
