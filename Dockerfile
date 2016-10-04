# Use .net core
FROM microsoft/dotnet:1.0.0-preview2-sdk

# Install node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_5.x | bash
RUN apt-get install nodejs
RUN node -v
RUN npm -v

# Install bower and gulp
RUN node install -g bower
RUN node install -g gulp

# Build out root dir
RUN mkdir /app
WORKDIR /app

# Restore
COPY src/DotCom/project.json /app
COPY NuGet.config /app
RUN ["dotnet", "restore"]

# Build the rest of the app
COPY src/DotCom /app
RUN dotnet build
RUN npm install
RUN bower install
RUN gulp minify:css

EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "run", "http://0.0.0.0:5000"]