using Lecture.Lib.DataBase;

namespace Lecture.Models.Home
{
    public class BoardModel
    {
        public int Idx { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int Reg_User { get; set; }

        public string Reg_Username { get; set; } = string.Empty;

        public DateTime Reg_Date { get; set; }

        public DateTime Mod_Date { get; set; }

        public int View_Cnt { get; set; } = 0;

        public bool Status_Flag { get; set; } = true;

        public static List<BoardModel> GetList(string search)
        {
            var sql = @"
SELECT 
       [idx]
      ,[title]
      ,[content]
      ,[reg_user]
      ,[reg_username]
      ,[reg_date]
      ,[mod_date]
      ,[view_cnt]
      ,[status_flag]
FROM 
    [Board] 
WHERE
    status_flag = 1 
    AND title LIKE CONCAT('%', @search, '%')
ORDER BY 
    idx DESC
";
            // or title Content CONCAT('%', @search, '%') 
            using (var db = new MsSqlDapperHelper())
            {
                return db.Query<BoardModel>(sql, new { search = search });
            }
        }

        public static BoardModel Get(int idx)
        {
            string sql = @"
SELECT 
       [idx]
      ,[title]
      ,[content]
      ,[reg_user]
      ,[reg_username]
      ,[reg_date]
      ,[mod_date]
      ,[view_cnt]
      ,[status_flag]
FROM 
    [Board] 
WHERE
    status_flag = 1 
    AND idx = @Idx
";
            using (var db = new MsSqlDapperHelper())
            {
                return db.QuerySingle<BoardModel>(sql, new { Idx = idx });
            }
        }

        public int Insert()
        {
            CheckContent();

            var sql = @"
INSERT INTO [Board] (
    [title]
    ,[content]
    ,[reg_user]
    ,[reg_username]
    ,[view_cnt]
    ,[status_flag]
)
VALUES(
    @Title
    ,@Content
    ,@Reg_User
    ,@Reg_Username
    ,@View_Cnt
    ,@Status_Flag
)
";
            using (var db = new MsSqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        public int Update()
        {
            CheckContent();

            var sql = @"
UPDATE [Board]
Set
    title = @Title
    ,content = @Content
    ,mod_date = @Mod_Date
WHERE 
    idx = @Idx
";
            using (var db = new MsSqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int Delete()
        {
            //            string sql = @"
            //DELETE FROM [Board]
            //WHERE 
            //    idx = @Idx
            //";
            var sql = @"
UPDATE [Board]
Set
    [status_flag] = @Status_Flag
WHERE 
    idx = @Idx
";
            using (var db = new MsSqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        private void CheckContent()
        {
            if (string.IsNullOrWhiteSpace(this.Title))
            {
                throw new Exception("제목을 입력해 주세요.");
            }
            if (string.IsNullOrWhiteSpace(this.Content))
            {
                throw new Exception("내용을 입력해 주세요.");
            }
        }
    }
}
