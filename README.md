🌟 BoughtIt 🌟
🎉 Empowering e-commerce innovation

BoughtIt is a robust e-commerce platform that leverages modern web technologies like .NET and Angular to deliver a scalable, seamless online shopping experience.

🚀 Features

🔐 Authentication & Authorization using JWT Tokens
📦 Microservices Architecture (UserOrder & Inventory)
📡 RabbitMQ Integration for messaging
📊 Real-Time Updates
🛠️ Clean Architecture with CQRS & Mediator

🖥️ Technologies Used
Frontend: Angular 17 (with Standalone Components)
Backend: .NET 8, ASP.NET Core, EF Core
Messaging: RabbitMQ
Database: SQL Server
API Gateway: Ocelot

🛠️ Setup Instructions

Prerequisites
Install .NET SDK, Node.js, and RabbitMQ
Set up SQL Server for database operations

Frontend
  cd frontend
  npm install
  ng serve
  
Backend
  cd backend
  dotnet restore
  dotnet run

RabbitMQ
  Ensure RabbitMQ server is running locally.

🎯 How It Works
1️⃣ Product Creation
When a product is added in the Inventory Service, RabbitMQ sends a message to the UserOrder Service to synchronize the product data.

2️⃣ Order Placement
Users can place orders seamlessly, with updates synced across services in real time.

3️⃣ API Gateway
Routes and authenticates requests to the appropriate microservice.
