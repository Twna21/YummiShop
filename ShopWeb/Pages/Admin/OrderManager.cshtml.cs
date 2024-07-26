using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject;
using Repository; // Ensure this namespace matches your actual project structure
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopWeb.Pages.Admin
{
    public class OrderManagerModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManagerModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [BindProperty]
        public IList<Order> Orders { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrEmpty(SearchTerm))
            {
                Orders = (await _orderRepository.GetOrders()).ToList();
            }
            else
            {
                Orders = (await _orderRepository.GetOrders())
                    .Where(o => o.OrderId.ToString().Contains(SearchTerm) ||
                                o.CustomerId.ToString().Contains(SearchTerm))
                    .ToList();
            }
        }

        public async Task<IActionResult> OnGetDeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.Delete(order);

            return RedirectToPage("./OrderManager");
        }
    }
}
