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

1. initialize git submodules - `git submodule init` followed by `git submodule update

2. make sure you have a running kubernetes cluster you can use (on Windows docker for windows with kubernetes turned on will do)

3. webapi requires a kafka cluster for todos storage. Create a `secrets.json` file in webapi\src\GraphQLApi folder and add configuration for it:
```
{
  "KafkaSecrets": {
    "TodosTopicName": "[name of topic with todos]",
    "Username": "[username]",
    "Password": "[password]",
    "BootstrapServers": "[comma delimited list of kafka bootstrap servers"
  }
}
```
Also change names of kafka topics in `kafka-topics.json`.

I plan to add a local kafka cluster to kubernetes, but it is not done for now

4. install skaffold (https://skaffold.dev/) or tilt (https://tilt.dev/)

5. run `skaffold dev` or `tilt up` and wait for cluster to start

6. website should be available at localhost:8080

7. graphql webapi should be available at localhost:8081. You can play with it on localhost:8081/playground. Try adding a new todo:
```
mutation {
  addTodo(input: { text: "test" }) {
    todo {
      completed,
      text,
      id
    }
  }
}
```
and this query:
```
{
  todos {
    completed,
    text
  }
}
```

hot reloading should be working so whenever you change code, respective service should be recompiled instantly without building new docker conatiner.
