using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStoreApi.Collections;

namespace WebStoreApiUTest.Mockdata
{
    internal class UserMockData
    {
        public static List<User> GetUsers()
        {
            return new List<User> {
                new User
                {
                    Id = new ObjectId(),
                    FirstName = "Teste",
                    LastName = "Teste",
                    Telephone = "Teste",
                    Username = "Teste",
                    Password = "Teste",
                    Email = "Teste@teste.com",
                    OptInForEmails = false,
                    OptInForSMS = true,
    },
                new User
                {
                    Id = new ObjectId(),
                    FirstName = "João",
                    LastName = "Silva",
                    Telephone = "123456789",
                    Username = "joaosilva",
                    Password = "senha123",
                    Email = "joao.silva@example.com",
                    OptInForEmails = true,
                    OptInForSMS = false
                },
                new User
                {
                    Id = new ObjectId(),
                    FirstName = "Maria",
                    LastName = "Santos",
                    Telephone = "987654321",
                    Username = "msantos",
                    Password = "abcd1234",
                    Email = "maria.santos@example.com",
                    OptInForEmails = true,
                    OptInForSMS = true
                },
            };
        }
    }
}
