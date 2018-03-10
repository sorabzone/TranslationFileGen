using OfficeOpenXml;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Web.UI;

namespace TranslationFileGen
{
    public partial class ManageTables : System.Web.UI.Page
    {
        public string connString = "Data Source=C:\\Learn\\TranslationFileGen\\TranslationFileGen\\App_Data\\TranslationData.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            Msg.Text = "";
            successMsg.Text = "";
            if (!IsPostBack)
            {
                mvTables.ActiveViewIndex = 0;
                Tab1.Enabled = false;
                Tab1.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
                Tab2.Enabled = true;
                Tab2.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
                Tab3.Enabled = true;
                Tab3.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
                Tab4.Enabled = true;
                Tab4.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            }
        }

        #region Tab Change Events
        protected void Tab1_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 0;
            Tab1.Enabled = false;
            Tab1.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            Tab2.Enabled = true;
            Tab2.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab3.Enabled = true;
            Tab3.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab4.Enabled = true;
            Tab4.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 1;
            Tab1.Enabled = true;
            Tab1.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab2.Enabled = false;
            Tab2.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            Tab3.Enabled = true;
            Tab3.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab4.Enabled = true;
            Tab4.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 2;
            Tab1.Enabled = true;
            Tab1.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab2.Enabled = true;
            Tab2.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab3.Enabled = false;
            Tab3.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
            Tab4.Enabled = true;
            Tab4.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
        }

        protected void Tab4_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 3;
            Tab1.Enabled = true;
            Tab1.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab2.Enabled = true;
            Tab2.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab3.Enabled = true;
            Tab3.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "blue";
            Tab4.Enabled = false;
            Tab4.Attributes.CssStyle[HtmlTextWriterStyle.Color] = "gray";
        }
        #endregion

        #region Image
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SQLiteDataReader objReader = null;
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtSku.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "SELECT sku, image_id FROM tblSKU_ImageID WHERE sku = '" + txtSku.Text + "';";
                            objReader = cmd.ExecuteReader();

                            if (objReader.Read())
                            {
                                txtImage.Text = Convert.ToString(objReader["image_id"]);
                            }
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter SKU.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if(cmd != null)
                    cmd.Dispose();

                if(objReader != null)
                    objReader = null;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtSku.Text) || String.IsNullOrEmpty(txtImage.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "UPDATE tblSKU_ImageID SET Image_Id = " + txtImage.Text + ", UpdatedDate = CURRENT_TIMESTAMP WHERE SKU = '" + txtSku.Text + "';";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter SKU and Image Id.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtNewSku.Text) || String.IsNullOrEmpty(txtNewImage.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "INSERT INTO tblSKU_ImageID (SKU, Image_Id, UpdatedDate) VALUES ('" + txtNewSku.Text + "', '" + txtNewImage.Text + "', CURRENT_TIMESTAMP);";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter SKU and Image Id.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        protected void btnImageImport_Click(object sender, EventArgs e)
        {
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
                                if(hdnReplace.Value.Equals("1"))
                                {
                                    cmd.CommandText = "DELETE FROM tblSKU_ImageID;";
                                    cmd.ExecuteNonQuery();
                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    DateTime oDate = DateTime.ParseExact(Convert.ToString(row["updateddate"]), "M/d/yyyy", null);

                                    cmd.Parameters.AddWithValue("@SKU", Convert.ToString(row["SKU"]));
                                    cmd.Parameters.AddWithValue("@Image_Id", Convert.ToString(row["Image_Id"]));
                                    cmd.Parameters.AddWithValue("@Date", oDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    
                                    cmd.CommandText = "INSERT INTO tblSKU_ImageID (SKU, Image_Id, UpdatedDate) SELECT @SKU, @Image_Id, @Date \n WHERE NOT EXISTS (SELECT 1 FROM tblSKU_ImageID WHERE SKU = @SKU);";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
                    successMsg.Text = "Records imported successfully";
                }
                else
                    Msg.Text = "Please select valid excel file.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                hdnReplace.Value = "0";
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion

        #region Chinese Name
        protected void btnSearchC_Click(object sender, EventArgs e)
        {
            SQLiteDataReader objReader = null;
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtSkuC.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "SELECT SKU, EnglishName, ChineseName, ChineseDesc FROM tblSKU_Chinese WHERE SKU = '" + txtSkuC.Text + "';";
                            objReader = cmd.ExecuteReader();

                            if (objReader.Read())
                            {
                                txtChineseName.Text = Convert.ToString(objReader["ChineseName"]);
                                txtChineseDesc.Text = Convert.ToString(objReader["ChineseDesc"]);
                            }
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter SKU.";
            }
            catch (Exception ex)
            {
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

        protected void btnUpdateC_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtSkuC.Text) || String.IsNullOrEmpty(txtChineseName.Text) || String.IsNullOrEmpty(txtChineseDesc.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "UPDATE tblSKU_Chinese SET ChineseName = " + txtChineseName.Text + ", ChineseDesc = " + txtChineseDesc.Text + ", UpdatedDate = CURRENT_TIMESTAMP WHERE SKU = '" + txtSkuC.Text + "';";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter SKU, Chinese Name and Chinese Description.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        protected void btnAddC_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtNewSkuC.Text) || String.IsNullOrEmpty(txtNewChineseName.Text) || String.IsNullOrEmpty(txtNewChineseDesc.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "INSERT INTO tblSKU_Chinese (SKU, ChineseName, ChineseDesc, Updateddate) VALUES ('" + txtNewSkuC.Text + "', '" + txtNewChineseName.Text + "', '" + txtNewChineseDesc.Text + "', CURRENT_TIMESTAMP);";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter SKU, Chinese Name and Chinese Description.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        protected void btnChineseImport_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            
            try
            {
                string filecontent = Convert.ToBase64String(uploadFileChinese.FileBytes);
                
                if (Path.GetExtension(uploadFileChinese.FileName).Equals(".xlsx"))
                {
                    var excel = new ExcelPackage(uploadFileChinese.FileContent);
                    var dt = excel.ToDataTable();

                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            using (var transaction = conn.BeginTransaction())
                            {
                                if (hdnReplace.Value.Equals("1"))
                                {
                                    cmd.CommandText = "DELETE FROM tblSKU_Chinese;";
                                    cmd.ExecuteNonQuery();
                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    cmd.Parameters.AddWithValue("@ProductSKU", Convert.ToString(row["ProductSKU"]));
                                    cmd.Parameters.AddWithValue("@EnglishName", Convert.ToString(row["EnglishName"]));
                                    cmd.Parameters.AddWithValue("@ChineseName", Convert.ToString(row["ChineseName"]));
                                    cmd.Parameters.AddWithValue("@ChineseLongName", Convert.ToString(row["ChineseLongName"]));
                                    cmd.CommandText = "INSERT INTO tblSKU_Chinese (SKU, EnglishName, ChineseName, ChineseDesc, UpdatedDate) SELECT @ProductSKU, @EnglishName, @ChineseName, @ChineseLongName, CURRENT_TIMESTAMP \n WHERE NOT EXISTS (SELECT 1 FROM tblSKU_Chinese WHERE SKU = @ProductSKU AND EnglishName = @EnglishName);";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
                    successMsg.Text = "Records imported successfully";
                }
                else
                    Msg.Text = "Please select valid excel file.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                hdnReplace.Value = "0";
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion

        #region Meta Data
        protected void btnSearchM_Click(object sender, EventArgs e)
        {
            SQLiteDataReader objReader = null;
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtMetaData.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "SELECT EnglishName, ChineseName FROM tblMetadata WHERE EnglishName = '" + txtMetaData.Text + "';";
                            objReader = cmd.ExecuteReader();

                            if (objReader.Read())
                            {
                                txtChineseTrans.Text = Convert.ToString(objReader["ChineseName"]);
                            }
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter Meta Data.";
            }
            catch (Exception ex)
            {
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

        protected void btnUpdateM_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtMetaData.Text) || String.IsNullOrEmpty(txtChineseTrans.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "UPDATE tblMetadata SET ChineseName = " + txtChineseTrans.Text + " WHERE EnglishName = '" + txtMetaData.Text + "';";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter Meta Data and Chinese Name.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        protected void btnAddM_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            try
            {
                if (!(String.IsNullOrEmpty(txtNewMetaData.Text) || String.IsNullOrEmpty(txtNewChineseTrans.Text)))
                {
                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            cmd.CommandText = "INSERT INTO tblMetadata (EnglishName, ChineseName) VALUES ('" + txtNewMetaData.Text + "', '" + txtNewChineseTrans.Text + "');";
                            cmd.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                }
                else
                    Msg.Text = "Please enter Meta Data and Chinese Name.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        protected void btnMetaDataImport_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            string abc = "";
            try
            {
                string filecontent = Convert.ToBase64String(uploadFileMetaData.FileBytes);

                if (Path.GetExtension(uploadFileMetaData.FileName).Equals(".xlsx"))
                {
                    var excel = new ExcelPackage(uploadFileMetaData.FileContent);
                    var dt = excel.ToDataTable();

                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            using (var transaction = conn.BeginTransaction())
                            {
                                if (hdnReplace.Value.Equals("1"))
                                {
                                    cmd.CommandText = "DELETE FROM tblMetadata;";
                                    cmd.ExecuteNonQuery();
                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    cmd.Parameters.AddWithValue("@EnglishName", Convert.ToString(row["EnglishName"]));
                                    cmd.Parameters.AddWithValue("@ChineseName", Convert.ToString(row["ChineseName"]));
                                    cmd.CommandText = "INSERT INTO tblMetadata (EnglishName, ChineseName) SELECT @EnglishName, @ChineseName \n WHERE NOT EXISTS (SELECT 1 FROM tblMetadata WHERE EnglishName = @EnglishName);";
                                    abc = Convert.ToString(row["EnglishName"]);
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
                    successMsg.Text = "Records imported successfully";
                }
                else
                    Msg.Text = "Please select valid excel file.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                hdnReplace.Value = "0";
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion

        #region Category
        protected void btnCategoryImport_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            string code, category = string.Empty, cat01 = string.Empty, cat02 = string.Empty, cat03 = string.Empty;
            int level = 0;
            try
            {
                string filecontent = Convert.ToBase64String(uploadFileCategory.FileBytes);

                if (Path.GetExtension(uploadFileCategory.FileName).Equals(".xlsx"))
                {
                    var excel = new ExcelPackage(uploadFileCategory.FileContent);
                    var dt = excel.ToDataTable();

                    using (var conn = new SQLiteConnection(connString))
                    {
                        conn.Open();
                        using (cmd = new SQLiteCommand(conn))
                        {
                            using (var transaction = conn.BeginTransaction())
                            {
                                if (hdnReplace.Value.Equals("1"))
                                {
                                    cmd.CommandText = "DELETE FROM tblCategory_Raw; DELETE FROM tblCategory;";
                                    cmd.ExecuteNonQuery();
                                }

                                foreach (DataRow row in dt.Rows)
                                {
                                    code = Convert.ToString(row["Code"]);
                                    category = Convert.ToString(row["Category"]);
                                    level = string.IsNullOrEmpty(Convert.ToString(row["Level"])) ? 0 : Convert.ToInt32(row["Level"]);

                                    cmd.Parameters.AddWithValue("@Code", code);
                                    cmd.Parameters.AddWithValue("@Category", category);
                                    cmd.Parameters.AddWithValue("@Level", level);
                                    cmd.CommandText = "INSERT INTO tblCategory_Raw (Category, Code, Level) SELECT @Category, @Code, @Level \n WHERE NOT EXISTS (SELECT 1 FROM tblCategory_Raw WHERE Code = @Code);";
                                    cmd.ExecuteNonQuery();

                                    switch (level)
                                    {
                                        case 1:
                                            cat01 = code;
                                            cat02 = "";
                                            cat03 = "";
                                            break;
                                        case 2:
                                            cat02 = code;
                                            cat03 = "";
                                            break;
                                        case 3:
                                            cat03 = code;
                                            break;
                                    }

                                    //switch (code.Length)
                                    //{
                                    //    case 6:
                                    //        cat01 = code.Substring(0, 2);
                                    //        cat02 = code.Substring(0, 4);
                                    //        cat03 = code.Substring(0, 6);
                                    //        break;
                                    //    case 4:
                                    //        cat01 = code.Substring(0, 2);
                                    //        cat02 = code.Substring(0, 4);
                                    //        break;
                                    //    case 2:
                                    //        cat01 = code.Substring(0, 2);
                                    //        break;
                                    //}   

                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@cat01", cat01);
                                    cmd.Parameters.AddWithValue("@cat02", cat02);
                                    cmd.Parameters.AddWithValue("@cat03", cat03);
                                    cmd.Parameters.AddWithValue("@Category", Convert.ToString(row["Category"]));
                                    cmd.Parameters.AddWithValue("@Level", string.IsNullOrEmpty(Convert.ToString(row["Level"])) ? 0 : Convert.ToInt32(row["Level"]));
                                    cmd.CommandText = "INSERT INTO tblCategory (Cat01, Cat02, Cat03, CatDesc, Level) SELECT @cat01, @cat02, @cat03, @Category, @Level \n WHERE NOT EXISTS (SELECT 1 FROM tblCategory WHERE Cat01 = @cat01 AND Cat02 = @cat02 AND Cat03 = @cat03);";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
                    successMsg.Text = "Records imported successfully";
                }
                else
                    Msg.Text = "Please select valid excel file.";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                hdnReplace.Value = "0";
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
    }
}