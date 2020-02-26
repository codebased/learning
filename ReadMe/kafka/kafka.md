<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents*-  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Introduction](#introduction)
  - [Before Kafka](#before-kafka)
  - [Why Kafka](#why-kafka)
- [Setup Environment](#setup-environment)
  - [Vanilla Installation](#vanilla-installation)
  - [Docker Installation](#docker-installation)
  - [Important Configuration](#important-configuration)
    - [message.max.bytes](#messagemaxbytes)
    - [num.partitions](#numpartitions)
    - [log.retention.ms](#logretentionms)
    - [log.retention.bytes](#logretentionbytes)
    - [log.segment.bytes](#logsegmentbytes)
    - [log.segment.ms](#logsegmentms)
- [Kafka Components](#kafka-components)
  - [Topic](#topic)
    - [Topic Commands](#topic-commands)
    - [Partitioning](#partitioning)
    - [Message Retention Policy](#message-retention-policy)
  - [Producer](#producer)
    - [Delivery Guarantees](#delivery-guarantees)
    - [Kafka Producer Consumer Console](#kafka-producer-consumer-console)
    - [Fault-tolerance](#fault-tolerance)
  - [Consumer](#consumer)
    - [The Poll Loop](#the-poll-loop)
    - [The Offset](#the-offset)
      - [Offset commands](#offset-commands)
    - [Internal __consumer_offsets](#internal-__consumer_offsets)
    - [Read from __consumer_offsets](#read-from-__consumer_offsets)
    - [Consumer Group](#consumer-group)
      - [Commands](#commands)
  - [Zookeeper](#zookeeper)
  - [Confluent Schema Registry](#confluent-schema-registry)
    - [Confluent Schema Registry UI](#confluent-schema-registry-ui)
  - [Confluent REST Proxy](#confluent-rest-proxy)
  - [KSQL](#ksql)
- [REFERENCES](#references)
  - [Confluent Schema](#confluent-schema)
- [Future Topics](#future-topics)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Introduction

Apache Kafka is publish subscribe messaging rethought as a distributed commit log.

## Before Kafka

- Database Replication (log shipping)- RDBMS to RDBMS , Or Database specific, which is tightly coupled with the schema that is difficult to modify. Also, the impact of replication on overall system.

- ETL (Extract, Transform, and Load) - most of the times are proprietary, or a custom application build and maintain by the company. It has scalability and performance drawbacks.

- MSMQ - Limited functionality and scaling out was a biggest challenge with MSMQ that can only work with Microsoft technology. If the bug is there with the consumer, how to retrieve that message again? So it has many drawbacks such as consistency concerns, load handling,  Atomic transaction, that increase when you have complex system with big data set.

## Why Kafka

It is based on messaging system, which is based on messaging broker that commits to deliver the message. It scales out over many servers known as brokers. It can replay messages when required.

- Designed for high throughput,  and lowest latency with distributed messaging (log) system. 
- It is used to move data at high volumes.
- It was open sourced under Apache Software Foundation in 2012 by LinkedIn.

Main use cases:

- Connecting multiple sources of data
- Large scale data movement pipeline
- Big Data Integration

# Setup Environment

## Vanilla Installation

- Download kafka binary and unzip.
- Start the zookeeper
  `bin/zookeeper-server-start.sh config/zookeeper.properties`
- Now you will configure the broker server pointing to the zookeeper address in server.properties `zookeeper.connect=localhost:2181`
- Start the broker with this command: `bin/kafka-server-start.sh config/server.properties`
- If you want to run an another broker/ worker node, just create a new server.properties with unique id `broker.id=1` and now you are ready to run a server command `bin/kafka-server-start.sh config/server1.properties`

## Docker Installation

Easiest way to setup kafka environment is to use confluent docker images.

Just download this docker compose file and follow through this guide: <https://docs.confluent.io/current/quickstart/ce-docker-quickstart.html>

> This is the location where docker-compose file is stored for me /Users/codebased/Documents/Softwares/examples/cp-all-in-one
> Use the following commands
> `docker-compose up` or `docker-compose down`.

After the successful response, you will have the whole kafka environment running, including control center, broker, zookeeper, schema manager

![Kafka Architecture](https://www.learningjournal.guru/_resources/img/jpg-7x/kafka-enterprise-architecture.jpg)

>Pre-requisite
>
> Install Docker Compose <https://docs.docker.com/compose/install/>

## Important Configuration

The following configuration will have a direct impact on Kafka broker performance.

### message.max.bytes

The default value for your *compressed* message should be 1 MB. The larger your message size is, broker thread will be more busy with network connection, and request will be working longer on each request; larger message will also increase disk write operations.rome

Please note that the value in `message.max.bytes` must be coordinated with the `fetch.message.max.bytes` configuration on consumer clients. If the value is small in `fetch.message.meax.bytes` then consumer will not be able to fetch a larger messages. Thus the consumer will get stuck and will never be able to proceed further.

The same applies to `replica.fetch.max.bytes` configuration on the brokers when configured in a cluster.

### num.partitions

The number of partitions for your topic. By default it is set as 1. Please note that the number of partitions for a topic can only increase and never decrease. Generally speaking, number of partitions are equal to number of brokers in the cluster. It makes message load management much easier.

You can use this calculation to decide number of partitions:

1. Number of messages produced per seconds
2. Number of messages a consumer can process per second
3. Divide point 1 with point 2 and you will get the partition number

E.g. Producer can produce 100 records and your one consumer can process only 25 per second, then you need 100/25 = 4 partitions, so that you can also run four consumer to consume 100 messages per second.

### log.retention.ms

It is about how long Kafka will retain messages by time. The default value is set as 168 hours, which is 7 day.

### log.retention.bytes

Another way to expire your messages is based on the total number of bytes of messages retained. It applies at individual partitions, not the topic.

If you have specified `log.retention.ms` as well `log.retention.bytes` then whichever comes first will be used to expire messages.

### log.segment.bytes

Partitions are further divided into segments, which is like a small bucket of water in a pool. Once the log segment has reached the size, which is default to 1 GB, the log segment is closed and new one is opened.

Please note here that the once the log segment has been closed, it can be considered for expiration. If the segment bytes side is too small then the system has to perform expensive open and close operation. However, if it is not too small then it will not be fill and `log.retention.ms` value logic will not kick in.

For e.g, if you are producing 100 MB of data in a producer per day, and your segment size is 1 GB, then it will take 10 days to fill your segment. If your retention period is 7 days, then it will expire your messages in 17 days.

It will be right to say that the `log.retention.ms` is applied on log segment and not on individual message.

Another factor to consider while defining segment size is, when requesting offsets by timestamps.

### log.segment.ms

You can control the behavior of closing segment file using `log.segment.ms`. There is no default setting set for this property.

# Kafka Components

## Topic

It is a primary abstraction of Kafka. It is a category of messages, or a mailbox for messages addressed to a specific topic defined by a name.

Each topic is stored in a form of a log file or multiple log files based on number of partitions defined for that topic.

Characteristics

- Topic names are unique
- It must have at least one partition and one replication
- Messages are always append in the end
- Stored Messages cannot be changed in the topic
- Topics can be replicated between multiple brokers/ worker nodes
- Consumers for topics are responsible to maintain offset/ bookmark
- Topic data is stored in distributed commit log
- By default partition data for each topic is stored in /tmp/kafka-logs/{topic}-{partition} directory that can be configured through server.properties file of each broker.

### Topic Commands

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Create | ./kafka-topics.sh --create --topic {topic_name} --replication-factor {#replication-factor} --partitions {#partition} --zookeeper {host}:{port} | ./kafka-topics.sh --create --topic topic_name --replication-factor 1 --partitions 10 --zookeeper localhost:2181
| List | ./kafka-topics.sh --list --zookeeper {host}:{port} | ./kafka-topics.sh --list --zookeeper localhost:2181 |
| Delete | ./kafka-topics.sh --zookeeper {host}:{port} --delete --topic {topic_name} | ./kafka-topics.sh --zookeeper localhost:2181 --delete --topic topic_name |
| Describe | ./kafka-topics.sh --describe --zookeeper {host}:{port} --topic {topic_name} | ./kafka-topics.sh --describe --zookeeper localhost:2181 --topic test |
| Perge |  bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic {topic_name} --config retention.ms=1000 bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic topic_name} --delete-config retention.ms | `bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic mytopic --config retention.ms=1000` Once the retention value is changed, now delete the configuration to bring back default retention configuration i.e. 7 days. `bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic mytopic --delete-config retention.ms` |

### Partitioning

In order to get better performance on reading data, partitioning topics queue into a various brokers can be very useful.

These are the following Trade-offs of partitions.

- The more partitions the greater the zookeeper overhead
  - With large partition numbers ensure proper ZK capacity
- Message ordering can become complex
  - One partition for global ordering that means horizontal scaling through multiple partitions may not be achievable
  - Consumer-handling for ordering
- The more partitions the longer the leader fail-over time

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Partition | ./bin/kafka-topics.sh --zookeeper {server}:{port} --alter --topic {topic_name} --partitions {partition_number} | ./bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic accounts_service --partitions 3 |

### Message Retention Policy

Even though I have been using messages to represent a data transferred through topics, in kafka terms it is called records. It is also consumed through ProducerRecord and ConsumerRecord classes.

Kafka messages are immutable that means once it is written you cannot change. Apache Kafka retains all published message regardless of consumption.

The retention period is configurable however the default is set as 168 hours or seven days. The retention period is defined on a per -topic basis.

Of course it will be within the restriction of the storage size.

You can use this command to set the retention policy on topic.

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Record/Message Retention | bin/kafka-topics.sh --zookeeper {server}:{port} --alter --topic {topic} --config retention.ms={duration} | bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic mytopic --config retention.ms=28800000- |

## Producer

In order to produce messages for kafka, you need to provide at least three properties that are mandatory to connect with the Kafka.

- `bootstrap.servers` - it is a comma separated list of broker servers, in the format of {servername/ip}:{port}, which is used to retrieve the information about the entire cluster membership: partition leaders, etc.

- `key.serializer` - Class used for message key serialization. By defining key serializer you are defining stronly type for the key that is expected.

- `value.serializer` - Class used for message value serialization. By defining value serializer you are defining strongly type for the value that is expected.

```java
props.put("bootstrap.servers", "localhost:9092,localhost:9093");
props.put("key.serializer", "org.apache.kafka.common.serialization.StringSerializer");
props.put("value.serializer", "org.apache.kafka.common.serialization.StringSerializer");
```

These are the three mandatory properties and for more optional settings, please refer [this](http://kafka.apache.org/documentation.html#producerconfigs) link.

`ProducerRecord` - Message is presented through ProducerRecord type object defined in `org.apache.kafka` package. 

```java
new ProducerRecord<String, String>("my_topic", "my message");
```

> You need to have org.apache.kafka java package.

Mandatory Properties

- Topic Name
- Message - You must specify the topic name and the message value that you would want to send through.

Optionals

- Partition - Specific partition with the topic to send ProducerRecord
- Timestamp - The unix timestamp applied to the record.
- Key - A value to be used as  basis of determining the partition strategy to be employed by the kafka producer.
  - It has a very useful purpose to route your message to a specific partition.

`KafkaProducer` - Now you need a KafkaProducer instance that can send this ProducerRecord type object.

```java
KafkaProducer producer = new KafkaProducer(props);
producer.send(producerRecord)l;
producer.close();
```

When send command is thrown:

- Serializer
- Partitioner - These are the possible strategies
  - Direct - has a valid partition otherwise throw an exception.
  - round-robin - No partition and has no key defined in the producerrecord then it goes through round-robin. If it has a key, and no custom strategy defined in producer config then use key mod-hash (defaultpartitioner)
  - key mode hash
  - custom

### Delivery Guarantees

Kafka provides three different types of guarantee settings that can be set at topic level.

KAFKA AT-MOST-ONCE

Read the message and save its offset position before it possibly processes the message record in entirety; i.e. save to data lake. In case of failure, the consumer recovery process will resume from the previously saved offset position which is further beyond the last record saved to the data lake. This example demonstrates at-most-once semantics.  Data loss at data lake, for example, is possible.

KAFKA AT-LEAST-ONCE

The consumer can also read, process the messages and save to data lake, and afterward save its offset position.  In a failure scenario which occurs after the data lake storage and offset location update, a Kafka consumer addressing the failed process could duplicate data. This forms the “at-least-once” semantics in case of consumer failure.  Duplicates in the data lake could happen.

KAFKA EXACTLY ONCE

When it comes to exactly once semantics with Kafka and external systems, the restriction is not necessarily a feature of the messaging system, but the necessity for coordination of the consumer’s position with what is stored in the destination.  For example, a destination might be in an HDFS or object store based data lake. One of the most common ways to do this is by introducing a two-phase commit between the consumer output’s storage and the consumer position’s storage.  Or in other words, taking control of offset location commits in relation to data lake storage on a per record basis.

By default, Kafka provides at-least-once delivery guarantees.

| AT-MOST-ONCE | AT-LEAST-ONCE | EXACTLY ONCE |
|------------- |---------------|--------------|
|Message is sent at least once | Messages are pulled once or more time and processed every time |  Message pulled one or more times and processed only once |
| The ack may or may not have received by the broker | Receipt Guaranteed| Receipt Guaranteed| 
| No duplicate message is possible | Likely Duplicate message | No duplicate message |
| Possibility of missing data | No Missing data | No missing data|

### Kafka Producer Consumer Console

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Produce | `$ kafka-console-producer.sh --broker-list {broker server list} --topic {topic name}` | `$ kafka-console-producer.sh --broker-list localhost:9092 --topic test` |
| Consumer | `kafka-console-consumer.sh --bootstrap-server {serverip:port} --topic {topic name} --from-beginning` (new api) | `kafka-console-consumer.sh     --bootstrap-server localhost:9092     --topic test     --from-beginning`  |

### Fault-tolerance

- Broker failure
  
    If the broker is a leader, then another leader will be selected by the zookeeper. That is why zookeeper keeps a check of the health of each broker. But to remove any possibilities of data loss, it is important that multiple replication is set so that another broker has got the partition data that failed broker used to have.

    The replication is set through `--replication-factor` configuration property set against each topic. By redundancy factory you can achieve:

  - Redundancy of messages
  - Cluster resiliency
  - Fault-tolerance
  - N-1 broker failure tolerance
  
- Network failure
- Disk failure

To check the current replication factor of a topic:

`./bin/kafka-topics.sh --describe --zookeeper localhost:2181 --topic accounts_service`

It will show IRS - in-sync replica. When it is equal to number of replication factor than it is considered that replication is in a healthy state.

## Consumer

Just like producer, consumer also need three mandatory properties.

- `bootstrap.servers` - it is a comma separated list of broker servers, in the format of {servername/ip}:{port}, which is used to retrieve the information about the entire cluster membership: partition leaders, etc.

- `key.deserializer` - Class used for message key deserialization. By defining key deserializer you are defining strongly type for the key that is expected.

- `value.deserializer` - Class used for message value deserialization. By defining value deserializer you are defining strongly type for the value that is expected.

```java
props.put("bootstrap.servers", "localhost:9092, localhost:9093");
props.put("key.deserializer","org.apache.kafka.common.serialization.StringDeserializer");
props.put("value.deserializer","org.apache.kafka.common.serialization.StringDeserializer");
```

You can consume topics through two ways:

- Subscribe

  Consumers can subscribe to a topic or multiple topics using subscribe(...). You don't have to worry about each partition where the data will come from. If there is any new partition, the cluster metadata will be changed, thus the subscriber will get to know automatically and will get data from that new partition as well; it works like auto-administrating mode.

    ```java
    KafkaConsumer consumer = new KafkaConsumer(props);

    ArrayList<String> topic = new ArrayList<String>();
    topic.add("accounts_service");
    consumer.subscribe(topic);
    ```

- Assign

  Consumers can subscribe to a specific topic partition using assign(...). In this version of subscription, the consumer will tell which partition it would want to listen and consume messages from. Thus, it will not read messages coming to some specific partition; it works like self-administrating mode.

  ```java
  TopicPartition topicPartition = new TopicPartition("accounts_service",0);
  consumer.assign(Arrays.asList(topicPartition));
  ```

### The Poll Loop

By invoking poll method, the consumer will continuously polling the brokers for data. You will pass an integer, represent timeout milliseconds. It will call the bootstrap server to fetch metadata. It is a timeout setting for how long the consumer fetcher will take time to retrieve one or more consumer messages.

The poll() process is a single-threaded operation.

### The Offset

It is a placeholder/ or a bookmark that is maintained by the Kafka Consumer. It represents the last read message position in the form of message id. The consumer can decide from where it wants to read, from the beginning, the last or from the middle of a topic message queue.

Offset Types:

- Last committed offset - the last record that the consumer has confirmed that it has processed. For each partition, there will be a separate offset.
- Current position offset - As the consumer reads message, it moves the counter.
- Log-end offset - last index of a message in that partition.
- Un-committed offsets - As the consumer advances, the difference between last committed offset and customer position offset is called un-committed offsets.

Important Configuration properties

- enable.auto.commit(true) - it uses auto.commit.interval.ms (5000) power to commit the last committed offset flag automatically.
- auto.commit.interval.ms(5000ms) - It is defined in ms. Please note that even if the process takes longer than 5000 ms, it will commit the record even when it is not processed by the system. What happened if the process is failed and "last committed offset" has already moved on?
- auto.offset.reset(latest) - It can be latest, earliest, none value.

Kafka stores offsets in __consumer_offsets topic. It has 0 to 49 (50) partitions.Consumer cordinator is responsible to manage __consumer_offsets topic.

Log end offset - The last record offset in the partition.

Manual Offset management:

By setting enable.auto.commit to false will make offset management manually.

- commitSync()
- commitAsync()account

Why you want to take a offset management control?

- You will use manual offset management for higher consistency control.
- Atomicity - Exactly once vs At least once

Common configuration

- Consumer performance and efficiency
  - fetch.min.bytes
  - max.fetch.wait.ms
  - max.partition.fetch.bytes
  - max.poll.records
kink

#### Offset commands

| What | Example |
|---------------|--------------|
| Get a list partitions for offset topics | `bin/kafka-topics.sh --zookeeper localhost:2181 --describe --topic __consumer_offsets` |
|Get the earliest offset still in a topic| `bin/kafka-run-class.sh kafka.tools.GetOffsetShell --broker-list localhost:9092 --topic mytopic --time -2`|
|Get the latest offset still in a topic|`bin/kafka-run-class.sh kafka.tools.GetOffsetShell --broker-list localhost:9092 --topic mytopic --time -1`|
|Get the consumer offsets for a topic|`bin/kafka-consumer-offset-checker.sh --zookeeper=localhost:2181 --topic=mytopic --group=my_consumer_group`|

### Internal __consumer_offsets

### Read from __consumer_offsets

Add the following property to

`config/consumer.properties`:
`exclude.internal.topics=false`

`bin/kafka-console-consumer.sh --consumer.config config/consumer.properties --from-beginning --topic __consumer_offsets --zookeeper localhost:2181 --formatter "kafka.coordinator.GroupMetadataManager\$OffsetsMessageFormatter"`

### Consumer Group

It is used to bring parallelism in managing and processing events.
Consumer can subscribe to a group using "group.id" property.
You can create number of consumer equal or more than partitions. If you have more consumer than partitions, the consumer will seat idle.

#### Commands

| Type   | Command      |     Example    |  
|--------|--------------|----------------|
| List the consumer groups known to Kafka | `bin/kafka-consumer-groups.sh --zookeeper localhost:2181 --list`  (old api) `bin/kafka-consumer-groups.sh --new-consumer --bootstrap-server localhost:9092 --list` (new api) | |

## Zookeeper

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Shell | bin/zookeeper-shell.sh {zookeeper}:{port} | bin/zookeeper-shell.sh localhost:2181 |
| List Brokers | ./bin/zookeeper-shell.sh {server:port} ls /brokers/ids | ./bin/zookeeper-shell.sh localhost:2181 ls /brokers/ids
| asdf | asdf | asdf

Challenges

Governance and Evolution - How do we control what is allowed and wha is not when sending data through Kafka.

In order to control this, schema registry has been introduced by the company called confluent. It provides schema registry that maintain the schema being adapted by specific topic.

Consistency and Productivity - There is an integration cost involved with developing a connect when transferring data from one place to another. In order to solve this Kafka Connect has emerged.

Fast Data - Kafka Stream based process has been introduced to stream data into a different environment such as Kafka Spark, Hadoop in real time.

## Confluent Schema Registry

It is a versioned schema registry for Apache Avro, which is used to serialize and deserilize complex data formats when interacting with kafka.
Schema registry becomes a central point to store these schemas, in which Kafka producers and consumers interact to know the message template. The producer can specify the complex format for the data that it wants to publish, and consumer will then consult schema registry to understand the message format and read appropriately.

Schemas can be applied on keys, and values in the message.

`Think of these schemas as WSDL in web service environment (WSDL is an XML document that describes a web service methods, request message schema and response message schema).`

![Confluent Schema Registry Block Diagram](https://github.com/codebased/learning/blob/master/ReadMe/kafka/drawio/Confluent%20Schema%20Registry.png?raw=true)

Schema compatibility checking is by default set as "Backward". For more details on different type of compatibility types, please see [this](https://docs.confluent.io/current/schema-registry/avro.html) and [this](https://docs.confluent.io/current/schema-registry/schema_registry_tutorial.html#schema-evolution-and-compatibility)

`You can interact with the registry api using http://localhost:8081`.

### Confluent Schema Registry UI

You can interact with schema registry, using [this](https://github.com/lensesio/schema-registry-ui) open source UI.

`docker run --rm -p 8000:8000 -e "SCHEMAREGISTRY_URL=http://192.168.1.107:8081"  -e "PROXY=true" landoop/schema-registry-ui`

> Known issue with [502 Bad Gateway](https://github.com/lensesio/schema-registry-ui/issues/64)

PS: This UI uses the confluent REST Proxy APIs.

## Confluent REST Proxy

![Confluent REST Proxy](https://github.com/codebased/learning/blob/master/ReadMe/kafka/drawio/Confluent%20REST%20Proxy.png?raw=true)

## KSQL

KSQL is a stream on kafka topic with the schema.

You can create a shce

CREATE STREAM MQTT_RAW
    (TID  VARCHAR, BATT INTEGER, LON       DOUBLE,  LAT  DOUBLE,
     TST  BIGINT,  ALT  INTEGER, COG       INTEGER, VEL  INTEGER,
     P    DOUBLE,  BS   INTEGER, CONN      VARCHAR, ACC  INTEGER,
     T    VARCHAR, VAC  INTEGER, INREGIONS VARCHAR, TYPE VARCHAR)
WITH (KAFKA_TOPIC = 'data_mqtt', VALUE_FORMAT='JSON');

# REFERENCES

## Confluent Schema

[Schema Management](https://docs.confluent.io/current/schema-registry/index.html)

[Schema Registry Tutorial](https://docs.confluent.io/current/schema-registry/schema_registry_tutorial.html)

# Future Topics

[SAGA Architecture](https://stackoverflow.com/questions/43845183/implementing-sagas-with-kafka)

[Compaction Log Architecture](http://cloudurable.com/blog/kafka-architecture-log-compaction/index.html)

[Multi Cluster replication](https://docs.confluent.io/current/multi-dc-replicator/index.html)

[MirrorMaker](https://cwiki.apache.org/confluence/pages/viewpage.action?pageId=27846330)
