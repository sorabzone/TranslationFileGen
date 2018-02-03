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
        public string connString = "Data Source=C:\\PS\\TranslationFileGen\\TranslationFileGen\\TranslationFileGen\\App_Data\\TranslationData.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnTranslate_Click(object sender, EventArgs e)
        {
            SQLiteDataReader objReader = null;
            SQLiteCommand cmd = null;
            try
            {
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
                                    sSQL.Append("'" + Convert.ToString(row["Product ID"]) + "', ");
                                    sSQL.Append("'" + Convert.ToString(row["English Name"]) + "', ");
                                    sSQL.Append("'" + Convert.ToString(row["English Description"]) + "', ");
                                    sSQL.Append(Convert.ToString(row["In-Stock"]) + ", ");
                                    sSQL.Append("'" + Convert.ToString(row["Category Code"]) + "', ");
                                    sSQL.Append("'" + Convert.ToString(row["Status"]) + "', ");
                                    sSQL.Append("'" + Convert.ToString(row["Brand Name"]) + "', ");
                                    sSQL.Append("'" + Convert.ToString(row["Product URL"]) + "' ");
                                    sSQL.Append("); ");

                                    cmd.CommandText = sSQL.ToString();
                                    cmd.ExecuteNonQuery();
                                }



                                #region Create Translation Table
                                sSQL = new StringBuilder();
                                sSQL.Append("CREATE TEMPORARY TABLE tblTranslations (  \n");
                                sSQL.Append("CategoryCode varchar(20) NOT NULL, CategoryEngNm TEXT NOT NULL, CategoryChineseNm TEXT  NOT NULL,  \n");
                                sSQL.Append("SubCategoryCode varchar(20) NOT NULL,SubCategoryEngNm TEXT NOT NULL, SubCategoryChineseNm TEXT NOT NULL,  \n");
                                sSQL.Append("SKU varchar(8) NOT NULL, \n");
                                sSQL.Append("ProductEngNm TEXT NOT NULL, ProductChineseNm TEXT NOT NULL, ProductEngDesc TEXT NOT NULL,  \n");
                                sSQL.Append("ProductChineseDesc TEXT NOT NULL, BrandEngNm TEXT NOT NULL, BrandChineseNm TEXT NOT NULL,  \n");
                                sSQL.Append("SearchKeyWordNmEng TEXT NOT NULL, SearchKeyWordNmChinese TEXT NOT NULL, MetaTagEng TEXT NOT NULL,  \n");
                                sSQL.Append("MetaTagChinese TEXT NOT NULL, ProductImageURL TEXT NOT NULL, ProductDetailURL TEXT NOT NULL,  \n");
                                sSQL.Append("PaydImageId TEXT NOT NULL, ProductID TEXT NOT NULL, Language TEXT NOT NULL );  \n");

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
                                sSQL.Append("UPDATE tblTranslations INNER JOIN tblSKU_ImageID ON tblTranslations.SKU = tblSKU_ImageID.SKU ");
                                sSQL.Append("SET PaydImageId = Image_Id, ProductImageURL = CONCAT('http://bookstore.fll.cc/img/product/id=',Image_Id) ");

                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 4.	Filled items' Chinese Translations in Translation Table by SKU
                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE tblTranslations INNER JOIN tblSKU_Chinese ON tblTranslations.SKU = tblSKU_Chinese.SKU ");
                                sSQL.Append("SET ProductChineseNm = ChineseName, ProductChineseDesc = ChineseDesc ");

                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 5.	Filled Category Info in Translation Table by SKU and CategoryCode
                                sSQL = new StringBuilder();
                                sSQL.Append("UPDATE	tblTranslations T \n ");
                                sSQL.Append("INNER JOIN ( \n ");
                                sSQL.Append("SELECT SKU , Cat01, '' AS Cat02 FROM (SELECT SKU , CategoryCode, \n ");
                                sSQL.Append("CASE position(' | ' IN CategoryCode) WHEN 0 THEN CategoryCode ELSE SUBSTR(CategoryCode,1,position(' | ' IN CategoryCode)-1) END AS SubCategory FROM tblinventory) A \n ");
                                sSQL.Append("LEFT JOIN (SELECT * FROM tblCategory_Raw) B ON SubCategory = Code LEFT JOIN (SELECT distinct Cat00 , Cat01 FROM translation.tblCategory) t1 ON Code = Cat01 WHERE Level = 1 \n ");

                                sSQL.Append("UNION \n ");

                                sSQL.Append("\n ");
                                sSQL.Append("SELECT SKU , Cat01, Cat02 FROM (SELECT SKU , CategoryCode, \n ");
                                sSQL.Append("CASE position(' | ' IN CategoryCode) WHEN 0 THEN CategoryCode ELSE SUBSTR(CategoryCode,1,position(' | ' IN CategoryCode)-1) END AS SubCategory FROM tblinventory ) A \n ");
                                sSQL.Append("LEFT JOIN (SELECT * FROM tblCategory_Raw) B ON SubCategory = Code LEFT JOIN (SELECT distinct Cat00 , Cat01 , Cat02 FROM translation.tblCategory) t2 ON Code = Cat02 WHERE Level = 2 \n ");

                                sSQL.Append("UNION \n ");

                                sSQL.Append("SELECT SKU , Cat01, Cat02 FROM (SELECT SKU , CategoryCode, \n ");
                                sSQL.Append("CASE position(' | ' IN CategoryCode) WHEN 0 THEN CategoryCode ELSE SUBSTR(CategoryCode,1,position(' | ' IN CategoryCode)-1) END AS SubCategory FROM tblinventory ) A \n ");
                                sSQL.Append("LEFT JOIN (SELECT * FROM tblCategory_Raw) B ON SubCategory = Code LEFT JOIN tblCategory ON Code = Cat03 WHERE Level = 3 \n ");
                                sSQL.Append(") T3 on T.SKU = T3.SKU \n ");
                                sSQL.Append("SET CatagoryCd = T3.Cat01, SubCatagoryCd = T3.Cat02; \n ");

                                sSQL.Append("UPDATE tblTranslations INNER JOIN tblCategory_Raw on CatagoryCd = Code set CategoryEngNm = Category; \n ");
                                sSQL.Append("UPDATE tblTranslations INNER JOIN tblMetadata on CategoryEngNm = EnglishName set CategoryChineseNm = ChineseName; \n ");
                                sSQL.Append("UPDATE tblTranslations INNER JOIN tblCategory_raw on SubCatagoryCode = Code set SubCategoryEngNm = Category; \n ");
                                sSQL.Append("UPDATE tblTranslations INNER JOIN tblMetadata on SubCategoryEngNm = EnglishName set SubCategoryChineseNm = ChineseName; \n ");

                                cmd.CommandText = sSQL.ToString();
                                cmd.ExecuteNonQuery();
                                #endregion

                                #region 6.	Producing final output file: Exporting the Translation table to an Excel File
                                sSQL = new StringBuilder();
                                sSQL.Append("SELECT CategoryCode,CategoryEngNm,CategoryChineseNm,SubCategoryCode,SubCategoryEngNm \n ");
                                sSQL.Append(",SubCategoryChineseNm,SKU,ProductEngNm,ProductChineseNm,ProductEngDesc \n ");
                                sSQL.Append(",ProductChineseDesc,BrandEngNm,BrandChineseNm,SearchKeyWordNmEng \n ");
                                sSQL.Append(",SearchKeyWordNmChinese,MetaTagEng,MetaTagChinese,ProductImageURL \n ");
                                sSQL.Append(",ProductDetailURL,PaydImageId,ProductID,Language FROM tblTranslations; \n ");

                                cmd.CommandText = sSQL.ToString();
                                objReader = cmd.ExecuteReader();

                                //Test
                                DataTable dts = new DataTable();
                                dts.Load(objReader); // <-- FormatException

                                while (objReader.Read())
                                {
                                    var students = new[]
                                    {
                                        new {
                                            Id = "101", Name = "Vivek", Address = "Hyderabad"
                                        },
                                        new {
                                            Id = "102", Name = "Ranjeet", Address = "Hyderabad"
                                        },
                                        new {
                                            Id = "103", Name = "Sharath", Address = "Hyderabad"
                                        },
                                        new {
                                            Id = "104", Name = "Ganesh", Address = "Hyderabad"
                                        },
                                        new {
                                            Id = "105", Name = "Gajanan", Address = "Hyderabad"
                                        },
                                        new {
                                            Id = "106", Name = "Ashish", Address = "Hyderabad"
                                        }
                                    };
                                    ExcelPackage excelExport = new ExcelPackage();
                                    var workSheet = excelExport.Workbook.Worksheets.Add("Sheet1");
                                    workSheet.TabColor = System.Drawing.Color.Black;
                                    workSheet.DefaultRowHeight = 12;
                                    //Header of table  
                                    //  
                                    workSheet.Row(1).Height = 20;
                                    workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    workSheet.Row(1).Style.Font.Bold = true;
                                    workSheet.Cells[1, 1].Value = "S.No";
                                    workSheet.Cells[1, 2].Value = "Id";
                                    workSheet.Cells[1, 3].Value = "Name";
                                    workSheet.Cells[1, 4].Value = "Address";
                                    
                                    //Body of table  
                                    //  
                                    int recordIndex = 2;
                                    foreach (var student in students)
                                    {
                                        workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                                        workSheet.Cells[recordIndex, 2].Value = student.Id;
                                        workSheet.Cells[recordIndex, 3].Value = student.Name;
                                        workSheet.Cells[recordIndex, 4].Value = student.Address;
                                        recordIndex++;
                                    }
                                    workSheet.Column(1).AutoFit();
                                    workSheet.Column(2).AutoFit();
                                    workSheet.Column(3).AutoFit();
                                    workSheet.Column(4).AutoFit();
                                    string excelName = "studentsRecord";
                                    using (var memoryStream = new MemoryStream())
                                    {
                                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                        Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                                        excelExport.SaveAs(memoryStream);
                                        memoryStream.WriteTo(Response.OutputStream);
                                        Response.Flush();
                                        Response.End();
                                    }
                                }
                                #endregion

                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                objReader = null;
            }
        }
    }
}