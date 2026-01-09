FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY *.csproj ./
RUN dotnet restore
COPY Controllers/ Controllers/
COPY Data/ Data/
COPY Migrations/ Migrations/
COPY Models/ Models/
COPY Helpers/ Helpers/
COPY Services/ Services/
COPY Views/ Views/
COPY wwwroot/ wwwroot/
COPY Program.cs appsettings*.json ./
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production
RUN mkdir -p /app/data
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "DotNetProject.dll"]
