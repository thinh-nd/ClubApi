### Get started
- Run ./elasticsearch-7.11.1/bin/elasticsearch.bin to start local storage
- Elasticsearch is config to use the default setting, which is localhost:9200
- Elasticsearch has been preconfigured to use 100MB of heap, to avoid it using too much memory

If you have postman, import the Clubs.postman_collection file to have the sample requests, but make sure that the API is running at localhost:44348

### Design questions
1. Could you please describe your ideal strategy to handle interservice communications in a microservice environment, especially when hosted in the cloud?
- Avoid synchronous communication, such as HTTP request/response in the request chain between microservices. Instead, use asynchronous communication like message-based, event-based (AMQP) or polling to communicate between microservices. It is depends on the problem to correctly select which type of communication to be used.
- A microservice should not allocate a thread to perform a network call to other microservices, since if too many request are issued, and other microservice may be down/unhealthy, then the request made can take long time to complete or timeout, wasting resource while waiting for the response. This bring back to the point of using asynchronous communication method above.
- If there are many microservices in the system, and they communicate frequently, using gRPC/Protobuf can boost throughput and therefore increase performance instead of REST/JSON
- Uses appropriate retry strategy to handle network calls. For example, if a network call failed, a service could retry 1-2 times in case the other service was down temporarily or being restarted, if multiple called failed multiple times in a row, then block calls to that services for some period (circuit breaker)
- As in the cloud, the microservices should be within a network, rather than spreading over multiple network or region, in order to minimize network latency

2. What could be the consequences of not adequately handling interservice communications in a microservice environment?
- Long response time from the client's pespective
- Backing up requests in a node, causing swamp resources (threads/CPU/memory), and eventually bring down node/service in worst case
- Coupling between microservices, causing a microservice can't function properly when dependant microservices are down or unhealthy, thus reduce functionality/availability of the system overall
