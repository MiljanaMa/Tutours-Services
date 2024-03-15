using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using Explorer.BuildingBlocks.Core.Domain;

namespace Explorer.Stakeholders.Core.Domain;

public class Person : Entity
{
    public Person(long userId, string name, string surname, string email, string profileImage, string biography,
        string quote, int xP, int level, long? clubId)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Email = email;
        ProfileImage = profileImage;
        Biography = biography;
        Quote = quote;
        XP = xP;
        Level = level;
        Followers = new List<Person>();
        Following = new List<Person>();
        ClubId = clubId;
        Validate();
    }

    public long UserId { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string Email { get; init; }
    public string ProfileImage { get; init; }
    public string Biography { get; init; }
    public string Quote { get; init; }
    public int XP { get; set; }
    public int Level { get; set; }
    public long? ClubId { get; set; }
    public Club? Club { get; set; }

    [InverseProperty(nameof(Following))] public List<Person> Followers { get; } = new();

    [InverseProperty(nameof(Followers))] public List<Person> Following { get; } = new();

    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Surname)) throw new ArgumentException("Invalid Surname");
        if (!MailAddress.TryCreate(Email, out _)) throw new ArgumentException("Invalid Email");
        if (string.IsNullOrWhiteSpace(ProfileImage)) throw new ArgumentException("Invalid ProfileImage");
        if (string.IsNullOrWhiteSpace(Biography)) throw new ArgumentException("Invalid Biography");
        if (string.IsNullOrWhiteSpace(Quote)) throw new ArgumentException("Invalid Quote");
    }

    public bool IsPersonAlreadyFollowed(long personId)
    {
        return Following.Any(f => f.Id == personId);
    }

    public void AddFollowing(Person followed)
    {
        if (IsPersonAlreadyFollowed(followed.Id)) throw new ArgumentException("You already follow this person.");

        if (Id == followed.Id) throw new ArgumentException("You can't follow yourself");

        Following.Add(followed);
    }

    public void RemoveFollowing(Person followed)
    {
        if (!IsPersonAlreadyFollowed(followed.Id)) throw new ArgumentException("You don't follow this person.");

        if (Id == followed.Id) throw new ArgumentException("You can't unfollow yourself");

        Following.Remove(followed);
    }

    public void AddXp(int xp)
    {
        XP += xp;
        Level = (int)Math.Ceiling(XP / 1000.0);
    }

    public bool CanTouristCreateEncounters() 
    { 
        return Level >= 10 ? true : false;
    }   
}