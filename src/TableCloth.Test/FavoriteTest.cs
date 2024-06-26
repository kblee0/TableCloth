﻿using TableCloth.Commands.CatalogPage;
using TableCloth.Components;
using TableCloth.Models.Catalog;
using TableCloth.Models.Configuration;

namespace TableCloth.Test;

public class FavoriteTest
{
    // ShowFavorites
    [Fact]
    public async Task CatalogPageItemFavoriteCommandTest()
    {
        //arrange
        var mockPreferencesManager = new Mock<IPreferencesManager>();
        var defaultSettings = new PreferenceSettings
        {
            Favorites = new List<string>()
        };
        mockPreferencesManager.Setup(x => x.LoadPreferencesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(defaultSettings);
        mockPreferencesManager.Setup(x => x.SavePreferencesAsync(defaultSettings, It.IsAny<CancellationToken>()));

        CatalogInternetService service = new()
        {
            Id = "Service1",
            IsFavorite = true,
        };
        var command = new CatalogPageItemFavoriteCommand(mockPreferencesManager.Object);

        //act
        await command.ExecuteAsync(service);

        //assert
        var savedCountIsOne = defaultSettings.Favorites.Count == 1;
        Assert.True(savedCountIsOne);
    }
}