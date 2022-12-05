using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;

namespace BLL.SystemTools
{
    public class BLDBTools
    {
        #region Language
        public static void ImportDataFromExcel(string excelFilePath)
        {
            //declare variables - edit these based on your particular situation
            string sqlExcelTable = "ExcelDictionary";
            // make sure your sheet name is correct, here sheet name is sheet1, so you can change your sheet name if have different
            string excelLocalLanguageQuery = "select * from [Dictionary$]";
            try
            {
                //create our connection strings
                string excelConnectionString = @"provider=microsoft.jet.oledb.4.0;data source=" + excelFilePath +
                ";extended properties=" + "\"excel 8.0;hdr=yes;\"";

                string sqlConnectionString = ConfigurationManager.ConnectionStrings["IdentityDbContext"].ConnectionString;



                string createExcelTableQuery = "DROP TABLE " + sqlExcelTable + " \n" +
                                                "SET ANSI_NULLS ON \n" +
                                                "SET QUOTED_IDENTIFIER ON \n" +
                                                "CREATE TABLE " + sqlExcelTable + " ( \n" +
                                                "[Id][int] NULL, \n";

                var activeLanguage = new BLLanguage().GetActiveLanguages();
                foreach (var activeLang in activeLanguage)
                {
                    createExcelTableQuery += "[" + activeLang.CultureInfo + "]" + "[nvarchar](255) NULL, \n";
                }
                createExcelTableQuery += "[Orig]" + "[nvarchar](255) NULL, \n";

                createExcelTableQuery += ") ON[PRIMARY]";

                string sqlClrealQueries =
                                    createExcelTableQuery + "\n" +
                                    "delete " + sqlExcelTable + "\n" +
                                    "delete[dbo].[Dictionary]" + "\n" +
                                    "delete[dbo].[RefrenceWord]";

                SqlConnection sqlconn = new SqlConnection(sqlConnectionString);
                SqlCommand sqlcmd = new SqlCommand(sqlClrealQueries, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close();

                OleDbConnection oledbconn = new OleDbConnection(excelConnectionString);

                DataTable dtExcelData = new DataTable();

                using (OleDbDataAdapter oda = new OleDbDataAdapter(excelLocalLanguageQuery, oledbconn))
                {
                    oda.Fill(dtExcelData);
                }

                oledbconn.Close();

                using (SqlConnection con = new SqlConnection(sqlConnectionString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = sqlExcelTable;

                        sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                        sqlBulkCopy.ColumnMappings.Add("Orig", "Orig");

                        foreach (var activeLang in activeLanguage)
                        {
                            sqlBulkCopy.ColumnMappings.Add(activeLang.CultureInfo, activeLang.CultureInfo);
                        }

                        con.Open();
                        sqlBulkCopy.WriteToServer(dtExcelData);
                        con.Close();
                    }
                }

                string sqlFillDictionaryQueries =
                                   "insert [dbo].[RefrenceWord] (Id, Word) select Id, [Orig] from " + sqlExcelTable + "\n";

                foreach (var activeLang in activeLanguage)
                {

                    sqlFillDictionaryQueries +=
                                    "insert [dbo].[Dictionary] (CultureInfoCode, RefrenceWordId, Value) select '" + activeLang.CultureInfo + "', Id, [" + activeLang.CultureInfo + "] from " + sqlExcelTable + "\n";

                }

                sqlconn = new SqlConnection(sqlConnectionString);
                sqlcmd = new SqlCommand(sqlFillDictionaryQueries, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close();
            }
            catch
            {
                //handle exception
            }
        }
        #endregion

    }
}
