using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using MoneyTransactionTechChallenge.Models.Enums;

namespace MoneyTransactionTechChallenge.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public string Id { get; set; }
    
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string LastName { get; set; }
    
    [Column("email")]
    public string Email { get; set; }
    
    [JsonIgnore]
    [Column("password")]
    public string Password { get; set; }
    
    [JsonIgnore]
    [Column("cpf")]
    public string CPF { get; set; }
    
    [Column("user_type")]
    public Role Role { get; set; }
}