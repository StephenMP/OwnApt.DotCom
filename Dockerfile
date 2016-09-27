FROM microsoft/dotnet:1.0.0-preview2-sdk

# Install our dependencies and nodejs
RUN echo "deb http://archive.ubuntu.com/ubuntu precise main universe" > /etc/apt/sources.list
RUN apt-get update
RUN apt-get -y install python-software-properties git build-essential
RUN add-apt-repository -y ppa:chris-lea/node.js
RUN apt-get update
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