using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoffeeShop.Models;
using CoffeeShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {

        private readonly OrderService _orderService;
        private readonly ItemsOrderService _itemsOrderService;
        private readonly ItemService _itemService;
        private readonly TaxService _taxService;

        public OrderController(OrderService orderService, ItemsOrderService itemsOrderService, ItemService itemService, TaxService taxService)
        {
            _orderService = orderService;
            _itemsOrderService = itemsOrderService;
            _itemService = itemService;
            _taxService = taxService;
        }

        /// <summary>
        /// This service provides items available
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/getItems")]
        //////http://localhost:56874/order/api/GetItems
        public IList<Items> GetItems()
        {
            return _itemService.Read();
        }

        [HttpGet]
        [Route("api/getOrder/{id}")]
        //////http://localhost:56874/order/api/GetOrder/616469081f6e7cd73d585c85
        public Order GetOrder(string id)
        {
            return _orderService.Find(id);
        }

        /// <summary>
        /// This service to select items before placing an order
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/selectItem/{itemId}/{qty}")]
        //http://localhost:56874/order/api/SelectItem/61642a5050d240bb4a97b7d4/1
        public string SelectItem(string itemId, int qty)
        {
            try
            {
                ItemsOrder obj = new ItemsOrder() { 
                    itemId = itemId,
                    qty = qty,
                    createdBy = 1,
                    createdOn = DateTime.UtcNow
                };
                return _itemsOrderService.Create(obj).Id;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        void DoWorkAsync(Order order)
        {
            System.Threading.Thread.Sleep(20000);
            order.status = Status.ready.ToString();
            order.modifiedBy = 1;
            order.modifiedOn = DateTime.UtcNow;            
            _orderService.Update(order);
        }

        /// <summary>
        /// This service to place an order by providing items which are ordered
        /// </summary>
        /// <param name="itemsOrderIds"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/placeOrder")]
        //http://localhost:56874/order/api/placeOrder?itemsOrderIds=61646b581f6e7cd73d585c88&itemsOrderIds=61646c4d1f6e7cd73d585c8d
        public Order PlaceOrder([FromQuery] string[] itemsOrderIds)  //async Task<Order>
        {
            try
            {
                ItemsOrder itemsOrder = null;
                Items items = null;
                Tax tax = null;
                double itemPrice = 0;
                double rawPrice = 0;
                List<ItemsOrder> listItemsOrder = new List<ItemsOrder>();
                for (int i = 0; i < itemsOrderIds.Length; i++)
                {
                    itemsOrder = _itemsOrderService.Find(itemsOrderIds[i]);
                    listItemsOrder.Add(itemsOrder);
                    items = _itemService.Find(itemsOrder.itemId);
                    tax = _taxService.Find(items.categoryId);
                    rawPrice = items.unitPrice * itemsOrder.qty;
                    itemPrice = itemPrice + (rawPrice + (rawPrice * (tax.percTax / 100)));
                }

                Order obj = new Order() {
                    cost = itemPrice,
                    status = Status.open.ToString(), 
                    createdBy = 1,
                    createdOn=DateTime.UtcNow,
                    ItemsOrder = listItemsOrder
                };

                Order objOrder = _orderService.Create(obj);

                if(obj.cost > 225)
                {
                    // Free veg patties
                    listItemsOrder.Add(_itemsOrderService.Create(new ItemsOrder()
                    {
                        // Veg patties item Id
                        itemId = "6164624a50d240bb4a97b7d7",
                        qty = 1,
                        createdBy = 1,
                        createdOn = DateTime.UtcNow
                    })) ;
                }

                foreach (var itemOrderObj in listItemsOrder)
                {
                    itemOrderObj.orderId = objOrder.Id;
                    itemOrderObj.modifiedBy = 1;
                    itemOrderObj.modifiedOn = DateTime.UtcNow;
                    _itemsOrderService.Update(itemOrderObj);
                }

                Task.Run(() => DoWorkAsync(objOrder));

                return objOrder;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
