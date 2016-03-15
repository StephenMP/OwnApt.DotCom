FROM microsoft/aspnet:latest

EXPOSE 5000  
ENTRYPOINT ["dnx", "-p", "src/DotCom/project.json", "docker-web"]

COPY src/DotCom/project.json /app/  
WORKDIR /app  
RUN ["dnu", "restore"]  
COPY . /app 
