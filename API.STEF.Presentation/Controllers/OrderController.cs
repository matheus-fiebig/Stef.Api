using API.STEF.Application.Orders.Models.Request.CreateOrder;
using API.STEF.Application.Orders.Models.Request.UpdateOrder;
using API.STEF.Application.Orders.Services;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace API.STEF.Presentation.Controllers
{
    [Route("api/v1/orders")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }


        /// <summary>
        /// Obtem todos os pedidos paginados
        /// </summary>
        /// <param name="pageNumber">Pagina da busca</param>
        /// <param name="pageSize">Tamanho da pagina</param>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = -1, [FromQuery] int pageSize = -1)
        {
            return await HandleResponse(async () => await _service.GetAllAsync(pageNumber, pageSize));
        }

        /// <summary>
        /// Obtem um pedido
        /// </summary>
        /// <param name="id">Identificador do pedido</param>
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return await HandleResponse(async () => await _service.GetByIdAsync(id));
        }

        /// <summary>
        /// Cria um novo pedido
        /// </summary>
        /// <param name="request">Pedido a ser criado</param>
        [HttpPost]
        public async Task<IActionResult> Insert(CreateOrderRequest request)
        {
            return await HandleResponse(async () => await _service.InsertAsync(request));
        }

        /// <summary>
        /// Atualiza o pedido, desde que ele ainda nao tenha sido pago
        /// </summary>
        /// <param name="request">Dados atualizados do pedido</param>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOrderRequest request)
        {
            return await HandleResponse(async () => await _service.UpdateAsync(id, request));
        }

        /// <summary>
        /// Deleta o pedido desde que ele ainda nao tenha sido pago
        /// </summary>
        /// <param name="id">Identificador do pedido</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await HandleResponse(async () => await _service.DeleteAsync(id));
        }
    }
}
