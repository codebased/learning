<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
**Table of Contents**  *generated with [DocToc](https://github.com/thlorenz/doctoc)*

- [Introduction](#introduction)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

# Introduction

It is a fully managed search functionality that can be integrated in your website like elastic search.

* Connect to the search service
* Create an index
* Upload documents
* Perform searches on the corpus

```ps
$rg = "search"
$location = "westus"
$serviceName = "laaz203search"

az group create -n $rg -l $location

az search service create `
 --name $serviceName `
 -g $rg `
 --sku free

az search admin-key show `
 --service-name $serviceName `
 -g $rg `
 --query "primaryKey"
 
az search query-key list `
 --service-name $serviceName `
 -g $rg `
 --query "[0].key"

az group delete -n $rg --yes

```

```C#

var searchServiceName = configuration["SearchServiceName"];
var adminApiKey = configuration["SearchServiceAdminApiKey"];
var queryApiKey = configuration["SearchServiceQueryApiKey"];

// get management client
var serviceClient = new SearchServiceClient(
    searchServiceName, 
    new SearchCredentials(adminApiKey));

// Create index...
var definition = new Index()
{
    Name = "hotels",
    Fields = FieldBuilder.BuildForType<Hotel>()
};

serviceClient.Indexes.Create(definition);

var indexClientForUpload = 
    serviceClient.Indexes.GetClient("hotels");

// Upload documents...
var hotels = new Hotel[]
{
    new Hotel()
    { 
        HotelId = "1", 
        BaseRate = 199.0, 
        Description = "Best hotel in town",
        DescriptionFr = "Meilleur hôtel en ville",
        HotelName = "Fancy Stay",
        Category = "Luxury", 
        Tags = new[] { "pool", "view", "wifi", "concierge" },
        ParkingIncluded = false, 
        SmokingAllowed = false,
        LastRenovationDate = new DateTimeOffset(2010, 6, 27, 0, 0, 0, TimeSpan.Zero), 
        Rating = 5, 
        Location = GeographyPoint.Create(47.678581, -122.131577)
    },
    new Hotel()
    { 
        HotelId = "2", 
        BaseRate = 79.99,
        Description = "Cheapest hotel in town",
        DescriptionFr = "Hôtel le moins cher en ville",
        HotelName = "Roach Motel",
        Category = "Budget",
        Tags = new[] { "motel", "budget" },
        ParkingIncluded = true,
        SmokingAllowed = true,
        LastRenovationDate = new DateTimeOffset(1982, 4, 28, 0, 0, 0, TimeSpan.Zero),
        Rating = 1,
        Location = GeographyPoint.Create(49.678581, -122.131577)
    },
    new Hotel() 
    { 
        HotelId = "3", 
        BaseRate = 129.99,
        Description = "Close to town hall and the river"
    }
};

var batch = IndexBatch.Upload(hotels);
indexClientForUpload.Documents.Index(batch);

var indexClientForQuery = new SearchIndexClient(
    searchServiceName, 
    "hotels", 
    new SearchCredentials(queryApiKey));

// Search the entire index for the term 'budget' and return only the hotelName field
var parameters = new SearchParameters()
{
    Select = new[] { "hotelName" }
};

var results = indexClientForQuery.Documents
                                    .Search<Hotel>("budget", 
                                                parameters);
WriteDocuments(results);

// Apply a filter to the index to find hotels cheaper than $150 per night,
// and return the hotelId and description
parameters = new SearchParameters()
{
    Filter = "baseRate lt 150",
    Select = new[] { "hotelId", "description" }
};

results = indexClientForQuery.Documents
                                .Search<Hotel>("*", 
                                            parameters);
WriteDocuments(results);

// Search the entire index, order by a specific field (lastRenovationDate
// in descending order, take the top two results, and show only hotelName and lastRenovationDate
parameters = new SearchParameters()
{
    OrderBy = new[] { "lastRenovationDate desc" },
    Select = new[] { "hotelName", "lastRenovationDate" },
    Top = 2
};

results = indexClientForQuery.Documents
                                .Search<Hotel>("*", 
                                            parameters);
WriteDocuments(results);

// Search the entire index for the term 'motel'
parameters = new SearchParameters();
results = indexClientForQuery.Documents.Search<Hotel>("motel", parameters);
WriteDocuments(results);
}

private static void WriteDocuments(DocumentSearchResult<Hotel> searchResults)
{
foreach (SearchResult<Hotel> result in searchResults.Results)
{
    Console.WriteLine(result.Document);
}

Console.WriteLine();
}

```

```C#
[SerializePropertyNamesAsCamelCase]
public partial class Hotel
{
    [System.ComponentModel.DataAnnotations.Key]
    [IsFilterable]
    public string HotelId { get; set; }

    [IsFilterable, IsSortable, IsFacetable]
    public double? BaseRate { get; set; }

    [IsSearchable]
    public string Description { get; set; }

    [IsSearchable]
    [Analyzer(AnalyzerName.AsString.FrLucene)]
    [JsonProperty("description_fr")]
    public string DescriptionFr { get; set; }

    [IsSearchable, IsFilterable, IsSortable]
    public string HotelName { get; set; }

    [IsSearchable, IsFilterable, IsSortable, IsFacetable]
    public string Category { get; set; }

    [IsSearchable, IsFilterable, IsFacetable]
    public string[] Tags { get; set; }

    [IsFilterable, IsFacetable]
    public bool? ParkingIncluded { get; set; }

    [IsFilterable, IsFacetable]
    public bool? SmokingAllowed { get; set; }

    [IsFilterable, IsSortable, IsFacetable]
    public DateTimeOffset? LastRenovationDate { get; set; }

    [IsFilterable, IsSortable, IsFacetable]
    public int? Rating { get; set; }

    [IsFilterable, IsSortable]
    public GeographyPoint Location { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();

        if (!String.IsNullOrEmpty(HotelId))
        {
            builder.AppendFormat("ID: {0}\t", HotelId);
        }

        if (BaseRate.HasValue)
        {
            builder.AppendFormat("Base rate: {0}\t", BaseRate);
        }

        if (!String.IsNullOrEmpty(Description))
        {
            builder.AppendFormat("Description: {0}\t", Description);
        }

        if (!String.IsNullOrEmpty(DescriptionFr))
        {
            builder.AppendFormat("Description (French): {0}\t", DescriptionFr);
        }

        if (!String.IsNullOrEmpty(HotelName))
        {
            builder.AppendFormat("Name: {0}\t", HotelName);
        }

        if (!String.IsNullOrEmpty(Category))
        {
            builder.AppendFormat("Category: {0}\t", Category);
        }

        if (Tags != null && Tags.Length > 0)
        {
            builder.AppendFormat("Tags: [{0}]\t", String.Join(", ", Tags));
        }

        if (ParkingIncluded.HasValue)
        {
            builder.AppendFormat("Parking included: {0}\t", ParkingIncluded.Value ? "yes" : "no");
        }

        if (SmokingAllowed.HasValue)
        {
            builder.AppendFormat("Smoking allowed: {0}\t", SmokingAllowed.Value ? "yes" : "no");
        }

        if (LastRenovationDate.HasValue)
        {
            builder.AppendFormat("Last renovated on: {0}\t", LastRenovationDate);
        }

        if (Rating.HasValue)
        {
            builder.AppendFormat("Rating: {0}/5\t", Rating);
        }

        if (Location != null)
        {
            builder.AppendFormat("Location: Latitude {0}, longitude {1}\t", Location.Latitude, Location.Longitude);
        }

        return builder.ToString();
    }
}

```

References

[How to use Azure Search from a .NET Application](https://docs.microsoft.com/en-us/azure/search/search-howto-dotnet-sdk)

[How full text search works in Azure Search](https://docs.microsoft.com/en-us/azure/search/search-lucene-query-architecture)

[How to rebuild an Azure Search index](https://docs.microsoft.com/en-us/azure/search/search-howto-reindex)

Other references

https://www.henkboelman.com/articles/azure-search-the-basics/

