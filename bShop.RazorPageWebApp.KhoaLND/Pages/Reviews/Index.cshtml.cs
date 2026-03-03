using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using bShop.Entities.KhoaLND.Models;
using bShop.Repositories.KhoaLND.DBContext;
using bShop.Services.KhoaLND;

namespace bShop.RazorPageWebApp.KhoaLND.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly IReviewService _reviewService;

        public IndexModel(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        public IList<Review> Review { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Review = await _reviewService.GetAllReviewsAsync();
        }
    }
}
