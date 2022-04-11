using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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

        public DataTable GetAllAuctions()
        {
            DataTable dTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                Stopwatch sw = Stopwatch.StartNew();
                sqlConnection.Open();
                SqlDataAdapter sqlAda = new SqlDataAdapter("ViewAllAuctions", sqlConnection);
                sqlAda.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlAda.Fill(dTable);
            }

            return dTable;
        }

        public async Task<AuctionDetailsViewModel> GetAuctionDetailsByIdAsync(int auctionId)
        {
            var auctionDetailsVM = new AuctionDetailsViewModel();

            var sql = $" SELECT DISTINCT " +
                      $" AU.AUCTIONID, " +
                      $" AU.DESCRIPTION, " +
                      $" AU.AUCTIONDATE, " +
                      $" AI.ITEMID, " +
                      $" AI.DESCRIPTION, " +
                      $" AI.STARTPRICE " +
                      $" FROM AUCTIONITEMS AI " +
                      $" LEFT JOIN AUCTIONS AU " +
                      $" ON AU.AUCTIONID = AI.AUCTIONID " +
                      $" WHERE AU.AUCTIONID = {nameof(auctionId)}";

            IEnumerable<AuctionDetails> data = null;

            using (var db = new SqlConnection(ConnectionString))
            {
                Stopwatch sw = Stopwatch.StartNew();
                data = await db.QueryAsync<AuctionDetails>(sql, new { auctionId })
                                .ConfigureAwait(false);
                sw.Stop();
            }

            if (data != null && data.Any())
            {               
                var auctionItems = new List<AuctionItems>();

                foreach (var item in data)
                {
                    auctionDetailsVM.Auction = new Auctions
                    {
                        AuctionDate = item.AuctionDate,
                        AuctionId = item.AuctionId,
                        Description = item.AuctionDescription
                    };

                    auctionItems.Add(new AuctionItems
                    {
                        ItemId = item.ItemId,
                        ItemDescription = item.ItemDescription,
                        StartPrice = item.StartPrice
                    });
                }

                auctionDetailsVM.AuctionItems = auctionItems;
            }

            return auctionDetailsVM;
        }

        public void AddOrEditAuction(AuctionsViewModel auctionsVM)
        {
            using SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            Stopwatch sw = Stopwatch.StartNew();
            sqlConnection.Open();
            SqlCommand sqlCmd = new SqlCommand("ViewAuctionItemsById", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            sqlCmd.Parameters.AddWithValue("AuctionId", auctionsVM.AuctionId);
            sqlCmd.Parameters.AddWithValue("Description", auctionsVM.Description);
            sqlCmd.Parameters.AddWithValue("AuctionDate", auctionsVM.AuctionItemCount);
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
