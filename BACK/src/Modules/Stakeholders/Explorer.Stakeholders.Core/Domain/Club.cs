using System.Threading.Tasks.Dataflow;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class Club : Entity
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public string? Image { get; init; }
    public long OwnerId { get; init; }
    public int FightsWon { get; init; }
    public Person Owner { get; init; }
    public ICollection<Person> Members { get; set; }
    public ICollection<Achievement> Achievements { get; set; }

    public Club(){ }

    public Club(string name, string? description, string? image, int userId, int fightsWon)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Invalid name.");
        
        Name = name;
        Description = description;
        Image = image;
        OwnerId = userId;
        FightsWon = fightsWon;
    }
}