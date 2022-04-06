using HyperShop.DataAccess.Repository.IRepository;
using HyperShop.Models;
using HyperShop.Models.ViewModels;
using HyperShop.Utility.Class;
using Microsoft.AspNetCore.Mvc;

namespace HyperShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        IUnitOfWork _unitOfWork { get; set; }
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll().ToList();
            var variations = _unitOfWork.ProductVariation.GetAll().ToList();
            products = products.Where(x => variations.Select(v => v.Product_Id).Contains(x.Id)).ToList();

            Dictionary<Product, int> productList = new();
            foreach (var pro in products)
            {
                var numberOfVariation = _unitOfWork.PrimaryImage.GetAll().Where(v => v.Product_Id == pro.Id).ToList().Count();
                productList.Add(pro, numberOfVariation);
            }

            var viewModel = new CustomerProductListVM()
            {
                Products = productList,
                Brands = _unitOfWork.Brand.GetAll().ToList(),
                Colors = _unitOfWork.Color.GetAll().ToList()
            };
            return View(viewModel);
        }

        public IActionResult ProductDetail(int productId)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId);
            var variations = _unitOfWork.ProductVariation.GetAllByProductId(productId);
            var primaryImages = _unitOfWork.PrimaryImage.GetAllByProductId(productId);

            ProductDetailVM productDetail = new ProductDetailVM()
            {
                Product = product,
                Variations = variations.ToList(),
                PrimaryImages = primaryImages.ToList()
            };
            return View(productDetail);
        }

        #region API CALLS
        [HttpPost]
        public IActionResult GetAvailableProducts([FromBody]AvailableProducts data)
        {
            var variations = _unitOfWork.ProductVariation.GetAll();
            var products = _unitOfWork.Product.GetAll().Where(x => variations.Select(v => v.Product_Id).Contains(x.Id));

            if (data.Brand.Count != 0)
            {
                products = products.Where(x => data.Brand.Contains(x.Brand_Id)).ToList();
            }
            if (data.Gender.Count != 0)
            {
                products = products.Where(x => data.Gender.Contains(x.Gender)).ToList();
            }
            if (data.Height.Count != 0)
            {
                products = products.Where(x => data.Height.Contains(x.ShoesHeight)).ToList();
            }
            if (data.Search is not null)
            {
                products = products.Where(x => x.Name.Contains(data.Search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var quantity = products.Count();
            products = products.Skip((data.PageNumber-1)*data.ItemPerPage).Take(data.ItemPerPage).ToList();

            List<int> numberOfColors = new();
            foreach(var pro in products)
            {
                var numberOfVariation = _unitOfWork.PrimaryImage.GetAll().Where(v => v.Product_Id == pro.Id).ToList().Count();
                numberOfColors.Add(numberOfVariation);
            }
            return Json(new { products = products, color= numberOfColors, quantity=quantity, itemPerPage=data.ItemPerPage});
        }

        [HttpPost]
        public IActionResult GetVariations([FromBody]ColourVariations data)
        {
            var color = data.ColorId == -1 ?
                _unitOfWork.Image.GetAllByProductId(data.ProductId).Take(1).ToList()
                : _unitOfWork.Image.GetAllByProdAndColorId(data.ProductId, data.ColorId).ToList();

            var variationsSizes = _unitOfWork.ProductVariation.GetAllByProdIdAndColor(data.ProductId, color[0].Color_Id, "Size").Select(x=>x.Size.SizeValue).ToList();
            var images = _unitOfWork.Image.GetAllByProdAndColorId(data.ProductId, color[0].Color_Id);
            var sizes = _unitOfWork.Size.GetAll();
            return Json(new { variationSizes = variationsSizes, images=images, sizes=sizes });
        }
        #endregion
    }
}
