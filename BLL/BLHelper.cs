using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

namespace BLL
{
    public static class BLHelper
    {
        public static int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string GeneratePassword(int passwordLength)
        {
            string allowedChars = "abcdefghijklmnopqrstuvwxyz";
            string allowedUpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string allowedNoneDigitChars = "!@#$%&*_-+=";
            string allowedDigitChars = "0123456789";
            char[] chars = new char[passwordLength];

            Random random = new Random(Environment.TickCount);

            int i = 0;

            for (i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            int[] randomIndexs = new int[3];

            randomIndexs = GenerateUniqueRandomNumbers(3, 0, 5);

            chars[randomIndexs[0]] = allowedUpperChars[random.Next(0, allowedUpperChars.Length)];
            chars[randomIndexs[1]] = allowedNoneDigitChars[random.Next(0, allowedNoneDigitChars.Length)];
            chars[randomIndexs[2]] = allowedDigitChars[random.Next(0, allowedDigitChars.Length)];

            return new string(chars);
        }
        public static int[] GenerateUniqueRandomNumbers(int randomCount, int min, int max)
        {

            Random random = new Random(Environment.TickCount);

            int[] randomArray = new int[randomCount];

            int tempMin = 0;
            int tempMax = 0;
            int numbCount = max - min + 1;
            int minMaxIndex = 0;
            int partCount = numbCount / randomCount;

            double divResult = (double)numbCount / randomCount;

            if (divResult - Math.Truncate(divResult) > 0)
            {
                partCount++;
            }

            for (int i = 0; i < numbCount; i += partCount)
            {
                tempMin = min;
                tempMax = min + partCount - 1;
                min = tempMax + 1;
                minMaxIndex++;

                if (minMaxIndex == randomCount)
                {
                    tempMax = max;
                }

                randomArray[minMaxIndex - 1] = random.Next(tempMin, tempMax);
            }

            return randomArray;
        }

        public static int SendSMS(string receptor, string message)
        {
            //var sender = "100065995";

            //var api = new Kavenegar.KavenegarApi("4738656B564552554A57726B6C61523537476B7733556430375736765256415A");

            //var sender = "100065995";
            //var api = new Kavenegar.KavenegarApi("666234496E7478745430446539574F6B776E446F5A694D7A7A70316679443832");

            var sender = "100065995";
            var api = new Kavenegar.KavenegarApi("4C69472B665677527241736C6B2F65382B444B4733667655553471384D483343");

            return api.Send(sender, receptor, message).Status;
        }
    }

    public static class ExcelHandler
    {
        public static XLWorkbook ExportDataSetToExcel(DataSet ds, string excelFileName, params DataTable[] otherDataTables)
        {
            XLWorkbook xlWorkbook = new XLWorkbook();
            DataTable dt = ds.Tables[0];

            var ws = xlWorkbook.Worksheets.Add(dt, "DataSheet");
            int rowIndex = dt.Rows.Count + 5;

            if (otherDataTables.Count() > 0)
            {
                foreach (var odt in otherDataTables)
                {
                    xlWorkbook.Worksheet(1).Cell(rowIndex, 2).InsertTable(odt);
                    rowIndex += odt.Rows.Count + 3;
                }
            }

            return xlWorkbook;
        }
        public static DataTable ExportExcelToDataTable(string excelFileName)
        {
            DataTable dt = new DataTable();


            using (XLWorkbook workBook = new XLWorkbook(excelFileName))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString(), typeof(string));
                        }

                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }

                    }

                }

            }

            return dt;

        }

        public static DataTable ConvertExcelToDataTable(string FileName)
        {
            DataTable dtResult = null;
            int totalSheet = 0; //No of sheets on excel file  
            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                                                                FileName + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tempDataTable = (from dataRow in dt.AsEnumerable()
                                         where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                         select dataRow).CopyToDataTable();
                    dt = tempDataTable;
                    totalSheet = dt.Rows.Count;
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();

                return dtResult; //Returning Dattable  
            }
        }

        public static DataTable CreateExcelBaseDataTable(DataTable dt, params string[] columnNames)
        {
            DataTable newTable = new DataTable();

            DataView view = new DataView(dt);

            newTable = view.ToTable(false, columnNames);

            return newTable;
        }

        /// <summary>
        /// Convert a List{T} to a DataTable.
        /// </summary>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }


            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }


            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

    }

}
