﻿using System;
using System.Net;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;
using WasteProducts.Web.ExceptionHandling.Api;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;
using System.Threading.Tasks;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    /// <summary>
    /// Controller management product in group.
    /// </summary>
    [RoutePrefix("api/groups")]
    public class GroupProductController : BaseApiController
    {
        private readonly IGroupProductService _groupProductService;

        /// <summary>
        /// Creates an Instance of GroupProductController.
        /// </summary>
        /// <param name="groupProductService">Instance of GroupProductService from business logic</param>
        /// <param name="logger">Instance of logger</param>
        public GroupProductController(IGroupProductService groupProductService, ILogger logger) : base(logger)
        {
            _groupProductService = groupProductService;
        }

        /// <summary>
        /// Product create
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        /// <param name="userId">Primary key</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product create", typeof(GroupProduct))]
        [HttpPost, Route("{groupId}/product/{userId}")]
        public IHttpActionResult Create(GroupProduct item, [FromUri]string groupId, [FromUri]string userId)
        {
            item.Id = _groupProductService.Create(item, userId, groupId);

            return Ok(item);
        }

        /// <summary>
        /// Product update
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        /// <param name="userId">Primary key</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product update", typeof(GroupProduct))]
        [HttpPut, Route("{groupId}/product/{userId}")]
        public IHttpActionResult Update(GroupProduct item, [FromUri] string groupId, [FromUri] string userId)
        {
            _groupProductService.Update(item, userId, groupId);

            return Ok(item);
        }

        /// <summary>
        /// Product delete
        /// </summary>
        /// <param name="item">Object</param>
        /// <param name="groupId">Primary key</param>
        /// <param name="userId">Primary key</param>
        /// <returns>200()</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Product delete")]
        [HttpDelete, Route("{groupId}/product/{userId}")]
        public IHttpActionResult Delete(GroupProduct item, [FromUri] string groupId, [FromUri] string userId)
        {
            _groupProductService.Delete(item, userId, groupId);

            return Ok();
        }
    }
}
