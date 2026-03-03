using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bShop.Entities.KhoaLND.Models;
using bShop.Repositories.KhoaLND.DBContext;
using bShop.Services.KhoaLND;
using bShop.RazorPageWebApp.KhoaLND.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace bShop.RazorPageWebApp.KhoaLND.Pages.Reviews
{
    public class EditModel : PageModel
    {
        private readonly IReviewService _reviewService;
        private readonly IHubContext<ReviewHub> _hubContext;
        public EditModel(IReviewService reviewService, IHubContext<ReviewHub> hubContext)
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
            Review = review;
            ViewData["BuyerId"] = new SelectList(
                await _reviewService.GetBuyerSelectListAsync(),
                "Id",
                "Name"
            );

            ViewData["ProductId"] = new SelectList(
                await _reviewService.GetProductSelectListAsync(),
                "Id",
                "Name"
            );

            ViewData["OrderDetailId"] = new SelectList(
                await _reviewService.GetOrderDetailSelectListAsync(),
                "Id",
                "Name"
            );
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ViewData["BuyerId"] = new SelectList(
            await _reviewService.GetBuyerSelectListAsync(),
            "Id",
            "Name"
            );

            ViewData["ProductId"] = new SelectList(
                await _reviewService.GetProductSelectListAsync(),
                "Id",
                "Name"
            );

            ViewData["OrderDetailId"] = new SelectList(
                await _reviewService.GetOrderDetailSelectListAsync(),
                "Id",
                "Name"
            );
            try
            {
                await _reviewService.UpdateReviewAsync(Review);
                await _hubContext.Clients.All.SendAsync("ReceiveUpdate", Review);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReviewExists(Review.ReviewId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> ReviewExists(int id)
        {
            return await _reviewService.GetReviewByIdAsync(id) is null ? false : true;
        }
    }
}
