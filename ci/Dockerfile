FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /repo

# Copy repository (shallow copy is fine for tests)
COPY . /repo

# Restore and run tests
RUN dotnet restore "Bike Mania/Core.Tests/BikeMania.Core.Tests.csproj"

CMD ["dotnet", "test", "Bike Mania/Core.Tests/BikeMania.Core.Tests.csproj", "--verbosity", "normal"]
