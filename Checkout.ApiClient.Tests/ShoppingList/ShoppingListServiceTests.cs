using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

using Checkout.ApiServices.SharedModels;
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

            var orderedDrink = this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            orderedDrink.Should().NotBeNull();
            orderedDrink.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrink.Model.Should().NotBeNull();
            orderedDrink.Model.Quantity.Should().Be(2);
        }

        [Test]
        public void GetDetailsOfUnspecifiedDrink()
        {
            string drinkName = null;

            try
            {
                this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            }
            catch (ArgumentException)
            {
                return;
            }
            
            Assert.Fail("An exception should have been thrown by now.");
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
            var updateResponse = this.CheckoutClient.ShoppingListService.UpdateDrink(drinkOrder);
            updateResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            updateResponse.Model.Should().NotBeNull();
            updateResponse.Model.Quantity.Should().Be(1337);

            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            orderedDrinks.Model.Should().NotBeNull();
            orderedDrinks.Model.Quantity.Should().Be(1337);
        }

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
        [Test]
        public void DeleteExistingDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
            var drinkOrder = new DrinkOrder
            {
                Name = drinkName,
                Quantity = 2
            };

            var response = this.CheckoutClient.ShoppingListService.OrderDrink(drinkOrder);
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);

            var deleteResponse = this.CheckoutClient.ShoppingListService.DeleteDrink(drinkName);
            deleteResponse.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            deleteResponse.Model.Should().BeNull();            

            var orderedDrinks = this.CheckoutClient.ShoppingListService.GetDrinkDetails(drinkName);
            orderedDrinks.Should().NotBeNull();
            orderedDrinks.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteUnknownDrink()
        {
            var drinkName = Guid.NewGuid().ToString("N");
          
            var deleteResponse = this.CheckoutClient.ShoppingListService.DeleteDrink(drinkName);
            deleteResponse.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
            deleteResponse.Model.Should().BeNull();           
        }

        [Test]
        public void DeleteUnspecifiedDrink()
        {
            var drinkName = string.Empty;
          
            var deleteResponse = this.CheckoutClient.ShoppingListService.DeleteDrink(drinkName);
            deleteResponse.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
            deleteResponse.Model.Should().BeNull();           
        }
    }
    
}