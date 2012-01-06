using System;
using System.Text;
using EasyEPP.Entities;

namespace Helper2
{
    class Tests
    {
        public void TestDomainInfoResponse()
        {
            var eppResponse =
                new DomainInfoResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
<response>
<result code=""1000"">
<msg>Command completed successfully</msg>
</result>
<resData>
<domain:infData xmlns:domain=""urn:ietf:params:xml:ns:domain-1.0"">
<domain:name>pc-case3.ca</domain:name>
<domain:roid>CIRA-lifecycle-00122</domain:roid>
<domain:status s=""serverUpdateProhibited"">change registrant</domain:status>
<domain:status s=""serverDeleteProhibited""/>
<domain:status s=""serverRenewProhibited""/>
<domain:status s=""serverTransferProhibited""/>
<domain:status s=""serverHold""/>
<domain:registrant>rant003</domain:registrant>
<domain:contact type=""admin"">admin003</domain:contact>
<domain:contact type=""tech"">tech003</domain:contact>
<domain:ns>
<domain:hostObj>ns1.example.ca</domain:hostObj>
<domain:hostObj>ns2.example.ca</domain:hostObj>
</domain:ns>
<domain:host>ns1.pc-case3.ca</domain:host>
<domain:host>ns2.pc-case3.ca</domain:host>
<domain:clID>automatedRARsprint3</domain:clID>
<domain:crID>automatedRARsprint3</domain:crID>
<domain:crDate>2009-12-08T16:25:01.0Z</domain:crDate>
<domain:exDate>2010-12-08T16:25:01.0Z</domain:exDate>
<domain:authInfo>
<domain:pw>password2</domain:pw>
</domain:authInfo>
</domain:infData>
</resData>
<extension>
<cira:ciraInfo xmlns:cira=""urn:ietf:params:xml:ns:cira-1.0"">
<cira:domainStageOfLife>pending delete</cira:domainStageOfLife>
<cira:domainStageOfLifeEnd>2009-12-16T16:29:05.0Z</cira:domainStageOfLifeEnd>
</cira:ciraInfo>
</extension>
<trID>
<clTRID>ABC-12347</clTRID>
<svTRID>cira-000002-0000000005</svTRID>
</trID>
</response>
</epp>
"));

            PrintResponse(eppResponse);
        }

        public void TestHostCheckResponse()
        {
            var eppResponse = new HostCheckResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
<response>
<result code=""1000"">
<msg>Command completed successfully</msg>
</result>
<resData>
<host:chkData xmlns:host=""urn:ietf:params:xml:ns:host-1.0"">
<host:cd>
<host:name avail=""true"">ns1.example.ca</host:name>
</host:cd>
<host:cd>
<host:name avail=""false"">ns2.example.ca</host:name>
<host:reason>Selected host name is not available</host:reason>
</host:cd>
</host:chkData>
</resData>
<trID>
<clTRID>ABC-12345</clTRID>
<svTRID>cira-000001-0000001</svTRID>
</trID>
</response>
</epp>
"));

            PrintResponse(eppResponse);
        }

