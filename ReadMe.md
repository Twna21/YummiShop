```markdown
Web use ASP.NET core, RazorPage, SignalR<br>
DB: SQL Server

How to RUN:
1: dotnet ef database update
2: Set StartUP Project --> ShopWEB

If using .NET 8, change the following:
<PropertyGroup>
  <TargetFramework>net7.0</TargetFramework> <!-- change to 8.0 -->
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
</PropertyGroup>
