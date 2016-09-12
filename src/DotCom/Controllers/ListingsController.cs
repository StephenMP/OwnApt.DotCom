using Microsoft.AspNetCore.Mvc;

namespace OwnApt.DotCom.Controllers
{
    public class ListingsController : Controller
    {
        #region Methods

        public IActionResult Index()
        {
            return View();
        }

        #endregion Methods
    }
}
