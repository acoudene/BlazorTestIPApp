using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Sockets;

namespace BlazorTestIPApp.Server
{
  [Route("api/[controller]")]
  [ApiController]
  public class IpClientController : ControllerBase
  {

    [HttpGet]
    public Results<Ok<string>, ProblemHttpResult> GetClientIPv4()
    {
      IPAddress? remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
      if (remoteIpAddress is null)
        return TypedResults.Problem("No remote ip address");

      // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
      // This usually only happens when the browser is on the same machine as the server.
      if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
      {
        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
          .First(x => x.AddressFamily == AddressFamily.InterNetwork);
      }
      
      return TypedResults.Ok(remoteIpAddress.ToString());
    }
  }
}
