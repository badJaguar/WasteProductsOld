﻿using System;
using System.Collections.Generic;
using WasteProducts.DataAccess.Common.Models.Donation;

namespace WasteProducts.DataAccess.Common.Repositories.Donation
{
    interface IDonationRepository : IDisposable
    {
        /// <summary>
        /// Returns all donations in an enumerable.
        /// </summary>
        /// <returns>All the donations.</returns>
        IEnumerable<DonationDB> GetDonationList();

        /// <summary>
        /// Returns the donation by its payment number.
        /// </summary>
        /// <param name="paymentNo">Payment number of the requested donation.</param>
        /// <returns>Donation with the specific payment number.</returns>
        DonationDB GetDonation(int paymentNo);

        /// <summary>
        /// Creates new donation in the repository.
        /// </summary>
        /// <param name="user">New donation to add.</param>
        void Create(DonationDB donation);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void Save();
    }
}