# 🌟 **BoughtIt** 🌟  
🎉 **Feature-Packed E-Commerce Solution**  

BoughtIt is an advanced, feature-rich e-commerce platform built with **Angular** and **.NET Core**, focusing on scalability, seamless user experience, and modern development practices.  

---

## 🚀 **Key Features**  

### Backend (Powered by .NET Core 8)  
- 🔒 **Authentication & Authorization**  
  - JWT-based secure login/signup flow  
- 📡 **Microservices Architecture**  
  - **UserOrder Service** for handling orders  
  - **Inventory Service** for product management  
  - Communication via **RabbitMQ** for event-driven messaging  
- 🔄 **Caching**  
  - Integrated **Redis** for high-performance data caching  
- 📜 **Logging**  
  - Leveraged **Serilog** for structured, customizable application logging  
- 🛠️ **Clean Architecture**  
  - CQRS pattern with **Mediator** for organized and scalable code  

### Frontend (Angular 17 Standalone Components)  
- 🔒 **Google Authentication for quick access**
- 🛡️ **API Interceptor**  
  - Ensures secure and consistent API communication  
- 🎛️ **State Management**  
  - Using Angular Store with **Selectors**, **Reducers**, **Actions**, and **Effects** for unified and efficient data handling  
- ⚡ **Dynamic UI**  
  - Real-time product updates and user-friendly interfaces  

---

## 🖥️ **Technologies Stack**  

### Backend  
- **Frameworks**: .NET 8, ASP.NET Core, Entity Framework Core  
- **Messaging**: RabbitMQ  
- **Database**: SQL Server  
- **Caching**: Redis  
- **Logging**: Serilog  

### Frontend  
- **Framework**: Angular 17   

### Additional Tools  
- **API Gateway**: Ocelot for routing and authentication  

---


## 🎯 How It Works

1️⃣ User Authentication
Users can log in via email/password or Google Authentication.
JWT tokens ensure secure communication and session validation.

2️⃣ Product Synchronization
When a new product is added in the Inventory Service, RabbitMQ syncs it with the UserOrder Service database.

3️⃣ State Management
Data consistency across the app is ensured using Angular's Store for state management.

## 🛠️ Setup Instructions

Prerequisites
Install .NET SDK, Node.js, RabbitMQ, Redis, and SQL Server.

Frontend

	cd frontend
	npm install
	ng serve
 
Backend

	cd backend
	dotnet restore
	dotnet run
 
RabbitMQ

Start RabbitMQ and configure exchanges and queues.

## 🌟 Why BoughtIt?

BoughtIt is a demonstration of cutting-edge practices in web development, incorporating:

- 🛠️ Modern microservices architecture
- ⚡ Blazing-fast performance with caching
- 🔄 Event-driven synchronization using RabbitMQ
- 🎛️ Advanced state management in Angular
