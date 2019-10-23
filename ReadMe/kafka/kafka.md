# KAFKA

Apache Kafka is publish subscribe messaging rethought as a distributed commit log.

Before Kafka:

Database Replication (log shipping)- RDBMS to RDBMS , Or Database specific, which is tightly coupled with the schema that is difficult to modify. Also, the impact of replication on overall system.
 
ETL (Extract, Transform, and Load) - most of the times are proprietary, or a custom application build and maintain by the company. It has scalability and performance drawbacks.

MSMQ - Limited functionality and scaling out was a biggest challenge with MSMQ that can only work with Microsoft technology. If the bug is there with the consumer, how to retrieve that message again? So it has many drawbacks such as consistency concerns, load handling,  Atomic transaction, that increase when you have complex system with big data set.

Why Kafka

It is based on messaging system, which is based on messaging broker that commits to deliver the message. It scales out over many servers known as brokers. It can replay messages when required.

* Designed for high throughput,  and lowest latency with distributed messaging (log) system. 
* It is used to move data at high volumes.
* It was open sourced under Apache Software Foundation in 2012 by LinkedIn.

Main use cases:

* Connecting multiple sources of data
* Large scale data movement pipeline
* Big Data Integration

## Setup Environment

### Vanilla Installation

* Download kafka binary and unzip.
* Start the zookeeper
  `bin/zookeeper-server-start.sh config/zookeeper.properties`
* Now you will configure the broker server pointing to the zookeeper address in server.properties `zookeeper.connect=localhost:2181`
* Start the broker with this command: `bin/kafka-server-start.sh config/server.properties`
* If you want to run an another broker/ worker node, just create a new server.properties with unique id `broker.id=1` and now you are ready to run a server command `bin/kafka-server-start.sh config/server1.properties`

### Docker Installation

Easiest way to setup kafka environment is to use docker images.

Just download this docker compose file and follow through this guide: <https://docs.confluent.io/current/quickstart/ce-docker-quickstart.html>

After the successful response, you will have the whole kafka environment running, including control center, broker, zookeeper, schema manager

