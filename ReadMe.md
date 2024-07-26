```markdown
Web use ASP.NET core, RazorPage, SignalR<br>
DB: SQL Server

link demo: twna.shop
ac: tuongnmde170578@fpt.edu.vn
pw: manhtuong1

How to RUN: 

1: dotnet ef database update
2: Set StartUP Project --> ShopWEB

If using .NET 8, change the following:
<PropertyGroup>
  <TargetFramework>net7.0</TargetFramework> <!-- change to 8.0 -->
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
</PropertyGroup>

The project have 2 Dbcontext so:
      1: dotnet ef database update --context ShopDbContext
