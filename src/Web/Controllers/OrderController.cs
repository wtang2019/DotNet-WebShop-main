using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Web.Features.MyOrders;
using Microsoft.eShopWeb.Web.Features.OrderDetails;
using static NewRelic.Api.Agent.NewRelic;
using LibGit2Sharp;

namespace Microsoft.eShopWeb.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
[Route("[controller]/[action]")]
public class OrderController : Controller
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
     //   string repositoryPath = "/Users/wtang/VSCodeProjects/DotNet-WebShop";
     //   using (var repo = new Repository(repositoryPath))
    //{
    //var headCommit = repo.Head.Tip;
    //var sha = headCommit.Sha;
        NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction.AddCustomAttribute("tags.commit", "c61c5a0b3a814b5ae493edce7109920aba2f22e8");
    //}
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var viewModel = await _mediator.Send(new GetMyOrders(User.Identity.Name));

        return View(viewModel);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> Detail(int orderId)
    {
        var viewModel = await _mediator.Send(new GetOrderDetails(User.Identity.Name, orderId));

        if (viewModel == null)
        {
            try
        {
            throw new Exception("500 Internal Server Error");
        }
        catch(Exception ex)
        {
                NewRelic.Api.Agent.NewRelic.NoticeError(ex);
        }
        }

        return View(viewModel);
    }
}
