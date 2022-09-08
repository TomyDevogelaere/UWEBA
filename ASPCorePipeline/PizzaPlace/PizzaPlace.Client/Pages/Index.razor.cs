using PizzaPlace.Shared;

namespace PizzaPlace.Client.Pages;

public partial class Index
{
    private State State { get; set; } = new State();

    private string SpicinessImage(Spiciness spiceiness)
    {
        return $"images/{spiceiness.ToString().ToLower()}.png";
    }
    protected override void OnInitialized()
    {
        State.Menu.Add(new Pizza(1, "Pepperoni", 8.99M, Spiciness.Spicy));
        State.Menu.Add(new Pizza(2, "Margarita", 7.99M, Spiciness.None));
        State.Menu.Add(new Pizza(3, "Diabolo", 9.99M, Spiciness.Hot));
    }
    private void AddToBasket(Pizza pizza)
    {
        Console.WriteLine($"Added pizza {pizza.Name}");
        State.Basket.Add(pizza.Id);
    }
    private void RemoveFromBasket(int pos)
 => State.Basket.RemoveAt(pos);

    private void PlaceOrder()
    {
        Console.WriteLine("Placing order");
    }
}
