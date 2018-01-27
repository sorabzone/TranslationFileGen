using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TranslationFileGen
{
    public partial class ManageTables : System.Web.UI.Page
    {
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

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void btnImageImport_Click(object sender, EventArgs e)
        {

        }
    }
}