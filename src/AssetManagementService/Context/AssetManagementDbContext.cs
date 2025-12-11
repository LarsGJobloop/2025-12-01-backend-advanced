using Contracts.AssetManagement;
using Microsoft.EntityFrameworkCore;

namespace AssetManagementService.Context;

public class AssetManagementDbContext : DbContext
{
  public DbSet<Asset> Assets { get; set; }
}
