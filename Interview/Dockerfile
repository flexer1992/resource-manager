# Этап 1: Билд и тесты
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-test
WORKDIR /src
COPY . .

# Восстановление зависимостей
RUN dotnet restore

# Сборка
RUN dotnet build --no-restore

# Запуск тестов
RUN dotnet test --no-build --verbosity normal


# Публикация только основного проекта
FROM build-test AS publish
RUN dotnet publish "./Interview/Interview.csproj" -c Release -o /app/out

# Этап 2: Финальный runtime-образ без SDK и тестов
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/out .

ENTRYPOINT ["dotnet", "Interview.dll"]