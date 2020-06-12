# Simple Voting App

Simple voting app consist of 5 microservices (3 BE & 2 FE)
- Api Gateway Service
- Identity Service
- Voting Service
- Site Admin
- Site Client

## Api Gateway
Simple API Gateway to route request based on configuration. Configuration defined the routes and auth service to validate the request if the incoming request need to check its privilege first.
Build using:
- .NET Core API

## Identity Service
Identity service provide authentication. It has features for account, session, and check privilege.
Build using:
- .NET Core API
- SQL Server
- Entity Framework
- Kafka

## Voting Service
Voting service handle all voting related service like CRUD voting items & CRUD categories and feature for user to vote for a voting item.
Build using:
- .NET Core API
- SQL Server
- Entity Framework
- Kafka

## Site Admin
Admin site to handle CRUD for voting items & categories.
Build using:
- ASP .NET Core MVC
- Javascript
- JQuery
- Ajax
- HTML

## Site Client
Client site used for client to vote for a voting items. User can login/logout and register. Only registered user authorized to vote.
Build using:
- ASP .NET Core MVC
- Javascript
- JQuery
- Ajax
- HTML


# Requirement
- .NET Framework 4.8
- SQL Server 2019
- IIS
- Kafka
- IDE -> Visual Studio 2019 (reccomended)