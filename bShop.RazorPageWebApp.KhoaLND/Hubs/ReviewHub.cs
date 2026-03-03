using bShop.Entities.KhoaLND.Models;
using bShop.Services.KhoaLND;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace bShop.RazorPageWebApp.KhoaLND.Hubs
{
    public class ReviewHub : Hub
    { 
        private readonly IReviewService _reviewService;
        public ReviewHub(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        public async Task DeleteReview(string id)
        {
            await Clients.All.SendAsync("ReceiveDelete", id);
        }
/*        public async Task UpdateReview(Review review)
        {
            await Clients.All.SendAsync("ReceiveUpdate", review);
        }*/
        public async Task UpdateReview(string reviewJsonString)
        {
            var review = JsonConvert.DeserializeObject<Review>(reviewJsonString);
            await Clients.All.SendAsync("ReceiveUpdate", review);
        }
/*        public async Task CreateReview(Review review)
        {
            await Clients.All.SendAsync("ReceiveCreate", review);
        }*/
        public async Task CreateReview(string reviewJsonString)
        {
            var review = JsonConvert.DeserializeObject<Review>(reviewJsonString);
            await Clients.All.SendAsync("ReceiveCreate", review);
        }
        
    }
}
