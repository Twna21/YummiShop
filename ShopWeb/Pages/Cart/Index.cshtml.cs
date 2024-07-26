using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Text.Json;

namespace ShopWeb.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();
        private readonly ShopDbContext _context = new ShopDbContext();


        public async Task OnGetAsync()
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (!string.IsNullOrEmpty(cart))
            {
                CartItems = JsonSerializer.Deserialize<List<CartItem>>(cart);
            }

            HeaderModelView.UserName = HttpContext.Request.Cookies["UserName"] ?? "";

            if (int.TryParse(HttpContext.Request.Cookies["UserId"], out int uid))
            {
                if (_context.OrderDetails != null)
                {
                    OrderDetail = await _context.OrderDetails
                        .Include(o => o.Order)
                        .Include(o => o.Product)
                        .Where(o => o.Order.CustomerId == uid)
                        .ToListAsync();
                }
            }

            ViewData["UserName"] = HeaderModelView.UserName;
        }


        public IActionResult OnPostRemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cart))
            {
                return RedirectToPage();
            }

            var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cart);
            var itemToRemove = cartItems.Find(ci => ci.ProductId == productId);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cartItems));
            }

            return RedirectToPage();
        }


        public IActionResult OnPostProceedToPay()
        {
            try
            {
                string? UserNameSession = HttpContext.Request.Cookies["UserName"];
                var cart = HttpContext.Session.GetString("Cart");
             

                if (string.IsNullOrEmpty(UserNameSession) || string.IsNullOrEmpty(cart))
                {
                    TempData["ErrorMessage"] = "Cart is empty or user not logged in.";
                    return RedirectToPage();
                }

                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cart);
                var customer = _context.Accounts.FirstOrDefault(c => c.FullName == UserNameSession);

                if (customer == null)
                {
                    TempData["ErrorMessage"] = "Use must login again!";
                    return RedirectToPage();
                }

                var order = new Order
                {
                    CustomerId = customer.AccountID,
                    OrderDate = DateTime.Now,
                    ShippedDate = DateTime.Now,
                    Freight = 0,
                    ShipAddress = customer.Address,
                };

                var orderDetails = new List<BusinessObject.OrderDetail>();
                var productIdsAdded = new HashSet<int>();

                foreach (var cartItem in cartItems)
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);

                    if (product != null)
                    {
                        int curQuantity = product.QuantityPerUnit;

                        var orderDetail = new BusinessObject.OrderDetail
                        {
                            ProductId = product.ProductId,
                            UnitPrice = product.UnitPrice,
                            Quantity = cartItem.Quantity
                        };
                        if (!productIdsAdded.Contains(product.ProductId))
                        {

                            if (curQuantity < cartItem.Quantity)
                            {
                                TempData["ErrorMessage"] = $"Insufficient quantity for {product.ProductName}.";
                                return RedirectToPage();
                            }


                            product.QuantityPerUnit -= cartItem.Quantity;
                            _context.Entry(product).State = EntityState.Modified;
                            productIdsAdded.Add(product.ProductId);
                            orderDetails.Add(orderDetail);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = $"Product with ID {cartItem.ProductId} not found.";
                        return RedirectToPage();
                    }
                }

                // Add order and order details to database
                order.OrderDetails = orderDetails;
                _context.Orders.Add(order);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Order placed successfully.";
                HttpContext.Session.Remove("Cart");
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to place order.";
                return RedirectToPage();
            }
        }



        public void UpdateQuantityInDB(int productId, int newQuantity)
        {
            using (var context = new ShopDbContext())
            {
                var product = context.Products.Find(productId);
                if (product != null)
                {
                    product.QuantityPerUnit = newQuantity;
                    context.SaveChanges();
                }
            }
        }


        public async Task<IActionResult> OnPostChangeStatusAsync(int orderId, byte currentStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = (byte)1;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }


        public class HeaderModel
        {
            public string? UserName { get; set; }
            public string? Type { get; set; }
            public string? WelcomeMessage { get; set; }
        }
        public HeaderModel? HeaderModelView = new HeaderModel();
        public IList<OrderDetail> OrderDetail { get; set; } = default!;
    }

  


    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }

}

