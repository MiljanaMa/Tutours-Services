# Tutours-Services
# Overview
Welcome to our microservices-based project! This project is structured following the principles of microservice architecture, and it employs Docker for containerization.  

Each service within this project uses a different type of database to meet specific requirements:  
  -Tour Service: Uses MongoDB  
  -Encounter Service: Uses PostgreSQL  
  -Follower Service: Uses MongoDB  

Additionally, the Saga pattern is implemented to manage transactions across the Tour and Encounter services. All services communicate via gRPC.  

# Monitoring and Observability

1. Monitoring  
  Prometheus: For real-time monitoring of the services, collecting metrics, and alerting.

2. Logging  
  Loki: For centralized logging, making it easier to search and analyze logs from all services.

3. Tracing  
  Jaeger: For distributed tracing, allowing you to trace requests through the entire system and diagnose performance issues.

4. Visualization  
  Grafana: For visualizing metrics, logs, and traces collected by Prometheus, Loki, and Jaeger.

## Members:

- [Milena Marković](https://github.com/MilenaM06),        RA 83/2020
- [Miljana Marjanović](https://github.com/MiljanaMa),     RA 123/2020
- [Strahinja Praška](https://github.com/strahinjapraska), RA 245/2021
- [Anastasija Novaković](https://github.com/anastano),    RA 77/2020
