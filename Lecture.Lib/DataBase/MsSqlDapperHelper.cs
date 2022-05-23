using System.Data.SqlClient;

namespace Lecture.Lib.DataBase
{
    public class MsSqlDapperHelper : IDisposable
    {
        SqlConnection _Conn;
        SqlTransaction _Transaction = null;

        public MsSqlDapperHelper()
        {
            _Conn = new SqlConnection("Data Source=116.120.58.105,1433; Initial Catalog=Test; User ID=Test1; Password=study2022!!;");
            _Conn.Open();
        }

        #region Transaction

        public void BeginTransaction()
        {
            _Transaction = _Conn.BeginTransaction();
        }

        public void Commit()
        {
            _Transaction.Commit();
            _Transaction = null;
        }

        public void RollBack()
        {
            _Transaction.Rollback();
            _Transaction = null;
        }

        #endregion

        public List<T> Query<T>(string sql, object param)
        {
            return Dapper.SqlMapper.Query<T>(_Conn, sql, param, _Transaction).ToList();
        }

        public T QuerySingle<T>(string sql, object param)
        {
            return Dapper.SqlMapper.QuerySingleOrDefault<T>(_Conn, sql, param, _Transaction);
        }

        public int Execute(string sql, object param)
        {
            return Dapper.SqlMapper.Execute(_Conn, sql, param, _Transaction);
        }


        #region Dispose

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    _Conn.Dispose();

                    if (_Transaction != null)
                    {
                        _Transaction.Rollback();
                        _Transaction.Dispose();
                    }
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~MsSqlDapperHelper()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
