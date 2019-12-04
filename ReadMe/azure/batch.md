<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Batch](#batch)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Batch

If you have a program that performs extensive operation as a scheduler or on demand, you can use this feature called Azure Batch.

Here you will create a batch account first, and then pool of nodes. You will then create a job for that pool. This job will contains one or more tasks that can run any executable file.

So the flow is:

resource group > storage account > batch account > pool > job > task

`You can create a file .azcli in visual studio code and it will give you an intellisense feature by using an extension called Azure CLI in Visual Studio Code.`

`Alternatively, you can run az interactive command in shell script.`

<https://jussiroine.com/2019/03/mastering-azure-cli/>

``` sh

$rgName = "batch"
$stgAcctName = "laaz203batchsa"
$location = "westus"
$batchAcctName = "laaz203batchacct"
$poolName = "myPool"
```

``` sh

az group create `
 -l $location `
 -n $rgName
```

``` sh

az storage account create `
 -g $rgName `
 -n $stgAcctName `
 -l $location `
 --sku Standard_LRS
```

``` sh

az batch account create `
 -n $batchAcctName `
 --storage-account $stgAcctName `
 -g $rgName `
 -l $location
```

``` sh

az batch account login `
 -n $batchAcctName `
 -g $rgName `
 --shared-key-auth
```

``` sh

az batch pool create `
 --id $poolName `
 --vm-size Standard_A1_v2 `
 --target-dedicated-nodes 2 `
 --image `
   canonical:ubuntuserver:16.04-LTS `
 --node-agent-sku-id `
   "batch.node.ubuntu 16.04"
```

``` sh

az batch pool show `
 --pool-id $poolName `
 --query "allocationState"
```

```sh

az batch job create `
 --id myjob `
 --pool-id $poolName

```

``` sh
for ($i=0; $i -lt 4; $i++) {
    az batch task create `
     --task-id mytask$i `
     --job-id myjob `
     --command-line "/bin/bash -c 'printenv | grep AZ_BATCH; sleep 90s'"
}
```

``` sh
az batch task show `
 --job-id myjob `
 --task-id mytask1
```

``` sh
az batch task file list `
 --job-id myjob `
 --task-id mytask1 `
 --output table
```

``` sh
az batch task file download `
 --job-id myjob `
 --task-id mytask0 `
 --file-path stdout.txt `
 --destination ./stdout0.txt
```

``` sh
az batch pool delete -n $poolName
```

``` sh
az group delete -n $rgName
```

REFERENCES:

<https://docs.microsoft.com/en-us/cli/azure/reference-index?view=azure-cli-latest>