using System;
using System.Linq;
using System.Threading.Tasks;
using Menu.Core.Enums;
using Menu.Service;
using Microsoft.AspNetCore.SignalR;
using Menu.Api.Extensions;

namespace Menu.Api.Hubs
{
    public class OrderHub : Hub
    {
        private readonly IOrderService _orderService;

        public OrderHub(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var groupName = httpContext.Request.Query.GetQueryParameterValue<string>("groupName");
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await base.OnConnectedAsync();
        }

        public Task ReloadVenueOrders(string id, string groupName)
        {
            int venueId = Convert.ToInt32(id);

            var approvedOrders = _orderService.GetByVenueId(venueId, OrderStatus.Approved).Select(order => new
            {
                order.Id,
                order.Code,
                order.Description,
                OrderStatus = order.OrderStatus.ToOrderStatus(),
                CreatedDate = order.CreatedDate.ToString("HH:mm"),
                orderDetails = order.OrderDetail.Select(orderDetail => new
                {
                    orderDetail.Id,
                    orderDetail.Name,
                    orderDetail.Photo,
                    orderDetail.OptionItem,
                    orderDetail.Quantity
                }),
                Table = new
                {
                    order.OrderTable.Table.Name,
                },
                User = new
                {
                    order.OrderTable.User.Name,
                    order.OrderTable.User.Surname,
                    order.OrderTable.User.Photo
                },
                Waiter = new
                {
                    order.OrderWaiter.Waiter.Name,
                    order.OrderWaiter.Waiter.Surname
                }
            });

            var preparingOrders = _orderService.GetByVenueId(venueId, OrderStatus.Preparing).Select(order => new
            {
                order.Id,
                order.Code,
                order.Description,
                OrderStatus = order.OrderStatus.ToOrderStatus(),
                CreatedDate = order.CreatedDate.ToString("HH:mm"),
                orderDetails = order.OrderDetail.Select(orderDetail => new
                {
                    orderDetail.Id,
                    orderDetail.Name,
                    orderDetail.Photo,
                    orderDetail.OptionItem,
                    orderDetail.Quantity
                }),
                Table = new
                {
                    order.OrderTable.Table.Name,
                },
                User = new
                {
                    order.OrderTable.User.Name,
                    order.OrderTable.User.Surname,
                    order.OrderTable.User.Photo
                },
                Waiter = new
                {
                    order.OrderWaiter.Waiter.Name,
                    order.OrderWaiter.Waiter.Surname
                }
            });

            var preparedOrders = _orderService.GetByVenueId(venueId, OrderStatus.Prepared).Select(order => new
            {
                order.Id,
                order.Code,
                order.Description,
                OrderStatus = order.OrderStatus.ToOrderStatus(),
                CreatedDate = order.CreatedDate.ToString("HH:mm"),
                orderDetails = order.OrderDetail.Select(orderDetail => new
                {
                    orderDetail.Id,
                    orderDetail.Name,
                    orderDetail.Photo,
                    orderDetail.OptionItem,
                    orderDetail.Quantity
                }),
                Table = new
                {
                    order.OrderTable.Table.Name,
                },
                User = new
                {
                    order.OrderTable.User.Name,
                    order.OrderTable.User.Surname,
                    order.OrderTable.User.Photo
                },
                Waiter = new
                {
                    order.OrderWaiter.Waiter.Name,
                    order.OrderWaiter.Waiter.Surname
                }
            });

            return Clients.Group(groupName).SendAsync("ReceiveVenueOrders", new {
                approvedOrders,
                preparingOrders,
                preparedOrders
            });
        }
    }
}