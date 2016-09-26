FROM microsoft/dotnet:1.0.0-preview2-sdk

RUN mkdir /app
WORKDIR /app

COPY src/DotCom/project.json /app
COPY NuGet.config /app
RUN ["dotnet", "restore"]

COPY src/DotCom /app
RUN ["dotnet", "build"]

EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "run"]