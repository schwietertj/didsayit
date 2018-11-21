FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DidSayIt/DidSayIt.csproj", "DidSayIt/"]
RUN dotnet restore "DidSayIt/DidSayIt.csproj"
COPY . .
WORKDIR "/src/DidSayIt"
RUN dotnet build "DidSayIt.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DidSayIt.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DidSayIt.dll"]