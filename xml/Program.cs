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
            XNamespace epsos = "urn:epsos-org:ep:medication";
           

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
                                        .Select(Pr => new Prescription
                                        {
                                            status = Pr.Elements(ns + "statusCode")
                                                           .Select(c => ((string)c.Attribute("code")=="new"? "Συνταγογραφημένη":
                                                           ((string)c.Attribute("code") == "active")? "Μερικώς Εκτελεσμένη":
                                                           ((string)c.Attribute("code") == "completed") ? "Εκτελεσμένη" :
                                                           ((string)c.Attribute("code") == "cancelled") ? "Ακυρωμένη":null)
                                                           )
                                                           .FirstOrDefault()

                                            
                                           ,time_low = DateTime.ParseExact(
                                                             Pr.Elements(ns + "effectiveTime")
                                                             .Elements(ns + "low")
                                                           .Select(c => (string)c.Attribute("value"))?
                                                           .FirstOrDefault()??
                                                                 "00010101120000"
                                                             , "yyyyMMdd", CultureInfo.InvariantCulture)



                                           ,
                                            time_high = DateTime.ParseExact(
                                                             Pr.Elements(ns + "effectiveTime")
                                                             .Elements(ns + "high")
                                                           .Select(c => (string)c.Attribute("value"))?
                                                           .FirstOrDefault() ??
                                                                 "00010101"
                                                             , "yyyyMMdd", CultureInfo.InvariantCulture)


                                                           
                                            ,prescription_id = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.22")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                            
                                           , type = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.3")
                                                                .Select(c => ((string)c.Attribute("extension")=="1")? "Τυπική" :
                                                                             ((string)c.Attribute("extension") == "3")? "3μηνη":
                                                                             ((string)c.Attribute("extension") == "4")? "4μηνη":
                                                                             ((string)c.Attribute("extension") == "5")? "5μηνη": "6μηνη"
                                                                )
                                                                .FirstOrDefault()
                                           
                                           
                                           , duration_type = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.4")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                                                 
                                          
                                           , series = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.4.1")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                          
                                           
                                            ,series_barcode = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.4.2")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()


                                           , series_date = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.4.3")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                             
                                           , drug = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.11")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                                
                                           ,
                                            drugCategory = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.11.1")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()

                                           , desensitization_vaccine = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.8")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                             
                                            
                                            ,single_dose = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.4.11")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                               
                                            
                                           , monthly = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.4.9")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                             
                                           , two_months = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.4.10")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()

                                          ,
                                            no_paper = Pr.Elements(ns + "id")
                                                               .Where(n => (string)n.Attribute("root") == "1.5.10")
                                                               .Select(c => (string)c.Attribute("extension"))
                                                               .FirstOrDefault()




                                            ,
                                            portal_idika = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.40.1")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()

                                            ,
                                            heparin = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.30")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                            ,
                                            special_antibiotic = Pr.Elements(ns + "id")
                                                                .Where(n => (string)n.Attribute("root") == "1.1.13")
                                                                .Select(c => (string)c.Attribute("extension"))
                                                                .FirstOrDefault()
                                            ,
                                            ifet_insert = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.34")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                            
                                           , ekas = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.10.4")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                           , chronic = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.10.9")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                           
                                           
                                           , hospital_eopy = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.10")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                           
                                         ,   ifet = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.15")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                            
                                           , out_of_cost = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.17")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                           
                                           
                                          ,  commercial_name = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.3.1")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
   
                                            
                                           , commercial_id = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.3.2")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()


                                           
                                          
                                            ,commercial_notes = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.3.2")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                              
                                            
                                           , high_cost = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.7")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                           
                                          
                                            ,vaccine = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.24")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                                
                                           
                                           , law_816 = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.14")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()

                                           
                                           
                                          , pre_approval = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.16")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                    
                                            ,eopy_ph_only = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.9")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                           
                                            
                                            ,extra_cost = Pr.Elements(ns + "id")
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.22")
                                                                 .Select(c => (string)c.Attribute("extension"))
                                                                 .FirstOrDefault()
                                            
                                            ,has_extra_cost = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.22.2")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault() 
                                            
                                            ,had_extra_cost = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.22.1")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                        
                                           
                                           , execution_no = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.19")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()


                                           
                                           
                                           , has_opinion = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.23")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()
                                              
                                           
                                           , opinion_date = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.23.2")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                              
                                            
                                            ,opinion_doc_amka = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.23.1")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                                            
                                           
                                           , opinion_doc_csp = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.23.4")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()
                                            
                                            ,opinion_doc_name = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.1.23.5")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                            
                                            
                                            ,galenical = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.7.1")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                                 
                                            ,galenical_description = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.7.1.1")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                             
                                            
                                            ,galenical_quantity = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.7.1.15")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()

                                                
                                            ,
                                            galenical_unit = Pr.Elements(ns + "id")
                                                            .Where(n => (string)n.Attribute("root") == "1.7.1.16")
                                                            .Select(c => (string)c.Attribute("extension"))
                                                            .FirstOrDefault()
                                            //,insurance_group = Pr.Elements(ns + "id")
                                            //                .Where(n => (string)n.Attribute("root") == "1.1.22")
                                            //                .Select(c => (string)c.Attribute("extension"))
                                            //                .FirstOrDefault()


                                        }
                                        );



            var medicine = document.Descendants(ns + "component")
                                    .Elements(ns + "structuredBody")
                                    .Elements(ns + "component")
                                    .Elements(ns + "section")
                                    .Elements(ns + "text")
                                     .Elements(ns + "list")
                                     .Elements(ns + "item")
                                    .Where(n => n.Attribute("ID").Value.StartsWith("med_barcode"))
                                     .Select(d => new Medicine
                                     {
                                         Barcode = d.Value
                                        ,
                                         NameId = d.Attribute("ID").Value
                                     })
                                     .ToList();
            medicine.ForEach(

                 x =>
                 {
                     var doc = document.Descendants(ns + "substanceAdministration")
                                      .Where(z =>
                                            z.Elements(ns + "consumable")
                                             .Elements(ns + "manufacturedProduct")
                                             .Elements(ns + "manufacturedMaterial")
                                             .Elements(ns + "code")
                                             .Elements(ns + "originalText")
                                             .Elements(ns + "reference")
                                             .Attributes("value")
                                             .Any(f => f.Value.EndsWith(x.NameId))
                                                         );



                     var notes = document.Descendants(ns + "component")
                                    .Elements(ns + "structuredBody")
                                    .Elements(ns + "component")
                                    .Elements(ns + "section")
                                    .Elements(ns + "text")
                                     .Elements(ns + "list")
                                     .Elements(ns + "item")
                                    .Where(n => n.Attribute("ID").Value.EndsWith("notes_"+x.NameId.Last()));




                     var manufacturedMaterial = doc.Elements(ns + "consumable")
                                               .Elements(ns + "manufacturedProduct")
                                               .Elements(ns + "manufacturedMaterial");


                     var Similardocs = doc.Elements(ns + "entryRelationship")
                                       .Where(c => c.Attribute("typeCode").Value == "REFR"
                                                && c.Element(ns + "act").Attribute("classCode").Value == "ACT");


                     var medexc = doc.Elements(ns + "entryRelationship")
                                   .Where(c => c.Attribute("typeCode").Value == "SPRT");




                     var diagnosi = doc
                                .Elements(ns + "entryRelationship")
                                .Elements(ns + "act")
                                .Elements(ns + "entryRelationship")
                                .Elements(ns + "observation")
                                .Elements(ns + "value")
                                .Where(c => c.Attribute("codeSystem").Value == "1.3.6.1.4.1.12559.11.10.1.3.1.44.2");



                 x.Ingredients = manufacturedMaterial.Select(i => i.Elements(epsos + "ingredient")
                                                            .Elements(epsos + "ingredient")
                                                            .Select(n => n.Element(epsos + "name").Value).FirstOrDefault())
                                                             .FirstOrDefault();

                 x.notes = notes.Select(d => d.Value).FirstOrDefault();

                 x.LineId = doc.Select(c => c.Element(ns + "id").Attribute("extension").Value).FirstOrDefault();

                 x.quantity = doc.Elements(ns + "entryRelationship").Elements(ns + "supply").Select(z => z.Element(ns + "quantity").Attribute("value").Value).FirstOrDefault();

                 x.instructions =
                            new instructions
                            {

                                effectiveTime = new effectiveTime
                                {
                                    value = doc.Elements(ns + "effectiveTime").Elements(ns + "period").Select(c=> c.Attribute("value").Value).FirstOrDefault()
                                    ,unit= doc.Elements(ns + "effectiveTime").Elements(ns + "period").Select(c => c.Attribute("unit").Value).FirstOrDefault()

                                }
                     
                              ,doseQuantiy = new doseQuantiy
                               {
                                 high= new high
                                   {
                                       value= doc.Elements(ns + "doseQuantity").Elements(ns + "high").Select(c => c.Attribute("value").Value).FirstOrDefault()
                                       
                                     , unit= doc.Elements(ns + "doseQuantity").Elements(ns + "high").Select(c => c.Attribute("unit").Value).FirstOrDefault()
                                 }

                                 ,low = new low
                                 {
                                     value = doc.Elements(ns + "doseQuantity").Elements(ns + "low").Select(c => c.Attribute("value").Value).FirstOrDefault()

                                     
                                     ,unit = doc.Elements(ns + "doseQuantity").Elements(ns + "low").Select(c => c.Attribute("unit").Value).FirstOrDefault()
                                 }
                             }
                     
                             , period = new period
                               {
                                   high = new high
                                   {
                                       value = doc.Elements(ns + "rateQuantity").Elements(ns + "high").Select(c => c.Attribute("value").Value).FirstOrDefault()

                                     ,  unit = doc.Elements(ns + "rateQuantity").Elements(ns + "high").Select(c => c.Attribute("unit").Value).FirstOrDefault()
                                     
                                   }

                                 ,
                                   low = new low
                                   {
                                       value = doc.Elements(ns + "rateQuantity").Elements(ns + "low").Select(c => c.Attribute("value").Value).FirstOrDefault()

                                     ,
                                       unit = doc.Elements(ns + "rateQuantity").Elements(ns + "low").Select(c => c.Attribute("unit").Value).FirstOrDefault()
                                   }
                               }

                            };



                 x.FormName = manufacturedMaterial.Select(i => i.Elements(epsos + "asContent")
                                                          .Elements(epsos + "containerPackagedMedicine")
                                                          .Select(n => n.Element(epsos + "formCode").Attribute("displayName").Value).FirstOrDefault())
                                                             .FirstOrDefault();


                 x.FormCode = manufacturedMaterial.Select(i => i.Elements(epsos + "asContent")
                                                            .Elements(epsos + "containerPackagedMedicine")
                                                            .Select(n => n.Element(epsos + "formCode").Attribute("code").Value).FirstOrDefault())
                                                               .FirstOrDefault();

                 x.FormCapacity = manufacturedMaterial.Select(i => i.Elements(epsos + "asContent")
                                                                              .Elements(epsos + "containerPackagedMedicine")
                                                                              .Select(n => n.Element(epsos + "capacityQuantity").Attribute("value").Value).FirstOrDefault())
                                                                                 .FirstOrDefault();




                 x.Name = manufacturedMaterial.Select(i => i.Element(ns + "name").Value).FirstOrDefault();
                                                            
                   
                 x.EofCode = manufacturedMaterial.Select(i => i.Element(ns + "code").Attribute("code").Value).FirstOrDefault();


                 x.SimilarMedicines = Similardocs.Elements(ns + "act")
                                       .Select(c =>
                                        new SimilarMedicine
                                        {

                                            Barcode = c.Elements(ns + "id")
                                                       .Where(c => c.Attribute("root").Value == "1.7.5.1")
                                                       .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                                           ,
                                            Name = c.Elements(ns + "id")
                                                       .Where(c => c.Attribute("root").Value == "1.7.5.2")
                                                       .Select(c => c.Attribute("extension").Value).FirstOrDefault()



                                          ,
                                            RetailPrice = c.Elements(ns + "id")
                                                       .Where(c => c.Attribute("root").Value == "1.8.5.1")
                                                       .Select(c => c.Attribute("extension").Value).FirstOrDefault()


                                          ,
                                            ReferencePrice = c.Elements(ns + "id")
                                                       .Where(c => c.Attribute("root").Value == "1.8.5.2")
                                                       .Select(c => c.Attribute("extension").Value).FirstOrDefault()


                                           ,
                                            Wholesaleprice = c.Elements(ns + "id")
                                                               .Where(c => c.Attribute("root").Value == "1.8.5.3")
                                                               .Select(c => c.Attribute("extension").Value).FirstOrDefault()



                                            ,
                                            Hospital = c.Elements(ns + "id")
                                                               .Where(c => c.Attribute("root").Value == "1.8.5.5")
                                                               .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                                            ,
                                            PrGn = c.Elements(ns + "id")
                                                               .Where(c => c.Attribute("root").Value == "1.9.6.1")
                                                               .Select(c => c.Attribute("extension").Value).FirstOrDefault()



                                           ,
                                            InCluster = c.Elements(ns + "id")
                                                               .Where(c => c.Attribute("root").Value == "1.9.6.2")
                                                               .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                                        }
                                             ).ToList();

                     x.diagnosis = diagnosi.Select(c => c.Attribute("code").Value).ToList();

                   
                                        
                                        




                 });


            var dia = document.Descendants(ns + "entry")
                                .FirstOrDefault()
                                .Elements(ns +"act")
                                .Elements(ns + "entryRelationship")
                                .Elements(ns + "observation")
                                .Elements(ns + "value")
                                .Where(c => c.Attribute("codeSystem").Value == "1.3.6.1.4.1.12559.11.10.1.3.1.44.2");

            var digagnosis = dia.Select(c =>
                                        new Diagnosis
                                        {
                                            Code = c.Attribute("code").Value
                                            ,
                                            Name = c.Attribute("displayName").Value
                                        }

                                        );


            






















            var a = 1;



        }


        

        public class Prescription
        {
            public string status { get; set; }
            public DateTime time_low { get; set; }

            public DateTime time_high { get; set; }

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

            public string ifet_insert { get; set; }

            public string ekas { get; set; }

            public string chronic { get; set; }

            public string hospital_eopy { get; set; }

            public string ifet { get; set; }

            public string out_of_cost { get; set; }

            public string only_hospital { get; set; }

            public string commercial_name { get; set; }

            public string commercial_id { get; set; }


            public string commercial_notes { get; set; }
            public string high_cost { get; set; }
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

        public class instructions
        {
            public effectiveTime effectiveTime { get; set; }

            public doseQuantiy doseQuantiy { get; set; }

            public period period { get; set; }




        }
        public class doseQuantiy
        {
            public low low { get; set; }
            public high high { get; set; }


        }
        public abstract class baseInstructions
        {
            public string value { get; set; }
            public string unit { get; set; }
        }

        public class effectiveTime: baseInstructions { }
         
        
        public class low: baseInstructions { }
       public class high: baseInstructions { }

        public class period
        {
            public low low { get; set; }
            public high high { get; set; }
        }


        public class Medicine
        {

            public string NameId { get; set; }
            public string Barcode { get; set; }
            public string notes { get; set; }
            public string Ingredients { get; set; }
            public string quantity { get; set; }
            public instructions instructions { get; set; }
            public string LineId { get; set; }
            
            public string FormName { get; set; }
            public string FormCode { get; set; }
            public string FormCapacity { get; set; }
            public string Name { get; set; }
            public string EofCode { get; set; }
             public List<string> diagnosis { get; set; }
            public MedExecutionInfo ExecutionInfo { get; set; }
            public List<SimilarMedicine> SimilarMedicines { get; set; }
        }

        public class MedExecutionInfo
        {
            public string active_substance { get; set; }
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
