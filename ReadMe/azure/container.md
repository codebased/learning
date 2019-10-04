# Containers

## Docker file

FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

* Copy csproj and restore as distinct layers

COPY webapp/*.csproj ./
RUN dotnet restore

* Copy everything else and build

COPY ./webapp ./
RUN dotnet publish -c Release -o out

* Build runtime image

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "webapp.dll"]

## Azure Kubernetes Services (AKS)

Create resource group > Create aks cluster > Get credentials > Get nodes > Apply yaml which will deploy the container

declare rg
rg="aksdemos"
declare cluster
cluster="akscluster"

az group create -n $rg -l westus

`Create a cluster with one node and enable monitoring`
az aks create -g $rg -n $cluster --node-count 1 --generate-ssh-keys --enable-addons monitoring

`It is a kind of login into the cluster and after that you can use kubectrl to control your cluster and nodes`
az aks get-credentials -g $rg -n $cluster

`It will return number of nodes in the cluster`
kubectl get nodes

`It will deploy everything into azure vote`
kubectl apply -f azure-vote.yaml

`It will get the service and the external ip address`
kubectl get service azure-vote-front --watch

`delete the aks cluster and resource group `
az aks delete --name $cluster --resource-gro $rg
