# asp.net-core-microservice-prod-ready-api
asp.net core web API - micro service and starter project for REST based web service. Optimised for performance and used best practices

Services used:
  1. Json web token (JWT) 
  2. Allowed Cross Region Resource sharing
  3. Redis Distribution cache + E-Tags caching mechanism
  4. Add versioning 
  5. Added Swagger support
  6. Repository (json based database file) 
      - some English monarchs tenures data (preparing for life in the uk test now a days, so monarchy is on top of my head all the time)
 
Middleware used:
  1. Error handling middleware 
  2. Response Compression
  3. Limiting Middleware to restrict number of request to avoid basic level of DOS attacks and crawlers
  4. UseCookiePolicy for EU GDPR regulations
  5. Health checks
  6. Seri Logger File Rolling
  
  
