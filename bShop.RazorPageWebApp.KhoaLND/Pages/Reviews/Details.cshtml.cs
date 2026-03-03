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
    public class DetailsModel : PageModel
    {
        private readonly IReviewService _reviewService;
        public DetailsModel(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewService.GetReviewByIdAsync(id.Value);
            if (review == null)
            {
                return NotFound();
            }
            else
            {
                Review = review;
            }
            return Page();
        }
    }
}
