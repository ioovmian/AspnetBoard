using Lecture.Lib.DataBase;
using System.Data;
using System.Data.SqlClient;

namespace Lecture.Models.Home
{
    public class TicketModel 
    {
        public int Product_Id { get; set; }

        public string Product_Name { get; set; }

        public int Brand_Id { get; set; }

        public int Category_Id { get; set; }

        public string Model_Year { get; set; }

        public decimal List_Price { get; set; }

        public static List<TicketModel> GetList()
        {
            var sql = @"SELECT *
FROM [production].[products]
";

            using (var db = new MsSqlDapperHelper())
            {
                return db.Query<TicketModel>(sql, null).ToList();
            }
        }

        public int Update()
        {
            var sql = @"
UPDATE [production].[products]
SET 
    [category_id] = @Category_Id
WHERE 
    [product_id] = @Product_Id
";
            using (var db = new MsSqlDapperHelper())
            {
                try
                {
                    db.BeginTransaction();
                    
                    var nResult = db.Execute(sql, this);

                    db.Commit();

                    return nResult;
                }
                catch (Exception ex)
                {
                    db.RollBack();
                    throw ex;
                }
            }
        }
    }
}
