# BhaavBridge — Project Specification

**Course:** INT402 – Modern Web Programming Tools and Techniques  
**Academic session:** 2026–27  
**Selected question:** Q1 – Fair Trade Agri-Portal: Empowering Farmers with Better Market Rates  
**Submission target:** 16 November 2026  
**Team:** Ganpat Singh, Rahul Bora, Gautam, Aakash Singh

## 1. Product identity

**BhaavBridge** is a Harvest Price Discovery and Buyer Negotiation Platform for Fairer Agricultural Trade.

**Tagline:** Know the offer. Compare the value. Make the right deal.

## 2. Abstract

BhaavBridge is a web-based agricultural marketplace designed to connect farmers directly with buyers and improve transparency in produce pricing. Farmers can publish available harvest lots with crop, quantity, quality, location and expected price information. Buyers can discover listings and submit purchase offers. Farmers can review competing offers, negotiate through counteroffers, and compare estimated net returns after farmer-borne expenses. The platform also displays dated market-reference prices maintained by an administrator and supports order creation and status tracking after an offer is accepted.

The application will be developed using C#, ASP.NET Core MVC, Razor Views, Entity Framework Core and Microsoft SQL Server. ASP.NET Core Web APIs will expose selected CRUD and workflow operations and will be validated using Postman.

## 3. Problem statement

Farmers may have limited visibility into buyer options and may find it difficult to compare offers received through different channels. A higher quoted price does not always produce a higher return when transportation, loading or other farmer-borne expenses differ. BhaavBridge provides a central place to publish produce, receive and negotiate buyer offers, compare estimated net realization and manage accepted transactions.

## 4. Objectives

1. Connect farmers and buyers through a centralized marketplace.
2. Allow farmers to publish and manage harvest lots.
3. Allow buyers to browse, search and filter available produce.
4. Support offers, counteroffers, acceptance and rejection.
5. Compare offers using estimated farmer-borne costs.
6. Display dated market-reference prices for context.
7. Create and track orders from accepted offers.
8. Demonstrate the INT402 ASP.NET Core, MVC, SQL Server, EF Core, security and Web API concepts.

## 5. User roles

### Farmer
- Manage farmer profile.
- Create, edit, publish, close and view own harvest lots.
- View offers received on own lots.
- Accept, reject or counter an offer.
- Compare estimated net returns.
- View orders for own accepted deals.

### Buyer
- Manage buyer profile.
- Browse and search published harvest lots.
- Submit offers and counteroffer responses.
- View own offer history and order status.
- Cannot view another buyer's private offers or manage another user's listings.

### Administrator
- Manage user and crop records.
- Manage dated reference market-price records.
- Monitor platform listings and orders.
- Access administrative functions only.

## 6. Functional requirements

| ID | Requirement |
|---|---|
| FR-01 | Farmer and buyer registration |
| FR-02 | Secure login and logout |
| FR-03 | Role-based access control |
| FR-04 | Farmer and buyer profile management |
| FR-05 | Harvest-lot CRUD and publishing/closure |
| FR-06 | Marketplace browsing |
| FR-07 | Search and filtering by crop, location, quantity and price |
| FR-08 | Buyer offer submission |
| FR-09 | Counteroffer and offer revision history |
| FR-10 | Farmer offer comparison |
| FR-11 | Net Daam calculation |
| FR-12 | Dated reference market-price display |
| FR-13 | Order creation from accepted offers |
| FR-14 | Order viewing and status tracking |
| FR-15 | Administrator management |
| FR-16 | Authenticated REST API endpoints |

## 7. Signature feature — Net Daam Calculator

- Gross offer value = offered price per quintal × quantity.
- Farmer-borne expenses = transportation + loading/handling + other applicable farmer costs.
- Estimated net return = gross offer value − farmer-borne expenses.
- Estimated net return per quintal = estimated net return ÷ quantity.

Example: 50 quintals × ₹2,550 = ₹1,27,500 gross. If farmer-borne costs total ₹5,000, estimated net return is ₹1,22,500.

The calculator is an estimate based on entered values and offer terms. It must not describe estimates as guaranteed proceeds or live logistics prices. Validate that quantity is positive and costs/prices are non-negative.

## 8. Reference price information

Reference prices are administrator-maintained, dated records. A record should identify the crop, market/location, effective date, source and available price fields (minimum, maximum and modal price where supplied). Display the effective date and source. Do not label manually maintained values as automatically updated live prices.

## 9. Non-functional requirements

- Security: authentication, authorization and ownership checks.
- Data integrity: relational constraints and validated references.
- Reliability: exception handling and meaningful error responses.
- Usability: consistent navigation and understandable role-specific dashboards.
- Responsiveness: usable desktop and mobile layouts.
- Maintainability: separation of controllers, services, models, data and views.
- Performance: efficient queries and pagination/filtering where needed.
- Validation: server-side validation; client-side validation as a usability aid.

## 10. Technology stack

- C# and ASP.NET Core
- MVC and Razor Views
- HTML, CSS and JavaScript
- Microsoft SQL Server
- Entity Framework Core
- ASP.NET Core Identity and authorization
- ASP.NET Core Web API
- Built-in dependency injection and middleware
- Postman

Record exact SDK/package/database versions after environment setup.

## 11. Proposed main entities

ApplicationUser, FarmerProfile, BuyerProfile, Crop, ProduceLot, MarketReferencePrice, Offer, OfferRevision and Order.

See [Database Design](DATABASE_DESIGN.md) for proposed relationships and integrity rules.

## 12. Architecture

A single ASP.NET Core solution will host MVC controllers/views and API controllers. Controllers handle request orchestration and model binding. Reusable business rules belong in services registered through dependency injection. Entity Framework Core accesses SQL Server through ApplicationDbContext. Authentication, authorization and middleware protect application and API routes.

## 13. Scope exclusions

The first submission excludes online payment processing, AI/ML crop prediction, IoT sensor integration, GPS live tracking, real-time chat, logistics booking and automatic live mandi-price integration.

## 14. Academic stack confirmation

The INT402 syllabus identifies C# and ASP.NET Core, while the separate project question sheet includes Django/Python-specific implementation language. The team is following the student's instruction to use the syllabus stack. Instructor confirmation that ASP.NET Core is accepted for Q1 remains an academic dependency and should be recorded before substantial implementation.

## 15. Definition of done

A feature is done only when implementation is complete, validation and authorization are tested, relevant tests pass, peer review is complete, defects are resolved, and documentation is updated. A phase additionally requires its exit criteria and formal team approval.
