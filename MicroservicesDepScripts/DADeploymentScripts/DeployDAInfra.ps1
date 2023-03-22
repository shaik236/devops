docker network create net_kafka

docker run --network=net_kafka --user redis --name avocet-cache -p 6379:6379 -d redis:alpine redis-server

docker run --network=net_kafka --detach --name zookeeper -p 2181:2181 -e ZOOKEEPER_CLIENT_PORT=2181 avocetodsrepo.azurecr.io/kafkaalpine:latest

Start-Sleep -Seconds 30

docker run --network=net_kafka -d --name=kafka -p 29092:29092 -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://kafka:29092 -e KAFKA_LISTENERS=PLAINTEXT://0.0.0.0:29092 -e KAFKA_BROKER_ID=2 -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 avocetodsrepo.azurecr.io/kafkaalpine:latest

docker create --name avocethotdbworker --network=net_kafka avocetodsrepo.azurecr.io/avoceteventworker
docker cp .\appset\hotworkerappsettings.json avocethotdbworker:/app/appsettings.json
docker cp .\configs\. avocethotdbworker:/app/config
docker container start avocethotdbworker

docker create --name avocetwarmdbworker --network=net_kafka avocetodsrepo.azurecr.io/avoceteventworker
docker cp .\appset\warmworkerappsettings.json avocetwarmdbworker:/app/appsettings.json
docker cp .\configs\. avocetwarmdbworker:/app/config
docker container start avocetwarmdbworker

docker create --name avocethotdbconsumer --network=net_kafka avocetodsrepo.azurecr.io/avoceteventconsumer
docker cp .\appset\hotconsappsettings.json avocethotdbconsumer:/app/appsettings.json
docker cp .\configs\. avocethotdbconsumer:/app/config
docker container start avocethotdbconsumer

docker create --name avocetwarmdbconsumer --network=net_kafka avocetodsrepo.azurecr.io/avoceteventconsumer
docker cp .\appset\warmconsappsettings.json avocetwarmdbconsumer:/app/appsettings.json
docker cp .\configs\. avocetwarmdbconsumer:/app/config
docker container start avocetwarmdbconsumer