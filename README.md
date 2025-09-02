# 🌐 GLOB-IMPORT-EXPORT-MODULE API

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

---

## 📝 Overview

This repository contains the source code for the **GLOB-IMPORT-EXPORT-MODULE**, a robust and scalable microservice designed for efficient project and issue management. Built with .NET 8, this API provides a comprehensive set of endpoints to handle projects, sprints, issues, and user assignments, making it a vital component in a modern project management ecosystem. The service is fully containerized using Docker, ensuring consistent deployment and scalability.

---

## 📊 Jira Friendly Viewer

A cleaner and more user-friendly way to visualize **Jira** projects.  
This project allows you to **import and export** Jira data and view it in a simplified interface — no complex setup, just a better way to look at your projects.

## ✨ Features

- **Project visualization**: Load Jira project data and display it in a more organized, easy-to-read format.  
- **Import & export**: Bring data from Jira and export it in a simplified format.  
- **User-friendly interface**: Focused on what matters, without the clutter.  
- **Containerized**: Ready to run in any **Docker**-compatible environment. 

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

### Local Installation

1.  **Clone the repository**
    ```sh
    git clone https://github.com/brayrpgs/GLOB-IMPORT-EXPORT-MODULE.git
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

### 🐳 Docker Start

This service runs automatically as a **Dockerized microservice** — no extra setup required.  

By default, it will be available at:  
👉 `http://localhost:5184`

## 📖 API Usage

Once the application is running, you can explore and interact with the API endpoints through the Swagger UI.

- **URL**: `http://localhost:5184/swagger`

The Swagger interface provides detailed documentation for all available endpoints, including request and response schemas.

---

## 📄 License

This project is licensed under the terms of the LICENSE file. See [LICENSE](LICENSE) for more details.
