<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Distributed Design](#distributed-design)
  - [Single Node Patterns](#single-node-patterns)
    - [The Sidecar Pattern](#the-sidecar-pattern)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Distributed Design

## Single Node Patterns

### The Sidecar Pattern

It is made up of two containers.  The first is the application container, which contains the core logic for the application.
There is a sidecar container, which is responsible to augment and improve the application container, without the application container knowlege.


Event sourcing is about how your store data. The record will be stored with the history. For e.g. if the user has changed quantity in his shopping cart item, it should store previous as well as a new row with new quantity. Idea is you retain your history, for every change as immuntable event. 

When two processes are listing the same event, one for raw data input, and another one for aggregating the data for future read. It leads to a CQRS - Command-Query responsibility segregation.

The beauty of going over docuemnt db is because you can separate things between source of truth (SOT) (mainly your database) and caches is that in your cche, you can denormalize data as your please, which makes reading very fast. You can rebuild this cache by feeding data from SOT.But how to keep it updated when SOT is updated? The solution is you need some form of events that is generated on change and your cache (docuemnt db) will update the same accordingly.

The whole of writing data into the database (SOR) and reading data differently has been present since you have been using indexes.

What is the benefit of this immuntable stream/log entries?

- Loose coupling
  - meaning that your write schema is not same as your read schema. Your write may be just a like in a facebook, but for read it stores additional details such as name, location, time etc.. 
- read and write performance
- scalability 
- flexibility 
- auditability/error recovery


