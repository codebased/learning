
Create an identity of your app service, and use this identity to get security from key vault.

* You will create a keyvault and a secret,
* Create a service principal and grant Key vault secret get permission.
* Then access secret from c# using MSI.

```ps
$location = "westus"
$rgname = "kvsecrets"
$kvname = "laaz203kvsecrets"
$spname = "LaAz203WebAppSecrets"

az group create --n $rgname -l $location

# Create a keyvault
az keyvault create `
 -n $kvname `
 -g $rgname `
 --sku standard

# Create a secret inside of keyvault
az keyvault secret set `
 --vault-name $kvname `
 --name "connectionString" `
 --value "this is the connection string"

az keyvault secret show `
 --vault-name $kvname `
 --name connectionString

# run the app

$sp = az ad sp create-for-rbac --name $spname | ConvertFrom-Json
$sp

$tenantid = az account show --query tenantId -o tsv
az login --service-principal `
  --username $sp.appId `
  --password $sp.password `
  --tenant $tenantid

az keyvault secret show `
 --vault-name $kvname `
 --name connectionString

# run the app - no access to secret

az login #back to main account

# Create a policy for a newly created app service principal to get an access to the keyvault

az keyvault set-policy `
 --name $kvname `
 --spn $sp.Name `
 --secret-permissions get

az login --service-principal --username $sp.appId --password $sp.password --tenant $tenantid

az keyvault secret show `
 --vault-name $kvname `
 --name connectionString

# run the app

az ad sp delete --id $sp.appId
az keyvault delete --name $kvname

az group delete -n $rgname --yes

```

```C#
var azureServiceTokenProvider1 = new AzureServiceTokenProvider();

var kvc = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
    azureServiceTokenProvider1.KeyVaultTokenCallback));

var kvBaseUrl = "https://laaz203kvsecrets.vault.azure.net/";

var secret = await kvc.GetSecretAsync(
kvBaseUrl, "connectionString");
System.Console.WriteLine(secret.Value);

```