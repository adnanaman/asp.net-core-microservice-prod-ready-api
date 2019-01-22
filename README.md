# asp.net-core-microservice-prod-ready-api
asp.net core web API - micro service and starter project for REST based web service. Optimised for performance and used best practices

Services:
  1. Json web token (JWT) 
      - To do the authorization based on tokens, clients can generate tokens, some operations are admin based so for that has to 
      create admin based token. It includes TokenGenerator, which shouldn't be part of it but for ease purpose i included. 
  2. Allowed Cross Region Resource sharing
      - To allow different origin requests
  3. Redis Distribution cache + E-Tags caching mechanism
      - Redis is in-memory database used for caching, super fast and reliable. I implemented that only for demo purpose.
  4. Add versioning 
      - For having different versions of services, its necessary to know best practices
  5. Added Swagger support
      - For me its WSDL for REST full services
  6. Repository (json based database file) 
      - some English monarchs tenures data (preparing for life in the uk test now a days, so monarchy is on top of my head all the time)
 
Middleware:
  1. Error handling middleware
      - Handle exception handling by logging problems only
  2. Response Compression 
      - Preferably use Hosting server based compression like IIS, Apache, Nginx but useful if Hosting on Kestrel or HTTP.sys server 
  3. Limiting Middleware 
      - To restrict number of request to avoid basic level of DOS attacks and crawlers
  4. UseCookiePolicy 
      - for EU GDPR regulations
  5. Health checks
      - asp.net core offers built in health checks libraries 
  6. Seri Logger File Rolling
      - .net core comes with built in logger but thats very limited, I used SeriLog library as its faster than others like NLog and Log4net
      and provides structured logs. logging is backbone of any production based APIs so has to be very careful with that what you choose.
  
  
  Best practices:
    1. Dependency Injection
    2. Async actions optimized to use max processor threads
    3. Middleware to handle request of ErrorHandling, ETag, Limiting requests 
    4. Extension methods for versioning, swagger and JWT authentication
    5. Redis-ETag cache helper 
  
