using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

using Checkout.ApiServices.ShoppingList;
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

        [Test]
        public void GetDetailsWithMalformedPayload()
        {
            string drinkName = null;

            var drinkOrder = new DrinkOrder
            {
                Name = drinkName,
                Quantity = 2
            };

            var response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);

            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Model.Should().BeNull();
            response.GetError<ModelErrorCollection>().Should().NotBeNull();
            response.GetError<ModelErrorCollection>().Count.Should().Be(1);
        }

        // TODO: Update drink

        [Test]
        public void UpdateExistingDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            var drinkOrder = new DrinkOrder
            {
                Name = drinkName,
                Quantity = 2
            };

            var response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            drinkOrder.Quantity = 1337;
            response = this.CheckoutClient.ShoppingListService.UpdateDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrinks.Model.Should().NotBeNull();
            orderedDrinks.Model.Quantity.Should().Be(1337);
        }

        // TODO: Update unknown drink
        [Test]
        public void UpdateUnknownDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            var drinkOrder = new DrinkOrder
            {
                Name = drinkName,
                Quantity = 2
            };
        
            drinkOrder.Quantity = 1337;
            var response = this.CheckoutClient.ShoppingListService.UpdateDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
            response.Model.Should().BeNull();
        }

        // TODO: Delete drink
    }
    
}