using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Visib.Api.Models
{
    public enum UserType
    {
        Admin = 1,
        Provider = 2,
        Consumer = 3
    }

    public class AppUser : IdentityUser
    {
        // Extended Properties        
        // Extended Properties        
        public string Name { get; set; }
        public UserType Type { get; set; }
        public int Code { get; set; }
        public DateTime? BirthDate { get; set; }
        public string MobilePhone { get; set; }
        public long? FacebookId { get; set; }
        public bool HasSetPassword { get; set; }
        public string DesiredLanguage { get; set; }
        public string About { get; set; }
        public string Website { get; set; }
        public string AddressNumber { get; set; }
        public string BusinessPhone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
        public string AddressAddon { get; set; }
        public string Country { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public class Relation
    {

        [Key]
        public Guid Id { get; set; }

        public string FollowedId { get; set; }


        public string FollowerId { get; set; }
    }
}