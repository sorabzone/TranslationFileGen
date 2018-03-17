using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace TranslationFileGen
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string connString = "Data Source=C:\\TFG\\TranslationFileGen\\TranslationFileGen\\App_Data\\TranslationData.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            ErroMsg.Text = "";
        }

        protected void btnTranslate_Click(object sender, EventArgs e)
        {
            SQLiteDataReader objReader = null;
            SQLiteCommand cmd = null;
            try
            {
                ErroMsg.Text = "";
                string filecontent = Convert.ToBase64String(uploadFile.FileBytes);

                if (Path.GetExtension(uploadFile.FileName).Equals(".xlsx"))
                {
                    var excel = new ExcelPackage(uploadFile.FileContent);
                    var dt = excel.ToDataTable();

                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            using (var transaction = conn.BeginTransaction())
                            {
                                StringBuilder sSQL = new StringBuilder();
                                sSQL.Append("CREATE TEMPORARY TABLE tblInventory ( ");
                                sSQL.Append("SKU varchar(8) UNIQUE NOT NULL, ");
                                sSQL.Append("EnglishName TEXT  NOT NULL, ");
                                sSQL.Append("EnglishDescription TEXT  NOT NULL, ");
                                sSQL.Append("InStock INT NOT NULL, ");
                                sSQL.Append("CategoryCode varchar(20) NOT NULL, ");
                                sSQL.Append("Status varchar(20) NOT NULL, ");
                                sSQL.Append("BrandName TEXT  NOT NULL, ");
                                sSQL.Append("ProductUrl TEXT  NOT NULL );");

                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                foreach (DataRow row in dt.Rows)
                                {
                                    sSQL = new StringBuilder();
                                    sSQL.Append("INSERT INTO tblInventory (SKU, EnglishName, EnglishDescription, InStock, CategoryCode, Status, BrandName, ProductUrl) VALUES ( ");
                                    sSQL.Append("@SKU, @EnglishName, @EnglishDescription, @InStock, @CategoryCode, @Status, @BrandName, @ProductUrl");
                                    sSQL.Append("); ");

                                    cmd.Parameters.AddWithValue("@SKU", Convert.ToString(row["Product ID"]));
                                    cmd.Parameters.AddWithValue("@EnglishName", Convert.ToString(row["English Name"]));
                                    cmd.Parameters.AddWithValue("@EnglishDescription", Convert.ToString(row["English Description"]));
                                    cmd.Parameters.AddWithValue("@InStock", Convert.ToString(row["In-Stock"]));
                                    cmd.Parameters.AddWithValue("@CategoryCode", Convert.ToString(row["Category Code"]));
                                    cmd.Parameters.AddWithValue("@Status", Convert.ToString(row["Status"]));
                                    cmd.Parameters.AddWithValue("@BrandName", Convert.ToString(row["Brand Name"]));
                                    cmd.Parameters.AddWithValue("@ProductUrl", Convert.ToString(row["Product URL"]));

                                    cmd.CommandText = sSQL.ToString();
                                    cmd.ExecuteNonQuery();
                                }


                                cmd.Parameters.Clear();
                                #region Create Translation Table
                                sSQL = new StringBuilder();
                                sSQL.Append("CREATE TEMPORARY TABLE tblTranslations (  \n");
                                sSQL.Append("CategoryCode varchar(20) NULL, CategoryEngNm TEXT NULL, CategoryChineseNm TEXT NULL,  \n");
                                sSQL.Append("SubCategoryCode varchar(20) NULL,SubCategoryEngNm TEXT NULL, SubCategoryChineseNm TEXT NULL,  \n");
                                sSQL.Append("SKU varchar(8) NOT NULL, \n");
                                sSQL.Append("ProductEngNm TEXT NULL, ProductChineseNm TEXT NULL, ProductEngDesc TEXT NULL,  \n");
                                sSQL.Append("ProductChineseDesc TEXT NULL, BrandEngNm TEXT NULL, BrandChineseNm TEXT NULL,  \n");
                                sSQL.Append("SearchKeyWordNmEng TEXT NULL, SearchKeyWordNmChinese TEXT NULL, MetaTagEng TEXT NULL,  \n");
                                sSQL.Append("MetaTagChinese TEXT NULL, ProductImageURL TEXT NULL, ProductDetailURL TEXT NULL,  \n");
                                sSQL.Append("PaydImageId TEXT NULL, ProductID TEXT NULL, Language TEXT NULL );  \n");

                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 2.	Insert items into Translation Table from Inventory Table
                                sSQL = new StringBuilder();
                                sSQL.Append("INSERT INTO tblTranslations(SKU, ProductEngNm, ProductEngDesc, ProductDetailURL, ProductID, BrandEngNm, Language) \n");
                                sSQL.Append("SELECT SKU, EnglishName, EnglishDescription, ProductUrl, SUBSTR(ProductUrl,33,7), BrandName, \n");
                                sSQL.Append("CASE WHEN SUBSTR(EnglishName,1,2)='CB' THEN 'ZH' WHEN SUBSTR(EnglishName,1,3)='CCD' THEN 'ZH' \n");
                                sSQL.Append("WHEN SUBSTR(EnglishName,1,4)='CDVD' THEN 'ZH' ELSE 'EN' END \n");
                                sSQL.Append("FROM tblInventory ");

                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 3.	Filled Image Id and Image Link in Translation Table by SKU
                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET PaydImageId = (SELECT Image_Id FROM tblSKU_ImageID WHERE tblSKU_ImageID.SKU = tblTranslations.SKU)");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET ProductImageURL = (SELECT 'http://bookstore.fll.cc/img/product/id=' || Image_Id FROM tblSKU_ImageID WHERE tblSKU_ImageID.SKU = tblTranslations.SKU)");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 4.	Filled items' Chinese Translations in Translation Table by SKU
                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET ProductChineseNm = (SELECT ChineseName FROM tblSKU_Chinese WHERE tblSKU_Chinese.SKU = tblTranslations.SKU)");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET ProductChineseDesc = (SELECT ChineseDesc FROM tblSKU_Chinese WHERE tblSKU_Chinese.SKU = tblTranslations.SKU)");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 5.	Filled Category Info in Translation Table by SKU and CategoryCode
                                StringBuilder sSQL2 = new StringBuilder();
                                sSQL2.Append("(SELECT SKU , Cat01, '' AS Cat02 FROM (SELECT SKU , CategoryCode, \n ");
                                sSQL2.Append("CASE INSTR(CategoryCode,'|') WHEN 0 THEN CategoryCode ELSE SUBSTR(CategoryCode,1,INSTR(CategoryCode,'|')-1) END AS SubCategory FROM tblinventory) A \n ");
                                sSQL2.Append("LEFT JOIN (SELECT * FROM tblCategory_Raw) B ON SubCategory = Code LEFT JOIN (SELECT distinct Cat00 , Cat01, Level FROM tblCategory) t1 ON Code = t1.Cat01 WHERE t1.Level = 1 \n ");

                                sSQL2.Append("UNION \n ");

                                sSQL2.Append("SELECT SKU , Cat01, Cat02 FROM (SELECT SKU , CategoryCode, \n ");
                                sSQL2.Append("CASE INSTR(CategoryCode,'|') WHEN 0 THEN CategoryCode ELSE SUBSTR(CategoryCode,1,INSTR(CategoryCode,'|')-1) END AS SubCategory FROM tblinventory ) A \n ");
                                sSQL2.Append("LEFT JOIN (SELECT * FROM tblCategory_Raw) B ON SubCategory = Code LEFT JOIN (SELECT distinct Cat00 , Cat01, Cat02, Level FROM tblCategory) t2 ON Code = t2.Cat02 WHERE t2.Level = 2 \n ");

                                sSQL2.Append("UNION \n ");

                                sSQL2.Append("SELECT SKU , Cat01, Cat02 FROM (SELECT SKU , CategoryCode, \n ");
                                sSQL2.Append("CASE INSTR(CategoryCode,'|') WHEN 0 THEN CategoryCode ELSE SUBSTR(CategoryCode,1,INSTR(CategoryCode,'|')-1) END AS SubCategory FROM tblinventory ) A \n ");
                                sSQL2.Append("LEFT JOIN (SELECT * FROM tblCategory_Raw) B ON SubCategory = Code LEFT JOIN tblCategory ON Code = Cat03 WHERE tblCategory.Level = 3 \n ");
                                sSQL2.Append(") T3 \n");

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE	tblTranslations SET CategoryCode = (SELECT T3.Cat01 FROM " + sSQL2.ToString() + " WHERE T3.SKU = tblTranslations.SKU);");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE	tblTranslations SET SubCategoryCode = (SELECT T3.Cat02 FROM " + sSQL2.ToString() + " WHERE T3.SKU = tblTranslations.SKU);");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET CategoryEngNm = (SELECT Category FROM tblCategory_Raw WHERE tblCategory_Raw.Code = tblTranslations.CategoryCode);");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET CategoryChineseNm = (SELECT ChineseName FROM tblMetadata WHERE tblMetadata.EnglishName = tblTranslations.CategoryEngNm);");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET SubCategoryEngNm = (SELECT Category FROM tblCategory_raw WHERE tblCategory_raw.Code = tblTranslations.SubCategoryCode);");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();

                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations SET SubCategoryChineseNm = (SELECT ChineseName FROM tblMetadata WHERE tblMetadata.EnglishName = tblTranslations.SubCategoryEngNm);");
                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 6.	Producing final output file: Exporting the Translation table to an Excel File
                                sSQL = new StringBuilder();
                                sSQL.Append("SELECT IFNULL(CategoryCode,''),IFNULL(CategoryEngNm,''),IFNULL(CategoryChineseNm,''),IFNULL(SubCategoryCode,''),IFNULL(SubCategoryEngNm,'') \n ");
                                sSQL.Append(",IFNULL(SubCategoryChineseNm,''),IFNULL(SKU,''),IFNULL(ProductEngNm,''),IFNULL(ProductChineseNm,''),IFNULL(ProductEngDesc,'') \n ");
                                sSQL.Append(",IFNULL(ProductChineseDesc,''),IFNULL(BrandEngNm,''),IFNULL(BrandChineseNm,''),IFNULL(SearchKeyWordNmEng,'') \n ");
                                sSQL.Append(",IFNULL(SearchKeyWordNmChinese,''),IFNULL(MetaTagEng,''),IFNULL(MetaTagChinese,''),IFNULL(ProductImageURL,'') \n ");
                                sSQL.Append(",IFNULL(ProductDetailURL,''),IFNULL(PaydImageId,''),IFNULL(ProductID,''),IFNULL(Language,'') FROM tblTranslations; \n ");

                                cmd.CommandText = sSQL.ToString();
                                objReader = cmd.ExecuteReader();

                                //Test
                                DataTable dts = new DataTable();
                                dts.Load(objReader); // <-- FormatException

                                ExcelPackage excelExport = new ExcelPackage();
                                var workSheet = excelExport.Workbook.Worksheets.Add("Transation");
                                workSheet.TabColor = System.Drawing.Color.Black;
                                workSheet.DefaultRowHeight = 12;
                                //Header of table  
                                workSheet.Row(1).Height = 20;
                                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Row(1).Style.Font.Bold = true;
                                workSheet.Cells[1, 1].Value =  dts.Columns[0].ColumnName.Substring(7, dts.Columns[0].ColumnName.Length - 11);
                                workSheet.Cells[1, 2].Value = dts.Columns[1].ColumnName.Substring(7, dts.Columns[1].ColumnName.Length - 11);
                                workSheet.Cells[1, 3].Value = dts.Columns[2].ColumnName.Substring(7, dts.Columns[2].ColumnName.Length - 11);
                                workSheet.Cells[1, 11].Value = dts.Columns[3].ColumnName.Substring(7, dts.Columns[3].ColumnName.Length - 11);
                                workSheet.Cells[1, 5].Value = dts.Columns[4].ColumnName.Substring(7, dts.Columns[4].ColumnName.Length - 11);
                                workSheet.Cells[1, 6].Value = dts.Columns[5].ColumnName.Substring(7, dts.Columns[5].ColumnName.Length - 11);
                                workSheet.Cells[1, 7].Value = dts.Columns[6].ColumnName.Substring(7, dts.Columns[6].ColumnName.Length - 11);
                                workSheet.Cells[1, 8].Value = dts.Columns[7].ColumnName.Substring(7, dts.Columns[7].ColumnName.Length - 11);
                                workSheet.Cells[1, 9].Value = dts.Columns[8].ColumnName.Substring(7, dts.Columns[8].ColumnName.Length - 11);
                                workSheet.Cells[1, 10].Value = dts.Columns[9].ColumnName.Substring(7, dts.Columns[9].ColumnName.Length - 11);
                                workSheet.Cells[1, 11].Value = dts.Columns[10].ColumnName.Substring(7, dts.Columns[10].ColumnName.Length - 11);
                                workSheet.Cells[1, 12].Value = dts.Columns[11].ColumnName.Substring(7, dts.Columns[11].ColumnName.Length - 11);
                                workSheet.Cells[1, 13].Value = dts.Columns[12].ColumnName.Substring(7, dts.Columns[12].ColumnName.Length - 11);
                                workSheet.Cells[1, 111].Value = dts.Columns[13].ColumnName.Substring(7, dts.Columns[13].ColumnName.Length - 11);
                                workSheet.Cells[1, 15].Value = dts.Columns[14].ColumnName.Substring(7, dts.Columns[14].ColumnName.Length - 11);
                                workSheet.Cells[1, 16].Value = dts.Columns[15].ColumnName.Substring(7, dts.Columns[15].ColumnName.Length - 11);
                                workSheet.Cells[1, 17].Value = dts.Columns[16].ColumnName.Substring(7, dts.Columns[16].ColumnName.Length - 11);
                                workSheet.Cells[1, 18].Value = dts.Columns[17].ColumnName.Substring(7, dts.Columns[17].ColumnName.Length - 11);
                                workSheet.Cells[1, 19].Value = dts.Columns[18].ColumnName.Substring(7, dts.Columns[18].ColumnName.Length - 11);
                                workSheet.Cells[1, 20].Value = dts.Columns[19].ColumnName.Substring(7, dts.Columns[19].ColumnName.Length - 11);
                                workSheet.Cells[1, 21].Value = dts.Columns[20].ColumnName.Substring(7, dts.Columns[20].ColumnName.Length - 11);
                                workSheet.Cells[1, 22].Value = dts.Columns[21].ColumnName.Substring(7, dts.Columns[21].ColumnName.Length - 11);

                                //Body of table  
                                int recordIndex = 2;
                                foreach (DataRow record in dts.Rows)
                                {
                                    workSheet.Cells[recordIndex, 1].Value = Convert.ToString(record[0]);
                                    workSheet.Cells[recordIndex, 2].Value = Convert.ToString(record[1]);
                                    workSheet.Cells[recordIndex, 3].Value = Convert.ToString(record[2]);
                                    workSheet.Cells[recordIndex, 4].Value = Convert.ToString(record[3]);
                                    workSheet.Cells[recordIndex, 5].Value = Convert.ToString(record[4]);

                                    workSheet.Cells[recordIndex, 6].Value = Convert.ToString(record[5]);
                                    workSheet.Cells[recordIndex, 7].Value = Convert.ToString(record[6]);
                                    workSheet.Cells[recordIndex, 8].Value = Convert.ToString(record[7]);
                                    workSheet.Cells[recordIndex, 9].Value = Convert.ToString(record[8]);
                                    workSheet.Cells[recordIndex, 10].Value = Convert.ToString(record[9]);

                                    workSheet.Cells[recordIndex, 11].Value = Convert.ToString(record[10]);
                                    workSheet.Cells[recordIndex, 12].Value = Convert.ToString(record[11]);
                                    workSheet.Cells[recordIndex, 13].Value = Convert.ToString(record[12]);
                                    workSheet.Cells[recordIndex, 14].Value = Convert.ToString(record[13]);
                                    workSheet.Cells[recordIndex, 15].Value = Convert.ToString(record[14]);

                                    workSheet.Cells[recordIndex, 16].Value = Convert.ToString(record[15]);
                                    workSheet.Cells[recordIndex, 17].Value = Convert.ToString(record[16]);
                                    workSheet.Cells[recordIndex, 18].Value = Convert.ToString(record[17]);
                                    workSheet.Cells[recordIndex, 19].Value = Convert.ToString(record[18]);
                                    workSheet.Cells[recordIndex, 20].Value = Convert.ToString(record[19]);

                                    workSheet.Cells[recordIndex, 21].Value = Convert.ToString(record[20]);
                                    workSheet.Cells[recordIndex, 22].Value = Convert.ToString(record[21]);
                                    recordIndex++;
                                }
                                workSheet.Column(1).AutoFit();
                                workSheet.Column(2).AutoFit();
                                workSheet.Column(3).AutoFit();
                                workSheet.Column(4).AutoFit();
                                workSheet.Column(5).AutoFit();
                                workSheet.Column(6).AutoFit();
                                workSheet.Column(7).AutoFit();
                                workSheet.Column(8).AutoFit();
                                workSheet.Column(9).AutoFit();
                                workSheet.Column(10).AutoFit();
                                workSheet.Column(11).AutoFit();
                                workSheet.Column(12).AutoFit();
                                workSheet.Column(13).AutoFit();
                                workSheet.Column(14).AutoFit();
                                workSheet.Column(15).AutoFit();
                                workSheet.Column(16).AutoFit();
                                workSheet.Column(17).AutoFit();
                                workSheet.Column(18).AutoFit();
                                workSheet.Column(19).AutoFit();
                                workSheet.Column(20).AutoFit();
                                workSheet.Column(21).AutoFit();
                                workSheet.Column(22).AutoFit();

                                string excelName = "Translation";
                                using (var memoryStream = new MemoryStream())
                                {
                                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                                    excelExport.SaveAs(memoryStream);
                                    memoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }

                                #endregion

                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
                }
                else
                    ErroMsg.Text = "Please select valid excel file.";
            }
            catch (Exception ex)
            {
                ErroMsg.Text = ex.Message + "\n Stack Trace: " + ex.StackTrace;
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (objReader != null)
                    objReader = null;
            }
        }
    }
}