using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Data.SQLite;

namespace TranslationFileGen
{
    public partial class ManageTables : System.Web.UI.Page
    {
        public string connString = "Data Source=C:\\PS\\TranslationFileGen\\TranslationFileGen\\TranslationFileGen\\App_Data\\TranslationData.db;Version=3;";

        protected void Page_Load(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 0;
        }

        #region Tab Change Events
        protected void Tab1_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 0;
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 1;
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 2;
        }

        protected void Tab4_Click(object sender, EventArgs e)
        {
            mvTables.ActiveViewIndex = 3;
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
                            cmd.CommandText = "UPDATE tblSKU_ImageID SET image_id = " + txtImage.Text + ", updateddate = CURRENT_TIMESTAMP WHERE sku = '" + txtSku.Text + "';";
                            cmd.ExecuteNonQuery();
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
                            cmd.CommandText = "INSERT INTO tblSKU_ImageID (sku, image_id, updateddate) VALUES ('" + txtNewSku.Text + "', '" + txtNewImage.Text + "', CURRENT_TIMESTAMP);";
                            cmd.ExecuteNonQuery();
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
                                    cmd.CommandText = "INSERT INTO tblSKU_ImageID (sku, image_id, updateddate) VALUES ('" + Convert.ToString(row["sku"]) + "', '" + Convert.ToString(row["image_id"]) + "', CURRENT_TIMESTAMP);";
                                    cmd.ExecuteNonQuery();
                                }
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
                            cmd.CommandText = "SELECT sku, englishname, chinesename, chinesedesc FROM tblSKU_Chinese WHERE sku = '" + txtSkuC.Text + "';";
                            objReader = cmd.ExecuteReader();

                            if (objReader.Read())
                            {
                                txtChineseName.Text = Convert.ToString(objReader["chinesename"]);
                                txtChineseDesc.Text = Convert.ToString(objReader["chinesedesc"]);
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
                            cmd.CommandText = "UPDATE tblSKU_Chinese SET chinesename = " + txtChineseName.Text + ", chinesedesc = " + txtChineseDesc.Text + ", updateddate = CURRENT_TIMESTAMP WHERE sku = '" + txtSkuC.Text + "';";
                            cmd.ExecuteNonQuery();
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
                            cmd.CommandText = "INSERT INTO tblSKU_Chinese (sku, chinesename, chinesedesc, updateddate) VALUES ('" + txtNewSkuC.Text + "', '" + txtNewChineseName.Text + "', '" + txtNewChineseDesc.Text + "', CURRENT_TIMESTAMP);";
                            cmd.ExecuteNonQuery();
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
                                    cmd.CommandText = "INSERT INTO tblSKU_Chinese (sku, chinesename, chinesedesc, updateddate) VALUES ('" + Convert.ToString(row["sku"]) + "', '" + Convert.ToString(row["chinesename"]) + "', '" + Convert.ToString(row["chinesedesc"]) + "', CURRENT_TIMESTAMP);";
                                    cmd.ExecuteNonQuery();
                                }
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
                            cmd.CommandText = "SELECT englishname, chinesename, chinesedesc FROM tblMetadata WHERE englishname = '" + txtMetaData.Text + "';";
                            objReader = cmd.ExecuteReader();

                            if (objReader.Read())
                            {
                                txtChineseTrans.Text = Convert.ToString(objReader["chinesename"]);
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

        protected void btnUpdateM_Click1(object sender, EventArgs e)
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
                            cmd.CommandText = "UPDATE tblMetadata SET chinesename = " + txtChineseTrans.Text + " WHERE englishname = '" + txtMetaData.Text + "';";
                            cmd.ExecuteNonQuery();
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
                            cmd.CommandText = "INSERT INTO tblMetadata (englishname, chinesename) VALUES ('" + txtNewMetaData.Text + "', '" + txtNewChineseTrans.Text + "');";
                            cmd.ExecuteNonQuery();
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
                                    cmd.CommandText = "INSERT INTO tblMetadata (englishname, chinesename) VALUES ('" + Convert.ToString(row["englishname"]) + "', '" + Convert.ToString(row["chinesename"]) + "');";
                                    cmd.ExecuteNonQuery();
                                }
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
            }
        }
        #endregion

        #region Category
        protected void btnCategoryImport_Click(object sender, EventArgs e)
        {
            SQLiteCommand cmd = null;
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
                                    cmd.CommandText = "INSERT INTO tblCategory_raw (category, code, level) VALUES ('" + Convert.ToString(row["category"]) + "', '" + Convert.ToString(row["code"]) + "', " + Convert.ToInt32(row["level"]) + ");";
                                    cmd.ExecuteNonQuery();
                                }
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
            }
        }
        #endregion
    }
}