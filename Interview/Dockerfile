# Стандартный SDK образ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Копируем всё
WORKDIR /app
COPY . .

# Собираем
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .

# Запуск
ENTRYPOINT ["dotnet", "Interview.dll"]