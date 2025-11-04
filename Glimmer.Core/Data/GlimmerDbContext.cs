using Glimmer.Core.Models;
using MongoDB.Driver;

namespace Glimmer.Core.Data;

public partial class GlimmerDbContext
{
	private readonly IMongoDatabase _database;
	
	public GlimmerDbContext(IMongoDatabase database)
	{
		_database = database;
	}

    public IMongoCollection<Universe> Universes => _database.GetCollection<Universe>("universes");
    
}