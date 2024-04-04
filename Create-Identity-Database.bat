dotnet ef migrations remove  -v --project .\Mimeo.Middle -c ApplicationDbContext
dotnet ef migrations add CreateIdentitySchema -v --project .\Mimeo.Middle -c ApplicationDbContext
dotnet ef database update -v --project ..\Mimeo.Middle -c ApplicationDbContext

