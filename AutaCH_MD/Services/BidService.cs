using AutaCH_MD.Contexts;
using AutaCH_MD.Models;
using Microsoft.EntityFrameworkCore;

namespace AutaCH_MD.Services
{
    public class BidService
    {
        private readonly AppDataContext _ctx;

        public BidService(AppDataContext ctx)
        {
            _ctx = ctx;
        }

        public void UpdateBidStatus()
        {
            var now = DateTime.Now;
            var bids = this._ctx.Bids
                .Include(b => b.Car)
                .Where(b => b.IsActive && b.Car.EndAuction < now)
                .ToList();

            foreach (var bid in bids)
            {
                var highestBid = this._ctx.Bids
                    .Where(b => b.CarId == bid.CarId && b.IsActive)
                    .OrderByDescending(b => b.BidAmount)
                    .FirstOrDefault();

                if (highestBid != null)
                {
                    if (bid.BidId == highestBid.BidId)
                    {
                        bid.Status = BidStatus.Winning;
                    }
                    else
                    {
                        bid.Status = BidStatus.Lost;
                    }
                }
                else
                {
                    bid.Status = BidStatus.Lost;
                }
            }
            _ctx.SaveChanges();
        }
    }

}
