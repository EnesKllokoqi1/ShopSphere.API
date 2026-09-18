using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Enums;
using System.Security.Claims;

namespace ShopService.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        private Guid? GetCurrentUserId() =>
            Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

        private bool IsOwnerOrAdmin(Guid? ownerId) =>
      User.IsInRole(nameof(UserRole.Admin)) ||
      (ownerId is not null && GetCurrentUserId() == ownerId);

        // Returns the order only if the caller owns it (or is an admin).
        // Missing order and someone else's order both give null, so the API
        // never reveals that another user's order exists.
        private async Task<OrderResponseDTO?> GetAccessibleOrderAsync(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return order is not null && IsOwnerOrAdmin(order.UserId) ? order : null;
        }

        private NotFoundObjectResult OrderNotFound() => NotFound(new
        {
            Status = "Error",
            ErrorMessage = "Order not found",
            Timestamp = DateTime.UtcNow
        });
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> MakeOrderAsync([FromBody] PlaceOrderDTO placeOrderDTO)
        {
            var userId = GetCurrentUserId();
            if (userId is null)
            {
                return Unauthorized(new
                {
                    Status = "Error",
                    Message = "User has not been authenticated ",
                    Timestamp = DateTime.UtcNow
                });
            }

            try
            {
                var result = await _orderService.MakeOrderAsync(placeOrderDTO, userId.Value);
                return CreatedAtAction(
                     nameof(GetOrderById),
                     new { orderId = result.Id },
                     result
                 );
            }
            catch (InvalidOperationException ex)
            {
                // TODO: replace with proper logging before finishing the project, don't expose ex.Message to client
                Console.WriteLine(ex.ToString());
                return BadRequest(new
                {
                    Status = "Error",
                    ErrorMessage = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                // TODO: replace with proper logging before finishing the project, don't expose ex.Message to client
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new
                {
                    Status = "Error",
                    ErrorMessage = "An unexpected error occurred.",
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        [Authorize]
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById([FromRoute] Guid orderId)
        {
            var result = await GetAccessibleOrderAsync(orderId);
            if (result is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Order has been found",
                Order = result,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpPut("{orderId:guid}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrder([FromBody] UpdateOrderDTO updateOrderDTO, [FromRoute] Guid orderId)
        {

            var existing = await GetAccessibleOrderAsync(orderId);
            if (existing is null)
            {
                return OrderNotFound();
            }
            if (existing.OrderStatus != OrderStatus.Pending && !User.IsInRole(nameof(UserRole.Admin)))
            {
                return Conflict(new
                {
                    Status = "Error",
                    ErrorMessage = "Only pending orders can be edited",
                    Timestamp = DateTime.UtcNow
                });
            }
            var order = await _orderService.UpdateOrderAsync(updateOrderDTO, orderId);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "success",
                Message = "Order updated successfully.",
                Data = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult<OrderResponseDTO>> CancelOrder([FromRoute] Guid orderId, [FromBody] OrderNotesDTO? reason = null)
        {
            if (await GetAccessibleOrderAsync(orderId) is null)
            {
                return OrderNotFound();
            }

            var order = await _orderService.CancelOrderAsync(orderId, reason?.Notes);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "success",
                Message = "Order cancelled successfully.",
                Order = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpGet("user/{userId:guid}/orders")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByUserId([FromRoute] Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (!IsOwnerOrAdmin(userId)) return Forbid();

            var userOrders = await _orderService.GetOrdersByUserIdAsync(userId, pageNumber, pageSize);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {userOrders.Count()} orders for the user.",
                Data = userOrders,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpGet("user/{userId:guid}/total-spent")]
        public async Task<ActionResult<decimal?>> GetTotalSpentByUser([FromRoute] Guid userId)
        {
            if (!IsOwnerOrAdmin(userId)) return Forbid();

            var result = await _orderService.GetTotalSpentByUserAsync(userId);
            if (result is null)
            {
                return NotFound(new
                {
                    Status = "Error",
                    ErrorMessage = "User not found",
                    Timestamp = DateTime.UtcNow
                });
            }
            return Ok(new
            {
                Success = "Success",
                Total = $"{result}$",
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpGet("user/{userId:guid}/count")]
        public async Task<ActionResult<int>> GetOrderCountByUser([FromRoute] Guid userId)
        {
            if (!IsOwnerOrAdmin(userId)) return Forbid();

            var result = await _orderService.GetOrderCountByUserAsync(userId);
            return Ok(new
            {
                Status = "Success",
                Data = $"Order count by user: {result}",
                TimeStamp = DateTime.UtcNow
            });
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var orders = await _orderService.GetAllAsync(pageNumber, pageSize);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {orders.Count()} orders.",
                Data = orders,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult> DeleteOrder([FromRoute] Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order is null)
            {
                return OrderNotFound();
            }
            await _orderService.DeleteOrderAsync(order.Id);
            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{orderId:guid}/status")]
        public async Task<ActionResult<OrderStatus>> GetOrderStatus([FromRoute] Guid orderId)
        {
            var status = await _orderService.GetOrderStatusAsync(orderId);
            if (status is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "success",
                OrderStatus = status.ToString(),
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{orderId:guid}/confirm")]
        public async Task<ActionResult<OrderResponseDTO>> ConfirmOrder([FromRoute] Guid orderId)
        {
            var order = await _orderService.ConfirmOrderAsync(orderId);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "success",
                Message = "Order confirmed successfully.",
                Order = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{orderId:guid}/process")]
        public async Task<ActionResult<OrderResponseDTO>> ProcessOrder([FromRoute] Guid orderId, [FromBody] OrderNotesDTO? notes = null)
        {
            var order = await _orderService.ProcessOrderAsync(orderId, notes);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Order processed successfully.",
                Order = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{orderId:guid}/ship")]
        public async Task<ActionResult<OrderResponseDTO>> ShipOrder([FromRoute] Guid orderId, [FromBody] ShipOrderDTO shipOrderDTO)
        {
            var order = await _orderService.ShipOrderAsync(orderId, shipOrderDTO);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Order shipped successfully",
                Order = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{orderId:guid}/deliver")]
        public async Task<ActionResult<OrderResponseDTO>> DeliverOrder([FromRoute] Guid orderId, [FromBody] OrderNotesDTO? deliveryNotes = null)
        {
            var order = await _orderService.DeliverOrderAsync(orderId, deliveryNotes);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Order delivered successfully",
                Order = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{orderId:guid}/return")]
        public async Task<ActionResult<OrderResponseDTO>> ReturnOrder([FromRoute] Guid orderId, [FromBody] ReturnOrderDTO returnOrderDTO)
        {
            var order = await _orderService.ReturnOrderAsync(orderId, returnOrderDTO);
            if (order is null)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Order returned successfully",
                Order = order,
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{orderId:guid}/payment-status")]
        public async Task<ActionResult> UpdatePaymentStatus([FromRoute] Guid orderId, [FromBody] UpdatePaymentStatusDTO paymentStatus)
        {
            var result = await _orderService.UpdatePaymentStatusAsync(orderId, paymentStatus);
            if (!result)
            {
                return OrderNotFound();
            }
            return Ok(new
            {
                Status = "Success",
                Message = "Payment updated successfully",
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByStatus([FromQuery] OrderStatus orderStatus, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var ordersByStatus = await _orderService.GetOrdersByStatusAsync(orderStatus, pageNumber, pageSize);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {ordersByStatus.Count()} orders by status",
                Data = ordersByStatus,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                Timestamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(new
            {
                Status = "Success",
                Message = $"Retrieved {orders.Count()} orders by date range",
                Data = orders,
                StartDate = startDate,
                EndDate = endDate,
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalOrderCount()
        {
            var result = await _orderService.GetTotalOrderCountAsync();
            return Ok(new
            {
                Status = "Success",
                Data = $"Order count : {result}",
                TimeStamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("status/count")]
        public async Task<ActionResult<int>> GetOrderCountByStatus([FromQuery] OrderStatus orderStatus)
        {
            var result = await _orderService.GetOrderCountByStatusAsync(orderStatus);
            return Ok(new
            {
                Status = "Success",
                Data = $"Order count by status : {result}",
                OrderStatus = orderStatus.ToString(),
                TimeStamp = DateTime.UtcNow
            });
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("date-range/count")]
        public async Task<ActionResult<int>> GetOrderCountByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _orderService.GetOrderCountByDateRangeAsync(startDate, endDate);
            return Ok(new
            {
                Status = "Success",
                Data = $"Order count by date range : {result}",
                StartDate = startDate,
                EndDate = endDate,
            });
        }
    }
}