![Kafka Architecture](https://www.learningjournal.guru/_resources/img/jpg-7x/kafka-enterprise-architecture.jpg)

>Pre-requisite
>
> Install Docker Compose <https://docs.docker.com/compose/install/>

## Architecture Components

### Topic

It is a primary abstraction of Kafka. It is a category of messages, or a mailbox for messages addressed to a specific topic defined by a name.

Each topic is stored in a form of a log file or multiple log files based on number of partitions defined for that topic.

Characteristics

* Topic names are unique
* It must have at least one partition and one replication
* Messages are always append in the end
* Stored Messages cannot be changed in the topic
* Topics can be replicated between multiple brokers/ worker nodes
* Consumers for topics are responsible to maintain offset/ bookmark
* Topic data is stored in distributed commit log
* By default partition data for each topic is stored in /tmp/kafka-logs/{topic}-{partition} directory that can be configured through server.properties file of each broker.

#### Topic Commands

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Create | ./kafka-topics.sh --create --topic {topic_name} --replication-factor {#replication-factor} --partitions {#partition} --zookeeper {host}:{port} | ./kafka-topics.sh --create --topic topic_name --replication-factor 1 --partitions 10 --zookeeper localhost:2181
| List | ./kafka-topics.sh --list --zookeeper {host}:{port} | ./kafka-topics.sh --list --zookeeper localhost:2181 |
| Delete | ./kafka-topics.sh --zookeeper {host}:{port} --delete --topic {topic_name} | ./kafka-topics.sh --zookeeper localhost:2181 --delete --topic topic_name |
| Describe | ./kafka-topics.sh --describe --zookeeper {host}:{port} --topic {topic_name} | ./kafka-topics.sh --describe --zookeeper localhost:2181 --topic test |
| Perge |  bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic {topic_name} --config retention.ms=1000 bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic topic_name} --delete-config retention.ms | `bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic mytopic --config retention.ms=1000` Once the retention value is changed, now delete the configuration to bring back default retention configuration i.e. 7 days. `bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic mytopic --delete-config retention.ms` |

#### Partitioning

In order to get better performance on reading data, partitioning topics queue into a various brokers can be very useful.

These are the following Trade-offs of partitions.

* The more partitions the greater the zookeeper overhead
  * With large partition numbers ensure proper ZK capacity
* Message ordering can become complex
  * One partition for global ordering that means horizontal scaling through multiple partitions may not be achievable
  * Consumer-handling for ordering
* The more partitions the longer the leader fail-over time

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Partition | ./bin/kafka-topics.sh --zookeeper {server}:{port} --alter --topic {topic_name} --partitions {partition_number} | ./bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic accounts_service --partitions 3 |

#### Message Retention Policy


Even though I have been using messages to represent a data transferred through topics, in kafka terms it is called records. It is also consumed through ProducerRecord and ConsumerRecord classes.

Kafka messages are immutable that means once it is written you cannot change. Apache Kafka retains all published message regardless of consumption.

The retention period is configurable however the default is set as 168 hours or seven days. The retention period is defined on a per -topic basis.

Of course it will be within the restriction of the storage size.

You can use this command to set the retention policy on topic.

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Record/Message Retention | bin/kafka-topics.sh --zookeeper {server}:{port} --alter --topic {topic} --config retention.ms={duration} | bin/kafka-topics.sh --zookeeper localhost:2181 --alter --topic mytopic --config retention.ms=28800000* |

## Kafka Producer Consumer Console

| Type   | Command      |     Example    |  
|--------|-------|----------------|
| Produce | `$ kafka-console-producer.sh --broker-list {broker server list} --topic {topic name}` | `$ kafka-console-producer.sh --broker-list localhost:9092 --topic test` |
| Consumer | `kafka-console-consumer.sh --bootstrap-server {serverip:port} --topic {topic name} --from-beginning` (new api) | `kafka-console-consumer.sh     --bootstrap-server localhost:9092     --topic test     --from-beginning`  |
| asdf | asdf | asdf |


## Concepts



### Commit Logs

It is a source of truth. It is stored in an order the events are happened. It is a point of recovery and basis for replication and redundancy.

### Partitions

Each topic has one or more partitions, which is entirely configurable at topic level.

Partitioning Strategy

## Message Buffer

### Producer

In order to produce messages for kafka, you need to provide at least three properties that are mandatory to connect with the Kafka.

* `bootstrap.servers` - it is a comma separated list of broker servers, in the format of {servername/ip}:{port}, which is used to retrieve the information about the entire cluster membership: partition leaders, etc.

* `key.serializer` - Class used for message key serialization. By defining key serializer you are defining stronly type for the key that is expected.

* `value.serializer` - Class used for message value serialization. By defining value serializer you are defining strongly type for the value that is expected.

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

* Topic Name
* Message - You must specify the topic name and the message value that you would want to send through.

Optionals

* Partition - Specific partition with the topic to send ProducerRecord
* Timestamp - The unix timestamp applied to the record.
* Key - A value to be used as  basis of determining the partition strategy to be employed by the kafka producer.
  * It has a very useful purpose to route your message to a specific partition.

`KafkaProducer` - Now you need a KafkaProducer instance that can send this ProducerRecord type object.

```java
KafkaProducer producer = new KafkaProducer(props);
producer.send(producerRecord)l;
producer.close();
```

When send command is thrown: 

* Serializer
* Partitioner - These are the possible strategies
  * Direct - has a valid partition otherwise throw an exception.
  * round-robin - No partition and has no key defined in the producerrecord then it goes through round-robin. If it has a key, and no custom strategy defined in producer config then use key mod-hash (defaultpartitioner)
  * key mode hash
  * custom

#### Delivery Guarantees

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



Fault-tolerance

* Broker failure
  
    If the broker is a leader, then another leader will be selected by the zookeeper. That is why zookeeper keeps a check of the health of each broker. But to remove any possibilities of data loss, it is important that multiple replication is set so that another broker has got the partition data that failed broker used to have.

    The replication is set through `--replication-factor` configuration property set against each topic. By redundancy factory you can achieve:

  * Redundancy of messages
  * Cluster resiliency
  * Fault-tolerance
  * N-1 broker failure tolerance
  
* Network failure
* Disk failure

To check the current replication factor of a topic:

`./bin/kafka-topics.sh --describe --zookeeper localhost:2181 --topic accounts_service`

It will show IRS - in-sync replica. When it is equal to number of replication factor than it is considered that replication is in a healthy state.

### Consumer

Just like producer, consumer also need three mandatory properties.

* `bootstrap.servers` - it is a comma separated list of broker servers, in the format of {servername/ip}:{port}, which is used to retrieve the information about the entire cluster membership: partition leaders, etc.

* `key.deserializer` - Class used for message key deserialization. By defining key deserializer you are defining stronly type for the key that is expected.

* `value.deserializer` - Class used for message value deserialization. By defining value deserializer you are defining strongly type for the value that is expected.

```java
props.put("bootstrap.servers", "localhost:9092, localhost:9093");
        props.put("key.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
        props.put("value.deserializer", "org.apache.kafka.common.serialization.StringDeserializer");
```

You can consume topics through two ways:

* Subscribe

  Consumers can subscribe to a topic or multiple topics using subscribe(...). You don't have to worry about each partition where the data will come from. If there is any new partition, the subscriber will get to know automatically and will get data from that partition as well; it works like auto-administrating mode.

    ```java
    KafkaConsumer consumer = new KafkaConsumer(props);

    ArrayList<String> topic = new ArrayList<String>();
    topic.add("accounts_service");
    consumer.subscribe(topic);
    ```

* Assign
  
  Consumers can subscribe to a specific topic partition using assign(...). In this version of subscription, the consumer will tell which partition it would want to listen and consume messages from. Thus, it will not read messages coming to some specific partition; it works like self-administrating mode.


#### The Offset

It is a placeholder/ or a bookmark that is maintained by the Kafka Consumer. It represents the last read message position in the form of message id. The consumer can decide from where it wants to read, from the beginning, the last or from the middle of a topic message queue.

##### Offset commands

| What | Example |
|---------------|--------------|
|Get the earliest offset still in a topic| `bin/kafka-run-class.sh kafka.tools.GetOffsetShell --broker-list localhost:9092 --topic mytopic --time -2`|
|Get the latest offset still in a topic|`bin/kafka-run-class.sh kafka.tools.GetOffsetShell --broker-list localhost:9092 --topic mytopic --time -1`|
|Get the consumer offsets for a topic|`bin/kafka-consumer-offset-checker.sh --zookeeper=localhost:2181 --topic=mytopic --group=my_consumer_group`|

#### Internal __consumer_offsets

#### Read from __consumer_offsets

Add the following property to

`config/consumer.properties`:
`exclude.internal.topics=false`

`bin/kafka-console-consumer.sh --consumer.config config/consumer.properties --from-beginning --topic __consumer_offsets --zookeeper localhost:2181 --formatter "kafka.coordinator.GroupMetadataManager\$OffsetsMessageFormatter"`

## Kafka Consumer Groups

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
