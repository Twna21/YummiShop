
Alternatively, you can use the HTML line break tag `<br>`:

```markdown
Web use ASP.NET core, RazorPage, SignalR<br>
DB: SQL Server<br>

How to RUN:<br>
1: dotnet ef database update<br>
2: Set StartUP Project --> ShopWEB<br>

If using .NET 8, change the following:<br>
```xml
<PropertyGroup>
  <TargetFramework>net7.0</TargetFramework> <!-- change to 8.0 -->
  <ImplicitUsings>enable</ImplicitUsings>
  <Nullable>enable</Nullable>
</PropertyGroup>
