using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopService.Application.DTOs.OrderDTOs;
using ShopService.Application.Interfaces;
using ShopService.Domain.Enums;

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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> MakeOrderAsync([FromBody] PlaceOrderDTO placeOrderDTO)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("{orderId:guid}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderByIdAsync([FromRoute] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllAsync([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpDelete("{orderId:guid}")]
        public async Task<ActionResult> DeleteOrder([FromRoute] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPut("{orderId:guid}")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrder([FromBody] UpdateOrderDTO updateOrderDTO, [FromRoute] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("{orderId:guid}/status")]
        public async Task<ActionResult<OrderStatus>> GetOrderStatus([FromRoute] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("{orderId:guid}/cancel")]
        public async Task<ActionResult<OrderResponseDTO>> CancelOrder([FromRoute] Guid orderId, [FromBody] OrderNotesDTO? reason = null)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("{orderId:guid}/confirm")]
        public async Task<ActionResult<OrderResponseDTO>> ConfirmOrder([FromRoute] Guid orderId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("{orderId:guid}/process")]
        public async Task<ActionResult<OrderResponseDTO>> ProcessOrder([FromRoute] Guid orderId, [FromBody] OrderNotesDTO? notes = null)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("{orderId:guid}/ship")]
        public async Task<ActionResult<OrderResponseDTO>> ShipOrder([FromRoute] Guid orderId, [FromBody] ShipOrderDTO shipOrderDTO)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("{orderId:guid}/deliver")]
        public async Task<ActionResult<OrderResponseDTO>> DeliverOrder([FromRoute] Guid orderId, [FromBody] OrderNotesDTO? deliveryNotes = null)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPost("{orderId:guid}/return")]
        public async Task<ActionResult<OrderResponseDTO>> ReturnOrder([FromRoute] Guid orderId, [FromBody] ReturnOrderDTO returnOrderDTO)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpPut("{orderId:guid}/payment-status")]
        public async Task<ActionResult> UpdatePaymentStatus([FromRoute] Guid orderId, [FromBody] UpdatePaymentStatusDTO paymentStatus)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByUserId([FromRoute] Guid userId, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByStatus([FromQuery] OrderStatus orderStatus, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("user/{userId:guid}/total-spent")]
        public async Task<ActionResult<decimal?>> GetTotalSpentByUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetOrdersByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("user/{userId:guid}/count")]
        public async Task<ActionResult<int>> GetOrderCountByUser([FromRoute] Guid userId)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalOrderCount()
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("status/count")]
        public async Task<ActionResult<int>> GetOrderCountByStatus([FromQuery] OrderStatus orderStatus)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [HttpGet("date-range/count")]
        public async Task<ActionResult<int>> GetOrderCountByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}