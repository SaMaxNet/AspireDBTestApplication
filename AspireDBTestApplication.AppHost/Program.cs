var builder = DistributedApplication.CreateBuilder(args);

//var sql = builder.AddSqlServer("sql").AddDatabase("sqldata");


var dbmanager = builder.AddProject<Projects.AspireDBTestApplication_PeopleDbManager>("dbmanager");
//.WithReference(sql);

var apiService = builder.AddProject<Projects.AspireDBTestApplication_ApiService>("apiservice")
    .WithReference(dbmanager);
    //.WithReference(sql);

builder.AddProject<Projects.AspireDBTestApplication_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();
