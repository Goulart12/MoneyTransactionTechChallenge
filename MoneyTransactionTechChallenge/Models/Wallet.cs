using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTransactionTechChallenge.Models;

[Table("wallets")]
public class Wallet
{
    [Column("wallet_id")]
    public string Wallet_Id { get; set; }
    
    [Column("balance")]
    public decimal Balance { get; set; }
    
    [Column("user")]
    public User User { get; set; }
    
    [Column("user_id")]
    public string User_Id { get; set; }
    
}