# BhaavBridge — Proposed Database Design

This document is the initial logical design. Confirm it with the team before creating migrations. It is not a substitute for the final ER diagram.

## 1. Entity inventory

### ApplicationUser
Use ASP.NET Core Identity rather than creating a second password store. Identity maintains the authentication principal, password hash and account security fields.

### FarmerProfile
- FarmerProfileId (PK)
- UserId (unique FK to ApplicationUser)
- FullName / display name if not already held in Identity
- Phone
- Village
- District
- State
- Optional farm details

### BuyerProfile
- BuyerProfileId (PK)
- UserId (unique FK to ApplicationUser)
- BusinessName
- ContactPerson
- Phone
- Address
- District
- State

### Crop
- CropId (PK)
- Name
- Variety/category fields as agreed
- IsActive

### ProduceLot
- ProduceLotId (PK)
- FarmerProfileId (FK)
- CropId (FK)
- Variety
- QuantityQuintals (decimal, positive)
- QualityGrade
- HarvestDate
- Location fields
- AskingPricePerQuintal (decimal, non-negative)
- Description
- Status (Draft, Published, Closed)
- CreatedAt / UpdatedAt

### MarketReferencePrice
- MarketReferencePriceId (PK)
- CropId (FK)
- MarketName / location
- EffectiveDate
- MinimumPrice (nullable decimal)
- MaximumPrice (nullable decimal)
- ModalPrice (nullable decimal)
- SourceName
- SourceUrl (optional)
- RecordedAt

### Offer
- OfferId (PK)
- ProduceLotId (FK)
- BuyerProfileId (FK)
- CurrentPricePerQuintal (decimal, non-negative)
- QuantityQuintals (decimal, positive; must not exceed available lot quantity under the chosen business rule)
- BuyerBorneCosts (optional)
- FarmerBorneCosts (or structured cost data)
- Status (Pending, Countered, Accepted, Rejected, Withdrawn, Expired)
- CreatedAt / UpdatedAt

### OfferRevision
- OfferRevisionId (PK)
- OfferId (FK)
- RevisionNumber
- ProposedByUserId (FK to ApplicationUser)
- ProposedPricePerQuintal
- Agreed/assumed cost fields or structured snapshot
- Message (optional)
- CreatedAt

### Order
- OrderId (PK)
- OfferId (unique FK)
- ProduceLotId (FK)
- FarmerProfileId (FK)
- BuyerProfileId (FK)
- AgreedQuantityQuintals
- AgreedPricePerQuintal
- GrossValue
- FarmerBorneCostsSnapshot
- EstimatedNetReturnSnapshot
- Status (Created, Confirmed, ReadyForPickup, Completed, Cancelled; final transitions to be agreed)
- CreatedAt / UpdatedAt

## 2. Relationships

- ApplicationUser 1 — 0..1 FarmerProfile.
- ApplicationUser 1 — 0..1 BuyerProfile.
- FarmerProfile 1 — many ProduceLot.
- Crop 1 — many ProduceLot.
- Crop 1 — many MarketReferencePrice.
- ProduceLot 1 — many Offer.
- BuyerProfile 1 — many Offer.
- Offer 1 — many OfferRevision.
- Offer 1 — 0..1 Order.
- ProduceLot 1 — many Order history records if re-listed under an explicitly defined business process; otherwise enforce one successful order for a lot.

## 3. Integrity and business rules

1. Monetary fields use SQL Server decimal types, not float/double.
2. Quantity must be greater than zero.
3. Prices and expense amounts cannot be negative.
4. Foreign keys must prevent orphan records.
5. Only the owning farmer can manage a ProduceLot.
6. Only the submitting buyer can manage that buyer's Offer.
7. A closed lot cannot receive new offers.
8. Only one offer can be accepted for a lot.
9. Acceptance must be handled transactionally to prevent concurrent acceptance.
10. An accepted Offer can create at most one Order.
11. Preserve negotiation history in OfferRevision.
12. Store a snapshot of agreed price, quantity and cost assumptions in Order so later edits do not rewrite the transaction history.
13. Reference prices must retain effective date and source.
14. Use an explicit deletion/retention policy; do not cascade-delete financial/order history.

## 4. Net Daam calculation

- GrossValue = PricePerQuintal × QuantityQuintals.
- FarmerBorneCosts = transportation + loading/handling + other agreed farmer-borne expenses.
- EstimatedNetReturn = GrossValue − FarmerBorneCosts.
- EstimatedNetReturnPerQuintal = EstimatedNetReturn ÷ QuantityQuintals.

Use decimal arithmetic and validate positive quantity. The accepted order should preserve the values used at the time of acceptance.

## 5. EF Core implementation notes

- Use Fluent API for relationships, indexes, decimal precision and constraints.
- Consider a SQL Server filtered unique index on ProduceLotId for accepted offers, paired with a transaction and clear application-level error handling.
- Add a unique index on Order.OfferId.
- Use migrations for schema evolution.
- Seed only non-sensitive reference/demo records.
- Do not store authentication passwords in FarmerProfile or BuyerProfile.

## 6. Decisions to confirm before migration

- Whether a buyer can make multiple active offers on the same lot.
- Whether an offer may cover part of a lot or must equal the full lot quantity.
- Whether partial acceptance is permitted.
- Exact cost fields and whether costs are borne by farmer or buyer.
- Exact order status transitions.
- Whether a closed lot can be re-opened.
- Required profile fields and whether phone/email verification is in scope.

Update this document and the ER diagram after the team resolves these decisions.
