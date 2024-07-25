using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository; // Ensure this namespace matches your actual project structure
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWeb.Pages.Admin
{
    public class OrderDetailManagerModel : PageModel
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailManagerModel(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        [BindProperty]
        public IList<OrderDetail> OrderDetails { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                OrderDetails = (await _orderDetailRepository.GetOrderDetails()).ToList();
            }
            else
            {
                // Implement a search filter based on SearchTerm if applicable
                // For example, filter by OrderId or ProductId if needed
                OrderDetails = (await _orderDetailRepository.GetOrderDetails())
                    .Where(d => d.OrderId.ToString().Contains(SearchTerm) ||
                                d.ProductId.ToString().Contains(SearchTerm))
                    .ToList();
            }
        }

        public async Task<IActionResult> OnGetDeleteOrderDetailAsync(int orderId, int productId)
        {
            var orderDetail = await _orderDetailRepository.GetOrderDetailByIds(orderId, productId);
            if (orderDetail == null)
            {
                return NotFound();
            }

            await _orderDetailRepository.Delete(orderDetail);

            return RedirectToPage("./OrderDetailManager");
        }
    }
}
