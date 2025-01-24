namespace Cuisine.Domain.Enums;

public static class RecipeOptions
{
    public static List<KeyValuePair<int, string>> Difficulty =
    [
        new KeyValuePair<int, string>(1, "Facile"),
        new KeyValuePair<int, string>(2, "Moyen"),
        new KeyValuePair<int, string>(3, "Difficile")
    ];

    public static List<KeyValuePair<int, string>> Price =
    [
        new KeyValuePair<int, string>(1, "Bon marché"),
        new KeyValuePair<int, string>(2, "Moyen"),
        new KeyValuePair<int, string>(3, "Cher")
    ];

    public static List<KeyValuePair<int, string>> CookingType =
    [
        new KeyValuePair<int, string>(0, "Pas de cuisson"),
        new KeyValuePair<int, string>(1, "Four"),
        new KeyValuePair<int, string>(2, "Plaque"),
        new KeyValuePair<int, string>(3, "Vapeur"),
        new KeyValuePair<int, string>(4, "Friteuse"),
        new KeyValuePair<int, string>(5, "Barbecue"),
        new KeyValuePair<int, string>(6, "Autre")
    ];
}