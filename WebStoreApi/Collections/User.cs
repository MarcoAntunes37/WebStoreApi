using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

  namespace WebStoreApi.Collections;
public class User
{
    public ObjectId Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Telephone { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }    
    [EmailAddress]
    public string Email { get; set; }
    public bool OptInForEmails { get; set; }
    public bool OptInForSMS { get; set; }
    public SecurityQuestion SecurityQuestions { get; set; }    
    public List<EmailsSent> EmailsSent { get; set; }
    public Address BillingAddress { get; set; }
    public Address ShippingAddress { get; set; }
    public PaymentInformation PaymentInfo { get; set; }    
    public string RefreshToken { get; set; }    
    public DateTime RefreshTokenExpiryTime { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
}

public class PaymentInformation
{ 
    public string CreditCardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string CVV { get; set; }
}

public class SecurityQuestion
{
    public string Question { get; set; }
    public string Answer { get; set; }
}

public class EmailsSent
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public DateTime DtEmail { get; set; } 
}