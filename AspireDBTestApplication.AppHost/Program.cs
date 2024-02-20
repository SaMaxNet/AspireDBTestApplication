var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireDBTestApplication_ApiService>("apiservice");

builder.AddProject<Projects.AspireDBTestApplication_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();
