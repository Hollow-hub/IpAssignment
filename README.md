# IP Information Service

## Overview

The IP Information Service is a .NET application designed to retrieve detailed information about IP addresses using the IPStack service. It provides both IPv4 and IPv6 support and caches results to improve performance and reduce external API calls.

---

## Features

- Retrieve detailed information about IP addresses.
- Support for both IPv4 and IPv6 addresses.
- Caching of IP information to improve performance.
- Integration with IPStack service for IP details.
- Asynchronous methods for better scalability and responsiveness.
- SQLite database for efficient and lightweight data storage.

---

## Installation

### Prerequisites

- .NET 9.0 SDK or higher

### Setup

You will need to pass an api_key to your enviroment variables with this name: `IpStackSettings__ApiKey`

## Usage

### Endpoints

- **GET** `/api/ip/{ip}`: Retrieves the details for a specified IP address.

You can browse all available endponts here: `/scalar/v1` 

## Implementation Details

### Asynchronous IP Details Retrieval

The application uses an asynchronous method to retrieve IP details from the IPStack service. This approach is preferred over the synchronous alternative as it allows for better scalability and responsiveness.

### Database Choice: SQLite

For this application, SQLite was chosen for the following reasons:

- **Lightweight**: Requires minimal setup and maintenance.
- **Ease of Use**: Integrates seamlessly with .NET applications without needing a separate database server.
