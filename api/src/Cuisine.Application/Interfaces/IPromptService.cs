namespace Cuisine.Application.Interfaces;

public interface IPromptService
{
    string PromptListIngredients(string[] listIngredients);
    string PromoptIdea(string idea);
    Task<string> PromptImproveAsync(Guid id);
    string PromptImage(string title);
}
