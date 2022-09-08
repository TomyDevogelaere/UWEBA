namespace PizzaPlace.Shared;
public class State
{
  public Menu Menu { get; } = new Menu();

  public ShoppingBasket Basket { get; } = new ShoppingBasket();

  public decimal TotalPrice
  => Basket.Orders.Sum(id => Menu.GetPizza(id)!.Price);
}

