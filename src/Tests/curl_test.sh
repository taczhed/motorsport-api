#!/bin/bash

BASE_URL="http://localhost:8080/api"

# Login
echo "Logging in..."
TOKEN=$(curl -s -X POST "$BASE_URL/Auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}' | jq -r '.token')

echo "Token: $TOKEN"

AUTH_HEADER="Authorization: Bearer $TOKEN"

##########################
# Create a Driver
echo "Creating a driver..."
DRIVER_ID=$(curl -s -X POST "$BASE_URL/Drivers" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"name":"John Doe","age":30}' | jq -r '.id')

# Create a Track
echo "Creating a track..."
TRACK_ID=$(curl -s -X POST "$BASE_URL/Tracks" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"name":"Silverstone","location":"UK","lengthKm":5.89}' | jq -r '.id')

# Create a Race
echo "Creating a race..."
RACE_ID=$(curl -s -X POST "$BASE_URL/Races" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"name":"British GP","date":"2025-07-01T12:00:00Z","trackId":'"$TRACK_ID"'}' | jq -r '.id')

# Create a Car
echo "Creating a car..."
CAR_ID=$(curl -s -X POST "$BASE_URL/Cars" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"brand":"Mercedes","model":"W13","number":"44","driverId":'"$DRIVER_ID"'}' | jq -r '.id')

##########################
# GET requests
echo "Fetching drivers..."
curl -s -X GET "$BASE_URL/Drivers" -H "$AUTH_HEADER"

echo "Fetching driver $DRIVER_ID..."
curl -s -X GET "$BASE_URL/Drivers/$DRIVER_ID" -H "$AUTH_HEADER"

echo "Fetching cars..."
curl -s -X GET "$BASE_URL/Cars" -H "$AUTH_HEADER"

echo "Fetching car $CAR_ID..."
curl -s -X GET "$BASE_URL/Cars/$CAR_ID" -H "$AUTH_HEADER"

echo "Fetching tracks..."
curl -s -X GET "$BASE_URL/Tracks" -H "$AUTH_HEADER"

echo "Fetching track $TRACK_ID..."
curl -s -X GET "$BASE_URL/Tracks/$TRACK_ID" -H "$AUTH_HEADER"

echo "Fetching races..."
curl -s -X GET "$BASE_URL/Races" -H "$AUTH_HEADER"

echo "Fetching race $RACE_ID..."
curl -s -X GET "$BASE_URL/Races/$RACE_ID" -H "$AUTH_HEADER"

echo "Fetching stats..."
curl -s -X GET "$BASE_URL/Stats" -H "$AUTH_HEADER"

##########################
# PUT requests
echo "Updating driver..."
curl -s -X PUT "$BASE_URL/Drivers/$DRIVER_ID" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"name":"John Updated","age":31}'

echo "Updating car..."
curl -s -X PUT "$BASE_URL/Cars/$CAR_ID" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"brand":"Mercedes","model":"W14","number":"44","driverId":'"$DRIVER_ID"'}'

echo "Updating track..."
curl -s -X PUT "$BASE_URL/Tracks/$TRACK_ID" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"name":"Silverstone Updated","location":"UK","lengthKm":5.89}'

echo "Updating race..."
curl -s -X PUT "$BASE_URL/Races/$RACE_ID" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"name":"British GP Updated","date":"2025-07-02T12:00:00Z","trackId":'"$TRACK_ID"'}'

##########################
# POST driver to race
echo "Assigning driver to race..."
curl -s -X POST "$BASE_URL/Races/$RACE_ID/drivers" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"driverId":'"$DRIVER_ID"',"position":1,"time":"01:20:00"}'

# Update driver's race result
echo "Updating driver in race..."
curl -s -X PUT "$BASE_URL/Races/$RACE_ID/drivers/$DRIVER_ID" \
  -H "$AUTH_HEADER" -H "Content-Type: application/json" \
  -d '{"driverId":'"$DRIVER_ID"',"position":2,"time":"01:18:00"}'

##########################
# DELETE requests
echo "Removing driver from race..."
curl -s -X DELETE "$BASE_URL/Races/$RACE_ID/drivers/$DRIVER_ID" -H "$AUTH_HEADER"

echo "Deleting race..."
curl -s -X DELETE "$BASE_URL/Races/$RACE_ID" -H "$AUTH_HEADER"

echo "Deleting car..."
curl -s -X DELETE "$BASE_URL/Cars/$CAR_ID" -H "$AUTH_HEADER"

echo "Deleting driver..."
curl -s -X DELETE "$BASE_URL/Drivers/$DRIVER_ID" -H "$AUTH_HEADER"

echo "Deleting track..."
curl -s -X DELETE "$BASE_URL/Tracks/$TRACK_ID" -H "$AUTH_HEADER"