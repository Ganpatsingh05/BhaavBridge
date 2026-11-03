# BhaavBridge — API Plan

This is the initial endpoint plan. Final request/response DTOs and exact status codes must match the implemented controllers.

## Conventions

- Base path: `/api`.
- JSON request and response bodies.
- Protect user-specific operations with authentication and role/ownership checks.
- Validate all input on the server.
- Do not expose another buyer's private offer data.
- Use DTOs rather than exposing EF Core entities directly.

## Crops and listings

| Method | Route | Intended access | Purpose |
|---|---|---|---|
| GET | /api/crops | Authenticated/public as decided | List active crops |
| GET | /api/listings | Buyer/Farmer/Admin | Browse published listings |
| GET | /api/listings/{id} | Authorized | Listing details |
| POST | /api/listings | Farmer | Create a harvest lot |
| PUT | /api/listings/{id} | Owning Farmer | Update own lot |
| DELETE | /api/listings/{id} | Owning Farmer/Admin | Remove or soft-close according to retention rules |

## Offers

| Method | Route | Intended access | Purpose |
|---|---|---|---|
| POST | /api/offers | Buyer | Submit an offer |
| GET | /api/offers/my | Buyer/Farmer | List the caller's relevant offers |
| GET | /api/listings/{id}/offers | Owning Farmer | View offers on own lot |
| POST | /api/offers/{id}/counter | Authorized offer participant | Submit counteroffer |
| POST | /api/offers/{id}/accept | Owning Farmer | Accept an eligible offer |
| POST | /api/offers/{id}/reject | Owning Farmer | Reject an eligible offer |

## Orders and market prices

| Method | Route | Intended access | Purpose |
|---|---|---|---|
| GET | /api/orders | Buyer/Farmer/Admin | List permitted orders |
| GET | /api/orders/{id} | Order participant/Admin | Order detail |
| PATCH | /api/orders/{id}/status | Authorized participant/Admin according to transition rules | Update status |
| GET | /api/market-prices | Authenticated users | Retrieve dated reference prices |
| POST | /api/market-prices | Administrator | Add reference price |
| PUT | /api/market-prices/{id} | Administrator | Correct/update a record with audit expectations |

## Validation and error cases to test

- Invalid or missing request fields.
- Negative price, quantity or expenses.
- Missing listing, offer or order.
- Offer against closed listing.
- Offer quantity beyond available lot quantity.
- Unauthorized role.
- Authenticated user attempting to access another user's records.
- Duplicate acceptance or duplicate order creation.
- Invalid order status transition.

## Postman collection

Create a collection named `BhaavBridge API` with folders for Authentication, Crops, Listings, Offers, Orders and Market Prices. Use environment variables for base URL and test tokens/cookies. Never commit real credentials or tokens.
