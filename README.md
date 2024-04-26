# SkyWise
This project is the practical part of my graduate thesis.
 
The application is main goal is to server private pilots with accurate weather data at departure airport, arrival airport and along the route. Logged in user should also be able to save a flight for a specific date and track if there is a go or no-go based on weather forcast.

## Problem
I am a private pilot myself, allowed to fly according to visual flight rules (VFR), which mean that I have to consider visibility and cloud levels before I head to the airport.

There are plenty of services that show current aviation weather, but they often lack a user friendly interface and it can be time-consuming to check weather along a route and also get a quick overview the day before to see if the flight is doable or not.

The database need to handle a lot of data that is constantly changing, and it's important that data is accurate. 

## Solution
I will create a simple, easy to use, application that quickly respond (read: well built backend) with current data and a forcast for the upcoming day. 


## Planned tech stack
### Backend
- API and backen services  in **C# / .NET**
- **SQL database**
  - Hold user information and airport and airport weather data. Will require spatial data.
- Redis cache holding updated data about popular airports.

### Frontend
- Web application built i **SvelteKit**
