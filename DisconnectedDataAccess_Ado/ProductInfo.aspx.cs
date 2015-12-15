using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DisconnectedDataAccess_Ado
{
    public partial class ProductInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }


        //method to get employee data from the database
        private void GetAllEmployees()
        {
            string conString = ConfigurationManager.ConnectionStrings["CS1"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            string queryString = "Select * from Product";
            SqlDataAdapter dataAdapt = new SqlDataAdapter(queryString, con);

            DataSet ds = new DataSet();

            //"Fill" method will connect to the DB, retrieve the data and load it into the dataset
            //"ProductsTable" is the name we will call the retrieved table inside the dataset
            dataAdapt.Fill(ds, "ProductsTable");

            //Set the column that represents the primarykey inside ProductsTable table in the datset 
            ds.Tables["ProductsTable"].PrimaryKey = new DataColumn[] { ds.Tables["ProductsTable"].Columns["ProductID"] };

            //store the dataset in a cache object, Store the dataset in Cache for 24 hours
            Cache.Insert("DATASET", ds, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);

            gdViewProducts.DataSource = ds;
            gdViewProducts.DataBind();

        
            lblMessages.Text = "Products Data loaded from database";
        }


        //Method to get product data from cache
        private void GetDataFromCache()
        {
            if (Cache["DATASET"] != null)
            {
                DataSet dataSet = (DataSet)Cache["DATASET"];
                gdViewProducts.DataSource = dataSet;
                gdViewProducts.DataBind();
            }
        }

        protected void btnGetProductsFromDB_Click(object sender, EventArgs e)
        {
            GetAllEmployees();
            btnUpdateProductsInDB.Visible = true;
        }

        protected void gdViewProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gdViewProducts.EditIndex = e.NewEditIndex;
            GetDataFromCache();
        }

        protected void gdViewProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Cache["DATASET"] != null)
            {
                DataSet dataSet = (DataSet)Cache["DATASET"];
                DataRow dr = dataSet.Tables["ProductsTable"].Rows.Find(e.Keys["ProductID"]);
                dr["ProductName"] = e.NewValues["ProductName"];
                dr["Price"] = e.NewValues["Price"];
                dr["Brand"] = e.NewValues["Brand"];
                Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
             
                gdViewProducts.EditIndex = -1;
                GetDataFromCache();
            }
        }

        protected void gdViewProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gdViewProducts.EditIndex = -1;
            GetDataFromCache();
        }

        protected void gdViewProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Cache["DATASET"] != null)
            {
                DataSet dataSet = (DataSet)Cache["DATASET"];
                DataRow dr = dataSet.Tables["ProductsTable"].Rows.Find(e.Keys["ProductID"]);
                dr.Delete();
                Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
                GetDataFromCache();
            }
        }

        protected void btnUpdateProductsInDB_Click(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["CS1"].ConnectionString;
            SqlConnection con = new SqlConnection(conString);
            string queryString = "Select * from Product";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(queryString, con);

            DataSet ds = (DataSet)Cache["DATASET"];
            string updCmd = "Update Product set ProductName=@ProductName, Price=@Price, Brand=@Brand where ProductID=@ProductID";
            SqlCommand updateCommand = new SqlCommand(updCmd, con);
            updateCommand.Parameters.Add("@ProductName", SqlDbType.VarChar, 50, "ProductName");
            updateCommand.Parameters.Add("@Price", SqlDbType.Decimal, 5, "Price");
            updateCommand.Parameters.Add("@Brand", SqlDbType.VarChar, 50, "Brand");
            updateCommand.Parameters.Add("@ProductID", SqlDbType.Int, 0, "ProductID");

            dataAdapter.UpdateCommand = updateCommand;

            string delCmd = "Delete from Product where ProductID=@ProductID";
            SqlCommand deleteCommand = new SqlCommand(delCmd, con);
            deleteCommand.Parameters.Add("@ProductID", SqlDbType.Int, 0, "ProductID");
            dataAdapter.DeleteCommand = deleteCommand;
            //dataAdapter.Update(ds, "ProductsTable");


            int x = dataAdapter.Update(ds, "ProductsTable");
            if (x > 0)
            {
                lblMessages.Text = "Employee data updated in the database";
            }
        }
    }
}