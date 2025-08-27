# 🌐 GLOB-IMPORT-EXPORT-MODULE API

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

---

## 📝 Overview

This repository contains the source code for the **GLOB-IMPORT-EXPORT-MODULE**, a robust and scalable microservice designed for efficient project and issue management. Built with .NET 8, this API provides a comprehensive set of endpoints to handle projects, sprints, issues, and user assignments, making it a vital component in a modern project management ecosystem. The service is fully containerized using Docker, ensuring consistent deployment and scalability.

---

## ✨ Features

- **Project Management**: Create, update, and manage projects.
- **Issue Tracking**: Full CRUD operations for issues, including status and priority management.
- **Sprint Planning**: Organize issues into sprints for agile development cycles.
- **User Association**: Assign users to projects.
- **Containerized**: Ready for deployment in any Docker-compatible environment.

---

## 🛠️ Technologies Used

- **Backend**: C#, .NET 8
- **API**: RESTful API with Swagger for documentation
- **Containerization**: Docker

---

## 🚀 Getting Started

Follow these instructions to get a development environment running.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- A code editor like [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

### Local Installation

1.  **Clone the repository**
    ```sh
    git clone https://github.com/your_username/your_repository.git
    ```
2.  **Navigate to the API directory**
    ```sh
    cd GLOB-IMPORT-EXPORT-MODULE/api
    ```
3.  **Restore .NET dependencies**
    ```sh
    dotnet restore
    ```
4.  **Run the application**
    ```sh
    dotnet run
    ```
The API will be running on the port specified in `Properties/launchSettings.json`.

### 🐳 Docker Installation

Alternatively, you can build and run the service using Docker.

1.  **Navigate to the API directory**
    ```sh
    cd GLOB-IMPORT-EXPORT-MODULE/api
    ```
2.  **Build the Docker image**
    ```sh
    docker build -t glob-api .
    ```
3.  **Run the Docker container**
    ```sh
    docker run -p 8080:8080 glob-api
    ```
The service will be accessible at `http://localhost:8080`.

---

## 📖 API Usage

Once the application is running, you can explore and interact with the API endpoints through the Swagger UI.

- **URL**: `http://localhost:<port>/swagger`

The Swagger interface provides detailed documentation for all available endpoints, including request and response schemas.

---

## 📄 License

This project is licensed under the terms of the LICENSE file. See [LICENSE](LICENSE) for more details.