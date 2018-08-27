﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using WasteProducts.Logic.Common.Models.Products;

namespace WasteProducts.Logic.Common.Models.Users
{
    // Удалим лишние проперти когда разберемся, а какие из них вообще лишние на бизнес-слое
    /// <summary>
    /// Full BL version of UserDB.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Email of the user.
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false.
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// PhoneNumber for the user.
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false.
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user.
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDateUtc { get; set; }

        /// <summary>
        /// Is lockout enabled for this userю
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout.
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// All user roles.
        /// </summary>
        public virtual ICollection<string> Roles { get; set; }

        /// <summary>
        /// All user claims.
        /// </summary>
        public virtual ICollection<Claim> Claims { get; set; }

        /// <summary>
        /// All user logins.
        /// </summary>
        public virtual ICollection<UserLogin> Logins { get; set; }

        /// <summary>
        /// Unique key for the user.
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Unique username.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// List of Users which belong to group of friends related to current User.
        /// </summary>
        public virtual IList<User> Friends { get; set; }

        /// <summary>
        /// List of Products which User have ever captured.
        /// </summary>
        public virtual IList<Product> Products { get; set; }

        // TODO decomment after the "Groups" model is enabled
        /// <summary>
        /// List of all Groups to which current User is assigned.
        /// </summary>
        //public virtual List<Group> GroupMembership { get; set; }

        public override bool Equals(object obj)
        {
            return obj is User other &&
                this.Id == other.Id &&
                this.AccessFailedCount == other.AccessFailedCount &&
                this.Claims == other.Claims &&
                this.Email == other.Email &&
                this.EmailConfirmed == other.EmailConfirmed &&
                this.Friends == other.Friends &&
                this.LockoutEnabled == other.LockoutEnabled &&
                this.LockoutEndDateUtc == other.LockoutEndDateUtc &&
                this.Logins == other.Logins &&
                this.Password == other.Password &&
                this.PhoneNumber == other.PhoneNumber &&
                this.PhoneNumberConfirmed == other.PhoneNumberConfirmed &&
                this.Products == other.Products &&
                this.Roles == other.Roles &&
                this.SecurityStamp == other.SecurityStamp &&
                this.TwoFactorEnabled == other.TwoFactorEnabled &&
                this.UserName == other.UserName;
        }
    }
}