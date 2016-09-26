FROM microsoft/dotnet:1.0.0-preview2-sdk

RUN mkdir /app
WORKDIR /app

COPY src/DotCom/project.json /app
COPY NuGet.config /app

RUN ["apt-get", "update"]
RUN ["apt-get", "install", "nodejs"]
RUN ["apt-get", "install", "npm"]
RUN ["npm", "install", "-g", "bower"]
RUN ["npm", "install", "-g", "gulp"]
RUN ["dotnet", "restore"]

COPY src/DotCom /app
RUN ["dotnet", "build"]

EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "run"]