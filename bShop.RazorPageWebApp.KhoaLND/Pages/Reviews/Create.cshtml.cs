using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using bShop.Entities.KhoaLND.Models;
using bShop.Repositories.KhoaLND.DBContext;
using bShop.Services.KhoaLND;
using bShop.RazorPageWebApp.KhoaLND.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace bShop.RazorPageWebApp.KhoaLND.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly IReviewService _reviewService;
        private readonly IHubContext<ReviewHub> _hubContext;

        public CreateModel(IReviewService reviewService, IHubContext<ReviewHub> hubContext)
        {
            _reviewService = reviewService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGet()
        {
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

        [BindProperty]
        public Review Review { get; set; } = default!;

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
                await _reviewService.CreateReviewAsync(Review);
                await _hubContext.Clients.All.SendAsync("ReceiveCreate",Review);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
