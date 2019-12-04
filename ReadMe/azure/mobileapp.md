<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Introduction](#introduction)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->


# Introduction

It is as same as app service. However, this APi can automatically generate API from SQL. You can also modify, such as adding notificaiton functionality.

```C#
// Initialize the client with the mobile app backend URL.
client = new MobileServiceClient(applicationURL);

var store = new MobileServiceSQLiteStore("localstore.db");
store.DefineTable<ToDoItem>();

// Uses the default conflict handler, which fails on conflict
// To use a different conflict handler, pass a parameter to InitializeAsync.
// For more details, see http://go.microsoft.com/fwlink/?LinkId=521416
await client.SyncContext.InitializeAsync(store).ContinueWith(_ =>
    {

        // Create an MSTable instance to allow us to work with the TodoItem table
        todoTable = client.GetSyncTable<ToDoItem>();
    });;

await todoTable.InsertAsync (todoItem); // Insert a new TodoItem into the local database.
// sync data back to the server.
await client.SyncContext.PushAsync();
// get anything that you have in the server.
if (pullData) {
    await todoTable.PullAsync("allTodoItems", todoTable.CreateQuery()); // query ID is used for incremental sync
}

```

References 

About Mobile Apps in Azure App Services

Create a Windows App

Enable offline sync for your Windows App
