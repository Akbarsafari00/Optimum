using Microsoft.AspNetCore.Mvc;
using Optimum.Cqrs.Contracts;
using Optimum.Cqrs.Contracts.Commands;
using Optimum.Cqrs.Contracts.Queries;
using Optimum.Messaging.Contracts;
using Optimum.Options;
using Optimum.ServiceDiscovery.Contracts;
using Optimum.ServiceDiscovery.Models;
using Optimum.Test.Api.Commands;
using Optimum.Test.Api.Documents;
using Optimum.Test.Api.Queries;

namespace Optimum.Test.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ServiceDiscoveryController : ControllerBase
{


    private readonly IServiceDiscovery _serviceDiscovery;
    private readonly ServiceOptions _serviceOptions;

    public ServiceDiscoveryController(IServiceDiscovery serviceDiscovery, ServiceOptions serviceOptions)
    {
        _serviceDiscovery = serviceDiscovery;
        _serviceOptions = serviceOptions;
    }

    [HttpGet("GetCurrentService")]
    public async Task<Service?> GetCurrentService()
    {
        var res = await _serviceDiscovery.GetServiceAsync(_serviceOptions.Id);
        return res;
    }

    [HttpGet("GetServiceById/{id}")]
    public async Task<Service?> GetServiceById(string id)
    {
        var res = await _serviceDiscovery.GetServiceAsync(id);
        return res;
    }

}