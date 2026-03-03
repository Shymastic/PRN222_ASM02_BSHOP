using bShop.Entities.KhoaLND.Models;
using bShop.RazorPageWebApp.KhoaLND.Hubs;
using bShop.Repositories.KhoaLND.DBContext;
using bShop.Services.KhoaLND;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bShop.RazorPageWebApp.KhoaLND.Pages.Reviews
{
    public class DeleteModel : PageModel
    {
        private readonly IReviewService _reviewService;
        private readonly IHubContext<ReviewHub> _hubContext;
        public DeleteModel(IReviewService reviewService, IHubContext<ReviewHub> hubContext)
        {
            _reviewService = reviewService;
            _hubContext = hubContext;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(id.Value);
                if (review != null)
                {
                    Review = review;
                    await _reviewService.RemoveReview(Review);
                    await _hubContext.Clients.All.SendAsync("ReceiveDelete", id.ToString());
                }

                return RedirectToPage("./Index");
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToPage("./Delete", new { id = id, saveChangesError = true });
            }
        }
    }
}
    
