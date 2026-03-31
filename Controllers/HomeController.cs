using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Unstorekle.Data;
using Unstorekle.Models;

namespace Unstore.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/get/all")]
    public IActionResult GetAll([FromServices] AppDbContext context)
        => Ok(context.Clients.ToList());

    [HttpPost("/create/")]
    public IActionResult Post([FromServices] AppDbContext context, [FromBody] Client client)
    {
        context.Clients.Add(client);
        context.SaveChanges();

        return Created($"/get/{client.Id}", client);
    }

    [HttpPut("/update/{id:int}")]
    public IActionResult Put([FromServices] AppDbContext context, [FromBody] Client client, [FromRoute] int id)
    {
        var clientTracked = context.Clients.FirstOrDefault(c => c.Id == id);
        if (clientTracked is not null)
        {
            clientTracked.Name = client.Name;
            clientTracked.Address = client.Address;
            clientTracked.Email = client.Email;
            clientTracked.ContactNumber = client.ContactNumber;
            
            context.Clients.Update(clientTracked);
            context.SaveChanges();
            
            return Ok("Client update: " + Environment.NewLine + JsonSerializer.Serialize(clientTracked));
        }
        else
        {
            return NotFound("Client not found " + id);
        }
    }

    [HttpDelete("/Delete/{id:int}")]
    public IActionResult Delete([FromServices] AppDbContext context, [FromRoute] int id)
    {
        var client = context.Clients.FirstOrDefault(c => c.Id == id);

        if (client is not null)
        {
            context.Clients.Remove(client);
            context.SaveChanges();

            return Ok("Client removed with success");
        }
        else
        {
            return NotFound("Id doesn't exists in the database.");
        }
    }
}