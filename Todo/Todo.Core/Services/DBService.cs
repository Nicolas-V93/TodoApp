using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Pra.Todo.Core
{
    class DBService
    {

        public static DataTable ExecuteSelect(string sqlInstruction)
        {
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlInstruction, Helper.GetConnectionString());
            try
            {
                sqlDataAdapter.Fill(dataSet);
            }
            catch
            {
                return null;
            }
            return dataSet.Tables[0];
        }

        public static bool ExecuteCommand(string sqlInstruction)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Helper.GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlInstruction, sqlConnection);
                try
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string ExecuteScalar(string sqlScalarInstruction)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Helper.GetConnectionString()))
            {
                SqlCommand sqlCommand = new SqlCommand(sqlScalarInstruction, sqlConnection);
                try
                {
                    sqlConnection.Open();
                    return sqlCommand.ExecuteScalar().ToString();
                }
                catch
                {
                    return null;
                }
            }

        }

    }
}
