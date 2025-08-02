var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.BlazorWASM>("webapp");
builder.Build().Run();
