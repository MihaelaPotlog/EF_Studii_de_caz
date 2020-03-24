using System;
using System.Collections.Generic;
using System.Linq;
using EF_StudiiDeCaz.Caz2;
using EF_StudiiDeCaz.Caz3;
using EF_StudiiDeCaz.Caz4;
using EF_StudiiDeCaz.Caz5;

namespace EF_StudiiDeCaz
{
    class Program
    {
        static void Main(string[] args)
        {
            StudiuCaz1();
            // StudiuCaz2();
            // StudiuCaz3();
            // StudiuCaz4();
            // StudiuCaz5();
            Console.ReadKey();
        }
        static void StudiuCaz1()
        {
            using (var context = new ModelSelfReferences())
            {
                var parent1 = new SelfReference() { Name = "Ana" };
                var child1 = new SelfReference()
                {
                    Name = "Maria",
                    ParentSelfReference = parent1
                };
                context.SelfReferences.Add(parent1);
                context.SelfReferences.Add(child1);

                var child2 = new SelfReference() { Name = "Silviu" };
                var parent2 = new SelfReference() { Name = "Alex" };
                parent2.References.Add(child2);
                context.SelfReferences.AddRange(new List<SelfReference>() { child2, parent2 });

                context.SaveChanges();
                foreach (var selfReference in context.SelfReferences)
                {
                    Console.WriteLine("SelfReference {0}", selfReference.Name);
                    foreach (var childReference in selfReference.References)
                    {
                        Console.WriteLine("    Child SelfReference {0}", childReference.Name);
                    }
                }
            }
        }
        
        private static void StudiuCaz2()
        {
            using (var context = new ProductContext())
            {

                var product1 = new Product()
                {
                    Description = "asus laptop",
                    ImageURL = "/Desktop/laptop",
                    Price = 4000.0m,
                    SKU = 0

                };
                var product2 = new Product
                {
                    SKU = 202,
                    Description = "Small Deployment Back Pack",
                    Price = 29.97M,
                    ImageURL = "/pack202.jpg"
                };
                // context.Products.Add(product1);
                context.Products.Add(product2);
                context.SaveChanges();
                foreach (var product in context.Products)
                {
                    Console.WriteLine("{0} {1} {2} {3}",product.SKU, product.ImageURL, product.Price, product.Description);
                }
            }
        }

       

        public static void StudiuCaz3()
        {
            byte[] thumbBits = new byte[100];
            byte[] fullBits = new byte[2000];
            using (var context = new PhotographContext())
            {
                var photo = new Photograph
                {
                    Title = "Cats",
                    ThumbnailBits = thumbBits
                };
                var fullImage = new PhotographFullImage
                {
                    HighResolutionBits =
                        fullBits
                };
                photo.PhotographFullImage = fullImage;
                context.Photographs.Add(photo);
                context.SaveChanges();
            }
            using (var context = new PhotographContext())
            {
                foreach (var photo in context.Photographs)
                {
                    Console.WriteLine("Photo: {0}, ThumbnailSize {1} bytes",
                        photo.Title, photo.ThumbnailBits.Length);
                    // explicitly load the "expensive" entity,
                    context.Entry(photo)
                        .Reference(p => p.PhotographFullImage).Load();
                    Console.WriteLine("Full Image Size: {0} bytes",
                        photo.PhotographFullImage.HighResolutionBits.Length);
                }
            }
        }
        private static void StudiuCaz4()
        {
            using (var context = new BusinessContext())
            {
                var business = new Business
                {
                    Name = "Corner Dry Cleaning",
                    LicenseNumber = "100x1"
                };
                context.Businesses.Add(business);
                var retail = new Retail
                {
                    Name = "Shop and Save",
                    LicenseNumber =
                        "200C",
                    Address = "101 Main",
                    City = "Anytown",
                    State = "TX",
                    ZIPCode = "76106"
                };
                context.Businesses.Add(retail);
                var web = new eCommerce
                {
                    Name = "BuyNow.com",
                    LicenseNumber =
                        "300AB",
                    URL = "www.buynow.com"
                };
                context.Businesses.Add(web);
                context.SaveChanges();

                Console.WriteLine("\n--- All Businesses ---");
                foreach (var b in context.Businesses)
                {
                    Console.WriteLine("{0} (#{1})", b.Name, b.LicenseNumber);
                }
                Console.WriteLine("\n--- Retail Businesses ---");
                foreach (var r in context.Businesses.OfType<Retail>())
                {
                    Console.WriteLine("{0} (#{1})", r.Name, r.LicenseNumber);
                    Console.WriteLine("{0}", r.Address);
                    Console.WriteLine("{0}, {1} {2}", r.City, r.State, r.ZIPCode);
                }
                Console.WriteLine("\n--- eCommerce Businesses ---");
                foreach (var e in context.Businesses.OfType<eCommerce>())
                {
                    Console.WriteLine("{0} (#{1})", e.Name, e.LicenseNumber);
                    Console.WriteLine("Online address is: {0}", e.URL);
                }
            }
        }
        static void StudiuCaz5()
        {
            using (var context = new EmployeeContext())
            {
                Console.WriteLine("Firstname:");
                var firstName1 = Console.ReadLine();
                Console.WriteLine("Lastname:");
                var lastName1 = Console.ReadLine();
                Console.WriteLine("Salary:");
                decimal salary1 = Decimal.Parse(Console.ReadLine());

                context.Employees.Add(new  FullTimeEmployee()
                {
                    FirstName = firstName1,
                    LastName = lastName1,
                    Salary = salary1
                });

                Console.WriteLine("Firstname:");
                var firstName2 = Console.ReadLine();
                Console.WriteLine("Lastname:");
                var lastName2 = Console.ReadLine();
                Console.WriteLine("Salary:");
                decimal wage2 = Decimal.Parse(Console.ReadLine());

                context.Employees.Add(new HourlyEmployee()
                {
                    FirstName = firstName2,
                    LastName = lastName2,
                    Wage = wage2
                });
                context.SaveChanges();
           
                Console.WriteLine("--- All Employees ---");
                foreach (var emp in context.Employees)
                {
                    bool fullTime = emp is HourlyEmployee ? false : true;
                    Console.WriteLine("{0} {1} ({2})", emp.FirstName, emp.LastName, fullTime ? "Full Time" : "Hourly");
                }
                Console.WriteLine("--- Full Time ---");
                foreach (var fte in context.Employees.OfType<FullTimeEmployee>())
                {
                    Console.WriteLine("{0} {1}", fte.FirstName, fte.LastName);
                }
                Console.WriteLine("--- Hourly ---");
                foreach (var hourly in context.Employees.OfType<HourlyEmployee>())
                {
                    Console.WriteLine("{0} {1}", hourly.FirstName,
                        hourly.LastName);
                }
            }
        }
    }
}
