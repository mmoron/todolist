This project is a testbed for different technologies I'd like to play with. Its aim is to create a microservice based web app hosted in docker via kubernetes. It is not (and is not meant to be) production ready and secure. Current plan includes:

* microfrontend in react
* apollo graphql client
* graphql webapi
* event sourcing based backend with Kafka
* CQRS
* microservices in .netcore mvc, node and (not selected yet) functional language
* and more... ;-)

Everything should be independently deployable.

## How to get started:

1. make sure you have a running kubernetes cluster you can use (on Windows docker for windows with kubernetes turned on will do)
2. install skaffold (https://skaffold.dev/) or tilt (https://tilt.dev/)
3. run `skaffold dev` or `tilt up` and wait for cluster to start
4. website should be available at localhost:8080
5. graphql webapi should be available at localhost:8081. You can play with it on localhost:8081/playground. Try this query:
```
{
  todos {
    completed,
    text
  }
}
```

hot reloading should be working so whenever you change code respective service should be recompiled instantly without building new docker conatiner.
