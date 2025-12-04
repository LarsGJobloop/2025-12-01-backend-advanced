# Capabilities Summary

> [!NOTE]
>
> This is suggestions for tests that you could write.

## Asset Management Service

### Feature: Asset Registration

**Scenario: Create asset with valid request**
- Given a valid asset registration request
- When I create an asset
- Then the asset is created with HTTP 201 Created status
- And the asset response contains a non-empty AssetId

**Scenario: Reject invalid registration request**
- Given an invalid asset registration request with empty name
- When I create an asset
- Then the request is rejected with HTTP 400 Bad Request

**Scenario: Retrieve asset details after registration**
- Given a successful asset registration
- When I get the asset details by AssetId
- Then the asset details are returned
- And the retrieved asset name matches the registration request name

**Scenario: Handle non-existing asset**
- Given a non-existing asset ID
- When I get the asset details
- Then the request is rejected with HTTP 404 Not Found

### Feature: Asset Listing

**Scenario: List assets when empty**
- Given an empty asset list
- When I list the assets
- Then the response is an empty list

**Scenario: List multiple registered assets**
- Given a set of assets have been registered
- When I list the assets
- Then the response contains all registered assets
- And the listed assets match the registered assets

### Feature: Asset Availability Management

**Scenario: Default status on registration**
- Given a valid asset registration request
- When I create an asset
- Then the asset is returned with Inactive status

**Scenario: Activate asset**
- Given an inactive asset
- When I call the make available endpoint
- Then the asset is made available
- And the asset status changes to Active

**Scenario: Deactivate asset**
- Given an active asset
- When I call the make unavailable endpoint
- Then the asset is made unavailable
- And the asset status changes to Inactive

## Booking Service

### Feature: Place Reservation

**Scenario: Create reservation with valid request**
- Given a valid asset that exists and is active
- And a valid reservation request
- When I place a reservation
- Then the reservation is created with HTTP 201 Created status
- And the reservation response contains a non-empty ReservationId

**Scenario: Reject invalid reservation request**
- Given an invalid reservation request with invalid asset ID
- When I place a reservation
- Then the reservation is not created with HTTP 400 Bad Request

**Scenario: Reject reservation for inactive asset**
- Given an inactive asset
- And a valid reservation request
- When I place a reservation
- Then the reservation is rejected with HTTP 409 Conflict

**Scenario: Prevent conflicting reservations**
- Given an existing reservation for an asset
- When I place a reservation for the same asset with overlapping dates
- Then the reservation is rejected with HTTP 409 Conflict

**Scenario: Allow non-conflicting reservations**
- Given an existing reservation for an asset
- When I place a reservation for the same asset with different (non-overlapping) dates
- Then the reservation is created successfully

### Feature: Cancel Reservation

**Scenario: Cancel existing reservation**
- Given a valid reservation
- When I cancel the reservation
- Then the reservation is cancelled
- And the reservation status changes to Cancelled

### Feature: Query Reservation

**Scenario: Retrieve reservation details**
- Given a valid reservation
- When I query the reservation by ReservationId
- Then the reservation is returned with HTTP 200 OK
- And the reservation contains complete details
- And the retrieved reservation matches the original request (AssetId, dates, status)
- And the reservation includes a CreatedAt timestamp
