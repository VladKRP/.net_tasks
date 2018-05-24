using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;
using Ninject;
using NLog;

namespace MvcMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MusicStoreEntities _storeContext = new MusicStoreEntities();

        private readonly ILogger _logger;

        public ShoppingCartController(){}

        [Inject]
        public ShoppingCartController(ILogger logger)
        {
            _logger = logger;
        }

        // GET: /ShoppingCart/
        public async Task<ActionResult> Index()
        {

        #if DEBUG
            _logger?.Debug($"Index action of controller {this.GetType().Name} invoked");
        #endif

            var cart = ShoppingCart.GetCart(_storeContext, this);

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems().ToListAsync(),
                CartTotal = await cart.GetTotal()
            };

            return View(viewModel);
        }

        // GET: /ShoppingCart/AddToCart/5
        public async Task<ActionResult> AddToCart(int id)
        {

        #if DEBUG
            _logger?.Debug($"AddToCart action of controller {this.GetType().Name} invoked");
        #endif
            
            var cart = ShoppingCart.GetCart(_storeContext, this);

            await cart.AddToCart(await _storeContext.Albums.SingleAsync(a => a.AlbumId == id));

            await _storeContext.SaveChangesAsync();
            _logger?.Info($"Album with id {id} added to cart. Cart count equals {await cart.GetCount()}");

            return RedirectToAction("Index");
        }

        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int id)
        {

        #if DEBUG
            _logger?.Debug($"RemoveFromCart action of controller {this.GetType().Name} invoked");
        #endif

            var cart = ShoppingCart.GetCart(_storeContext, this);

            var albumName = await _storeContext.Carts
                .Where(i => i.RecordId == id)
                .Select(i => i.Album.Title)
                .SingleOrDefaultAsync();

            var itemCount = await cart.RemoveFromCart(id);

            await _storeContext.SaveChangesAsync();
            _logger.Info($"Album with id {id} removed from cart.Cart count {await cart.GetCount()}");

            var removed = (itemCount > 0) ? " 1 copy of " : string.Empty;

            var results = new ShoppingCartRemoveViewModel
            {
                Message = removed + albumName + " has been removed from your shopping cart.",
                CartTotal = await cart.GetTotal(),
                CartCount = await cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {

        #if DEBUG
            _logger?.Debug($"CartSummary action of controller {this.GetType().Name} invoked");
        #endif

            var cart = ShoppingCart.GetCart(_storeContext, this);

            var cartItems = cart.GetCartItems()
                .Select(a => a.Album.Title)
                .OrderBy(x => x)
                .ToList();

            ViewBag.CartCount = cartItems.Count();
            ViewBag.CartSummary = string.Join("\n", cartItems.Distinct());

            return PartialView("CartSummary");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storeContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
