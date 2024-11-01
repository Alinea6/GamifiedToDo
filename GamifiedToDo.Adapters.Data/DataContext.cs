using Microsoft.EntityFrameworkCore;

namespace GamifiedToDo.Adapters.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}