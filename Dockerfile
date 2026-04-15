# ── Build stage ────────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first for layer-cached restore
COPY RaoCattles.Api/RaoCattles.Api.csproj RaoCattles.Api/
COPY RaoCattles.BuildingBlocks/RaoCattles.BuildingBlocks.csproj RaoCattles.BuildingBlocks/
COPY Modules/Products/RaoCattles.Modules.Products.Application/RaoCattles.Modules.Products.Application.csproj Modules/Products/RaoCattles.Modules.Products.Application/
COPY Modules/Products/RaoCattles.Modules.Products.Domain/RaoCattles.Modules.Products.Domain.csproj Modules/Products/RaoCattles.Modules.Products.Domain/
COPY Modules/Products/RaoCattles.Modules.Products.Infrastructure/RaoCattles.Modules.Products.Infrastructure.csproj Modules/Products/RaoCattles.Modules.Products.Infrastructure/
COPY Modules/Products/RaoCattles.Modules.Products.Presentation/RaoCattles.Modules.Products.Presentation.csproj Modules/Products/RaoCattles.Modules.Products.Presentation/
COPY Modules/Users/RaoCattles.Modules.Users.Application/RaoCattles.Modules.Users.Application.csproj Modules/Users/RaoCattles.Modules.Users.Application/
COPY Modules/Users/RaoCattles.Modules.Users.Domain/RaoCattles.Modules.Users.Domain.csproj Modules/Users/RaoCattles.Modules.Users.Domain/
COPY Modules/Users/RaoCattles.Modules.Users.Infrastructure/RaoCattles.Modules.Users.Infrastructure.csproj Modules/Users/RaoCattles.Modules.Users.Infrastructure/
COPY Modules/Users/RaoCattles.Modules.Users.Presentation/RaoCattles.Modules.Users.Presentation.csproj Modules/Users/RaoCattles.Modules.Users.Presentation/
RUN dotnet restore RaoCattles.Api/RaoCattles.Api.csproj

# Copy everything else and publish
COPY . .
RUN dotnet publish RaoCattles.Api/RaoCattles.Api.csproj -c Release -o /app --no-restore

# ── Runtime stage ─────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "RaoCattles.Api.dll"]
