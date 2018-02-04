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
        public string connString = "Data Source=C:\\PS\\TranslationFileGen\\TranslationFileGen\\TranslationFileGen\\App_Data\\TranslationData.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            Msg.Text = "";
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
                                foreach (DataRow row in dt.Rows)
                                {
                                    cmd.CommandText = "INSERT INTO tblSKU_ImageID (SKU, Image_Id, UpdatedDate) VALUES ('" + Convert.ToString(row["SKU"]) + "', '" + Convert.ToString(row["Image_Id"]) + "', CURRENT_TIMESTAMP);";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
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
                                foreach (DataRow row in dt.Rows)
                                {
                                    cmd.CommandText = "INSERT INTO tblSKU_Chinese (SKU, ChineseName, ChineseDesc, UpdatedDate) VALUES ('" + Convert.ToString(row["SKU"]) + "', '" + Convert.ToString(row["ChineseName"]) + "', '" + Convert.ToString(row["ChineseDesc"]) + "', CURRENT_TIMESTAMP);";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
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
                                foreach (DataRow row in dt.Rows)
                                {
                                    cmd.CommandText = "INSERT INTO tblMetadata (EnglishName, ChineseName) VALUES ('" + Convert.ToString(row["EnglishName"]) + "', '" + Convert.ToString(row["ChineseName"]) + "');";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
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
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion

        #region Category
        protected void btnCategoryImport_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
            string code, cat01 = string.Empty, cat02 = string.Empty, cat03 = string.Empty;
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
                                foreach (DataRow row in dt.Rows)
                                {
                                    code = Convert.ToString(row["Code"]);
                                    cmd.CommandText = "INSERT INTO tblCategory_Raw (Category, Code, Level) VALUES ('" + Convert.ToString(row["Category"]) + "', '" + code + "', " + Convert.ToInt32(row["Level"]) + ");";
                                    cmd.ExecuteNonQuery();

                                    switch(code.Length)
                                    {
                                        case 6:
                                            cat01 = code.Substring(0, 2);
                                            cat02 = code.Substring(0, 4);
                                            cat03 = code.Substring(0, 6);
                                            break;
                                        case 4:
                                            cat01 = code.Substring(0, 2);
                                            cat02 = code.Substring(0, 4);
                                            break;
                                        case 2:
                                            cat01 = code.Substring(0, 2);
                                            break;
                                    }

                                    cmd.CommandText = "INSERT INTO tblCategory (Cat01, Cat02, Cat03, CatDesc, Level) VALUES ('" + cat01 + "','" +  cat02 + "','" + cat03 + "','" + Convert.ToString(row["Category"]) + "', " + Convert.ToInt32(row["Level"]) + ");";
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        conn.Close();
                    }
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
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        #endregion
    }
}