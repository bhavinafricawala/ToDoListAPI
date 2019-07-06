FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["ToDoListAPI.csproj", ""]
RUN dotnet restore "ToDoListAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ToDoListAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ToDoListAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

CMD dotnet ToDoListAPI.dll --urls "http://*:$PORT"

