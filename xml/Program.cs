using System;
using System.Collections.Generic;
using System.Globalization;
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

            var patient = document.Descendants(ns + "patientRole")
                            .Select(patie => new Patient
                            {
                                Amka = patie.Elements(ns + "id")
                                            .Where(n => (string)n.Attribute("root") == "1.10.1")
                                            .Select(n => n.Attribute("extension").Value)
                                            .FirstOrDefault()


                                ,
                                InsuranceID = patie.Elements(ns + "id")
                                                        .Where(n => (string)n.Attribute("root") == "1.10.30.1")
                                                        .Select(n => n.Attribute("extension").Value)
                                                        .FirstOrDefault()


                                ,
                                InsuranceName = patie.Elements(ns + "id")
                                                       .Where(n => (string)n.Attribute("root") == "1.10.30.2")
                                                       .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()

                               

                               , InsuranceType= patie.Elements(ns + "id")
                                                       .Where(n => (string)n.Attribute("root") == "1.20.1")
                                                       .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()



                               , Ama = patie.Elements(ns + "id")
                                            .Where(n => (string)n.Attribute("root") == "1.10.30.2")
                                            .Select(n => n.Attribute("extension").Value)
                                             .FirstOrDefault()




                                ,
                                InsuranceLastUpdateDT = 

                                                        DateTime.ParseExact(
                                                          patie.Elements(ns + "id")
                                                         .Where(n => (string)n.Attribute("root") == "1.30.1")
                                                         .Select(n => n.Attribute("extension").Value)
                                                         ?.FirstOrDefault() ?? "00010101120000"
                                                         , "yyyyMMdd", CultureInfo.InvariantCulture)


                                                    

                               ,
                                InsuranceExpirationDT = 

                                                       DateTime.ParseExact(
                                                          patie.Elements(ns + "id")
                                                         .Where(n => (string)n.Attribute("root") == "1.30.2")
                                                         .Select(n => n.Attribute("extension").Value)
                                                         ?.FirstOrDefault() ?? "00010101120000"
                                                         , "yyyyMMdd", CultureInfo.InvariantCulture)

                                                        


                                ,
                                fulladdress = patie.Elements(ns + "addr")
                                                  .Select(d => new FullAddress
                                                  {
                                                      Address = d.Element(ns + "streetAddressLine").Value,
                                                      City = d.Element(ns + "city").Value,
                                                      PostalCode = d.Element(ns + "postalCode").Value,
                                                      State = d.Element(ns + "state").Value
                                                  })
                                                  .FirstOrDefault()


                                
                                ,fullname = patie.Elements(ns + "patient")
                                                .Elements(ns + "name")
                                                .Select(d => new Fullname
                                                {
                                                    firstname = d.Element(ns + "given").Value,
                                                    lastname = d.Element(ns + "family").Value,
                                                })
                                                  .FirstOrDefault()
                                ,Phone= patie.Elements(ns + "telecom")
                                             .Select(n => n.Attribute("value").Value) // get element's value
                                             .FirstOrDefault()

                                
                                ,Birthday =             


                                                         DateTime.ParseExact(
                                                         patie.Elements(ns + "patient")
                                                              .Elements(ns + "birthTime")
                                                              .Select(n => n.Attribute("value").Value)
                                                              ?.FirstOrDefault() ?? "00010101120000"
                                                            , "yyyyMMdd", CultureInfo.InvariantCulture)

                                                         

                                ,
                                LanguageCode =patie.Elements(ns + "patient")
                                                    .Elements(ns + "languageCommunication")
                                                    .Elements(ns + "languageCode")
                                                    .Select(n => n.Attribute("code").Value)
                                                    .FirstOrDefault()

                                
                                ,gender= patie.Elements(ns + "patient")
                                                    .Elements(ns + "administrativeGenderCode")
                                                    .Select(n => n.Attribute("displayName").Value)
                                                    .FirstOrDefault()

                            }
                              );



            var doctor = document.Descendants(ns + "author")
                        .Select(doc => new Doctor
                        {
                            RetrieveTime = 
                                            DateTime.ParseExact(
                                              doc.Elements(ns + "time")
                                                .Select(n => n.Attribute("value").Value)
                                                ?.FirstOrDefault() ?? "00010101120000"
                                                , "yyyyMMdd", CultureInfo.InvariantCulture)
                                           

                           ,
                            Id= doc.Elements(ns + "assignedAuthor")
                                    .Elements(ns + "id")
                                    .Where(n => (string)n.Attribute("root") == "1.18")
                                    .Select(n => n.Attribute("extension").Value) 
                                    .FirstOrDefault()


                           ,Amka= doc.Elements(ns + "assignedAuthor")
                                     .Elements(ns + "id")
                                     .Where(n => (string)n.Attribute("root") == "1.19")
                                     .Select(n => n.Attribute("extension").Value) 
                                     .FirstOrDefault()

                           
                           ,Etaa=doc.Elements(ns + "assignedAuthor")
                                    .Elements(ns + "id")
                                    .Where(n => (string)n.Attribute("root") == "1.20")
                                    .Select(n => n.Attribute("extension").Value)
                                    .FirstOrDefault()
                          
                            ,Phone = doc.Elements(ns + "assignedAuthor")
                                        .Elements(ns + "telecom")
                                        .Where(n => (string)n.Attribute("use") == "MC")
                                        .Select(n => n.Attribute("value").Value) 
                                        .FirstOrDefault()



                           ,Email = doc.Elements(ns + "assignedAuthor")
                                        .Elements(ns + "telecom")
                                        .Where(n => (string)n.Attribute("use") == "HP")
                                        .Select(n => n.Attribute("value").Value) // get element's value
                                        .FirstOrDefault()
                           ,
                            fullname =      doc.Elements(ns + "assignedAuthor")
                                                 .Elements(ns + "assignedPerson")
                                                .Elements(ns + "name")
                                                .Select(d => new Fullname
                                                {
                                                    firstname = d.Element(ns + "given").Value,
                                                    lastname = d.Element(ns + "family").Value,
                                                })
                                                  .FirstOrDefault()
                            
                        
                            ,AuthorProffession= doc.Elements(ns + "assignedAuthor")
                                                    .Elements(ns + "id")
                                                    .Where(n => (string)n.Attribute("root") == "1.19.2")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()

                            ,
                            fulladdress = doc.Elements(ns + "assignedAuthor")
                                                .Elements(ns + "representedOrganization")
                                                .Elements(ns + "addr")
                                                .Select(d => new FullAddress
                                                {
                                                    Address = d.Element(ns + "streetAddressLine").Value,
                                                    City = d.Element(ns + "city").Value,
                                                    PostalCode = d.Element(ns + "postalCode").Value,
                                                    State = d.Element(ns + "state").Value,
                                                    Country = d.Element(ns + "country").Value
                                                })
                                                  .FirstOrDefault()


                        }
                        );


            var visit = document.Descendants(ns + "componentOf")
                       .Select(vi => new Visit
                       {
                            Id= vi.Elements(ns + "encompassingEncounter")
                                 .Elements(ns + "id")
                                 .Where(n => (string)n.Attribute("root") == "1.80")
                                 .Select(n => n.Attribute("extension").Value)
                                 .FirstOrDefault()

                           , UnitId = vi.Elements(ns + "encompassingEncounter")
                                                    .Elements(ns + "id")
                                                    .Where(n => (string)n.Attribute("root") == "1.80.1")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()
                           ,InsuranceId= vi.Elements(ns + "encompassingEncounter")
                                                    .Elements(ns + "id")
                                                    .Where(n => (string)n.Attribute("root") == "1.80.2")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()
                           
                           ,Comments = vi.Elements(ns + "encompassingEncounter")
                                                    .Elements(ns + "id")
                                                    .Where(n => (string)n.Attribute("root") == "1.80.3")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()
                           
                           ,Reason= vi.Elements(ns + "encompassingEncounter")
                                                    .Elements(ns + "id")
                                                    .Where(n => (string)n.Attribute("root") == "1.80.3.1")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()

                          ,EncounterInLimit = vi.Elements(ns + "encompassingEncounter")
                                                    .Elements(ns + "id")
                                                    .Where(n => (string)n.Attribute("root") == "1.80.4")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()

                           ,
                           Status = vi.Elements(ns + "encompassingEncounter")
                                                    .Elements(ns + "code")
                                                    .Select(n => n.Attribute("code").Value)
                                                    .FirstOrDefault()


                           ,
                           
                           TimeLow = 
                                                 DateTime.ParseExact(
                                                          vi.Elements(ns + "encompassingEncounter")
                                                               .Elements(ns + "effectiveTime")
                                                               .Elements(ns + "low")
                                                               .Select(n => n.Attribute("value").Value)
                                                               ?.FirstOrDefault() ?? "00010101120000"
                                                             , "yyyyMMddHHmmss", CultureInfo.InvariantCulture)
                                                          



                            ,
                          
                                TimeHigh =                 DateTime.ParseExact(
                                                          vi.Elements(ns + "encompassingEncounter")
                                                               .Elements(ns + "effectiveTime")
                                                               .Elements(ns + "high")
                                                               .Select(n => n.Attribute("value").Value )
                                                               ?.FirstOrDefault()?? "00010101120000"
                                                             , "yyyyMMddHHmmss", CultureInfo.InvariantCulture) 
                                                          
                       }
                       );


            var a = 1;



        }

        public class Patient
        {

            public Fullname fullname { get; set; }
            public string Amka { get; set; }
            public string InsuranceID { get; set; }
            public string InsuranceName { get; set; }
            public string InsuranceType { get; set; }
            public string Ama { get; set; }
           
            public DateTime? InsuranceLastUpdateDT { get; set; }
            public DateTime? InsuranceExpirationDT { get; set; }

            public FullAddress fulladdress { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        
            public DateTime? Birthday { get; set; }
            public string LanguageCode { get; set; }
            public string gender { get; set; }

        }

        public class Doctor
        {

            public DateTime RetrieveTime { get; set; }
            public string Id { get; set; }
            public string Amka { get; set; }
            public string Etaa { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public Fullname fullname { get; set; }
            public string UnitId { get; set; }
            public string UnitName { get; set; }
            public FullAddress fulladdress { get; set; }
            public string AuthorProffession { get; set; }



        }


        public class Visit
        {
            public string Id { get; set; }
            public string UnitId { get; set; }
            public string InsuranceId { get; set; }
            public string Comments { get; set; }
            public string Reason { get; set; }
            public string EncounterInLimit { get; set; }
            public string Status { get; set; }
            public DateTime? TimeLow { get; set; }
            public DateTime? TimeHigh { get; set; }
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
            public string Country { get; set; }
        }

    }
}
