namespace EF.DBFirst.Models;

public class TripRegistration
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telephone { get; set; } = null!;
    public string Pesel { get; set; } = null!; 
    public int IdTrip { get; set; }
    public String TripName { get; set; }
    public DateTime PaymentDate { get; set; }
}