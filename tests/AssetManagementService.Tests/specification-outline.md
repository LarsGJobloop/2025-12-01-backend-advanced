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
