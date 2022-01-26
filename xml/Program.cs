using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace xml
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string path = Path.Combine(Directory.GetParent(workingDirectory).Parent.FullName, @"test\", @"test.xml");

            XDocument document = XDocument.Load
                (path);
            XNamespace ns = "urn:hl7-org:v3";

            var amka = document.Descendants(ns + "patientRole")
                                    .Elements(ns +"id")
                                    .Where(n => (string)n.Attribute("root") == "1.10.1")
                                    .Select(n => n.Attribute("extension").Value) // get element's value
                                      .FirstOrDefault();                    // select only first value, if any

            var InsuranceID = document.Descendants(ns + "patientRole")
                                   .Elements(ns + "id")
                                   .Where(n => (string)n.Attribute("root") == "1.10.30.1")
                                   .Select(n => n.Attribute("extension").Value) // get element's value
                                     .FirstOrDefault();

            var InsuranceName = document.Descendants(ns + "patientRole")
                                  .Elements(ns + "id")
                                  .Where(n => (string)n.Attribute("root") == "1.10.30.2")
                                  .Select(n => n.Attribute("extension").Value) // get element's value
                                    .FirstOrDefault();

            var ama = document.Descendants(ns + "patientRole")
                                  .Elements(ns + "id")
                                  .Where(n => (string)n.Attribute("root") == "1.10.2")
                                  .Select(n => n.Attribute("extension").Value) // get element's value
                                    .FirstOrDefault();

            var InsuranceTypeCode = document.Descendants(ns + "patientRole")
                                  .Elements(ns + "id")
                                  .Where(n => (string)n.Attribute("root") == "1.20.1")
                                  .Select(n => n.Attribute("extension").Value) // get element's value
                                    .FirstOrDefault();


            var InsuranceLastUpdateDT = document.Descendants(ns + "patientRole")
                                  .Elements(ns + "id")
                                  .Where(n => (string)n.Attribute("root") == "1.30.1")
                                  .Select(n => n.Attribute("extension").Value) // get element's value
                                    .FirstOrDefault();

            var InsuranceExpirationDT = document.Descendants(ns + "patientRole")
                                  .Elements(ns + "id")
                                  .Where(n => (string)n.Attribute("root") == "1.30.2")
                                  .Select(n => n.Attribute("extension").Value) // get element's value
                                    .FirstOrDefault();


            var Addr = document.Descendants(ns + "patientRole")
                        .Elements(ns + "addr")
                    .Select(d => new FullAddress
                    {
                        Address = (string)d.Element(ns + "streetAddressLine"),
                        City = (string)d.Element(ns + "city"),
                        PostalCode = (string)d.Element(ns + "postalCode"),
                        State = (string)d.Element(ns + "state")
                    });

            var Phone = document.Descendants(ns + "patientRole")
                                  .Elements(ns + "telecom")
                                  .Select(n => n.Attribute("value").Value) // get element's value
                                   .FirstOrDefault();
                                  

            var name = document.Descendants(ns + "patientRole")
                        .Elements(ns + "patient")
                        .Elements(ns + "name")
                    .Select(d => new Fullname
                    {
                        firstname = (string)d.Element(ns + "given"),
                        lastname = (string)d.Element(ns + "family"),
                    });


            var Birthday = document.Descendants(ns + "patientRole")
                        .Elements(ns + "patient")
                        .Elements(ns + "birthTime")
                          .Select(n => n.Attribute("value").Value);

            var LanguageCode= document.Descendants(ns + "patientRole")
                               .Elements(ns + "patient")
                               .Elements(ns + "languageCommunication")
                               .Elements(ns + "languageCode")
                               .Select(n => n.Attribute("code").Value);


            var gender = document.Descendants(ns + "patientRole")
                               .Elements(ns + "patient")
                               .Elements(ns + "administrativeGenderCode")
                               .Select(n => n.Attribute("displayName").Value);


            var a = 1;



        }
        public class Fullname
        {
            public string firstname { get; set; }
            public string lastname { get; set; }

        }
        public class FullAddress
        {
            public string Address { get; set; }
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string State { get; set; }
        }
    }
}
