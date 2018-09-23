﻿using System.Net;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;
using Ninject.Extensions.Logging;
using Swagger.Net.Annotations;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    /// <summary>
    /// Controller management comment in group.
    /// </summary>
    [RoutePrefix("api/groups")]
    public class GroupCommentController : BaseApiController
    {
        private readonly IGroupCommentService _groupCommentService;

        /// <summary>
        /// Creates an Instance of GroupCommentController.
        /// </summary>
        /// <param name="groupCommentService">Instance of GroupCommentService from business logic</param>
        /// <param name="logger">Instance of logger</param>
        public GroupCommentController(IGroupCommentService groupCommentService, ILogger logger) : base(logger)
        {
            _groupCommentService = groupCommentService;
        }

        /// <summary>
        /// Comment create
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Comment create", typeof(GroupComment))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPost, Route("{groupId}/comment")]
        public IHttpActionResult Create([FromUri]string groupId, GroupComment item)
        {
            item.Id = _groupCommentService.Create(item, groupId);

            return Ok(item);
        }

        /// <summary>
        /// Comment update
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <param name="item">Object</param>
        /// <returns>200(Object)</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Comment update", typeof(GroupComment))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpPut, Route("{groupId}/comment")]
        public IHttpActionResult Update([FromUri] string groupId, GroupComment item)
        {
            _groupCommentService.Update(item, groupId);

            return Ok(item);
        }

        /// <summary>
        /// Comment delete
        /// </summary>
        /// <param name="groupId">Primary key</param>
        /// <param name="item">Object</param>
        /// <returns>200()</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Comment delete")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Not Found")]
        [HttpDelete, Route("{groupId}/comment")]
        public IHttpActionResult Delete([FromUri] string groupId, GroupComment item)
        {
            _groupCommentService.Delete(item, groupId);

            return Ok();
        }
    }
}
