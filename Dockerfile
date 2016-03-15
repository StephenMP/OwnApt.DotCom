FROM microsoft/aspnet:latest

EXPOSE 5000  
ENTRYPOINT ["dnx", "-p", "src/TestDnx/project.json", "web"]

COPY src/TestDnx/project.json /app/  
WORKDIR /app  
RUN ["dnu", "restore"]  
COPY . /app 
