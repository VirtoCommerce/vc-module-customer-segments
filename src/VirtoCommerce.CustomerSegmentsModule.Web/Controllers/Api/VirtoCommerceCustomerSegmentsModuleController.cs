using VirtoCommerce.CustomerSegmentsModule.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace VirtoCommerce.CustomerSegmentsModule.Web.Controllers.Api
{
    [Route("api/VirtoCommerceCustomerSegmentsModule")]
    public class VirtoCommerceCustomerSegmentsModuleController : Controller
    {
        // GET: api/VirtoCommerceCustomerSegmentsModule
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
