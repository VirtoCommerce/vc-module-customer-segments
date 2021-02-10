using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CustomerSegmentsModule.Core;
using VirtoCommerce.CustomerSegmentsModule.Core.Models;
using VirtoCommerce.CustomerSegmentsModule.Core.Models.Search;
using VirtoCommerce.CustomerSegmentsModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.CustomerSegmentsModule.Web.Controllers.Api
{
    [Route("api/customersegments")]
    public class CustomerSegmentController : Controller
    {
        private readonly ICustomerSegmentService _customerSegmentService;
        private readonly ICustomerSegmentSearchService _customerSegmentSearchService;

        public readonly int _maxAllowedSegments;
        public readonly int _maxActiveSegments;

        public CustomerSegmentController(ICustomerSegmentService customerSegmentService,
            ICustomerSegmentSearchService customerSegmentSearchService,
            ISettingsManager settingsManager)
        {
            _customerSegmentService = customerSegmentService;
            _customerSegmentSearchService = customerSegmentSearchService;

            _maxAllowedSegments = settingsManager.GetValue(ModuleConstants.Settings.General.MaxAllowedSegments.Name, 1000);
            _maxActiveSegments = settingsManager.GetValue(ModuleConstants.Settings.General.MaxActiveSegments.Name, 20);
        }

        /// <summary>
        /// Get new customer segment object
        /// </summary>
        /// <remarks>Return a new customer segment object with populated dynamic expression tree</remarks>
        [HttpGet]
        [Route("new")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public async Task<ActionResult<CustomerSegment>> GetNewCustomerSegment()
        {
            if (await CanAddNewSegment())
            {
                var result = AbstractTypeFactory<CustomerSegment>.TryCreateInstance();

                result.ExpressionTree = AbstractTypeFactory<CustomerSegmentTree>.TryCreateInstance();
                result.ExpressionTree.MergeFromPrototype(AbstractTypeFactory<CustomerSegmentTreePrototype>.TryCreateInstance());
                result.IsActive = true;

                return Ok(result);
            }

            return GetAddNewSegmenErrorResult();
        }

        /// <summary>
        /// Get customer segment by ID
        /// </summary>
        /// <param name="id">Customer segment ID</param>
        [HttpGet]
        [Route("{id}")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<CustomerSegment>> GetCustomerSegmentById([FromRoute] string id)
        {
            var result = (await _customerSegmentService.GetByIdsAsync(new[] { id })).FirstOrDefault();

            result?.ExpressionTree?.MergeFromPrototype(AbstractTypeFactory<CustomerSegmentTreePrototype>.TryCreateInstance());

            return Ok(result);
        }

        /// <summary>
        /// Create/Update customer segments.
        /// </summary>
        /// <param name="customerSegments">The customer segments.</param>
        [HttpPost]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        public async Task<ActionResult<CustomerSegment[]>> SaveCustomerSegments([FromBody] CustomerSegment customerSegment)
        {
            if (customerSegment != null)
            {
                if (customerSegment.IsTransient() && !await CanAddNewSegment())
                {
                    return GetAddNewSegmenErrorResult();
                }
                else if (customerSegment.IsActive && !await CanSetSegmentActive(customerSegment.Id))
                {
                    return GetSetIsActiveErrorResult();
                }

                await _customerSegmentService.SaveChangesAsync(new[] { customerSegment });
            }

            return Ok(customerSegment);
        }

        /// <summary>
        /// Deletes customer segments by IDs.
        /// </summary>
        /// <param name="ids">Customer segment IDs.</param>
        [HttpDelete]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCustomerSegments([FromQuery] string[] ids)
        {
            await _customerSegmentService.DeleteAsync(ids);

            return NoContent();
        }

        /// <summary>
        /// Search customer segments by specified search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Search result with total number of found customer segments and all found customer segments.</returns>
        [HttpPost]
        [Route("search")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<CustomerSegmentSearchResult>> SearchCustomerSegments([FromBody] CustomerSegmentSearchCriteria criteria)
        {
            var result = await _customerSegmentSearchService.SearchCustomerSegmentsAsync(criteria);

            return Ok(result);
        }


        private async Task<bool> CanAddNewSegment()
        {
            var searchResult = await _customerSegmentSearchService.SearchCustomerSegmentsAsync(new CustomerSegmentSearchCriteria { Skip = 0, Take = 0 });
            return _maxAllowedSegments > searchResult.TotalCount;
        }

        private async Task<bool> CanSetSegmentActive(string segmentId)
        {
            bool result;

            if (!string.IsNullOrEmpty(segmentId))
            {
                var segment = (await _customerSegmentService.GetByIdsAsync(new[] { segmentId })).FirstOrDefault();
                result = segment != null && segment.IsActive;
            }
            else
            {
                var searchResultActive = await _customerSegmentSearchService.SearchCustomerSegmentsAsync(new CustomerSegmentSearchCriteria { IsActive = true, Skip = 0, Take = 0 });
                result = _maxActiveSegments > searchResultActive.TotalCount;
            }

            return result;
        }

        private BadRequestObjectResult GetAddNewSegmenErrorResult()
        {
            return BadRequest(new
            {
                Message = $"Can't create a new segment, there are already {_maxActiveSegments} segments created."
            });
        }

        private BadRequestObjectResult GetSetIsActiveErrorResult()
        {
            return BadRequest(new
            {
                Message = $"Can't set an active segment, there are already {_maxAllowedSegments} active segments."
            });
        }
    }
}
