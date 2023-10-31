#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ThirdProject-BackEnd/ThirdProject-BackEnd.csproj", "ThirdProject-BackEnd/"]
RUN dotnet restore "ThirdProject-BackEnd/ThirdProject-BackEnd.csproj"
COPY . .
WORKDIR "/src/ThirdProject-BackEnd"
RUN dotnet build "ThirdProject-BackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThirdProject-BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThirdProject-BackEnd.dll"]