using NUnit.Framework;
using Dotnet.MiniJira.Domain.Entities;
using Dotnet.MiniJira.Domain.Helpers;

namespace Dotnet.MiniJira.Tests;
public class TopFavoritesDynamicSortReflectionTests : MockedBaseTest
{
    public List<Domain.Entities.Task> recordsToSort = new List<Domain.Entities.Task>();

    [OneTimeSetUp]
    public void SetUpTasks()
    {
        recordsToSort.AddRange(new List<Domain.Entities.Task>
        {
                new Domain.Entities.Task { IsFavorite = false, Name = "AAAA" },
                new Domain.Entities.Task { IsFavorite = true, Name = "BBBB" },
                new Domain.Entities.Task { IsFavorite = true, Name = "CCCC" },
                new Domain.Entities.Task { IsFavorite = false, Name = "DDDD" },
                new Domain.Entities.Task { IsFavorite = false, Name = "EEEE" },
        });
    }

    [OneTimeTearDown]
    public void BaseTearDownTasks()
    {
        recordsToSort = null;
    }

    [Test]
    public void Order_Dynamic_List_By_Desc()
    {
        var expctedResult = new List<Domain.Entities.Task>
        {
                recordsToSort[4],
                recordsToSort[3],
                recordsToSort[2],
                recordsToSort[1],
                recordsToSort[0]
        };

        var sortedDesc = recordsToSort.CustomSortBy("Name", true);

        Assert.AreEqual(expctedResult, sortedDesc);
    }

    [Test]
    public void Order_Dynamic_List_By_Asc()
    {
        // Shuffle list by sorting by new Guid
        var suffledList = recordsToSort.OrderBy(a => Guid.NewGuid()).ToList(); ;

        var sortedDesc = suffledList.CustomSortBy("Name", false);

        Assert.AreEqual(recordsToSort, sortedDesc);
    }

    [Test]
    public void Order_Dynamic_List_Favorites_On_Top_Desc_ThenBy_Asc()
    {
        var expctedResult = new List<Domain.Entities.Task>
        {
                recordsToSort[1], // BBBB IsFavorite = true
                recordsToSort[2], // CCCC IsFavorite = true 
                recordsToSort[0], // AAAA
                recordsToSort[3], // DDDD
                recordsToSort[4]  // EEEE
        };

        var sortedDesc = recordsToSort.CustomSortBy("Name", false, "IsFavorite");

        Assert.AreEqual(expctedResult, sortedDesc);
    }

    [Test]
    public void Order_Dynamic_List_Favorites_On_Top_Desc_ThenBy_Desc()
    {
        var expctedResult = new List<Domain.Entities.Task>
        {
                recordsToSort[2], // CCCC IsFavorite = true
                recordsToSort[1], // BBBB IsFavorite = true 
                recordsToSort[4], // EEEE
                recordsToSort[3], // DDDD
                recordsToSort[0]  // AAAA
        };

        var sortedDesc = recordsToSort.CustomSortBy("Name", true, "IsFavorite");

        Assert.AreEqual(expctedResult, sortedDesc);
    }

    [Test]
    public void Order_Dynamic_List_Favorites_On_Top_Asc_ThenBy_Asc()
    {
        var expctedResult = new List<Domain.Entities.Task>
        {
                recordsToSort[1], // BBBB IsFavorite = true
                recordsToSort[2], // CCCC IsFavorite = true 
                recordsToSort[0], // AAAA
                recordsToSort[3], // DDDD
                recordsToSort[4]  // EEEE
        };

        var sortedDesc = recordsToSort.CustomSortBy("Name", false, "IsFavorite");

        Assert.AreEqual(expctedResult, sortedDesc);
    }

    [Test]
    public void Order_Dynamic_List_Favorites_On_Top_Asc_ThenBy_Desc()
    {
        var expctedResult = new List<Domain.Entities.Task>
        {
                recordsToSort[1], // BBBB IsFavorite = true
                recordsToSort[2], // CCCC IsFavorite = true 
                recordsToSort[0], // AAAA
                recordsToSort[3], // DDDD
                recordsToSort[4]  // EEEE
        };

        var sortedDesc = recordsToSort.CustomSortBy("Name", false, "IsFavorite");

        Assert.AreEqual(expctedResult, sortedDesc);
    }
}

