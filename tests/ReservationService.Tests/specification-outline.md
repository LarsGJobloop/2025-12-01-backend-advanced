# Capabilities Summary

> [!NOTE]
>
> This is suggestions for tests that you could write.

## Reservation Service

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
