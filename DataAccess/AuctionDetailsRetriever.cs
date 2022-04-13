using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuctionManagement
{
    public class AuctionDetailsRetriever : IAuctionDetailsRetreiver
    {
        public IConfiguration Configuration { get; }
        public string ConnectionString = string.Empty;

        public AuctionDetailsRetriever(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Auctions>> GetAllAuctions()
        {

            var sql = $" SELECT DISTINCT " +
                      $" AU.AUCTIONID, " +
                      $" AU.DESCRIPTION, " +
                      $" AU.AUCTIONDATE, " +
                      $" COUNT(ITEMID) as TotalAuctionItems " +
                      $" FROM AUCTIONS AU " +
                      $" LEFT JOIN AUCTIONITEMS AI " +
                      $" ON AU.AUCTIONID = AI.AUCTIONID " +
                      $" GROUP BY AU.AUCTIONID, AU.DESCRIPTION, AU.AUCTIONDATE ";

            IEnumerable<Auctions> data = null;

            using (var db = new SqlConnection(ConnectionString))
            {
                Stopwatch sw = Stopwatch.StartNew();
                data = await db.QueryAsync<Auctions>(sql)
                                .ConfigureAwait(false);
                sw.Stop();
            }

            return data;
        }

        public async Task<IEnumerable<AuctionDetails>> GetAuctionDetailsByIdAsync(int auctionId)
        {
            var sql = $" SELECT DISTINCT " +
                      $" AU.AUCTIONID, " +
                      $" AU.DESCRIPTION AS AUCTIONDESCRIPTION, " +
                      $" AU.AUCTIONDATE, " +
                      $" AI.ITEMID, " +
                      $" AI.DESCRIPTION AS ITEMDESCRIPTION, " +
                      $" AI.STARTPRICE " +
                      $" FROM AUCTIONITEMS AI " +
                      $" LEFT JOIN AUCTIONS AU " +
                      $" ON AU.AUCTIONID = AI.AUCTIONID " +
                      $" WHERE AI.AUCTIONID = {auctionId}";

            IEnumerable<AuctionDetails> data = null;

            using (var db = new SqlConnection(ConnectionString))
            {
                Stopwatch sw = Stopwatch.StartNew();
                data = await db.QueryAsync<AuctionDetails>(sql, new { auctionId })
                                .ConfigureAwait(false);
                sw.Stop();
            }            

            return data;
        }

        public void AddOrEditAuction(AuctionsViewModel auctionVM)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            Stopwatch sw = Stopwatch.StartNew();
            sqlConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("AddOrEditAuctions", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCmd.Parameters.AddWithValue("AuctionId", auctionVM.AuctionId);
            sqlCmd.Parameters.AddWithValue("Description", auctionVM.Description);
            sqlCmd.Parameters.AddWithValue("AuctionDate", auctionVM.AuctionDate);
            sqlCmd.ExecuteNonQuery();
        }

        public void AddOrEditAuctionItem(AuctionItemViewModel auctionItemsVM)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            Stopwatch sw = Stopwatch.StartNew();
            sqlConnection.Open();

            SqlCommand sqlCmd = new SqlCommand("ViewAuctionItemsById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCmd.Parameters.AddWithValue("AuctionId", auctionItemsVM.AuctionId);
            sqlCmd.Parameters.AddWithValue("ItemId", auctionItemsVM.ItemId);
            sqlCmd.Parameters.AddWithValue("Description", auctionItemsVM.Description);
            sqlCmd.Parameters.AddWithValue("StartPrice", auctionItemsVM.StartPrice);
            sqlCmd.ExecuteNonQuery();
        }
    }
}
