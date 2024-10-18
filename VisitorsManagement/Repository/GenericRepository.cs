using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace VisitorsManagement.Repository
{
    public class GenericRepository : IGenericRepository
    {
        //private readonly IConfiguration _config;

        //public GenericRepository(IConfiguration config)
        //{
        //    _config = config;
        //}

        private IDbConnection Connection
        {
            get
            {
                return new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            }
        }

        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            }
        }


        public async Task<IEnumerable<T>> GetAsync<T>(string sQuery, DynamicParameters param = null)
        {
            try
            {
                IDbConnection conn = Connection;

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                var result = await conn.QueryAsync<T>(sQuery, param).ConfigureAwait(false);

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
           

        }

        public async Task<DataSet> GetAsyncProc<T>(string ProcName, DynamicParameters param = null)
        {
            try
            {
                IDbConnection conn = Connection;

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DataSet result = null;
                using (var sqlConnection = new SqlConnection(Connection.ConnectionString))
                {
                    using (var command = sqlConnection.CreateCommand())
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter(command))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            command.CommandText = ProcName;
                            //if (param != null)
                            //{
                            //    command.Parameters.AddRange(param);
                            //}
                            result = new DataSet();
                            sda.Fill(result);
                        }
                    }
                }                

                if (conn.State == ConnectionState.Open)
                    conn.Close();

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task<int> ExecuteCommandAsync(string query, DynamicParameters param = null)
        {
            IDbConnection conn = Connection;
            conn.Open();

            var result = await conn.ExecuteAsync(query, param, null, 180).ConfigureAwait(false);
            conn.Close();

            return result;

        }

        public async Task<int> ExecuteMultipleCommadsAsync(List<SqlQueryModel> sqlQueries, DynamicParameters param = null)
        {
            IDbConnection conn = Connection;
            conn.Open();

            using (var transction = conn.BeginTransaction())
            {
                var returnValue = 0;
                var index = 0;

                try
                {
                    foreach (var item in sqlQueries)
                    {
                        if (item.IsReturn)
                            returnValue = await conn.ExecuteAsync(item.SqlQuery, item.SqlParameters, transction, 180).ConfigureAwait(false);
                        else
                            await conn.ExecuteAsync(item.SqlQuery, item.SqlParameters, transction, 180).ConfigureAwait(false);

                        index++;
                    }

                    transction.Commit();
                }
                catch (Exception)
                {
                    transction.Rollback();
                }
            }
            conn.Close();
            return 0;
        }
    }
}