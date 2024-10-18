using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VisitorsManagement.Models;

namespace VisitorsManagement.Repository
{
    public interface IGenericRepository
    {
        Task<IEnumerable<T>> GetAsync<T>(string sQuery, DynamicParameters param = null);
        Task<DataSet> GetAsyncProc<T>(string ProcName, DynamicParameters param = null);
        Task<int> ExecuteCommandAsync(string query, DynamicParameters param = null);
        Task<int> ExecuteMultipleCommadsAsync(List<SqlQueryModel> sqlQueries, DynamicParameters param = null);
    }
}