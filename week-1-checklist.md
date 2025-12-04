# Week 1 Checklist: Suggested Scenarios to Consider

Consider implementing the following scenarios using TDD (red → green → refactor) as you develop your services.

> [!NOTE]
> **Loosely Coupled Design**: The services are designed to be loosely coupled - they communicate via HTTP contracts but remain independent. This means:
> - ✅ You can progress with the current set (even a single CRUD service is fine)
> - ✅ Services can be developed and deployed independently
> - ✅ You don't need to implement everything to move forward
> 
> The scenarios below are **suggestions** to consider - pointers that might help your services provide value. We don't measure what the market values here (yet), so treat these as guidelines, not requirements.

## AssetManagementService

### Error Handling
- [ ] **Handle invalid asset ID format** - Requesting an asset with malformed ID (e.g., empty string, special characters) should return `400 Bad Request`
- [ ] **Validate required fields** - Registration requests missing required fields (e.g., empty name) should return `400 Bad Request`

### Business Logic
- [ ] **Decide: Can assets be deleted if they have reservations?** - This is fundamentally a **business decision**, not a technical one. The service-oriented architecture makes this decision visible and explicit.
  
  > [!NOTE]
  > **The Real Question**: Should an asset with active reservations be deletable?
  > 
  > **Why This Matters**: In a monolith, you might enforce this with a simple database constraint. In a service-oriented architecture, the architecture **reveals** that this business rule requires coordination between services. The technical "circular dependency" is just the architecture correctly exposing that this business rule spans service boundaries.
  > 
  > **Real-World Parallel**: Think of hotel or flight overbooking - when a hotel room or flight seat becomes unavailable (maintenance, cancellation by provider), the system must coordinate with existing reservations. Service-oriented architecture makes this coordination explicit rather than hiding it in database constraints.
  > 
  > **Your Options** (each reflects a different business decision):
  > - **Option A**: "No, assets with reservations cannot be deleted" → AssetManagementService must check ReservationService (bidirectional dependency)
  > - **Option B**: "Yes, but we keep historical data" → Soft delete (mark as deleted, keep for history)
  > - **Option C**: "Assets are never deleted, only deactivated" → No deletion endpoint needed
  > - **Option D**: "Yes, deletion is allowed even with reservations" → No coordination needed (but may violate business rules)
  > 
  > **The Point**: Service-oriented architecture doesn't hide this complexity - it makes it explicit. Choose the option that matches your business requirements, then implement it. The "technical problem" is just the architecture correctly showing you that this business rule requires service coordination.

## ReservationService

### Core Features
- [ ] **Query reservation by ID** - `GET /reservations/{id}` should return reservation details or `404 Not Found`
- [ ] **Cancel reservation** - `DELETE /reservations/{id}` should mark reservation as cancelled and return `200 OK`
- [ ] **List reservations** - `GET /reservations` should return all reservations (optionally filtered by asset ID)

### Error Handling
- [ ] **Reject invalid date ranges** - Start date after end date should return `400 Bad Request`
- [ ] **Reject past reservations** - Reservations with start date in the past should return `400 Bad Request`
- [ ] **Handle non-existent reservation** - Cancelling or querying non-existent reservation should return `404 Not Found`
- [ ] **Prevent cancelling already cancelled reservation** - Should return `409 Conflict`

### Business Logic
- [ ] **Edge case: Same start/end dates** - Reservations with identical start and end dates should be handled consistently (either allowed or rejected)
- [ ] **Edge case: Adjacent reservations** - Reservations ending exactly when another starts (no gap) should be handled consistently

## Integration Scenarios

### Service Communication
- [ ] **Handle AssetManagementService downtime** - When AssetManagementService is unavailable, ReservationService should return `503 Service Unavailable` (not crash)
- [ ] **Handle slow AssetManagementService responses** - Implement timeout handling for asset validation calls

## General Quality

### Validation
- [ ] **Input validation** - All endpoints should validate input data types and constraints
- [ ] **Consistent error responses** - Error responses should follow a consistent format (e.g., `{ "error": "message" }`)

### Testing
- [ ] **Edge case coverage** - Ensure tests cover boundary conditions (empty strings, null values, max dates, etc.)
- [ ] **Integration test coverage** - All service-to-service interactions should have integration tests

---

## Notes

- **Use TDD**: Write failing tests first (red), implement to make them pass (green), then refactor
- **Focus on value**: Prioritize scenarios that add real business value, not just technical completeness
- **Keep it simple**: Don't over-engineer. Simple, working solutions are better than complex ones
- **Document decisions**: If you choose not to implement something, document why in your code or tests

## When Are Your Services Ready?

Consider your services ready when:
- ✅ The scenarios you've chosen to implement are tested
- ✅ All tests pass consistently
- ✅ Services handle errors gracefully (no crashes on invalid input)
- ✅ Service-to-service communication is reliable (where applicable)

> [!TIP]
> **Remember**: The services are loosely coupled - you can progress with what you have! Even a single, well-tested CRUD service demonstrates solid service-oriented architecture principles. The checklist above are suggestions to consider, not requirements. The market would determine what's actually valuable, but we don't measure that here (yet).

Good luck! 🚀
