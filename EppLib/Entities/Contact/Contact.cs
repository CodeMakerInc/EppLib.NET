using System;

namespace EppLib.Entities
{
    public class Contact
    {
        public Contact(string contactId, string fullName, string companyName, string city, string streetAddress, string province, string postalCode, string countryCode, string email, Telephone voice, Telephone fax)
        {
            var postalAddress = new PostalAddress
            {
                City = city,
                Street1 = streetAddress,
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
        public string Language;
        public string CprCategory;
        public string Individual;
        public string CiraAgreementVersion;
        public string AgreementTimestamp;
        public string WhoisDisplaySetting;

        public string Roid { get; set; }
        public string Status { get; set; }
        public string ClId { get; set; }
        public string CrId { get; set; }
        public string CrDate { get; set; }
    }
}
