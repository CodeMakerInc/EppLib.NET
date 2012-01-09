using System;
using System.Collections.Generic;

namespace EppLib.Entities
{
    public class Domain
    {
        public Domain()
        {
            NameServers = new List<string>();
            Hosts = new List<string>();
            Status = new List<Status>();
            Contacts = new List<DomainContact>();
        }

        /// <summary>
        /// The name of the domain
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The identifier assigned by the regystry when the object was created
        /// </summary>
        public string Roid { get; set; }

        /// <summary>
        /// One or more entries describing the status of the domain
        /// </summary>
        public IList<Status> Status { get; set; }

        /// <summary>
        /// The contact id that identifies the owner of the domain name
        /// </summary>
        public string RegistrantId { get; set; }

        /// <summary>
        /// Names of delegated hosts
        /// </summary>
        public IList<string> Hosts { get; set; }

        /// <summary>
        /// Name servers
        /// </summary>
        public IList<string> NameServers { get; set; }

        /// <summary>
        /// The identifier of the sponsoring Registrar
        /// </summary>
        public string ClId { get; set; }

        /// <summary>
        /// The identifier of the Registrar that created the object
        /// </summary>
        public string CrId { get; set; }

        /// <summary>
        /// The creation date and time of the object
        /// </summary>
        public string CrDate { get; set; }

        /// <summary>
        /// The identifier of the Registrar that last updated the object (null if the object has been never updated)
        /// </summary>
        public string UpId { get; set; }

        /// <summary>
        /// The date and time of the most recent object modification (null if the object has been never updated)
        /// </summary>
        public string UpDate { get; set; }

        /// <summary>
        /// The date and time of the most recent sucessful domain name transfer (null if the domain has never been transfered)
        /// </summary>
        public string TrDate { get; set; }

        /// <summary>
        /// The expiry date and time of the domain registration period
        /// </summary>
        public string ExDate { get; set; }

        /// <summary>
        /// The authorization code associated with the domain name. This is only returned if the queriying registrar is the sponsoring registrar and the registrar has provided valid authorization with the request
        /// </summary>
        public string Password { get; set; }

        public List<DomainContact> Contacts { get; set; }
    }
}
