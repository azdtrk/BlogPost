using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => this._mediator ??= this.HttpContext.RequestServices.GetService<IMediator>();
    }
}
