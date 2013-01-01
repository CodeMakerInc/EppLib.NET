// Copyright 2012 Code Maker Inc. (http://codemaker.net)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections;
namespace EppLib.Entities
{
    public class Contact
    {
        public Contact(string contactId, string fullName, string companyName, string city, string street1, string street2, string street3, string province, string postalCode, string countryCode, string email, Telephone voice, Telephone fax)
        {
            var postalAddress = new PostalAddress
            {
                City = city,
                Street1 = street1,
                Street2 = street2,
                Street3 = street3,
                StateProvince = province,
                PostalCode = postalCode,
                CountryCode = countryCode
            };

            PostalInfo = new PostalInfo
            {
                m_name = fullName,
                m_org = companyName,
                m_type = PostalAddressType.LOC,
                m_address = postalAddress
            };

            Email = email;
            Voice = voice;
            Fax = fax;
            Id = contactId;

        }

        public Contact(string contactId, string fullName, string companyName, string city, string streetAddress, string province, string postalCode, string countryCode, string email, Telephone voice, Telephone fax)
            : this(contactId, fullName, companyName, city, streetAddress, null, null, province, postalCode, countryCode, email, voice, fax)
        {
        }

        public Contact()
        {
        }

        /// <summary>
        /// Contact Id (3 - 16 characters)
        /// </summary>
        public string Id;

        public Telephone Voice;
        public Telephone Fax;
        public string Email;

        public PostalInfo PostalInfo;

        [Obsolete("Use CIRA Extension", false)]
        public string Language;
        [Obsolete("Use CIRA Extension", false)]
        public string CprCategory;
        [Obsolete("Use CIRA Extension", false)]
        public string Individual;
        [Obsolete("Use CIRA Extension", false)]
        public string CiraAgreementVersion;
        [Obsolete("Use CIRA Extension", false)]
        public string AgreementTimestamp;
        [Obsolete("Use CIRA Extension", false)]
        public string WhoisDisplaySetting;

        public string Roid { get; set; }
        [Obsolete("Use StatusList", false)]
        public string Status { get; set; }
        public IList StatusList { get; set; }
        public string ClId { get; set; }
        public string CrId { get; set; }
        public string UpId { get; set; }
        public string CrDate { get; set; }
        public string UpDate { get; set; }
        public string TrDate { get; set; }
        public string Password { get; set; }
        public DiscloseFlags DiscloseMask { get; set; }

        [Flags]
        public enum DiscloseFlags
        {
            All = ~0,
            None = 0,
            NameInt = 1,
            NameLoc = 2,
            OrganizationInt = 4,
            OrganizationLoc = 8,
            AddressInt = 16,
            AddressLoc = 32,
            Voice = 64,
            Fax = 128,
            Email = 256
        }
    }
}
