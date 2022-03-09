using Newtonsoft.Json;
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


            var sdafd = document.Elements("ApiError").Select(c=> c.Element("description").Value);

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
                                            .Where(n => (string)n.Attribute("root") == "1.10.2")
                                            .Select(n => n.Attribute("extension").Value)
                                             .FirstOrDefault()




                                ,
                                InsuranceLastUpdateDT =

                                                        StrtoDt(
                                                          patie.Elements(ns + "id")
                                                         .Where(n => (string)n.Attribute("root") == "1.30.1")
                                                         .Select(n => n.Attribute("extension").Value)
                                                         .FirstOrDefault() )


                                                    

                               ,
                                InsuranceExpirationDT =

                                                        StrtoDt(
                                                          patie.Elements(ns + "id")
                                                         .Where(n => (string)n.Attribute("root") == "1.30.2")
                                                         .Select(n => n.Attribute("extension").Value)
                                                         .FirstOrDefault() )

                                                        


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


                                                          StrtoDt(
                                                         patie.Elements(ns + "patient")
                                                              .Elements(ns + "birthTime")
                                                              .Select(n => n.Attribute("value").Value)
                                                              .FirstOrDefault() )

                                                         

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

                                            StrtoDt(  doc.Elements(ns + "time")
                                                .Select(n => n.Attribute("value").Value)
                                                .FirstOrDefault().ToString() )


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
                                                  StrtoDtime(
                                                          vi.Elements(ns + "encompassingEncounter")
                                                               .Elements(ns + "effectiveTime")
                                                               .Elements(ns + "low")
                                                               .Select(n => n.Attribute("value").Value)
                                                               .FirstOrDefault())
                                                          



                            ,
                          
                                TimeHigh =           StrtoDtime(
                                                          vi.Elements( "encompassingEncounter")
                                                               .Elements(ns + "effectiveTime")
                                                               .Elements(ns + "high")
                                                               .Select(n => n.Attribute("value").Value )
                                                               .FirstOrDefault())
                                                          
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

                                            
                                           ,time_low =  StrtoDt(
                                                             Pr.Elements(ns + "effectiveTime")
                                                             .Elements(ns + "low")
                                                           .Select(c => (string)c.Attribute("value"))?
                                                           .FirstOrDefault())



                                           ,
                                            time_high = StrtoDt(
                                                             Pr.Elements(ns + "effectiveTime")
                                                             .Elements(ns + "high")
                                                           .Select(c => (string)c.Attribute("value"))?
                                                           .FirstOrDefault())


                                                           
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
                                                                 .Where(n => (string)n.Attribute("root") == "1.1.3.3")
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
                                        ).FirstOrDefault();


            // var asafsad = 
            var barcode = document.Descendants(ns + "substanceAdministration")
                                    .Elements(ns + "entryRelationship")
                                    .Elements(ns + "act")
                                    .Elements(ns + "entryRelationship")
                                    .Elements(ns + "act")
                                    .Elements(ns + "id")
                                    .Where(n => (string)n.Attribute("root") == "1.8.5.1");
                                     








            //.Elements(ns + "id")
            //.Where(c => (string)c.Attribute("root") == "1.7.5.1");
            //.Select(c => c.Attribute("extension").Value).Where(c=>c.Attribute("id"));


            //.Where(n => n.Attribute("root").Value == "1.7.5.1")
            //.Select(c => c.Attribute("extension").Value)
            //.FirstOrDefault()
            //       );
            //.Where(c => (string)c.Element(ns + "id") == "1.7.5.1");

            //  .Elements(ns + "id").FirstOrDefault();
            // .Elements(ns + "id");
            // .Where(c => (string)c.Attribute("root") == "1.7.5.1")
            // .Select(c => (string)c.Attribute("extension")?).FirstOrDefault();    

            // ;

            // .Where(c => c.Attribute("root")?.Value == "1.7.5.1");



            var galenic = (from p in document.Descendants(ns + "substanceAdministration")
                          select new
                          {

                     lineid = p.Element(ns+"id")?.Attribute("extension")?.Value,


                     quanity = p.Elements(ns+ "entryRelationship").Elements(ns + "supply").Elements(ns + "quantity").FirstOrDefault().Attribute("value").Value,

                     pharmaceutical_desc = p.Elements(ns + "entryRelationship")
                                                 .Elements(ns + "act").Elements(ns + "id")
                                                 .Where(c=> c.Attribute("root").Value== "1.7.1.1")
                                                 .Select(c=> c.Attribute("extension").Value).FirstOrDefault(),

                   galenical_quantity= p.Elements(ns + "entryRelationship")
                                              .Elements(ns + "act").Elements(ns + "id")?
                                                 .Where(c => c.Attribute("root")?.Value == "1.7.1.15")
                                                 .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),



                    galenical_quantity_unit = p.Elements(ns + "entryRelationship")
                                                 .Elements(ns + "act").Elements(ns + "id")?
                                                 .Where(c => c.Attribute("root")?.Value == "1.7.1.16")
                                                 .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),
                   galenical_custom_desc = p.Elements(ns + "entryRelationship")
                                                 .Elements(ns + "act").Elements(ns + "id")?
                                                 .Where(c => c.Attribute("root")?.Value == "1.7.1.43")
                                                 .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),
                  active_substance = p.Elements(ns + "entryRelationship")
                                            .Elements(ns + "act").Elements(ns + "id")?
                                            .Where(c => c.Attribute("root")?.Value == "1.4.25")
                                            .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                  form_code = p.Elements(ns + "entryRelationship")
                                        .Elements(ns + "act").Elements(ns + "id")?
                                        .Where(c => c.Attribute("root")?.Value == "1.4.26")
                                        .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),
                  content = p.Elements(ns + "entryRelationship")
                                         .Elements(ns + "act").Elements(ns + "id")?
                                         .Where(c => c.Attribute("root")?.Value == "1.4.27")
                                         .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                    commercial_name = p.Elements(ns + "entryRelationship")
                            .Elements(ns + "act").Elements(ns + "id")?
                            .Where(c => c.Attribute("root")?.Value == "1.4.29")
                            .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                    dose_unit = p.Elements(ns + "entryRelationship")
                            .Elements(ns + "act").Elements(ns + "id")?
                            .Where(c => c.Attribute("root")?.Value == "1.4.29")
                            .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                      pharm_dose_unit = p.Elements(ns + "entryRelationship")
                            .Elements(ns + "act").Elements(ns + "id")?
                            .Where(c => c.Attribute("root")?.Value == "1.4.30")
                            .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     pharm_content = p.Elements(ns + "entryRelationship")
                            .Elements(ns + "act").Elements(ns + "id")?
                            .Where(c => c.Attribute("root")?.Value == "1.4.31")
                            .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                      package = p.Elements(ns + "entryRelationship")
                        .Elements(ns + "act").Elements(ns + "id")?
                        .Where(c => c.Attribute("root")?.Value == "1.4.32")
                        .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),


                    unit_price = p.Elements(ns + "entryRelationship")
                        .Elements(ns + "act").Elements(ns + "id")?
                        .Where(c => c.Attribute("root")?.Value == "1.4.33")
                        .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                      participation_percentage = p.Elements(ns + "entryRelationship")
                        .Elements(ns + "act").Elements(ns + "id")?
                        .Where(c => c.Attribute("root")?.Value == "1.4.18")
                        .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                       remaining_quantity = p.Elements(ns + "entryRelationship")
                        .Elements(ns + "act").Elements(ns + "id")?
                        .Where(c => c.Attribute("root")?.Value == "1.4.19")
                        .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                      participation_price = p.Elements(ns + "entryRelationship")
                        .Elements(ns + "act").Elements(ns + "id")?
                        .Where(c => c.Attribute("root")?.Value == "1.4.20")
                        .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     patient_difference = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.21")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     total_difference = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.21.1")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                    similar_list_id = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.22")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     replace_medicine = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.23")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                 prescription_illness_id = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.24")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     prescription_illness_desc = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.24.1")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     proposed_illness_perc = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.24.2")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                   genetic = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.4.15")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                       retail_price = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.8.5.1")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     reference_price = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.8.5.2")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     wholesale_price = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.8.5.3")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                   hospital = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.8.5.5")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                   prgn = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.9.6.1")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                    cluster_with_genetic = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.9.6.2")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     participation_discount = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.9.7.1")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                     pop_up = p.Elements(ns + "entryRelationship")
                     .Elements(ns + "act").Elements(ns + "id")?
                     .Where(c => c.Attribute("root")?.Value == "1.9.7.2")
                     .Select(c => c.Attribute("extension")?.Value).FirstOrDefault(),

                  sepecial_info = 
                                 new
                                {
                                  type="MEDI",
                                  
                                   similar_medicines=
                                   new
                                   {
                                       barcode = p.Elements(ns + "entryRelationship")
                                                   .Elements(ns + "act")
                                                   .Elements(ns + "entryRelationship")
                                                   .Elements(ns + "act")
                                                   .Where(n => (string)n.Attribute("classCode") == "ACT")
                                                 
                                                    .Where(c => c.Attribute("root")?.Value == "1.7.5.1")
                                                    .Select(c => c.Attribute("extension")?.Value).ToList(),
                                   }

                                 }
                                    

                                            
                          

                          }).ToList();




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

                     var medexcinfo = medexc.Elements(ns + "act")
                                            .Where(c => c.Elements(ns + "templateId").Select(c=> c.Attribute("root").Value).FirstOrDefault() == "2.16.840.1.113883.10.12.301")
                                            .Elements(ns+"id");
                     var medexecutions = medexc.Elements(ns + "act")
                                            .Where(c => c.Element(ns + "templateId") == null);



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


                     x.MedExecutionInfo = new MedExecutionInfo
                     {
                         genetic = medexcinfo.Where(c=> c.Attribute("root").Value== "1.4.15").Select(c=>c.Attribute("extension").Value).FirstOrDefault()

                        ,active_substance = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.25").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,form_code = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.26").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                         ,content = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.27").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,commercial_name = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.28").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,dose_unit = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.29").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,pharm_dose_unit = medexcinfo.Where(c=> c.Attribute("root").Value== "1.4.30").Select(c=>c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,pharm_content = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.31").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,package = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.32").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,unit_price = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.33").Select(c => c.Attribute("extension").Value).FirstOrDefault()


                        , participation_percentage = medexcinfo.Where(c=> c.Attribute("root").Value== "1.4.18").Select(c=>c.Attribute("extension").Value).FirstOrDefault()


                         ,remaining_quantity = medexcinfo.Where(c=> c.Attribute("root").Value== "1.4.19").Select(c=>c.Attribute("extension").Value).FirstOrDefault()


                         ,participation_price = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.20").Select(c => c.Attribute("extension").Value).FirstOrDefault()


                        , patient_difference = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.21").Select(c => c.Attribute("extension").Value).FirstOrDefault()


                         ,total_difference = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.21.1").Select(c => c.Attribute("extension").Value).FirstOrDefault()


                        , similar_list_id = medexcinfo.Where(c => c.Attribute("root").Value == "1.4.22").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                         ,retail_price = medexcinfo.Where(c => c.Attribute("root").Value == "1.8.5.1").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,reference_price = medexcinfo.Where(c => c.Attribute("root").Value == "1.8.5.2").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                         ,wholesale_price = medexcinfo.Where(c => c.Attribute("root").Value == "1.8.5.3").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,hospital = medexcinfo.Where(c => c.Attribute("root").Value == "1.8.5.5").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                         ,prgn = medexcinfo.Where(c => c.Attribute("root").Value == "1.9.6.1").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                         ,
                         cluster_with_genetic = medexcinfo.Where(c => c.Attribute("root").Value == "1.9.6.2").Select(c => c.Attribute("extension").Value).FirstOrDefault()

                     };




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

                     x.MedExecutions = medexecutions.Select(c =>

                         new MedExecutions
                         {
                             execution_number = c.Elements(ns + "id")
                                                  .Where(c => c.Attribute("root").Value == "2.10.8")
                                                  .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         ,  execution_price = c.Elements(ns + "id")
                                                  .Where(c => c.Attribute("root").Value == "2.10.9")
                                                  .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                         ,   retail_price = c.Elements(ns + "id")
                                                  .Where(c => c.Attribute("root").Value == "2.10.11")
                                                  .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                          , reference_price = c.Elements(ns + "id")
                                                  .Where(c => c.Attribute("root").Value == "2.10.10")
                                                  .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                         
                         ,  lot_numbers = c.Elements(ns + "id")
                                                  .Where(c => c.Attribute("root").Value == "2.10.12")
                                                  .Select(c => c.Attribute("extension").Value).FirstOrDefault()
                           
                           ,  insurance_difference = c.Elements(ns + "id")
                                                  .Where(c => c.Attribute("root").Value == "1.4.21.2")
                                                  .Select(c => c.Attribute("extension").Value).FirstOrDefault()

                          
                          
                          ,   execution_date = c.Elements(ns + "effectiveTime")
                                                  
                                                  .Select(c => c.Attribute("value").Value).FirstOrDefault()

                         }

                        ).ToList();









                 });


            var dia = document.Descendants(ns + "entry")
                                .FirstOrDefault()
                                .Elements(ns + "act")
                                .Elements(ns + "entryRelationship")
                                .Elements(ns + "observation")
                                .Elements(ns + "value")
                                .Where(c => c.Attribute("codeSystem") !=null  && c.Attribute("codeSystem").Value == "1.3.6.1.4.1.12559.11.10.1.3.1.44.2");

            var digagnosis = dia.Select(c =>
                                        new Diagnosis
                                        {
                                            Code = c.Attribute("code").Value
                                            ,
                                            Name = c.Attribute("displayName").Value
                                        }

                                        ).ToList();






            var duplicateexc = from med in medicine

                               from exc in med.MedExecutions

                                   //group exc by exc.execution_number  into g
                               select new
                               {
                                   number = exc.execution_number
                            ,
                                   date = exc.execution_date
                            ,
                                   medicins = new dispensed_medicines
                                   {
                                       barcode = med.Barcode
                               ,
                                       quanity = med.MedExecutions.Count(c => c.execution_date == exc.execution_date).ToString()
                               ,
                                       lots = new List<string> { exc.lot_numbers }

                               ,
                                       retail_price = exc.retail_price

                               ,
                                       refernce_price = exc.reference_price

                               ,
                                       participation_price = med.MedExecutionInfo.participation_price

                               ,
                                       insurance_difference = med.MedExecutionInfo.patient_difference
                                   }


                               };

            var executions = (from med in duplicateexc

                              group med by new { med.number, med.date }
                        into g
                              select new Executions
                              {
                                  date = g.Key.date
                                 ,
                                  number = g.Key.number

                                  ,
                                  medicines = g.Select(c => c.medicins)
                                                .GroupBy(x => x.barcode).
                                                 Select(x =>
                                                      new dispensed_medicines
                                                     {
                                                         barcode = x.FirstOrDefault().barcode
                                
                                                        , quanity = x.FirstOrDefault().quanity
                                
                                                        , lots = x.SelectMany(c=>c.lots).ToList()

                                
                                                       ,  retail_price = x.FirstOrDefault().retail_price

                                
                                                       ,  refernce_price = x.FirstOrDefault().refernce_price

                                
                                                        , participation_price = x.FirstOrDefault().participation_price

                               
                                                       ,  insurance_difference = x.FirstOrDefault().insurance_difference
                                                      })  .ToList()
                                                    }).OrderBy(c => c.number).ToList();
            //new dispensed_medicines
            //{
            //    barcode = c.medicins.barcode
            // , quanity = c.medicins.quanity
            // , lots = c.medicins.lots.ToList()
            // , retail_price = c.medicins.retail_price
            // , refernce_price = c.medicins.refernce_price
            // , participation_price = c.medicins.participation_price
            //   ,
            //    insurance_difference= c.medicins.insurance_difference
            //}









            var asdas = new ClinicalDocument
            {
                Patient = patient
                ,
                Doctor = doctor
                ,
                Appointment = appointment

                ,
                Diagnosis = digagnosis
                ,
                Medicines = medicine
                ,
                prescription= prescription
              ,
              Executions= executions

          };

            var a = Newtonsoft.Json.JsonConvert.SerializeObject(asdas);













            var b = 1;



        }

        private static string StrtoDt(string date="19010101")
        {
            if (date == null)
                return null;

            return DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture).ToString().Replace("T"," ");
        }

        private static string StrtoDtime(string date )
        {
            if (date == null)
                return null;

            return DateTime.ParseExact(date, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString().Replace("T", " ");
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
            public Prescription prescription { get; set; }
            public List<Executions> Executions { get; set; }
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

        public class Executions
        {
            public string number  { get; set; }

            public string date { get; set; }

            public List<dispensed_medicines> medicines { get; set; }

        }

        public class dispensed_medicines
        {
            public string barcode { get; set; }
            public string quanity { get; set; }
            public List<string> lots { get; set; }
            public string retail_price { get; set; }
            public string refernce_price { get; set; }

            public string participation_price { get; set; }

            public string insurance_difference { get; set; }

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
           
            [JsonIgnore]
            public List<MedExecutions> MedExecutions { get; set; }
           public MedExecutionInfo MedExecutionInfo { get; set; }
            public List<SimilarMedicine> SimilarMedicines { get; set; }
        }

        public class MedExecutionInfo
        {
            public string genetic { get; set; }
            public string active_substance { get; set; }
            public string form_code { get; set; }
            public string content { get; set; }
            public string commercial_name { get; set; }
            public string dose_unit { get; set; }
            public string pharm_dose_unit { get; set; }
            public string pharm_content { get; set; }
            public string package { get; set; }
            public string unit_price { get; set; }
            public string participation_percentage { get; set; }
            public string remaining_quantity { get; set; }
            public string participation_price { get; set; }
            
            public string patient_difference { get; set; }
            public string total_difference { get; set; }
            public string similar_list_id { get; set; }

            public string retail_price { get; set; }
            public string reference_price { get; set; } 
            public string wholesale_price { get; set; }
            public string hospital { get; set; }
            public string prgn { get; set; }
            public string cluster_with_genetic { get; set; }


        }
       
        public class MedExecutions
        {
            public string  execution_number { get; set; }
        
            public string execution_price { get; set; }
            
            public string retail_price { get; set; }
            public string reference_price { get; set; }
            public string lot_numbers { get; set; }

            public string insurance_difference { get; set; }
            public string execution_date { get; set; }

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
           
            public string InsuranceLastUpdateDT { get; set; }
            public string InsuranceExpirationDT { get; set; }

            public FullAddress fulladdress { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        
            public string Birthday { get; set; }
            public string LanguageCode { get; set; }
            public string gender { get; set; }

        }

        public class Doctor
        {

            public string RetrieveTime { get; set; }
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
            public string TimeLow { get; set; }
            public string TimeHigh { get; set; }
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
