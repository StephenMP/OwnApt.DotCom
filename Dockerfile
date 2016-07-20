FROM microsoft/aspnet:latest

COPY . /app
WORKDIR /app
RUN ["dnu", "restore"]

EXPOSE 5000  
ENTRYPOINT ["dnx", "-p", "src/DotCom/project.json", "docker-web"]
