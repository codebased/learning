<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Hello](#hello)
  - [Definitions](#definitions)
    - [Concurrency](#concurrency)
    - [Parallelism](#parallelism)
    - [Critical Section](#critical-section)
    - [Multithread Synchronisation for Critical Section](#multithread-synchronisation-for-critical-section)
      - [Semaphore](#semaphore)
      - [CountDownLatch](#countdownlatch)
      - [CyclicBarrier](#cyclicbarrier)
      - [Phaser](#phaser)
      - [Exchanger](#exchanger)
  - [Manage Threads](#manage-threads)
    - [newCachedThreadPool](#newcachedthreadpool)
    - [newFixedThreadPool](#newfixedthreadpool)
    - [newSingleThreadExecutor](#newsinglethreadexecutor)
    - [Important Executor methods](#important-executor-methods)
      - [InvokeAny](#invokeany)
    - [invokeAll](#invokeall)
    - [Future.cancel](#futurecancel)
      - [ScheduledThreadPoolExecutor](#scheduledthreadpoolexecutor)
  - [FutureTask](#futuretask)
  - [Fork/ Join](#fork-join)
    - [ForkJoinPool](#forkjoinpool)
    - [ForkJoinTask](#forkjointask)
  - [Concurrent Collections](#concurrent-collections)
    - [ConcurrentLinkedDeque](#concurrentlinkeddeque)
    - [LinkedBlockingDeque](#linkedblockingdeque)
    - [LinkedTransferQueue](#linkedtransferqueue)
    - [PriorityBlockingQueue](#priorityblockingqueue)
    - [DelayQueue](#delayqueue)
    - [ConcurrentSkipListMap](#concurrentskiplistmap)
    - [ThreadLocalRandom](#threadlocalrandom)
    - [AtomicIntegerArray](#atomicintegerarray)
  - [Liveness](#liveness)
  - [ThreadLocalRandom](#threadlocalrandom-1)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Hello

## Definitions

### Concurrency

Means a serious of taks that run simultanelusly in a computer. It can be on a single process or a multi-core processor.

### Parallelism

When you execute your application with multiple threads in a multi-core processor or in the computer with more than one processor.

All parellelism is concurrency but not all concurrency is parellelism (in case of single core systems)

### Critical Section

This is the term that is highly used in concurrency where the code can bring incosistency in shared data between different threads.

### Multithread Synchronisation for Critical Section

The following different java data structure can help you to achieve multithread.

#### Semaphore

A semaphore is a counter that protects the access to one or more shared resources.

Binary Semaphores where it can take values of only 0 or 1

#### CountDownLatch

The CountDownLatch class is a mechanism provided by the Java
language that allows a thread to wait for the finalization of multiple operations.

#### CyclicBarrier

The CyclicBarrier class is another mechanism provided by the Java language that allows the synchronization of multiple threads in a common point.

#### Phaser

The Phaser class is another mechanism provided by the Java language that controls the execution of concurrent tasks divided in phases. All the threads must finish one phase before they can continue with the next one. This is a new feature of the Java 7 API.

#### Exchanger

The Exchanger class is another mechanism provided by the Java language that provides a point of data interchange between two threads.

## Manage Threads

While we have studied that in order to run a runnable, you need to create a thread. If we have 10 items to run for parallism, we have to create 10 threads.

But comes with the argument that you will have to manage these threads. If you create too many threads, you may reach system limitations.

In order to control this, ExecutorService, and the ThreadPoolExecutor class that implements Executor, as a part of Executor framework, will help to take care of managing Threads.

This mechanism separates the task creation and its execution. With an executor, you only
have to implement the Runnable objects and send them to the executor. It is responsible for
their execution, instantiation, and running with necessary threads. But it goes beyond that and
improves performance using a pool of threads. When you send a task to the executor, it tries to
use a pooled thread for the execution of this task, to avoid continuous spawning of threads.

### newCachedThreadPool

Use the executor created by the newCachedThreadPool() method
only when you have a reasonable number of threads or when they
have a short duration

### newFixedThreadPool

This executor has a maximum number of threads. If you send
more tasks than the number of threads, the executor won't create additional threads and
the remaining tasks will be blocked until the executor has a free thread. With this behavior,
you guarantee that the executor won't yield a poor performance of your application.

### newSingleThreadExecutor

It creates an executor with only one thread, so it can only execute one task at a time.

### Important Executor methods

#### InvokeAny

The invokeAny() method of the
ThreadPoolExecutor class receives a list of tasks, launches them, and returns the result
of the first task that finishes without throwing an exception. This method returns the same
data type that the call() method of the tasks you launch returns. In this case, it returns a
String value

### invokeAll

This method receives a list of the Callable objects and returns a list of the Future objects.

### Future.cancel

#### ScheduledThreadPoolExecutor

It schedules execution of a task after a given period of time.

## FutureTask

The FutureTask class provides a method called done() that allows you to execute some
code after the finalization of a task executed in an executor.

## Fork/ Join

The main difference between the Fork/Join and the Executor frameworks is the work-stealing
algorithm. Unlike the Executor framework, when a task is waiting for the finalization of the
subtasks it has created using the join operation, the thread that is executing that task (called
worker thread) looks for other tasks that have not been executed yet and begins its execution.
By this way, the threads take full advantage of their running time, thereby improving the
performance of the application.

### ForkJoinPool

It implements the core work-stealing algorithm and can execute `ForkJoinTask` process.

### ForkJoinTask

## Concurrent Collections

Basically, Java provides two kinds of collections to use in concurrent applications:

* Blocking collections: This kind of collection includes operations to add and remove
data. If the operation can't be made immediately, because the collection is full or
empty, the thread that makes the call will be blocked until the operation can
be made.

* Non-blocking collections: This kind of collection also includes operations to add and
remove data. If the operation can't be made immediately, the operation returns a
null value or throws an exception, but the thread that makes the call won't
be blocked.

### ConcurrentLinkedDeque

It is a non-blocking doubly linked list queue.
It has poll and remove to remove elements, but later throws an exception (NoSuchElementException), when the queue is empty.

### LinkedBlockingDeque

It is a blocking doubly linked list queue.

### LinkedTransferQueue

A blocking lists to be used with producers and consumers of data.

### PriorityBlockingQueue

A blocking lists that order their elements by priority.

### DelayQueue

Blocking lists with delayed elements, using the DelayQueue class

### ConcurrentSkipListMap

Non-blocking navigable maps, using the ConcurrentSkipListMap class

### ThreadLocalRandom

Random numbers, using the ThreadLocalRandom class

### AtomicIntegerArray

Atomic variables, using the AtomicLong and AtomicIntegerArray classes

You check for an interrupt by using `Thread.interrupt` but after checking the flag is reset. However, if you use nonstatic `isInterrupted` method, which is use by one thread to query the interrupt status of another, does not change the interrupt status flag.

`join` will make thread object to finish all executions and make the main thread to wait finish this task.

Without `join` the main will be executed while the program execution will finish after all threads are finished.
In other words `join` is just a way to block main execution and wait for all threads to finish before it move on to next statement in main thread.


`synchronized` lock vs `ReentrantLock` is that the later gives you full control to lock and unlock. You can even know the status. However, with the case of `synchornized` it has to be within the scope, and the object lock is released internally.

Atomic Access means either it is done or nothing happened at all. In order to achieve this use volatile.

## Liveness

A concurrent application ability to execute in a timely manner is known as its liveness. It brings two problems, starvation and livelock.

## ThreadLocalRandom 
Good to use random numbers from multiple threads or ForkJoinTasks.

