FROM mcr.microsoft.com/dotnet/sdk:8.0.413-noble-amd64

RUN mkdir -p /home/app

WORKDIR /home/app

COPY . .

EXPOSE 5184

CMD ["dotnet", "watch", "run"]