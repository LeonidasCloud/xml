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
            string path = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, @"test", @"test.xml");

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
                                              .Where(n => (string)n.Attribute("use") == "MC")
                                             .Select(n => n.Attribute("value").Value) // get element's value
                                             .FirstOrDefault()?.Replace("tel:+","")

                                
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

                                ,Email = patie.Elements(ns + "telecom")
                                              .Where(n => (string)n.Attribute("use") == "HP")
                                             .Select(n => n.Attribute("value").Value) // get element's value
                                             .FirstOrDefault()?.Replace("mailto:", "")
                            }
                              )
                            .FirstOrDefault();



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
                            Id = doc.Elements(ns + "assignedAuthor")
                                    .Elements(ns + "id")
                                    .Where(n => (string)n.Attribute("root") == "1.18")
                                    .Select(n => n.Attribute("extension").Value)
                                    .FirstOrDefault()


                           ,
                            Amka = doc.Elements(ns + "assignedAuthor")
                                     .Elements(ns + "id")
                                     .Where(n => (string)n.Attribute("root") == "1.19")
                                     .Select(n => n.Attribute("extension").Value)
                                     .FirstOrDefault()


                           ,
                            Etaa = doc.Elements(ns + "assignedAuthor")
                                    .Elements(ns + "id")
                                    .Where(n => (string)n.Attribute("root") == "1.20")
                                    .Select(n => n.Attribute("extension").Value)
                                    .FirstOrDefault()

                            ,
                            Phone = doc.Elements(ns + "assignedAuthor")
                                        .Elements(ns + "telecom")
                                        .Where(n => (string)n.Attribute("use") == "MC")
                                        .Select(n => n.Attribute("value").Value)
                                        .FirstOrDefault()?.Replace("tel:+", "")



                           ,
                            Email = doc.Elements(ns + "assignedAuthor")
                                        .Elements(ns + "telecom")
                                        .Where(n => (string)n.Attribute("use") == "HP")
                                        .Select(n => n.Attribute("value").Value) // get element's value
                                        .FirstOrDefault()?.Replace("mailto:",",")
                           ,
                            fullname = doc.Elements(ns + "assignedAuthor")
                                                 .Elements(ns + "assignedPerson")
                                                .Elements(ns + "name")
                                                .Select(d => new Fullname
                                                {
                                                    firstname = d.Element(ns + "given").Value,
                                                    lastname = d.Element(ns + "family").Value,
                                                })
                                                  .FirstOrDefault()


                            ,
                            AuthorProffession = doc.Elements(ns + "assignedAuthor")
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

                            ,
                            UnitId = doc.Elements(ns + "assignedAuthor")
                                        .Elements(ns + "representedOrganization")
                                        .Elements(ns + "id")
                                          .Where(n => (string)n.Attribute("root") == "1.80.1")
                                                    .Select(n => n.Attribute("extension").Value)
                                                    .FirstOrDefault()

                          ,
                            UnitName = doc.Elements(ns + "assignedAuthor")
                                          .Elements(ns + "representedOrganization")
                                          .Select(d => d.Element(ns +"name").Value)
                                          .FirstOrDefault()
                                         





                        })
                         .FirstOrDefault();


                       
                        


            var appointment = document.Descendants(ns + "componentOf")
                       .Select(vi => new Appointment
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
                                                          vi.Elements( "encompassingEncounter")
                                                               .Elements(ns + "effectiveTime")
                                                               .Elements(ns + "high")
                                                               .Select(n => n.Attribute("value").Value )
                                                               ?.FirstOrDefault()?? "00010101120000"
                                                             , "yyyyMMddHHmmss", CultureInfo.InvariantCulture) 
                                                          
                       }
                       )
                       .FirstOrDefault();


            var prescription = document.Descendants(ns + "component")
                                        .Elements(ns + "structuredBody")
                                        .Elements(ns + "component")
                                        .Elements(ns + "section")
                                        .Elements(ns + "entry")
                                        .Elements(ns + "act")
                                        .Where(n => (string)n.Attribute("classCode") == "INFRM")
                                        .Select(vi => vi.Elements(ns + "statusCode")
                                                       .Select(c => (string)c.Attribute("code"))
                                                       .FirstOrDefault()
                                                       ).FirstOrDefault();
            //.Select(vi => new Prescription
            //{
            //    status=  vi.Elements("statusCode")
            //                .Elements("entryRelationship")
            //               .Select(c=> c.Element("code").Value)
            //               .FirstOrDefault()



            //});


            //var medicine = document.Descendants(ns + "component")
            //                        .Elements(ns + "structuredBody")
            //                        .Elements(ns + "component")
            //                        .Elements(ns + "section")
            //                        .Elements(ns + "text")
            //                         .Elements(ns + "list")
            //                         .Elements(ns + "item")
            //                        .Where(n => n.Attribute("ID").Value.StartsWith("med_barcode"))
            //                         .Select(d => d.Value).ToList();
            //  .Where(n => n.Attribute("ID").Value.StartsWith("med_barcode"))















            var a = 1;



        }


        public class Prescription
        {
            public string status { get; set; }
            public string time_low { get; set; }

            public string time_high { get; set; }

            public string prescription_id { get; set; }
            public string type { get; set; }

            public string duration_type { get; set; }

            public string series { get; set; }

            public string series_barcode { get; set; }

            public string series_date { get; set; }

            public string drug { get; set; }

            public string drugCategory { get; set; }

            public string desensitization_vaccine { get; set; }

            public string single_dose { get; set; }

            public string monthly { get; set; }

            public string special_antibiotic { get; set; }
            public string two_months { get; set; }

            public string no_paper { get; set; }

            public string portal_idika { get; set; }

            public string heparin { get; set; }

            public string drug_insert { get; set; }

            public string ekas { get; set; }

            public string chronic { get; set; }

            public string hospital_eopy { get; set; }

            public string ifet { get; set; }

            public string out_of_cost { get; set; }

            public string only_hospital { get; set; }

            public string commercial_name { get; set; }

            public string commercial_id { get; set; }


            public string commercial_noteshigh_cost { get; set; }

            public string vaccine { get; set; }

            public string law_816 { get; set; }

            public string pre_approval { get; set; }

            public string extra_cost { get; set; }
            public string eopy_ph_only { get; set; }
            public string has_extra_cost { get; set; }
            public string had_extra_cost { get; set; }

            public string insurance_group { get; set; }
            public string execution_no { get; set; }
            public string has_opinion { get; set; }

            public string opinion_date { get; set; }

            public string opinion_doc_amka { get; set; }

            public string opinion_doc_csp { get; set; }

            public string opinion_doc_name { get; set; }

           public string galenical { get; set; }

            public string galenical_description { get; set; }

            public string galenical_quantity { get; set; }

            public string galenical_unit { get; set; }

            public string prescription_notes { get; set; }

            public string diagnosis_notes { get; set; }





        }


        public class ClinicalDocument
        {
            public Patient Patient { get; set; }
            public Doctor Doctor { get; set; }
            public Appointment Appointment { get; set; }
            public List<Diagnosis> Diagnosis { get; set; }
            public List<Medicine> Medicines { get; set; }
        }

        public class Diagnosis
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }

        public class Medicine
        {
            public string Barcode { get; set; }
            public string Ingredients { get; set; }
            public string Quantity { get; set; }
            public string LineId { get; set; }
            public string Instructions { get; set; }
            public string FormName { get; set; }
            public string FormCode { get; set; }
            public string FormCapacity { get; set; }
            public string Name { get; set; }
            public string EofCode { get; set; }
            public MedExecutionInfo ExecutionInfo { get; set; }
            public List<SimilarMedicine> SimilarMedicines { get; set; }
        }

        public class MedExecutionInfo
        {
            public string ParticipationPerc { get; set; }
            public string RemainingQty { get; set; }
            public string ParticipationPrice { get; set; }
            public string PatienceDifference { get; set; }
            public string TotalDifference { get; set; }
            public string SimilarListId { get; set; }
            public string Genetic { get; set; }
            public string RetailPrice { get; set; }
            public string ReferencePrice { get; set; }
            public string WholesalePrice { get; set; }
            public string Hospital { get; set; }
            public string PrGn { get; set; }
            public string InCluster { get; set; }
        }

        public class SimilarMedicine
        {
            public string Barcode { get; set; }
            public string Name { get; set; }
            public string RetailPrice { get; set; }
            public string ReferencePrice { get; set; }
            public string Wholesaleprice { get; set; }
            public string Hospital { get; set; }
            public string PrGn { get; set; }
            public string InCluster { get; set; }
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


        public class Appointment
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
