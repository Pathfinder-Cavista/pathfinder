![Docker Image Version](https://img.shields.io/docker/v/blueclikk/pathfinder-api?sort=semver&label=version)
![Docker Pulls](https://img.shields.io/docker/pulls/blueclikk/pathfinder-api)

# Pathfinder - Talent Data Tracker

Talent Data Tracker is a hackathon project built to help talent teams monitor and analyze the recruitment process.
The system provides insights into how long it takes for roles to be filled, tracks application trends, and generates actionable reports.

## Team Members

- Chukwudi Ike-nwako – Frontend & UI
- Folusho Onafowokan - Product Coordinator
- Shalom Gar - Backend
- Osehiase Ehilen - Mobile (Android)
- Toba Ojo - Backend

## Tech Stack

- Backend: .NET 8 Web API
- Frontend: Angular
- Background Jobs: Hangfire
- Database: PostgreSQL
- Excel Export: EPPlus
- File Uploads: Cloudinary
- Authentication: JWT + Custom API Key
- Dashboard: Hangfire Dashboard with Basic Auth
- Containerization: Docker & Docker Compose

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- Docker & Docker Compose
- PostgreSQL (optional if not using Docker)
- Cloudinary account for file uploads

### Steps

- Clone the repository:

```bash
git clone https://github.com/Pathfinder-Cavista/pathfinder.git
cd pathfinder
```

### Configure environment variables:

- Create a .env file in **pathfinder/docker** with variables like:

```bash
ConnectionStrings__DefaultConnection=connectionstring
HangfireSettings__Password=dashboardpassword
HangfireSettings__UserName=dashboardusername
JwtSettings__PrivateKey=jwt_signing_key
JwtSettings__Issuer=jwt_issuer
JwtSettings__Audience=jwt_audience
JwtSettings__Expires=expiration_in_hours
Cloudinary__ApiKey=cloudinary_apikey
Cloudinary__CloudName=cloudinary_cloudname
Cloudinary__Secret=cloudinary_secret
Analytics__Header=custom_header_name
Analytics__Value=custom_header_value
```

### Start services via Docker Compose:

```bash
cd approot/docker
docker-compose up -d
```

### Access API on:

```bash
http://localhost:5005
```

### Access Hangfire Dashboard (with basic auth credentials set in environment variables):

```bash
http://localhost:5005/admin/jobs
```

## Features & Implementation

### Role Tracking & Insights

- Tracks how long roles remain open from posting to hire.
- Records application timestamps to calculate trends and averages.

### Excel Reports

- Generates downloadable Excel reports using EPPlus.
- Includes insights like role fill times, application trends, and candidate statistics.

### File & Photo Uploads

- Talent-related files and candidate photos uploaded and stored via Cloudinary.
- Supports direct upload via API endpoints.

### Authentication

- JWT for user authentication.
- Custom API keys for analytics enpoints and automated service access.
- Secures Hangfire Dashboard with Basic Auth.

### API

- Full API documentation is generated from Swagger JSON.

Swagger UI: http://localhost:5005/swagger/index.html

## Endpoints include:

- Registration and Login
- Candidate application tracking
- File uploads
- Excel report generation

## Project Github Branches

- Backend: main
- Frontend: web
- Mobile: feature-Mobile-Recruitment-Tracker

## License

MIT License
