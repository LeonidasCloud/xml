﻿using System;
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
            //string workingDirectory = Environment.CurrentDirectory;
            //string path = Path.Combine(Directory.GetParent(workingDirectory).Parent.FullName, @"test\", @"test.xml");

            XDocument document = XDocument.Load
                (@"C:\Users\bullet\source\repos\xml\xml\test\test.xml");
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


             




            var a = 1;



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