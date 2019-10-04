# It will download sdk into the docker image and work directory to be /app
FROM microsoft/dotnet:sdk as build-env
WORKDIR /app

# Copy csprojects and restore under /app that is defined as a part of work directory.
COPY webapp/*.csproj ./
RUN dotnet restore

# copy everything else and build the release where the output will be in out folder.
COPY ./webapp ./
RUN dotnet publish -c Release -o out

# It will download only runtime and work directory tobe /app
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "webapp.dll"]

## to run this docker (-t means image tag)
## docker build -t webapp .
## docker run -d -p 8080:80 --name myapp webapp
