using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EppLib.Entities;
using EppLib.Extensions.Cira;

namespace EppLib.Tests
{
    public class Program
    {
        private const string registrarNumber = "d1234";

        private static void Main(string[] args) { CiraTecnicalTest(); }

        private static void CiraTecnicalTest()
        {
            var tcpTransport = new TcpTransport("epp.test.cira.ca", 700, new X509Certificate("cert.pfx", "password"), true);

            var service = new Service(tcpTransport);

            //1. SSL connection establishment
            Console.WriteLine("TEST: 1");
            service.Connect();

            //2. EPP <login> command with your ‘a’ account
            Console.WriteLine("TEST: 2");
            var logingCmd = new Login("username", "password");

            var response = service.Execute(logingCmd);

            PrintResponse(response);

            //3. Using the correct EPP call, get the latest CIRA Registrant Agreement
            Console.WriteLine("TEST: 3");
            var agreementCmd = new GetAgreement();

            var getAgreementResponse = service.Execute(agreementCmd);

            var agreementVersion = getAgreementResponse.AgreementVersion;
            var agreementText = getAgreementResponse.Agreement;
            var agreementLanguage = getAgreementResponse.Language;

            PrintResponse(response);
            Console.WriteLine("Agreement Version:{0}", agreementVersion);
            /*
             4. Create a Registrant contact using: 
                -the same ID as your Registrar Number prefixed with the word ‘rant’ (e.g. rant75)
                -CPR category CCT
                -Full postal information, phone number, fax number, and email address
                -Agreed to latest CIRA Registrant Agreement version
             */
            Console.WriteLine("TEST: 4");
            var registrantContact = new Contact("rant" + registrarNumber,
                "Registrant Step Four", "Example Inc.",
                "Toronto", "301 La Fanfan Ave.", "ON", "M5V 3T1", "CA",
                "foo@example.com",
                new Telephone { Value = "+1.6478913606", Extension = "333" },
                new Telephone { Value = "+1.6478913607" });


            var registrantContactCmd = new CiraContactCreate(registrantContact);

            registrantContactCmd.CprCategory = CiraCprCategories.CCT;
            registrantContactCmd.AgreementVersion = agreementVersion;
            registrantContactCmd.AggreementValue = "Y";
            registrantContactCmd.Language = "en";
            registrantContactCmd.OriginatingIpAddress = "127.0.0.1";
            registrantContactCmd.CreatedByResellerId = registrarNumber;
            
            var response1 = service.Execute(registrantContactCmd);

            PrintResponse(response1);

            /*
             5. Create an administrative contact
             -the same ID as your Registrar Number prefixed with the word ‘admin’ (e.g. admin75)
             -using all mandatory elements required for a Canadian administrative contact
             -omit CPR category (he have not agreed to the CIRA agreement)
            */
            Console.WriteLine("TEST: 5");
            var adminContact = new Contact("admin" + registrarNumber,
               "Administrative Step Five", "Example Inc.",
               "Toronto", "301 La Fanfan Ave.", "ON", "M5V 3T1", "CA",
               "foo@example.com",
               new Telephone { Value = "+1.6478913606", Extension = "333" },
               new Telephone { Value = "+1.6478913607" });

            var adminContactCmd = new CiraContactCreate(adminContact);
            
            adminContactCmd.CprCategory = null;
            adminContactCmd.AgreementVersion = null;
            adminContactCmd.AggreementValue = null;
            adminContactCmd.Language = "en";
            adminContactCmd.OriginatingIpAddress =  "127.0.0.1";
            adminContactCmd.CreatedByResellerId =  registrarNumber;
           
            const string adminContactId = "admin" + registrarNumber;

            var loginresponse = service.Execute(adminContactCmd);

            PrintResponse(loginresponse);

            //6. Get information for the contact created in operation #4
            Console.WriteLine("TEST: 6");
            var getContactInfo = new ContactInfo(registrantContact.Id);

            var contactInfoResponse = service.Execute(getContactInfo);

            PrintResponse(contactInfoResponse);

            //7. Do a Registrant transfer for domain <registrar number>-3.ca to the Registrant created in operation #4
            Console.WriteLine("TEST: 7");
            
            //NOTE: registrant transfers are domain updates
            
            var registrantTransferCmd = new DomainUpdate(registrarNumber + "-3.ca");
            //var registrantTransferCmd = new DomainUpdate("3406310-4.ca");

            var domainChange = new DomainChange { RegistrantContactId = registrantContact.Id };

            registrantTransferCmd.DomainChange = domainChange;

            var response2 = service.Execute(registrantTransferCmd);

            PrintResponse(response2);

            //8. Update the contact created in operation #4 to no longer have a fax number
            Console.WriteLine("TEST: 8");
            var contactUpdateCmd = new ContactUpdate(registrantContact.Id);

            var contactChange = new ContactChange(registrantContact);

            contactChange.Fax = new Telephone("", "");

            contactUpdateCmd.ContactChange = contactChange;

            //NOTE:the docs say that the cpr category is update for domain contact
            //but they show a sample of a contact update request that does not include the extension
            //NOTE: Organization cannot be entered when CPR Category indicates a non individual - see documentation
            contactUpdateCmd.Extensions.Add(new CiraContactUpdateExtension { CprCategory = CiraCprCategories.CCT });

            var response3 = service.Execute(contactUpdateCmd);

            PrintResponse(response3);

            //8.1 Get contact info and check the fax number dissapeared
            var contactInfoCmd1 = new ContactInfo(registrantContact.Id);

            var contactInfoResponse1 = service.Execute(contactInfoCmd1);

            PrintResponse(contactInfoResponse1);

            //9. Do a domain:check on <registrar number>a.ca
            Console.WriteLine("TEST: 9");
            const string domainStep10 = registrarNumber + "a.ca";

            var domainCheckCmd = new DomainCheck(domainStep10);

            var response4 = service.Execute(domainCheckCmd);

            PrintResponse(response4);

            /*
             10. Create a domain using:
             -a domain name set to <registrar number>a.ca
             -a Registrant who is a Permanent Resident
             -the same administrative contact as the Registrant
             -0 hosts
             -the minimum registration period
            */
            Console.WriteLine("TEST: 10");
            //NOTE: CPR categories CCT and RES where merged into CCR

            //BUG: the registrant needs to be a Permanent Resident
            //TODO: create a new contact that is a permanent resident
            //10.1 
            var registrantContact10 = new Contact("ten" + registrarNumber,
               "Registrant Step Ten", "Example Inc.",
               "Toronto", "301 La Fanfan Ave.", "ON", "M5V 3T1", "CA",
               "foo@example.com",
               new Telephone { Value = "+1.6478913606", Extension = "333" },
               new Telephone { Value = "+1.6478913607" });


            registrantContactCmd = new CiraContactCreate(registrantContact10);
            registrantContactCmd.CprCategory = CiraCprCategories.RES;
            registrantContactCmd.AgreementVersion = agreementVersion;
            registrantContactCmd.AggreementValue = "Y";
            registrantContactCmd.OriginatingIpAddress = "127.0.0.1";
            registrantContactCmd.Language = "en";
            registrantContactCmd.CreatedByResellerId = registrarNumber;

            //ContactCreate.MakeContact(registrantContact10,  agreementVersion, "Y", "127.0.0.1", "en", registrarNumber);

            var response5 = service.Execute(registrantContactCmd);

            PrintResponse(response5);

            //10.2
            var domainCreateCmd = new DomainCreate(domainStep10, registrantContact10.Id);

            domainCreateCmd.DomainContacts.Add(new DomainContact(registrantContact10.Id, "admin"));

            //NOTE: password is compulsory
            domainCreateCmd.Password = "s0mer4ndom";

            var response6 = service.Execute(domainCreateCmd);

            PrintResponse(response6);

            /*11. Do a host:check on hosts <registrar number>.example.com and <registrar number>.example.net*/
            Console.WriteLine("TEST: 11");
            var hostCheckCmd = new HostCheck(new List<string> { registrarNumber + ".example.com", registrarNumber + ".example.net" });

            var response7 = service.Execute(hostCheckCmd);

            PrintResponse(response7);

            /*
             12. Create 2 hosts with the following name formats:
             <registrar number>.example.com
             <registrar number>.example.net
             */
            Console.WriteLine("TEST: 12");
            //CIRA only creates a host at a time

            //12.1
            var hostCreateCmd = new HostCreate(new Host(registrarNumber + ".example.com"));

            var response8 = service.Execute(hostCreateCmd);

            PrintResponse(response8);

            //12.2
            hostCreateCmd = new HostCreate(new Host(registrarNumber + ".example.net"));

            var response9 = service.Execute(hostCreateCmd);

            PrintResponse(response9);

            /*
             13. Create a domain using:
             -a domain name set to <registrar number>b.ca
             -the pre-populated contact id <registrar number> as the administrative contact
             -a Registrant who is a Corporation
             -2 hosts created in operation #12 <- the nameservers
             -a maximum registration period (10 years)
             */
            Console.WriteLine("TEST: 13");
            //13.1 - Create a corporation
            
            //If it is a corporation you can not provide company name
            var corporation = new Contact("corp" + registrarNumber, "Acme Corp.", null, "Toronto",
                                          "some where 22", "ON", "M6G2L1", "CA", "joe@isqsolutions.com",
                                          new Telephone("+1.1234567890", null), new Telephone("+1.1234567890", null));

            //var createCorporationContactCmd = ContactCreate.MakeContact(corporation, CiraCprCategories.CCO, agreementVersion, "Y", "127.0.0.1", "en", registrarNumber);

            var createCorporationContactCmd = new CiraContactCreate(corporation);
            createCorporationContactCmd.CprCategory = CiraCprCategories.CCO;
            createCorporationContactCmd.AgreementVersion = agreementVersion;
            createCorporationContactCmd.AggreementValue = "Y";
            createCorporationContactCmd.OriginatingIpAddress = "127.0.0.1";
            createCorporationContactCmd.Language = "en";
            createCorporationContactCmd.CreatedByResellerId = registrarNumber;

            var response10 = service.Execute(createCorporationContactCmd);

            PrintResponse(response10);

            /* var domainUpdateCmd = new DomainUpdate(registrarNumber + "-10.ca");

             domainUpdateCmd.ToRemove.Status.Add(new Status("", "serverDeleteProhibited"));

             response = service.Execute(domainUpdateCmd);

             PrintResponse(response);*/

            //13.2 - Create the domain

            //var createDomainCmd = new DomainCreate(registrarNumber + "b.ca", corporation.Id);
            var createDomainCmd = new DomainCreate(registrarNumber + "b.ca", "corp" + registrarNumber);

            createDomainCmd.Period = new DomainPeriod(10, "y");

            //NOTE:The administrative or technical contact must be an Individual
            //BUG: admin contact needs be the prepopulated 3406310
            createDomainCmd.DomainContacts.Add(new DomainContact(registrarNumber, "admin"));

            //NOTE:Create the host on the Registry system before you assign it to a domain
            createDomainCmd.NameServers.Add(registrarNumber + ".example.com");
            createDomainCmd.NameServers.Add(registrarNumber + ".example.net");

            createDomainCmd.Password = "c0mb0site";

            var response11 = service.Execute(createDomainCmd);

            PrintResponse(response11);

            /*
             14. Create a domain using: 
             - a domain name set to <registrar number>c.ca
             - a Registrant who is an Association
             - the administrative contact set to the contact created in operation #5
             - maximum number of technical contacts assigned to it (max is 3)
            - 0 hosts
            - a 2-year term
             */
            Console.WriteLine("TEST: 14");
            var association = new Contact("assoc" + registrarNumber, "Beer Producers Association", null, "Toronto",
                                          "some where 22", "ON", "M6G2L1", "CA", "joe@isqsolutions.com",
                                          new Telephone("+1.1234567890", null), new Telephone("+1.1234567890", null));

            //var createAssociationContactCmd = ContactCreate.MakeContact(association, CiraCprCategories.ASS, agreementVersion, "Y", "127.0.0.1", "en", registrarNumber);

            var createAssociationContactCmd = new CiraContactCreate(association);
            createAssociationContactCmd.CprCategory = CiraCprCategories.ASS;
            createAssociationContactCmd.AgreementVersion = agreementVersion;
            createAssociationContactCmd.AggreementValue = "Y";
            createAssociationContactCmd.OriginatingIpAddress = "127.0.0.1";
            createAssociationContactCmd.Language = "en";
            createAssociationContactCmd.CreatedByResellerId = registrarNumber;

            var response12 = service.Execute(createAssociationContactCmd);

            PrintResponse(response12);

            //tech1
            var tech1 = new Contact("tech1" + registrarNumber, "Technician #1", "Beer Producers Association", "Toronto",
                                          "some where 22", "ON", "M6G2L1", "CA", "joe@isqsolutions.com",
                                          new Telephone("+1.1234567890", null), new Telephone("+1.1234567890", null));

            //var createTech1ContactCmd = ContactCreate.MakeContact(tech1, CiraCprCategories.CCT, agreementVersion, "Y", "127.0.0.1", "en", registrarNumber);

            var createTech1ContactCmd = new CiraContactCreate(tech1);
            createTech1ContactCmd.CprCategory = CiraCprCategories.CCT;
            createTech1ContactCmd.AgreementVersion = agreementVersion;
            createTech1ContactCmd.AggreementValue = "Y";
            createTech1ContactCmd.OriginatingIpAddress = "127.0.0.1";
            createTech1ContactCmd.Language = "en";
            createTech1ContactCmd.CreatedByResellerId = registrarNumber;

            var response13 = service.Execute(createTech1ContactCmd);

            PrintResponse(response13);

            //tech2
            var tech2 = new Contact("tech2" + registrarNumber, "Technician #2", "Beer Producers Association", "Toronto",
                                          "some where 22", "ON", "M6G2L1", "CA", "joe@isqsolutions.com",
                                          new Telephone("+1.1234567890", null), new Telephone("+1.1234567890", null));

            //var createTech2ContactCmd = ContactCreate.MakeContact(tech2, CiraCprCategories.CCT, agreementVersion, "Y", "127.0.0.1", "en", registrarNumber);

            var createTech2ContactCmd = new CiraContactCreate(tech2);
            createTech2ContactCmd.CprCategory = CiraCprCategories.CCT;
            createTech2ContactCmd.AgreementVersion = agreementVersion;
            createTech2ContactCmd.AggreementValue = "Y";
            createTech2ContactCmd.OriginatingIpAddress = "127.0.0.1";
            createTech2ContactCmd.Language = "en";
            createTech2ContactCmd.CreatedByResellerId = registrarNumber;

            var response14 = service.Execute(createTech2ContactCmd);

            PrintResponse(response14);

            //tech1
            var tech3 = new Contact("tech3" + registrarNumber, "Technician #3", "Beer Producers Association", "Toronto",
                                          "some where 22", "ON", "M6G2L1", "CA", "joe@isqsolutions.com",
                                          new Telephone("+1.1234567890", null), new Telephone("+1.1234567890", null));

            //var createTech3ContactCmd = ContactCreate.MakeContact(tech3, CiraCprCategories.CCT, agreementVersion, "Y", "127.0.0.1", "en", registrarNumber);

            var createTech3ContactCmd = new CiraContactCreate(tech3);
            createTech3ContactCmd.CprCategory = CiraCprCategories.CCT;
            createTech3ContactCmd.AgreementVersion = agreementVersion;
            createTech3ContactCmd.AggreementValue = "Y";
            createTech3ContactCmd.OriginatingIpAddress = "127.0.0.1";
            createTech3ContactCmd.Language = "en";
            createTech3ContactCmd.CreatedByResellerId = registrarNumber;


            var response15 = service.Execute(createTech3ContactCmd);

            PrintResponse(response15);

            const string step14domain = registrarNumber + "c.ca";

            createDomainCmd = new DomainCreate(step14domain, association.Id);

            createDomainCmd.Period = new DomainPeriod(2, "y");

            createDomainCmd.DomainContacts.Add(new DomainContact(adminContactId, "admin"));

            createDomainCmd.DomainContacts.Add(new DomainContact(tech1.Id, "tech"));
            createDomainCmd.DomainContacts.Add(new DomainContact(tech2.Id, "tech"));
            createDomainCmd.DomainContacts.Add(new DomainContact(tech3.Id, "tech"));

            createDomainCmd.Password = "c0mb0site";

            var response16 = service.Execute(createDomainCmd);

            PrintResponse(response16);

            /*
             15. Do a host:check for a host which the dot-ca domain name is registered
            */
            Console.WriteLine("TEST: 15");
            hostCheckCmd = new HostCheck("any." + registrarNumber + "b.ca");

            var response17 = service.Execute(hostCheckCmd);

            PrintResponse(response17);

            /*
             16. Create 2 subordinate hosts for the domain created in operation #14:
              - with format ns1.<domain> and ns2.<domain>
              - with IPv4 address information
            */
            Console.WriteLine("TEST: 16");
            var host1 = new Host("ns1." + step14domain);
            host1.Addresses.Add(new HostAddress("127.0.0.1", "v4"));
            var host2 = new Host("ns2." + step14domain);
            host2.Addresses.Add(new HostAddress("127.0.0.2", "v4"));

            var createHostCmd = new HostCreate(host1);

            var response18 = service.Execute(createHostCmd);

            PrintResponse(response18);

            createHostCmd = new HostCreate(host2);

            response18 = service.Execute(createHostCmd);

            PrintResponse(response18);

            /*
             17. Using the correct EPP call, get information on a host
            */
            Console.WriteLine("TEST: 17");
            var hostInfoCmd = new HostInfo(host1.HostName);

            var response19 = service.Execute(hostInfoCmd);

            PrintResponse(response19);

            /*18. Update the domain created in operation #14 such that the hosts created in operation #16 are delegated to the domain explicitly*/
            Console.WriteLine("TEST: 18");
            var domainUpdateCmd = new DomainUpdate(step14domain);

            //NOTE: Nameservers need different IP addresses
            domainUpdateCmd.ToAdd.NameServers = new List<string> { host1.HostName, host2.HostName };

            var response20 = service.Execute(domainUpdateCmd);

            PrintResponse(response20);

            //19. Update host ns1.<domain> created in operation #16 such that an IPv6 address is added
            Console.WriteLine("TEST: 19");
            var hostUpdateCmd = new HostUpdate(host1.HostName);

            var eppHostUpdateAddRemove = new EppHostUpdateAddRemove();

            eppHostUpdateAddRemove.Adresses = new List<HostAddress> { new HostAddress("1080:0:0:0:8:800:2004:17A", "v6") };

            hostUpdateCmd.ToAdd = eppHostUpdateAddRemove;

            var response21 = service.Execute(hostUpdateCmd);
            PrintResponse(response21);

            //20. Update host ns1.<domain> created in operation #16 such that an IPv4 address is removed
            Console.WriteLine("TEST: 20");
            hostUpdateCmd = new HostUpdate(host1.HostName);

            eppHostUpdateAddRemove = new EppHostUpdateAddRemove();

            eppHostUpdateAddRemove.Adresses = new List<HostAddress> { new HostAddress("127.0.0.1", "v4") };

            hostUpdateCmd.ToRemove = eppHostUpdateAddRemove;

            var response22 = service.Execute(hostUpdateCmd);
            PrintResponse(response22);

            //21. Update the status of ns1.<domain> such that it can no longer be updated
            Console.WriteLine("TEST: 21");
            hostUpdateCmd = new HostUpdate(host1.HostName);

            eppHostUpdateAddRemove = new EppHostUpdateAddRemove();

            eppHostUpdateAddRemove.Status = new List<Status> { new Status("", "clientUpdateProhibited") };

            hostUpdateCmd.ToAdd = eppHostUpdateAddRemove;

            response22 = service.Execute(hostUpdateCmd);
            PrintResponse(response22);

            //22. Using the correct EPP call, get information on a domain name without using WHOIS
            Console.WriteLine("TEST: 22");

            //const string domainStep10 = registrarNumber + "a.ca";

            var domainInfoCmd = new DomainInfo(domainStep10);
            var domainInfo = service.Execute(domainInfoCmd);

            PrintResponse(domainInfo);

            //23. Renew the domain created in operation #10 such that the domain’s total length of term becomes 3 years
            Console.WriteLine("TEST: 23");
            var renewCmd = new DomainRenew(domainStep10, domainInfo.Domain.ExDate, new DomainPeriod(2, "y"));

            var response23 = service.Execute(renewCmd);
            PrintResponse(response23);

            /*
             24. Do a Registrar transfer:
             - Domain name <registrar number>X2-1.ca, from your ‘e’ Registrar account 
             - Have the system auto-generate the contacts so that their information is identical to the contacts in the ‘e’ account
             */
            Console.WriteLine("TEST: 24");
            
            var transferCmd = new CiraDomainTransfer(registrarNumber + "X2-1.ca", null, null, new List<string>());
            //var transferCmd = new DomainTransfer("3406310x2-5.ca", null, null, new List<string>());

            transferCmd.Password = "cira3406310x2";
            var response24 = service.Execute(transferCmd);

            PrintResponse(response24);

            /*25. Do a Registrar transfer:
              - Domain name, <registrar number>X2-2.ca, from your ‘e’ Registrar account
              - Specify the same Registrant, administrative, and technical contacts used for the domain created in operation #14
             */
            Console.WriteLine("TEST: 25");
            
            //BUG: did not use all the technical contacts.

            transferCmd = new CiraDomainTransfer(registrarNumber + "X2-2.ca", association.Id, adminContactId, new List<string> { tech1.Id, tech2.Id, tech3.Id });
            //transferCmd = new DomainTransfer("3406310x2-10.ca", association.Id, adminContactId, new List<string> { tech1.Id, tech2.Id, tech3.Id });

            //Password is mandatory
            //TODO: find it in the control panel
            transferCmd.Password = "cira3406310x2";
            response24 = service.Execute(transferCmd);

            PrintResponse(response24);

            /*
             26. Do an update to the domain created in operation #14 to change the administrative contact to the pre-populated contact whose id is of format <registrar number>
             */
            Console.WriteLine("TEST: 26");
            domainUpdateCmd = new DomainUpdate(step14domain);

            //remove the previous admin
            domainUpdateCmd.ToRemove.DomainContacts.Add(new DomainContact(adminContactId, "admin"));

            domainUpdateCmd.ToAdd.DomainContacts.Add(new DomainContact(registrarNumber, "admin"));

            var response25 = service.Execute(domainUpdateCmd);

            PrintResponse(response25);

            /*27. Do an update to the status of the domain created in operation #14 such that it cannot be deleted*/
            Console.WriteLine("TEST: 27");
            domainUpdateCmd = new DomainUpdate(step14domain);

            domainUpdateCmd.ToAdd.Status.Add(new Status("", "clientDeleteProhibited"));

            var response26 = service.Execute(domainUpdateCmd);

            PrintResponse(response26);

            /*28. Do an update to the email address of the pre-populated contact whose id is of format <registrar number> to "accreditation@cira.ca" */
            Console.WriteLine("TEST: 28");
            //28.1 get the contact
            //var contactInfoCmd = new ContactInfo("rant" + registrarNumber);
            var contactInfoCmd = new ContactInfo(registrarNumber);

            contactInfoResponse = service.Execute(contactInfoCmd);

            PrintResponse(contactInfoResponse);

            if (contactInfoResponse.Contact != null)
            {

                //28.2 update the email address

                //ASSERT: contactInfoResponse.Contact != null
                contactUpdateCmd = new ContactUpdate(contactInfoResponse.Contact.Id);

                var contactchage = new ContactChange(contactInfoResponse.Contact);

                contactchage.Email = "accreditation@cira.ca";

                contactUpdateCmd.ContactChange = contactchage;

                //the extensions are compulsory
                contactUpdateCmd.Extensions.Add(new CiraContactUpdateExtension { CprCategory = contactInfoResponse.Contact.CprCategory });

                var response27 = service.Execute(contactUpdateCmd);

                PrintResponse(response27);


            }else
            {
                Console.WriteLine("Error: contact does not exist?");
            }


            /*
                 29. Do an update to the privacy status for Registrant contact created in operation #4 to now show full detail
            */
            Console.WriteLine("TEST: 29");
            contactUpdateCmd = new ContactUpdate("rant" + registrarNumber);

            //Invalid WHOIS display setting - valid values are PRIVATE or FULL
            contactUpdateCmd.Extensions.Add(new CiraContactUpdateExtension { WhoisDisplaySetting = "FULL" });

            var response28 = service.Execute(contactUpdateCmd);

            PrintResponse(response28);


            /*30. Delete the domain <registrar number>-10.ca*/
            Console.WriteLine("TEST: 30");
            //NOTE:check this domain status

            var deleteDomainCmd = new DomainDelete(registrarNumber + "-10.ca");
            //var deleteDomainCmd = new DomainDelete(registrarNumber + "-9.ca");

            var response29 = service.Execute(deleteDomainCmd);
            PrintResponse(response29);

            /*31. EPP <logout> command*/
            Console.WriteLine("TEST:31");
            var logOutCmd = new Logout();

            service.Execute(logOutCmd);

            /*32. Disconnect SSL connection*/
            Console.WriteLine("TEST: 32");
            service.Disconnect();

            /*33. SSL connection establishment*/
            Console.WriteLine("TEST: 33");
            service.Connect();

            /*34. EPP <login> command with your ‘e’ account*/
            Console.WriteLine("TEST: 34");
            logingCmd = new Login("username", "password");

            response = service.Execute(logingCmd);

            PrintResponse(response);

            /*35. Acknowledge all poll messages*/
            Console.WriteLine("TEST: 35");
            var thereAreMessages = true;

            while (thereAreMessages)
            {
                //request
                var poll = new Poll { Type = PollType.Request };

                var pollResponse = (PollResponse)service.Execute(poll);

                PrintResponse(pollResponse);

                if (!String.IsNullOrEmpty(pollResponse.Id))
                {
                    //acknowledge
                    poll = new Poll { Type = PollType.Acknowledge, MessageId = pollResponse.Id };

                    pollResponse = (PollResponse)service.Execute(poll);

                    PrintResponse(pollResponse);
                }

                Console.WriteLine("Messages left in the queue:" + pollResponse.Count);

                thereAreMessages = pollResponse.Count != 0;
            }

            /*36. EPP <logout> command*/
            Console.WriteLine("TEST: 36");
            logOutCmd = new Logout();

            service.Execute(logOutCmd);


        }

        private static byte[] ToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        private static void PrintResponse(EppResponse response)
        {
            if (response == null)
            {
                Console.WriteLine("Response was null");
                return;
            }

            if (response.Code != null) { Console.WriteLine("Code:" + response.Code); }
            if (response.Message != null) { Console.WriteLine("Message:" + response.Message); }
            if (response.ExtValue != null) { Console.WriteLine("ExtValue:" + response.ExtValue); }
            if (response.Reason != null) { Console.WriteLine("Code:" + response.Reason); }

            Console.WriteLine("** press <Enter> to continue **");
            Console.ReadLine();
        }


    }
}
