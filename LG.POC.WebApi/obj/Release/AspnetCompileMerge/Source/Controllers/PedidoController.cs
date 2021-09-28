using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;
using System;
using System.Net;
using System.Web.Http;

namespace LG.POC.WebApi.Controllers
{
    [RoutePrefix("[controller]")]
    public class PedidoController : ApiController
    {
        #region Propriedades

        private ServicoDePedidos Servico { get; }
        #endregion

        #region Construtor

        public PedidoController()
        {
            Servico = FabricaDeServicoDePedidos.Crie();
        }
        #endregion

        #region Manutenção

        [HttpPost]
        public IHttpActionResult Insira([FromBody] DtoPedido dto)
        {
            try
            {
                dto.Codigo = Servico.Insira(dto);

                return CreatedAtRoute("DefaultApi", new { codigo = dto.Codigo }, dto);
            }
            catch (ExcecaoDadosInvalidos e)
            {
                return BadRequest(e.Message);
            }
            catch (ExcecaoDadosDuplicados e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Ocorreu um erro interno"));
            }
        }

        [HttpDelete]
        public IHttpActionResult Exclua(int codigo)
        {
            try
            {
                var pedido = Servico.ObtenhaPorCodigo(codigo);

                if (pedido != null)
                {
                    Servico.Exclua(codigo);
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Ocorreu um erro interno"));
            }
        }
        #endregion

        #region Consulta

        [HttpGet]
        public IHttpActionResult ObtenhaTodos()
        {
            try
            {
                var pedidos = Servico.ObtenhaTodos();

                if (pedidos.Count > 0)
                {
                    return Ok(pedidos);
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Ocorreu um erro interno"));
            }
        }

        [HttpGet]
        public IHttpActionResult ObtenhaPorCodigo(int codigo)
        {
            try
            {
                var pedido = Servico.ObtenhaPorCodigo(codigo);

                if (pedido != null)
                {
                    return Ok(pedido);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Ocorreu um erro interno"));
            }
        }
        #endregion
    }
}
