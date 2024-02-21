using AspireDBTestApplication.PeopleDB;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AspireDBTestApplication.PeopleDbManager
{
    public class PeopleDbInitializer(IServiceProvider serviceProvider, ILogger<PeopleDbInitializer> logger)
    : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";

        private readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PeopleDbContext>();

            await InitializeDatabaseAsync(dbContext, cancellationToken);
        }

        private async Task InitializeDatabaseAsync(PeopleDbContext dbContext, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity("Initializing catalog database", ActivityKind.Client);

            var sw = Stopwatch.StartNew();

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(dbContext.Database.MigrateAsync, cancellationToken);

            await SeedAsync(dbContext, cancellationToken);

            logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
        }

        private async Task SeedAsync(PeopleDbContext dbContext, CancellationToken cancellationToken)
        {
            logger.LogInformation("Seeding database");

            static List<Person> GetPreconfiguredPeople()
            {
                return [
                    new() { FirstName = "Elvis", LastName = "Presley" },
                    new() { FirstName = "Michael", LastName = "Jackson" },
                    new() { FirstName = "Elton", LastName = "John" },
                    new() { FirstName = "David", LastName = "Bowie" },
                    new() { FirstName = "Phil", LastName = "Collins" }
                ];
            }

            //if (!dbContext.People.Any())
            //{
                //await dbContext.People.LoadAsync();
                //var foundPeopleInDB = dbContext.People.Local.ToList();
                await dbContext.People.ExecuteDeleteAsync();
                var people = GetPreconfiguredPeople();
                await dbContext.People.AddRangeAsync(people, cancellationToken);

                logger.LogInformation("Seeding {Peoplecount} people", people.Count);

                await dbContext.SaveChangesAsync(cancellationToken);
            //}

        }
    }
}
