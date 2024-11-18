namespace OpenAI.Playground.Repository.FakeRepository.PersonStore.Models;

public class Address
{
    public int id { get; set; }
    public string? street { get; set; }
    public string? streetName { get; set; }
    public string? buildingNumber { get; set; }
    public string? city { get; set; }
    public string? zipcode { get; set; }
    public string? country { get; set; }
    public string? country_code { get; set; }
    public float latitude { get; set; }
    public float longitude { get; set; }
}
