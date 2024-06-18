using EF.DBFirst.Context;
using EF.DBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF.DBFirst.Controllers;

[Route("/api/clients")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly Apbd8Context dbContext;

    public ClientController(Apbd8Context dbContext)
    {
        this.dbContext = dbContext;
    }
    
    [HttpGet]
    public IActionResult getClients()
    {
        return Ok();
    }
    
    [HttpDelete("{IdClient}")]
    public async Task<IActionResult> Delete(int IdClient)
    {
        try
        {
            var client = await dbContext.Clients
                .Where(client => client.IdClient == IdClient)
                .FirstAsync();
            Console.WriteLine(client.IdClient);
            var clientTrips = await dbContext.Trips
                .Where(trip => trip.ClientTrips.Any(clientTrip => clientTrip.IdClient == IdClient))
                .ToListAsync();
            
            if (clientTrips.Count > 0)
            {
                return BadRequest("Client " + IdClient + " is registered in " + clientTrips.Count + " trips!");
            }

            dbContext.Clients.Remove(client);
            await dbContext.SaveChangesAsync(); // DELETE
            return NoContent();
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.ToString());

        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.ToString());
        }
    }
    
    /*
     Wykład 7/8
    [HttpPost]
    public async Task<IActionResult> Create()
    {
        // INSERT - client
        var newClient = new Client
        {
            //id
            FirstName = "Adam",
            LastName = "Adamski",
            Pesel = "19191929",
            Telephone = "Phone",
            Email = "Email"
        };
        dbContext.Add(newClient);
        await dbContext.SaveChangesAsync(); // INSERT
        return Created("client", newClient);
    }

    [HttpPut]
    public async Task<IActionResult> update()
    {
        // 1.
        //var clientToUpdate = await dbContext.Clients.Where(c => c.IdClient == 1).SingleAsync();
        //clientToUpdate.Telephone = "19191919";
        //await dbContext.SaveChangesAsync();
        
        // 2.
        var clientToUpdate = new Client
        {
            IdClient = 1,
            Telephone = "11020202"
        };
        dbContext.Attach(clientToUpdate);
        
        var entry = dbContext.Entry((clientToUpdate));
        entry.Property(p => p.Telephone).IsModified = true;
        
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var clientToRemove = await dbContext.Clients.Where(c => c.IdClient == 1).SingleAsync();
        dbContext.Clients.Remove(clientToRemove);
        await dbContext.SaveChangesAsync(); // DELETE
        
        return Ok();
    }*/
    
    
}