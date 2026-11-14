# BhaavBridge API Documentation

Base URL (local): `https://localhost:7189`

The API uses the application's ASP.NET Core Identity cookie authentication. These endpoints are same-origin application APIs; they are not configured for bearer/JWT authentication.

## Authentication for Postman

1. Start BhaavBridge and sign in through the web login page with the required role.
2. In browser developer tools, copy the value of the `.AspNetCore.Identity.Application` cookie.
3. Set the Postman environment variable `authCookie` to the complete cookie name/value pair, for example `.AspNetCore.Identity.Application=<local-cookie-value>`.
4. The collection sends it as a `Cookie` header.
5. Do not export or commit an environment containing a real cookie. Clear the variable when finished.

For `/api` paths, unauthenticated requests return HTTP 401 and authenticated users lacking the required role return HTTP 403. The authentication cookie is Secure and SameSite Lax.

## Response conventions

- JSON request and response bodies.
- HTTP 200 for successful reads/updates.
- HTTP 201 for successful creation.
- HTTP 400 for invalid request data.
- HTTP 401 for missing/expired authentication.
- HTTP 403 for insufficient role or ownership.
- HTTP 404 for missing or inaccessible records.
- HTTP 409 for duplicate or invalid state conflicts.
- Never expose EF entities directly from list/detail API endpoints.
- No hard-delete endpoint is exposed for harvest lots, offers, revisions or orders.

## Implemented endpoints

### Crops

| Method | Route | Access | Purpose |
|---|---|---|---|
| GET | `/api/crops` | Farmer, Buyer, Administrator | List active crops |
| POST | `/api/crops` | Administrator | Create crop; duplicate names return 409 |
| PUT | `/api/crops/{id}` | Administrator | Update crop name/category/active state |

### Produce lots

| Method | Route | Access | Purpose |
|---|---|---|---|
| GET | `/api/produce-lots` | Farmer, Buyer, Administrator | Farmer sees own lots; Buyer/Admin see Published lots |
| GET | `/api/produce-lots/{id}` | Farmer, Buyer, Administrator | Authorized lot detail; buyer sees Published only |
| POST | `/api/produce-lots` | Farmer | Create Draft lot |
| PUT | `/api/produce-lots/{id}` | Owning Farmer | Update own Draft lot |
| POST | `/api/produce-lots/{id}/publish` | Owning Farmer | Publish Draft lot |
| POST | `/api/produce-lots/{id}/close` | Owning Farmer | Close own lot |

Optional GET filters: `cropId`, `district`, `state`, `minPrice`, `maxPrice`, `minQuantity`, `maxQuantity`.

### Offers

| Method | Route | Access | Purpose |
|---|---|---|---|
| POST | `/api/offers` | Buyer | Submit offer for a Published lot and create revision 1 |
| GET | `/api/offers/mine` | Buyer | List own offers and revision counts |
| GET | `/api/offers/incoming` | Farmer | List offers for own lots with gross/cost/Net Daam values |
| POST | `/api/offers/{id}/counter` | Owning Farmer | Add counteroffer revision and update current terms |

The Razor workflow also supports farmer accept/reject, buyer accept/reject counteroffer, and transactional order creation through `OfferController`.

### Orders

| Method | Route | Access | Purpose |
|---|---|---|---|
| GET | `/api/orders` | Buyer, Farmer, Administrator | Caller-scoped orders; Admin sees all |
| GET | `/api/orders/{id}` | Order participant, Administrator | Authorized order details |

Order status transitions are currently available through the Razor `OrderController` workflow:
- Farmer: Placed → Confirmed → ReadyForPickup
- Buyer: ReadyForPickup → Completed
- Administrator: may cancel an order that is not Completed/Cancelled

### Market reference prices

| Method | Route | Access | Purpose |
|---|---|---|---|
| GET | `/api/market-reference-prices` | Farmer, Buyer, Administrator | List dated reference prices with source |

Optional filters: `cropId`, `district`, `state`.

Administrators create/edit market reference prices through the Razor `MarketReferencePriceController` pages.

## Postman collection

Import `docs/postman/BhaavBridge.postman_collection.json` and `docs/postman/BhaavBridge.local.postman_environment.json`.

Set `authCookie` after signing in with the role needed for each request. Use the variables `cropId`, `produceLotId`, `offerId`, and `orderId` for IDs returned by create requests.

The collection is a runnable request starter. Run the applicable requests with Farmer, Buyer and Administrator sessions, as indicated in each request description. Do not commit actual credentials or cookie values.
