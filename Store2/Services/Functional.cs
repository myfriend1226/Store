﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Store2.Data;
using Store2.Models;

namespace Store2.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper  _mapper;

        public Functional(UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           SignInManager<ApplicationUser> signInManager,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
           IHttpContextAccessor httpContextAccessor,
           IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

      

        public async Task InitAppData()
        {
            try
            {
                var usd = new Currency { CurrencyName = "USD", CurrencyCode = "$", Description = "US Dollar" };
                var rupiah = new Currency { CurrencyName = "IDR", CurrencyCode = "Rp", Description = "Indonesia Rupiah" };

                await _context.Currency.AddAsync(usd);
                await _context.Currency.AddAsync(rupiah);
                await _context.SaveChangesAsync();

                var northwind = new Branch {
                    BranchName = "Northwind Traders",
                    Description = "Microsoft fictional company",
                    CurrencyId = usd.CurrencyId,
                    Address = "One Microsoft Way",
                    City = "Redmond",
                    State = "WA",
                    ZipCode = "98052",
                    Phone = "555-555-555",
                    Email = "hello@northwind.com",
                    ContactPerson = "Steve Balmer"
                };

                await _context.Branch.AddAsync(northwind);
                await _context.SaveChangesAsync();

                await _context.BillType.AddAsync(new BillType { BillTypeName = "Prod", Description = "Production" });
                await _context.BillType.AddAsync(new BillType { BillTypeName = "Ops", Description = "Operation" });
                await _context.SaveChangesAsync();                

                await _context.Warehouse.AddAsync(new Warehouse { WarehouseName = "Northwind WH1" });
                await _context.Warehouse.AddAsync(new Warehouse { WarehouseName = "Northwind WH2" });
                await _context.SaveChangesAsync();

                await _context.CashBank.AddAsync(new CashBank { CashBankName = "Petty Cash" });
                await _context.CashBank.AddAsync(new CashBank { CashBankName = "JPMorgan Chase" });
                await _context.CashBank.AddAsync(new CashBank { CashBankName = "Wells Fargo" });
                await _context.CashBank.AddAsync(new CashBank { CashBankName = "Morgan Stanley" });
                await _context.CashBank.AddAsync(new CashBank { CashBankName = "Barclays" });
                await _context.SaveChangesAsync();

                

                await _context.InvoiceType.AddAsync(new InvoiceType { InvoiceTypeName = "Print" });
                await _context.InvoiceType.AddAsync(new InvoiceType { InvoiceTypeName = "Email" });
                await _context.SaveChangesAsync();

                await _context.PaymentType.AddAsync(new PaymentType { PaymentTypeName = "Credit Cards" });
                await _context.PaymentType.AddAsync(new PaymentType { PaymentTypeName = "Bank Tansfer" });
                await _context.PaymentType.AddAsync(new PaymentType { PaymentTypeName = "ACH" });
                await _context.PaymentType.AddAsync(new PaymentType { PaymentTypeName = "E-Wallet" });
                await _context.SaveChangesAsync();

                await _context.PurchaseType.AddAsync(new PurchaseType { PurchaseTypeName = "Tender" });
                await _context.PurchaseType.AddAsync(new PurchaseType { PurchaseTypeName = "Direct" });
                await _context.SaveChangesAsync();

                await _context.SalesType.AddAsync(new SalesType { SalesTypeName = "Marketplace" });
                await _context.SalesType.AddAsync(new SalesType { SalesTypeName = "POS" });
                await _context.SalesType.AddAsync(new SalesType { SalesTypeName = "E-Commerce" });
                await _context.SalesType.AddAsync(new SalesType { SalesTypeName = "Phone" });
                await _context.SaveChangesAsync();

                await _context.ShipmentType.AddAsync(new ShipmentType { ShipmentTypeName = "DHL" });
                await _context.ShipmentType.AddAsync(new ShipmentType { ShipmentTypeName = "FedEx" });
                await _context.ShipmentType.AddAsync(new ShipmentType { ShipmentTypeName = "UPS" });
                await _context.ShipmentType.AddAsync(new ShipmentType { ShipmentTypeName = "SAP" });
                await _context.SaveChangesAsync();

                var pcs = new UnitOfMeasure { UnitOfMeasureName = "PCS" };
                var ea = new UnitOfMeasure { UnitOfMeasureName = "EA" };
                await _context.UnitOfMeasure.AddAsync(pcs);
                await _context.UnitOfMeasure.AddAsync(ea);
                await _context.SaveChangesAsync();

                var notebook = new ProductType { ProductTypeName = "Notebook", Description = "Notebook" };
                var smartPhone = new ProductType { ProductTypeName = "Smart Phone", Description = "Smart Phone" };
                var software = new ProductType { ProductTypeName = "Software", Description = "Software" };
                var electronic = new ProductType { ProductTypeName = "Electronic", Description = "Electronic" };

                await _context.ProductType.AddAsync(notebook);
                await _context.ProductType.AddAsync(smartPhone);
                await _context.ProductType.AddAsync(software);
                await _context.ProductType.AddAsync(electronic);
                await _context.SaveChangesAsync();

                List<Product> products = new List<Product>() {
                    new Product{ProductName = "Lenovo X1 Yoga", Description="Lenovo X1 Yoga", Barcode = "123456789", ProductTypeId = notebook.ProductTypeId, DefaultBuyingPrice = 1500.0, DefaultSellingPrice = 1700.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Asus ZenBook", Description="Asus ZenBook", Barcode = "123456789", ProductTypeId = notebook.ProductTypeId, DefaultBuyingPrice = 1500.0, DefaultSellingPrice = 1700.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "HP Envy", Description="HP Envy", Barcode = "123456789", ProductTypeId = notebook.ProductTypeId, DefaultBuyingPrice = 1500.0, DefaultSellingPrice = 1700.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Dell XPS", Description="Dell XPS", Barcode = "123456789", ProductTypeId = notebook.ProductTypeId, DefaultBuyingPrice = 1500.0, DefaultSellingPrice = 1700.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Oppo F5", Description="Oppo F5", Barcode = "123456789", ProductTypeId = smartPhone.ProductTypeId, DefaultBuyingPrice = 400.0, DefaultSellingPrice = 500.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Asus Zenfone Max Pro", Description="Asus Zenfone Max Pro", Barcode = "123456789", ProductTypeId = smartPhone.ProductTypeId, DefaultBuyingPrice = 400.0, DefaultSellingPrice = 500.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Nokia 5", Description="Nokia 5", Barcode = "123456789", ProductTypeId = smartPhone.ProductTypeId, DefaultBuyingPrice = 400.0, DefaultSellingPrice = 500.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Samsung Galaxy A7", Description="Samsung Galaxy A7", Barcode = "123456789", ProductTypeId = smartPhone.ProductTypeId, DefaultBuyingPrice = 400.0, DefaultSellingPrice = 500.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Samsung 55 Inch Smart TV", Description="Samsung 55 Inch Smart TV", Barcode = "123456789", ProductTypeId = electronic.ProductTypeId, DefaultBuyingPrice = 1000.0, DefaultSellingPrice = 1200.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Toshiba 55 Inch Smart TV", Description="Toshiba 55 Inch Smart TV", Barcode = "123456789", ProductTypeId = electronic.ProductTypeId, DefaultBuyingPrice = 1000.0, DefaultSellingPrice = 1200.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "LG 55 Inch Smart TV UHD", Description="LG 55 Inch Smart TV UHD", Barcode = "123456789", ProductTypeId = electronic.ProductTypeId, DefaultBuyingPrice = 1000.0, DefaultSellingPrice = 1200.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Philips 55 Inch Smart TV", Description="Philips 55 Inch Smart TV", Barcode = "123456789", ProductTypeId = electronic.ProductTypeId, DefaultBuyingPrice = 1000.0, DefaultSellingPrice = 1200.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "VMWare Workstation 14 PRO", Description="VMWare Workstation 14 PRO", Barcode = "123456789", ProductTypeId = software.ProductTypeId, DefaultBuyingPrice = 300.0, DefaultSellingPrice = 400.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Microsoft Office Professional 2016", Description="Microsoft Office Professional 2016", Barcode = "123456789", ProductTypeId = software.ProductTypeId, DefaultBuyingPrice = 300.0, DefaultSellingPrice = 400.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Autodesk AutoCAD", Description="Autodesk AutoCAD", Barcode = "123456789", ProductTypeId = software.ProductTypeId, DefaultBuyingPrice = 300.0, DefaultSellingPrice = 400.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId },
                    new Product{ProductName = "Microsoft Windows 10 Professional", Description="Microsoft Windows 10 Professional", Barcode = "123456789", ProductTypeId = software.ProductTypeId, DefaultBuyingPrice = 300.0, DefaultSellingPrice = 400.0, BranchId = northwind.BranchId, UnitOfMeasureId = pcs.UnitOfMeasureId, CurrencyId = usd.CurrencyId }

                };
                await _context.Product.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                var smb = new CustomerType { CustomerTypeName = "SMB", Description = "Small Business" };
                var enterprise = new CustomerType { CustomerTypeName = "Enterprise", Description = "Enterprise" };
                var gov = new CustomerType { CustomerTypeName = "Government", Description = "Government" };
                var ngo = new CustomerType { CustomerTypeName = "NGO", Description = "Non-Government" };

                await _context.CustomerType.AddAsync(smb);
                await _context.CustomerType.AddAsync(enterprise);
                await _context.CustomerType.AddAsync(gov);
                await _context.CustomerType.AddAsync(ngo);
                await _context.SaveChangesAsync();

                List<Customer> customers = new List<Customer>() {
                    new Customer{CustomerName = "Hanari Carnes", Address = "Rua do Paço, 67", CustomerTypeId = smb.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "HILARION-Abastos", Address = "Carrera 22 con Ave. Carlos Soublette #8-35", CustomerTypeId = smb.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Hungry Coyote Import Store", Address = "City Center Plaza 516 Main St.", CustomerTypeId = smb.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Hungry Owl All-Night Grocers", Address = "8 Johnstown Road", CustomerTypeId = smb.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Island Trading", Address = "Garden House Crowther Way", CustomerTypeId = enterprise.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Königlich Essen", Address = "Maubelstr. 90", CustomerTypeId = enterprise.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "La corne d'abondance", Address = "67, avenue de l'Europe", CustomerTypeId = enterprise.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "La maison d'Asie", Address = "1 rue Alsace-Lorraine", CustomerTypeId = enterprise.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Laughing Bacchus Wine Cellars", Address = "1900 Oak St.", CustomerTypeId = gov.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Lazy K Kountry Store", Address = "12 Orchestra Terrace", CustomerTypeId = gov.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Lehmanns Marktstand", Address = "Magazinweg 7", CustomerTypeId = gov.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Let's Stop N Shop", Address = "87 Polk St. Suite 5", CustomerTypeId = gov.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "LILA-Supermercado", Address = "Carrera 52 con Ave. Bolívar #65-98 Llano Largo", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "LINO-Delicateses", Address = "Ave. 5 de Mayo Porlamar", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Lonesome Pine Restaurant", Address = "89 Chiaroscuro Rd.", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Magazzini Alimentari Riuniti", Address = "Via Ludovico il Moro 22", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Maison Dewey", Address = "Rue Joseph-Bens 532", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Mère Paillarde", Address = "43 rue St. Laurent", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Morgenstern Gesundkost", Address = "Heerstr. 22", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Customer{CustomerName = "Old World Delicatessen", Address = "2743 Bering St.", CustomerTypeId = ngo.CustomerTypeId, ContactPerson = "Jhon Smith", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"}
                };
                await _context.Customer.AddRangeAsync(customers);
                await _context.SaveChangesAsync();

                var store = new VendorType { VendorTypeName = "Store", Description = "Store" };
                var distributor = new VendorType { VendorTypeName = "Distributor", Description = "Distributor" };

                await _context.VendorType.AddAsync(store);
                await _context.VendorType.AddAsync(distributor);
                await _context.SaveChangesAsync();

                List<Vendor> vendors = new List<Vendor>() {
                    new Vendor{VendorName = "Exotic Liquids", Address = "49 Gilbert St.", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "New Orleans Cajun Delights", Address = "P.O. Box 78934", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Grandma Kelly's Homestead", Address = "707 Oxford Rd.", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Tokyo Traders", Address = "9-8 Sekimai Musashino-shi", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Cooperativa de Quesos 'Las Cabras'", Address = "Calle del Rosal 4", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Mayumi's", Address = "92 Setsuko Chuo-ku", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Pavlova, Ltd.", Address = "74 Rose St. Moonie Ponds", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Specialty Biscuits, Ltd.", Address = "29 King's Way", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "PB Knäckebröd AB", Address = "Kaloadagatan 13", VendorTypeId = distributor.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Refrescos Americanas LTDA", Address = "Av. das Americanas 12.890", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Heli Süßwaren GmbH & Co. KG", Address = "Tiergartenstraße 5", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Plutzer Lebensmittelgroßmärkte AG", Address = "Bogenallee 51", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Nord-Ost-Fisch Handelsgesellschaft mbH", Address = "Frahmredder 112a", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Formaggi Fortini s.r.l.", Address = "Viale Dante, 75", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Norske Meierier", Address = "Hatlevegen 5", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Bigfoot Breweries", Address = "3400 - 8th Avenue Suite 210", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Svensk Sjöföda AB", Address = "Brovallavägen 231", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "Aux joyeux ecclésiastiques", Address = "203, Rue des Francs-Bourgeois", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"},
                    new Vendor{VendorName = "New England Seafood Cannery", Address = "Order Processing Dept. 2100 Paul Revere Blvd.", VendorTypeId = store.VendorTypeId, ContactPerson = "Jason Bourne", City = "Redmond", State = "WA", Email = "jhon@smith.com", Phone = "555-5555"}
                };
                await _context.Vendor.AddRangeAsync(vendors);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendEmailBySendGridAsync(string apiKey, 
            string fromEmail, 
            string fromFullName, 
            string subject, 
            string message, 
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                await smtp.SendMailAsync(message);

            }

        }

        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPagesAsync();

                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                if (result.Succeeded)
                {
                    //add to user profile
                    UserProfile profile = new UserProfile();
                    profile.FirstName = "Super";
                    profile.LastName = "Admin";
                    profile.Email = superAdmin.Email;
                    profile.ApplicationUserId = superAdmin.Id;
                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    await _roles.AddToRoles(superAdmin.Id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> UploadFile(List<IFormFile> files, string uploadFolder)
        {
            var result = "";
            var root = Directory.GetCurrentDirectory();
            var uploads = Path.Combine(root, uploadFolder);
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = System.IO.Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = System.IO.Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }

        public string GetCurrentLoginUserId()
        {
            string result;
            try
            {
                result = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public void AddAuditInfo<T>(ref T entity)
        {
            try
            {
                entity.GetType().GetProperty("CreateAtUtc", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, DateTime.UtcNow);
                entity.GetType().GetProperty("CreateBy", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, GetCurrentLoginUserId());
                entity.GetType().GetProperty("UpdateAtUtc", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, DateTime.UtcNow);
                entity.GetType().GetProperty("UpdateBy", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, GetCurrentLoginUserId());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateAuditInfo<T>(ref T entity)
        {
            try
            {
                entity.GetType().GetProperty("UpdateAtUtc", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, DateTime.UtcNow);
                entity.GetType().GetProperty("UpdateBy", BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public).SetValue(entity, GetCurrentLoginUserId());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TEntity GetById<TEntity>(object id) where TEntity : class
        {
            try
            {
                var entity = _context.Set<TEntity>().Find(id);

                return entity;
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        public IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                return _context.Set<TEntity>().Where(predicate).AsEnumerable();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public IEnumerable<TEntity> GetList<TEntity>() where TEntity : class
        {
            try
            {
                return _context.Set<TEntity>()
                    .AsNoTracking()
                    .AsEnumerable();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public ResultModel Insert<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                AddAuditInfo<TEntity>(ref entity);

                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();

                return new ResultModel { Success = true, Message = "Insert success.", Data = entity };
            }
            catch (Exception ex)
            {

                return new ResultModel { Success = false, Message = ex.Message, Data = null };
            }
        }

        public ResultModel Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                UpdateAuditInfo<TEntity>(ref entity);

                _context.Set<TEntity>().Update(entity);
                _context.SaveChanges();

                return new ResultModel { Success = true, Message = "Update success.", Data = entity };
            }
            catch (Exception ex)
            {

                return new ResultModel { Success = false, Message = ex.Message };
            }
        }

        public ResultModel Update<TSource, TDestination>(TSource entity, object id) where TSource : class where TDestination : class
        {
            try
            {
                ResultModel result = new ResultModel();

                TDestination objDest = GetById<TDestination>(id);
                if (objDest != null)
                {
                    _mapper.Map<TSource, TDestination>(entity, objDest);
                    result = Update<TDestination>(objDest);
                }
                
                return result;
            }
            catch (Exception ex)
            {

                return new ResultModel { Success = false, Message = ex.Message };
            }
        }

        public ResultModel Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();

                return new ResultModel { Success = true, Message = "Delete success.", Data = entity };
            }
            catch (Exception ex)
            {

                return new ResultModel { Success = false, Message = ex.Message };
            }
        }

        public ResultModel Delete<TEntity>(object id) where TEntity : class
        {
            try
            {
                var typeInfo = typeof(TEntity).GetTypeInfo();
                var key = _context.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
                var property = typeInfo.GetProperty(key?.Name);
                if (property != null)
                {
                    var entity = Activator.CreateInstance<TEntity>();
                    property.SetValue(entity, id);
                    _context.Entry(entity).State = EntityState.Deleted;
                }
                else
                {
                    var entity = _context.Set<TEntity>().Find(id);
                    if (entity != null) _context.Remove(entity);
                }

                _context.SaveChanges();

                return new ResultModel { Success = true, Message = "Delete success." };
            }
            catch (Exception ex)
            {

                return new ResultModel { Success = false, Message = ex.Message };
            }
        }
        

    }
}
