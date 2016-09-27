FROM microsoft/dotnet:1.0.0-preview2-sdk

# Install our dependencies and nodejs
RUN apt-get update
RUN apt-get -y install curl
RUN curl -sL https://deb.nodesource.com/setup_5.x | bash
RUN apt-get -y install nodejs
RUN npm install -g bower
RUN npm install -g gulp

# Create working directory
RUN mkdir /app
WORKDIR /app

# Copy over just the project.json (better performance)
COPY src/DotCom/project.json /app
COPY NuGet.config /app
RUN ["dotnet", "restore"]

EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "run", "http://0.0.0.0:5000"]