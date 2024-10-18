using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace VisitorsManagement
{
    //test commit
    public static class DB
    {
        private static SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static bool ExecuteNonQuery(string CommandName, CommandType cmdType, SqlParameter[] pars = null)
        {
            SqlCommand cmd = null;
            int res = 0;
            cmd = con.CreateCommand();

            cmd.CommandType = cmdType;
            cmd.CommandText = CommandName;
            if (pars != null)
            {
                cmd.Parameters.AddRange(pars);
            }

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                res = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                con.Close();
            }

            if (res >= 1)
            {
                return true;
            }
            return false;
        }
        public static string ExecuteNonQueryReturnPK(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {
            SqlCommand cmd = null;
            string res = string.Empty;
            cmd = con.CreateCommand();

            cmd.CommandType = cmdType;
            cmd.CommandText = CommandName;
            cmd.Parameters.AddRange(pars);
            cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.ExecuteNonQuery();
                res = cmd.Parameters["@id"].Value.ToString();
                //lblMessage.Text = "Record inserted successfully. ID = " + id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                con.Close();
            }

            return res;
        }

        public static string ExecuteNonQueryReturnOutParameter(string CommandName, CommandType cmdType, string outParam, SqlDbType type, int size = 0, SqlParameter[] pars = null)
        {
            SqlCommand cmd = null;
            string res = string.Empty;
            cmd = con.CreateCommand();

            cmd.CommandType = cmdType;
            cmd.CommandText = CommandName;

            if (pars != null)
                cmd.Parameters.AddRange(pars);
            if (size > 0)
                cmd.Parameters.Add(outParam, type, size).Direction = ParameterDirection.Output;
            else
                cmd.Parameters.Add(outParam, type).Direction = ParameterDirection.Output;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.ExecuteNonQuery();
                res = cmd.Parameters[outParam].Value.ToString();
                //lblMessage.Text = "Record inserted successfully. ID = " + id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                con.Close();
            }

            return res;
        }
        public static DataTable ExecuteSelectCommand(string CommandName, CommandType cmdType)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(CommandName, connection))
                {
                    cmd.CommandType = cmdType;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }

                } // command disposed here

            }

            return table;
        }

        public static string ExecuteSelectReturnSingleValue(string query, CommandType cmdType, SqlParameter[] param = null)
        {
            string val = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    if (param != null)
                        cmd.Parameters.AddRange(param);

                    cmd.CommandType = cmdType;
                    var returnValue = cmd.ExecuteScalar();

                    if (returnValue != null)
                    {
                        val = returnValue.ToString();
                    }

                }

            }

            return val;
        }

        public static DataTable ExecuteParamerizedSelectCommand(string CommandName,
                 CommandType cmdType, SqlParameter[] param = null)
        {
            //SqlCommand cmd = null;
            //DataTable table = new DataTable();

            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(CommandName, connection))
                {
                    cmd.CommandType = cmdType;

                    if (param != null)
                        cmd.Parameters.AddRange(param);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }

                } // command disposed here

            }

            //cmd = con.CreateCommand();

            //cmd.CommandType = cmdType;
            //cmd.CommandText = CommandName;
            //cmd.Parameters.AddRange(param);

            //try
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();

            //    SqlDataAdapter da = null;
            //    using (da = new SqlDataAdapter(cmd))
            //    {
            //        da.Fill(table);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    cmd.Dispose();
            //    cmd = null;
            //    con.Close();
            //}
            return table;
        }

        public static DataSet ExecuteParamerizedSelectCommandDS(string CommandName,
                 CommandType cmdType, SqlParameter[] param = null)
        {
            //SqlCommand cmd = null;
            //DataTable table = new DataTable();

            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(CommandName, connection))
                {
                    cmd.CommandType = cmdType;

                    if (param != null)
                        cmd.Parameters.AddRange(param);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }

                } // command disposed here

            }
            return ds;
        }

        public static bool ExecuteNonQueryWithTrasnction(string CommandName1, CommandType cmdType1, SqlParameter[] pars1, string CommandName2, CommandType cmdType2, SqlParameter[] pars2)
        {
            //string strConnString = "myconnectionstring"; // get it from Web.config file  
            SqlTransaction objTrans = null;
            bool res = false;

            using (SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction();

                SqlCommand cmd1 = null;
                cmd1 = objConn.CreateCommand();

                cmd1.CommandType = cmdType1;
                cmd1.CommandText = CommandName1;
                cmd1.Parameters.AddRange(pars1);

                SqlCommand cmd2 = null;

                cmd2 = objConn.CreateCommand();

                cmd2.CommandType = cmdType2;
                cmd2.CommandText = CommandName2;
                cmd2.Parameters.AddRange(pars2);

                try
                {
                    cmd1.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery(); // Throws exception due to foreign key constraint  

                    objTrans.Commit();
                    res = true;
                }
                catch (Exception)
                {
                    objTrans.Rollback();
                }
                finally
                {
                    objConn.Close();
                }
            }

            return res;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[column.ColumnName])))
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }

                        break;
                    }
                    else
                        continue;
                }
            }
            return obj;
        }

        public static DateTime getCurrentIndianDate()
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            return indianTime;
        }

        public static DataTable GetDataTableFromExceFile(string sFilePath, int iTemplateID)
        {
            DataTable dtDataTable = new DataTable();
            string sConnectionStringToImportExcelFile;
            if (sFilePath.Contains(".xlsx"))
                sConnectionStringToImportExcelFile = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sFilePath + " ; Extended Properties=Excel 12.0;";
            else
                sConnectionStringToImportExcelFile = "Provider=Microsoft.jet.OLEDB.4.0;Data Source=" + sFilePath + " ; Extended Properties=Excel 8.0;";
            System.Data.OleDb.OleDbConnection conOledbConnection = new System.Data.OleDb.OleDbConnection(sConnectionStringToImportExcelFile);
            try
            {
                conOledbConnection.Open();
                System.Data.OleDb.OleDbCommand cmdCommand;
                switch (iTemplateID)
                {
                    case 1:
                        {
                            cmdCommand = new System.Data.OleDb.OleDbCommand("SELECT TRIM(CountryCode) AS CountryCode, Date, WeightTransportedInTonnes, Type, Department FROM [Sheet1$]", conOledbConnection);
                            break;
                        }

                    default:
                        {
                            cmdCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [Sheet1$]", conOledbConnection);
                            break;
                        }
                }
                System.Data.OleDb.OleDbDataAdapter objOledbAdapter = new System.Data.OleDb.OleDbDataAdapter();
                objOledbAdapter.SelectCommand = cmdCommand;
                objOledbAdapter.Fill(dtDataTable);
                conOledbConnection.Close();
                return dtDataTable;
            }
            catch (Exception)
            {
                conOledbConnection.Close();
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
            finally
            {
                conOledbConnection.Close();
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static string ExportToExcel(DataTable dtDataTable, string fileName)
        {
            try
            {
                // If dtDataTable.Rows.Count = 0 Then Return False

                deleteOldUnusedFiles(System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\DownloadReports\");

                Collection<string> colColumnNames = new Collection<string>();

                foreach (DataColumn dcColoumnName in dtDataTable.Columns)
                    colColumnNames.Add(dcColoumnName.ColumnName.ToString());

                System.IO.StreamWriter oWrite;
                Int16 iColumnCount = 0;
                string sWrite = null;
                string sNullString = null;

                byte[] CSV;
                System.Data.DataTable _DataTable = dtDataTable.Copy();
                sWrite = "\"";


                foreach (System.Data.DataColumn _DataCol in _DataTable.Columns)
                {
                    sWrite = sWrite + colColumnNames[iColumnCount] + "\",\"";
                    iColumnCount += 1;
                    sNullString += "\"\"" + ",";
                }

                sWrite = sWrite.Substring(0, sWrite.Length - 2) + System.Environment.NewLine + "\"";

                foreach (DataRow _DataRow in _DataTable.Rows)
                {
                    foreach (System.Data.DataColumn _DataCol in _DataTable.Columns)
                        sWrite = sWrite + _DataRow[_DataCol] + "\",\"";
                    sWrite = sWrite.Substring(0, sWrite.Length - 2) + System.Environment.NewLine + "\"";
                }

                sWrite = sWrite.Substring(0, sWrite.Length - 3);

                CSV = Encoding.GetEncoding(1252).GetBytes(sWrite);
                sWrite = null;
                sWrite = Encoding.GetEncoding(1252).GetString(CSV);

                string FileName = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\DownloadReports\" + fileName + ".CSV";

                if (System.IO.File.Exists(FileName))
                    System.IO.File.Delete(FileName);


                oWrite = System.IO.File.CreateText(FileName);
                oWrite.WriteLine(sWrite);

                oWrite.Flush();
                oWrite.Close();


                return "true";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void deleteOldUnusedFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                    fi.Delete();
            }
        }

        private static string GetQuater(int iMonthID)
        {
            switch ((iMonthID))
            {
                case 1:
                case 2:
                case 3:
                    {
                        return "Q1";
                    }

                case 4:
                case 5:
                case 6:
                    {
                        return "Q2";
                    }

                case 7:
                case 8:
                case 9:
                    {
                        return "Q3";
                    }

                case 10:
                case 11:
                case 12:
                    {
                        return "Q4";
                    }
            }

            // Trial Return

            return iMonthID.ToString();
        }

        public static string getYears()
        {
            //SelectList lst =
            //        new SelectList(Enumerable.Range(2019, (DateTime.Now.Year - 2019) + 1));

            int startYear = 2019;
            int currentYear = DateTime.Now.Year;

            List<int> allyears = new List<int>();

            for (int i = startYear; i <= currentYear + 1; i++)
            {
                allyears.Add(i);
            }


            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(allyears);
            return jsonString;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static string GetImageUrl()
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/MiniLogo.jpg"));
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);
            Byte[] bytes = new Byte[memoryStream.Length];
            memoryStream.Position = 0;
            memoryStream.Read(bytes, 0, (int)bytes.Length);
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            string imageUrl = "data:image/png;base64," + base64String;
            return imageUrl;
        }

        public static void SendMailAsync(string to, string body, string subject,string qrCodePath = "")
        {

            try
            {

                const string SERVER = "smtp.hvwan.net";
                MailMessage oMail = new System.Net.Mail.MailMessage();
                oMail.From = new MailAddress("NoReply@husqvarnagroup.com");
                AlternateView altView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //string img1 = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/MiniLogo.jpg");
                //string img2 = System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/MainLogo.jpg");

                //LinkedResource miniLogo = new LinkedResource(img1, MediaTypeNames.Image.Jpeg);
                //miniLogo.ContentId = "MiniLogo";
                //altView.LinkedResources.Add(miniLogo);

                //LinkedResource mainLogo = new LinkedResource(img2, MediaTypeNames.Image.Jpeg);
                //mainLogo.ContentId = "MainLogo";
                //altView.LinkedResources.Add(mainLogo);

                //if (!string.IsNullOrEmpty(qrCodePath))
                //{
                //    string img3 = System.Web.HttpContext.Current.Server.MapPath(qrCodePath);
                //    LinkedResource QRImage = new LinkedResource(img3, MediaTypeNames.Image.Jpeg);
                //    QRImage.ContentId = "QRCode";
                //    altView.LinkedResources.Add(QRImage);
                //}

                string img1 = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/Images/MiniLogo.jpg"); ///System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/MiniLogo.jpg");
                string img2 = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/Images/MainLogo.jpg"); //System.Web.HttpContext.Current.Server.MapPath("~/Content/Images/MainLogo.jpg");

                LinkedResource miniLogo = new LinkedResource(img1, MediaTypeNames.Image.Jpeg);
                miniLogo.ContentId = "MiniLogo";
                altView.LinkedResources.Add(miniLogo);

                LinkedResource mainLogo = new LinkedResource(img2, MediaTypeNames.Image.Jpeg);
                mainLogo.ContentId = "MainLogo";
                altView.LinkedResources.Add(mainLogo);

                if (!string.IsNullOrEmpty(qrCodePath))
                {
                    string img3 = Path.Combine(HttpRuntime.AppDomainAppPath, qrCodePath);
                    LinkedResource QRImage = new LinkedResource(img3, MediaTypeNames.Image.Jpeg);
                    QRImage.ContentId = "QRCode";
                    altView.LinkedResources.Add(QRImage);
                }

                oMail.To.Add(to);
                oMail.Subject = subject;
                oMail.IsBodyHtml = true; // enumeration
                oMail.Priority = MailPriority.High; // enumeration
                //oMail.Body = body;
                oMail.AlternateViews.Add(altView);
                SmtpClient Client = new SmtpClient(SERVER);
                Client.UseDefaultCredentials = true;
                Client.Port = 25;
                //Client.Credentials = new System.Net.NetworkCredential("info@intuitiveinfotech.in", "T@ksh@8719");
                Client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                Client.Send(oMail);
            }
            catch (Exception ex)
            {
                insertErrorlog("DB", "SendMailAsync", ex.Message, 0, ex.StackTrace);
            }
        }

        public static string PopulateBody(string userNames, string ReportingGroup, string MonthYear)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/MailBody.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userNames);
            body = body.Replace("{ReportingGroup}", ReportingGroup);
            body = body.Replace("{MonthYear}", MonthYear);
            return body;
        }

        public static string HexAsciiConvert(string hex)

        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hex.Length - 2; i += 2)

            {

                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2),

                System.Globalization.NumberStyles.HexNumber))));

            }

            return sb.ToString();

        }

        public enum ReportingGroups
        {
            DirectEnergy = 1,
            Transportation = 4,
            General = 5,
            Water = 7,
            Finance = 8,
            BusinessTravel = 10,
            WasteByType = 11,
            IndirectEnergy = 13
        }

        //public DataSet Getrecord()
        //{
        //    SqlCommand com = new SqlCommand("Sp_InsertData", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;

        //}


        public class SiteKeys
        {
            public static string StyleVersion
            {
                get
                {
                    return "<link href=\"{0}?v=" + ConfigurationManager.AppSettings["version"] + "\" rel=\"stylesheet\"/>";
                }
            }
            public static string ScriptVersion
            {
                get
                {
                    return "<script src=\"{0}?v=" + ConfigurationManager.AppSettings["version"] + "\"></script>";
                }
            }
        }

        public static string encrypt(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string Encode(string encodeMe)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(encodeMe);
            return Convert.ToBase64String(encoded);
        }

        public static string Decode(string decodeMe)
        {
            byte[] encoded = Convert.FromBase64String(decodeMe);
            return System.Text.Encoding.UTF8.GetString(encoded);
        }

        public static void insertErrorlog(string controllerName, string methodName, string errorMessage, int UserID,string stackTrace = "")
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("ControllerName",controllerName),
                new SqlParameter("MethodName",methodName),
                new SqlParameter("ErrorMessage",errorMessage),
                new SqlParameter("UserID",UserID),
                new SqlParameter("StackTrace",stackTrace)
            };

            string query = @"INSERT INTO tbl_ErrorLog
                           (ControllerName,MethodName,ErrorMessage,StackTrace,AddedOn,UserID)
                            VALUES
                           (@ControllerName,@MethodName,@ErrorMessage,@StackTrace,GETDATE(),@UserID)";

            DB.ExecuteNonQuery(query, CommandType.Text, param);
        }

        public static string GetAbsoluteUrl(string page)
        {
            if (HttpContext.Current == null)
            {
                return page;
            }
            else if (VirtualPathUtility.IsAbsolute(page))
            {
                return
                  HttpContext.Current.Request.Url.Scheme + "://"
                  + HttpContext.Current.Request.Url.Authority
                  + HttpContext.Current.Request.ApplicationPath
                  + page;
            }
            else
            {
                return
                  HttpContext.Current.Request.Url.Scheme + "://"
                  + HttpContext.Current.Request.Url.Authority
                  + VirtualPathUtility.ToAbsolute(page);
            }
        }

        public static bool ColumnExists(string columnName, DataTable table)
        {

            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                return true;
            }
            return false;
        }


        public static string getModelErrors(ICollection<ModelState> modelState)
        {
            string errorMessage = string.Empty;
            foreach (ModelState modelstate in modelState)
            {
                foreach (ModelError error in modelstate.Errors)
                {
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = error.ErrorMessage;
                    else
                        errorMessage = errorMessage + "<br/>" + error.ErrorMessage;
                }
            }

            return errorMessage;
        }


        public static Dictionary<string, string> getDropDown(string key)
        {
            Dictionary<string, string> lst = new Dictionary<string, string>();

            DataTable dt = new DataTable();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@dGroup",key)
            };
            dt = DB.ExecuteParamerizedSelectCommand("SP_GetDropDownList", CommandType.StoredProcedure, param);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    lst.Add(dt.Rows[i]["dKey"].ToString(), dt.Rows[i]["dValue"].ToString());
                }
            }
            return lst;
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }

        public static string ContainColumn(string[] columnName, DataTable table)
        {
            string missingColumns = string.Empty;
            DataColumnCollection columns = table.Columns;
            for (int i = 0; i < columnName.Length; i++)
            {
                if (!columns.Contains(columnName[i]))
                {
                    if (string.IsNullOrEmpty(missingColumns))
                        missingColumns = columnName[i];
                    else
                        missingColumns = ", " + columnName[i];
                }
            }

            return missingColumns;
        }

        public enum COVINOUT
        {
            IN = 1,
            OUT = 2
        }

        public enum DocStatusID
        {
            Archieve = 1,
            Published = 2,
        }
        public enum NCNStatusID
        {
            NCN = 11,
            WaitingforSupplierAction = 12,
            WaitingforActionApproval = 13,
            CAApprovedandClosed = 14,
            Record = 15
        }

        public enum IA_Status
        {
            WaitingforInvestigation = 21,
            WaitingforClosure = 22,
            Closed = 23,
        }
        public enum Audit_Action
        {
            Required = 1,
            NotRequired = 2,
        }
        public enum Levels
        {
            Purchase = 1,
            Quality1 = 2,
            Stores1 = 3,
            Quality2 = 4,
            Stores2 = 5,
            Quality3 = 6,
            Production = 7
        }

        public enum SPCStatus
        {
            AcceptedwithDeviation = 31,
            Approved = 32,
            Completed = 33,
            InProcess = 34,
            Open = 35,
            Rejected = 36,
            UnderVerification = 37
        }

        public enum AuditStatus
        {
            Planned = 41,
            InProcess = 42,
            Complete = 43,

        }
        public enum MMStatus
        {
            Open = 11,
            InProcess = 12,
            Complete = 13,
            Close = 14,

        }

    }
}