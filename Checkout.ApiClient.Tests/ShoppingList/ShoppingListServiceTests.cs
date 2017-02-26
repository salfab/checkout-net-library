using System;
using System.Linq;
using System.Net;

using Checkout.ApiServices.ShoppingList.ResponseModel;

using FluentAssertions;

using NUnit.Framework;

namespace Tests.ShoppingList
{
    [TestFixture(Category = "ShoppingListApi")]
    public class ShoppingListServiceTests : BaseServiceTests
    {
        [Test]
        public void GetAllDrinks()
        {
            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetOrderedDrinks();

            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrinks.Model.Should().NotBeNull();
        }

        [Test]
        public void OrderDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            var drinkOrder = new DrinkOrder
                                        {
                                            Name = drinkName,
                                            Quantity = 2
                                        };

            var response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetOrderedDrinks();
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrinks.Model.Should().NotBeNull();
            orderedDrinks.Model.Should().Contain(order => order.Name == drinkName);
        }

        [Test]
        public void OrderDrinkTwice()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            var drinkOrder = new DrinkOrder
            {
                Name = drinkName,
                Quantity = 2
            };

            var response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);
            response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetOrderedDrinks();
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrinks.Model.Should().NotBeNull();
            orderedDrinks.Model.Single(order => order.Name == drinkName).Quantity.Should().Be(4);
        }

        [Test]
        public void GetDetailsOfExistingDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            var drinkOrder = new DrinkOrder
            {
                Name = drinkName,
                Quantity = 2
            };

            var response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrinks.Model.Should().NotBeNull();
            orderedDrinks.Model.Quantity.Should().Be(2);
        }

        [Test]
        public void GetDetailsOfUnexistingDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            
            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
            orderedDrinks.Model.Should().BeNull();
        }
    }
}