using System.Data;
using System.Data.SqlClient;
namespace ZYAPP.Tools
{
    public class SQLManager
    {
        private SqlConnection _dbInstance;
        private SqlCommand _sqlCommand;
        private SqlDataReader _sqlDataReader; //执行sql命令后返回的结果

        private string _dbPath= "Server=(localdb)\\mssqllocaldb;Database=ZYAPPContext-32288cbc-681a-4795-8210-9ba29ba9f021;Trusted_Connection=True;MultipleActiveResultSets=true";
   
        #region 数据库的链接与关闭
        /// <summary>
        /// 数据库的链接
        /// </summary>
        /// <returns>是否链接成功</returns>
        public bool DBConnection()
        {
            _dbInstance = new SqlConnection(_dbPath);
            bool isSuccess = true;
            try
            {
                if(_dbInstance.State!= ConnectionState.Open)
                {
                    _dbInstance.Open();
                }
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        /// <summary>
        /// 数据库的关闭
        /// </summary>
        /// <returns>是否关闭成功</returns>
        public bool DBConnnetionClose()
        {
            bool isSuccess = true;
            try
            {
                _dbInstance.Close();
            }
            catch
            {
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion

        #region SQL语句的执行
        /// <summary>
        /// 转成SQL字符创交给SQL编译器进行处理
        /// </summary>
        /// <param name="sqlcommandstr">sql转换成的字符串</param>
        /// <returns>SqlDataReader类型的结果对应方法在MSDN查询</returns>
        public SqlDataReader CompilerSQLCommand(string sqlcommandstr)
        {
            _sqlCommand = _dbInstance.CreateCommand();
            _sqlCommand.CommandText = sqlcommandstr;
            _sqlDataReader = _sqlCommand.ExecuteReader();
            return _sqlDataReader;
        }
        #endregion

        #region 数据库的增删改查
        /// <summary>
        /// 数据库的更新（局限：最多满足两个查询条件，也就是只有一个逻辑运算符）
        /// </summary>
        /// <param name="tablename">修改的表名</param>
        /// <param name="colsname">要改的列名</param>
        /// <param name="colsvlaues">想要改成的列对应值</param>
        /// <param name="selectkey">条件里面的列名</param>
        /// <param name="selectvalue">条件里面的列值</param>
        /// <param name="relationalstr">关系运算符</param>
        /// <param name="logicstr">逻辑运算符</param>
        /// <returns>更新完的数据</returns>
        public SqlDataReader UpdateData(string tablename,string[]colsname,string[]colsvlaues,string[] selectkey,string[]selectvalue, string[] relationalstr, string logicstr=null)
        {
            string _sqlcommandstr = "update " + tablename + " set ";
            for(int i=0; i<colsname.Length;i++)
            {
                _sqlcommandstr += colsname[i] + "='" + colsvlaues[i] + "' ";
            }
            if(string.IsNullOrEmpty(logicstr))
            {
                _sqlcommandstr += "where " + selectkey[0] + relationalstr[0] + "'"+selectvalue[0]+"'";
            }
            else
            {
                _sqlcommandstr += "where " + selectkey[0] + relationalstr[0] + "'" + selectvalue[0] + "' " + logicstr + " " + selectkey[1] + relationalstr[1] + "'" + selectvalue[1] + "' ";
            }

            return CompilerSQLCommand(_sqlcommandstr);
        }
        /// <summary>
        /// 插入数据库
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="colsname">要添加的列名</param>
        /// <param name="colsvalues">要添加的值</param>
        /// <returns>插入完后的数据库</returns>
        public SqlDataReader InsertData(string tablename,string[] colsname,string[] colsvalues)
        {
            string _sqlcommandstr = "insert into " + tablename+" (";
            for(int i=0;i<colsname.Length;i++)
            {
                _sqlcommandstr += colsname[i];
            }
            _sqlcommandstr += ") " + "values (";
            for (int j=0;j<colsvalues.Length;j++)
            {
                _sqlcommandstr += "'"+colsvalues[j]+"'";
            }
            _sqlcommandstr += ")";

            return CompilerSQLCommand(_sqlcommandstr);
        }


        

        #endregion
    }
}