        public void TestContactInfoResponse()
        {
            var eppResponse = new ContactInfoResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
<response>
<result code=""1000"">
<msg>Command completed successfully</msg>
</result>
<resData>
<contact:infData xmlns:contact=""urn:ietf:params:xml:ns:contact-1.0"">
<contact:id>11aabb</contact:id>
<contact:roid>roid1</contact:roid>
.CA Registry Guide for Registrars: Version 4.02
Contacts
Contact Check
<contact:status s=""ok""/>
<contact:postalInfo type=""loc"">
<contact:name>Contact Middle-name LastName</contact:name>
<contact:addr>
<contact:street>123 Everywhere Street</contact:street>
<contact:city>Ottawa</contact:city>
<contact:sp>ON</contact:sp>
<contact:pc>K1R7S8</contact:pc>
<contact:cc>CA</contact:cc>
</contact:addr>
</contact:postalInfo>
<contact:email>contact1@domain.ca</contact:email>
<contact:clID>123</contact:clID>
<contact:crID>456</contact:crID>
<contact:crDate>2010-01-07T18:18:53.0Z</contact:crDate>
</contact:infData>
</resData>
<extension>
<cira:ciraInfo xmlns:cira=""urn:ietf:params:xml:ns:cira-1.0"">
<cira:language>en</cira:language>
<cira:cprCategory>CCT</cira:cprCategory>
<cira:individual>Y</cira:individual>
<cira:ciraAgreementVersion>2.0</cira:ciraAgreementVersion>
<cira:agreementTimestamp>2010-01-26T18:18:53.0Z</cira:agreementTimestamp>
<cira:originatingIpAddress>192.168.45.59</cira:originatingIpAddress>
<cira:whoisDisplaySetting>PRIVATE</cira:whoisDisplaySetting>
</cira:ciraInfo>
</extension>
<trID>
<svTRID>CIRA-000108-0000000004</svTRID>
</trID>
</response>
</epp>
"));

            PrintResponse(eppResponse);
        }

        public void TestDomainCreate()
        {
            var domainCreate = new DomainCreate("fff.ca", "3333333") { Period = new DomainPeriod(2, "y") };

            domainCreate.DomainContacts.Add(new DomainContact("44444", "admin"));

            domainCreate.NameServers.Add("ns1.isqsolutions.com");
            domainCreate.NameServers.Add("ns2.isqsolutions.com");

            domainCreate.ToXml().Save(Console.OpenStandardOutput());
        }

        public void TestDomainCheckResponse()
        {

            var eppResponse = new DomainCheckResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
<response>
<result code=""1000"">
<msg>Command completed successfully</msg>
</result>
<resData>
<domain:chkData xmlns:domain=""urn:ietf:params:xml:ns:domain-1.0"">
<domain:cd>
<domain:name avail=""true"">abc123.ca</domain:name>
</domain:cd>
<domain:cd>
<domain:name avail=""true"">xyz987.ca</domain:name>
</domain:cd>
</domain:chkData>
</resData>
<trID>
<clTRID>ABC-12346</clTRID>
<svTRID>CIRA-000000000104-0000000002</svTRID>
</trID>
</response>
</epp>
"));
            PrintResponse(eppResponse);
        }

