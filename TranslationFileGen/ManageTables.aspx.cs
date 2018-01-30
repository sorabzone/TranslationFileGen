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
    }
}