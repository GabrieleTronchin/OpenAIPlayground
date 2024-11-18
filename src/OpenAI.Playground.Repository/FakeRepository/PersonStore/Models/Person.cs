namespace OpenAI.Playground.Repository.FakeRepository.PersonStore.Models;

public class Person
{
    public int id { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public string? birthday { get; set; }
    public string? gender { get; set; }
    public Address address { get; set; } = new();
    public string? website { get; set; }
    public string? image { get; set; }
}
