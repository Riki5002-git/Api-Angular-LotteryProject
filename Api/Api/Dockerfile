# שלב הבנייה (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# העתקת קובץ הפרויקט (שיניתי ל-Api.csproj)
COPY ["Api.csproj", "./"]
RUN dotnet restore

# העתקת כל שאר הקבצים ובנייה
COPY . .
RUN dotnet publish -c Release -o /app/publish

# שלב ההרצה (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]