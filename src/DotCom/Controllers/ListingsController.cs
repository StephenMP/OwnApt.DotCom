using Microsoft.AspNetCore.Mvc;

namespace OwnApt.DotCom.Controllers
{
    public class ListingsController : Controller
    {
        #region Public Methods

        public IActionResult Index()
        {
            return View();
        }

        #endregion Public Methods
    }
}
