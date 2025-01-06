using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AzadTurkSln.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => this._mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
    }
}
