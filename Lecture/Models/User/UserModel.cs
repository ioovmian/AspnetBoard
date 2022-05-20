using Lecture.Lib.DataBase;
using System.Data.SqlClient;

namespace Lecture.Models.User
{
    public class UserModel
    {
        public uint Seq { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        private void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.Password.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Password));

            //return Convert.ToBase64String(hash);
            this.Password = System.Convert.ToBase64String(hash);
        }

        internal int Register()
        {
            ConvertPassword();

            var sql = @"
INSERT INTO [User] ( 
      [name]
      ,[password]
      ,[email]
)
VALUES(
    @Name
    ,@Password
    ,@Email

)
";

            using (var db = new MsSqlDapperHelper())
            {
                return db.Execute(sql, this); 
            }
        }

        internal UserModel Login()
        {
            var sql = @"
SELECT *
FROM [User]
WHERE name = @Name
";
            UserModel model;

            using (var db = new MsSqlDapperHelper())
            {
                model =  db.QuerySingle<UserModel>(sql, this);
            }
            //using (SqlConnection conn = new SqlConnection("Data Source=116.120.58.105,1433; Initial Catalog=Test; User ID=stock; Password=stock1234;"))
            //{
            //    conn.Open();

            //    model =  Dapper.SqlMapper.QuerySingleOrDefault<UserModel>(conn, sql, this);
            //}

            if (model == null)
            {
                throw new Exception("사용자가 존재하지 않습니다.");
            }

            ConvertPassword();
            if (model.Password != this.Password)
            {
                throw new Exception("비밀번호가 틀렸습니다.");
            }

            return model;
        }
    }
}
