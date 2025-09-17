# Path Finder - Cavista Tech. Talent Data Tracker

This is a lightweight application to automate the tracking of open roles, calculate the number of days they have remain open and integrate the data into dashboards for real-time insights

---

## Running the App

From the `docker` directory (where the `docker-compose.yml` file is location), run:

```bash
docker-compose up -d db
```

Then run other services, e.g:

```
docker-compose up --build api
```

This will:

- First build the Database image and ensure it is fully initialized.
- Build the the image of the backend API
- Start the Database and the API services defined in the `docker-compose.yml`

## Accessing the Service

- Once running, the API service will be available at:

```
http://localhost:5005
```
