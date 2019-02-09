using EppLib.Entities;
using EppLib.Extensions.Fury;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EppLib.Tests
{
    [TestClass]
    public class FuryExtensionLocalTest
    {
        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryLoginCommand.xml")]
        public void FuryLogin()
        {
            string expected = File.ReadAllText("FuryLoginCommand.xml");

            var command = new Login("username", "password");
            command.Options = new Options { MLang = "en", MVersion = "1.0" };

            var services = new Services();

            services.ObjURIs.Add("urn:ietf:params:xml:ns:epp-1.0");
            services.ObjURIs.Add("urn:ietf:params:xml:ns:domain-1.0");
            services.ObjURIs.Add("urn:ietf:params:xml:ns:host-1.0");
            services.ObjURIs.Add("urn:ietf:params:xml:ns:contact-1.0");

            //Fury extension
            services.Extensions.Add("urn:ietf:params:xml:ns:fury-2.0");

            command.Services = services;

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }


        [TestMethod]
        [TestCategory("FuryExtension")]
        [DeploymentItem("TestData/FuryDomainCreateCommand.xml")]
        public void FuryCreateDomainWithPrivacy()
        {
            string expected = File.ReadAllText("FuryDomainCreateCommand.xml");

            var command = new DomainCreate("mydomain.ca", "jd1234");

            command.DomainContacts.Add(new DomainContact("sh8013", "admin"));
            command.DomainContacts.Add(new DomainContact("sh8013", "tech"));

            command.Extensions.Add(new FuryDomainCreateExtension(true));

            var xml = command.ToXml().InnerXml;

            Assert.AreEqual(expected, xml);
        }
    }
}
