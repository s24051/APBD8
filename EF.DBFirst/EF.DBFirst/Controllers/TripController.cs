using EF.DBFirst.Context;
using EF.DBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF.DBFirst.Controllers;

[Route("/api/trips")]
[ApiController]
public class TripController: ControllerBase
{
    private readonly Apbd8Context dbContext;
    public TripController(Apbd8Context dbContext)
    {
        this.dbContext = dbContext;
    }

    /*
     * Endpoint dla żądania HTTP GET /api/trips
     *  1. Endpoint powinien zwrócić listę wycieczek posortowanych
     *      malejącą po dacie rozpoczęcia wycieczki.
     *  2. Dodaj opcjonalną możliwość stronicowania wyniku z
     *      pomocą query stringa i parametrów page i pageSize.
     *      Możemy założyć, że domyślny pageSize to 10
     */
    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tripList = await dbContext.Trips.ToListAsync(); // tu leci transakcja
        var tripCount = tripList.Count; // zeby tu nie musiala
        var totalPages = (int)Math.Ceiling(tripCount / (double)pageSize);

        var trips = tripList
            .OrderByDescending(trip => trip.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        return Ok(new {trips, pageNum = page, pagesize = pageSize, allPages = totalPages});
    }

    [HttpPost("{TripId}/clients")]
    public async Task<IActionResult> PostTrip(int TripId, [FromBody]TripRegistration registration)
    {
        
        try
        {
            var clients = await dbContext.Clients
                .ToListAsync();
            
            // czy istnieje klient o tym peselu
            var samePesel =  clients
                .Where(client => client.Pesel == registration.Pesel)
                .First();
                
            // czy trip istnieje i czy nie jest w przeszłości
            var trip = await dbContext.Trips
                .Where(trip => trip.IdTrip == TripId && trip.DateFrom < DateTime.Now)
                .SingleAsync();

            // jak już się posypaliśmy przy kliencie o tym samym peselu, to ten warunek się nie wywoła
            var samePeselOnTrip = clients
                .Where(client =>
                    trip
                        .ClientTrips.Where(clientTrip =>
                            clientTrip.IdClient == client.IdClient && client.Pesel == registration.Pesel)
                        .FirstOrDefault() != null);

            var client = new Client
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Pesel = registration.Pesel,
                Telephone = registration.Telephone,
                Email = registration.Telephone
            };
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync(); // INSERT, dla clientTrip


            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdClientNavigation = client,
                IdTrip = trip.IdTrip,
                IdTripNavigation = trip,
                RegisteredAt = DateTime.Now,
                PaymentDate = registration.PaymentDate
            };
            dbContext.ClientTrips.Add(clientTrip);
            await dbContext.SaveChangesAsync(); // INSERT, dla clienta i tripów
            
            trip.ClientTrips.Add(clientTrip);
            client.ClientTrips.Add(clientTrip);
            await dbContext.SaveChangesAsync(); // INSERT
            return Ok(clientTrip);
        } 
        catch (ArgumentNullException e)
        {
            return BadRequest(e.ToString());

        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.ToString());
        }
        
        
        return Ok();
    }
}