        private static byte[] ToBytes(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public void TestEppResponse()
        {

            var eppResponse = new EppResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
    <response>
        <result code=""2004"">
            <msg>Parameter value range error</msg>
            <extValue>
                <value>7037</value>
                <reason>Invalid telephone number format - see documentation</reason>
            </extValue>
        </result>
        <trID>
            <svTRID>CIRA-000015074475-0000000002</svTRID>
        </trID>
    </response>
</epp>"));

            PrintResponse(eppResponse);
        }

        private void GetAgreementTest()
        {
            var agreement = new GetAgreement();

            agreement.ToXml().Save(Console.OpenStandardOutput());

        }

        private void TestDomainInfo()
        {
            var domainInfo = new DomainInfo("example.ca") { Password = "pa33w0rd", Hosts = "all" };

            domainInfo.ToXml().Save(Console.OpenStandardOutput());

        }

        private void TestDeleteContact()
        {
            var contactInfo = new ContactDelete("123454");

            contactInfo.ToXml().Save(Console.OpenStandardOutput());

        }

        private void TestContactInfo()
        {
            var contactInfo = new ContactInfo("123454");

            contactInfo.ToXml().Save(Console.OpenStandardOutput());

        }

        private void TestCreateContact()
        {
            var createContact = ContactCreate.Make("12345", "Pepe Antonio", "Example Inc.", "Toronto", "301 La Fanfan Ave.", "ON", "444mmm", "CA", "6478913606", "333", "222222344444", "CCT", "ademar.gonzalez@gmail.com", "en", "Y", "127.0.0.1", "3406310");

            createContact.ToXml().Save(Console.OpenStandardOutput());

        }

        /*private void RunTest1()
       {
           var transport = new TcpTransport();

           var service = new Service(transport);

           service.Connect();
           service.Read(); //<-- gets the greeting
           service.Login("a3406310", "a3406310");
           service.GetAgreement();
           service.Logout();

       }*/

        /*private void RunTest()
        {
            var service = new Service();

            service.Connect();
            service.Read();//<-- gets the greeting

            service.Login("a3406310", "a3406310");

            //service.GetGreeting();
            //service.GetAgreement();

            Console.WriteLine("Create Contact");

            const string adminContactId = "111111";

            var response = service.CreateContact(adminContactId, "Pepe Antonio", "Example Inc.", "Toronto", "301 La Fanfan Ave.", "ON", "M5V 3T1", "CA", "+1.6478913606", "333", "+1.6478913607", "CCT", "ademar.gonzalez@gmail.com");

            PrintResponse(response);

            Console.WriteLine("Transfer Domain");

            response = service.TransferDomain("testdomain.ca",null, adminContactId, null);

            PrintResponse(response);

            Console.WriteLine("Check Domain");

            response = service.CheckDomain("testdomain.ca");
            
            Console.WriteLine("Create Domain");

            response = service.CreateDomain("testdomain.ca",adminContactId,adminContactId,2);

            PrintResponse(response);

            service.Disconnect();

        }*/

        /* public void TestGreeting(Service service)
       {
           service.Connect();
           service.GetGreeting();
       }*/

        /* private void RunTest2()
       {
           var transport = new TcpTransport();

           transport.Connect();

           var loging = new Login("", "", "");

           var response = service.Execute(loging);

           PrintResponse(response);

           var obj = new DomainCheck("test.ca");

           response = obj);

           PrintResponse(response);


       }*/

        private void TestPollResponse1()
        {
            var eppResponse =
                new PollResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
    <response>
        <result code=""1301"">
            <msg>Command completed successfully; ack to dequeue</msg>
        </result>
        <msgQ id=""2710"" count=""2"">
            <qDate>2011-04-27T17:43:02.0Z</qDate>
            <msg lang=""en"">Domain 3406310x2-2.ca has been transferred to another Registrar.</msg>
        </msgQ>
        <resData>
            <poll:extData xmlns:poll=""urn:ietf:params:xml:ns:poll-1.0"">
                <poll:msgID>3027</poll:msgID>
                <poll:domainName>3406310x2-2.ca</poll:domainName>
            </poll:extData>
        </resData>
        <trID>
            <clTRID>95e1fc7a-b5b8-488f-84cb-9e94d4102832</clTRID>
            <svTRID>CIRA-000003594172-0000000002</svTRID>
        </trID>
    </response>
</epp>
"));
            PrintResponse(eppResponse);
        }

        private void TestPollResponse2()
        {
            var eppResponse =
                new PollResponse(ToBytes(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?>
<epp xmlns=""urn:ietf:params:xml:ns:epp-1.0"">
    <response>
        <result code=""1000"">
            <msg>Command completed successfully</msg>
        </result>
        <msgQ id=""2710"" count=""1""/>
        <trID>
            <clTRID>98487317-76e2-4869-99d7-833bc71ad708</clTRID>
            <svTRID>CIRA-000003594172-0000000003</svTRID>
        </trID>
    </response>
</epp>
"));

            PrintResponse(eppResponse);
        }

        private static void PrintResponse(EppResponse response)
        {
            //ASSERT: response!=null

            if (response.Code != null) { Console.WriteLine("Code:" + response.Code); }
            if (response.Message != null) { Console.WriteLine("Message:" + response.Message); }
            if (response.ExtValue != null) { Console.WriteLine("ExtValue:" + response.ExtValue); }
            if (response.Reason != null) { Console.WriteLine("Code:" + response.Reason); }

            Console.WriteLine("** press <Enter> to continue **");
            Console.ReadLine();
        }
    }
}
