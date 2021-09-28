using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;

namespace LG.POC.WebApi.Controllers
{
    /// <summary>
    /// Controller de Pedidos
    /// </summary>
    [RoutePrefix("[controller]")]
    public class PedidoController : ApiController
    {
        #region Constantes

        private const string CADASTRADO = "Pedido cadastrado com sucesso";
        private const string DADOS_INVALIDOS = "Dados informados são inválidos.";
        private const string BUSCA_OK = "Busca concluída.";
        private const string BUSCA_SEM_RESULTADOS = "Busca concluída, mas sem resultados";
        #endregion

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

        /// <summary>
        /// Endpoint responsável por cadastrar um pedido.
        /// </summary>
        /// <param name="pedido">Obrigatóriamente deve ter no mínimo 1 produto e estar vinculado a um cliente.</param>
        [HttpPost]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, CADASTRADO, typeof(DtoPedido))]
        [SwaggerResponse(HttpStatusCode.BadRequest, DADOS_INVALIDOS)]
        public IHttpActionResult Insira([FromBody] DtoPedidoInsercao pedido)
        {
            try
            {
                var _pedido = Servico.Insira(pedido);

                return CreatedAtRoute("DefaultApi", new { c = _pedido.Codigo }, _pedido);
            }
            catch (ExcecaoDadosInvalidos e)
            {
                return BadRequest(e.Message);
            }
            catch (ExcecaoDadosDuplicados e)
            {
                return BadRequest(e.Message);
            }
            catch (SqlException)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_BANCO_DE_DADOS));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_INTERNO));
            }
        }

        /// <summary>
        /// Endpoint responsável por excluir um pedido.
        /// </summary>
        /// <param name="codigo">Identificador do pedido</param>
        /// <response code="204">Pedido excluído com sucesso.</response>
        /// <response code="404">Pedido não encontrado na base de dados.</response>
        [HttpDelete]
        public IHttpActionResult Exclua(int codigo)
        {
            try
            {
                Servico.Exclua(codigo);
                
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (SqlException)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_BANCO_DE_DADOS));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_INTERNO));
            }
        }

        /// <summary>
        /// Endpoint responsável por excluir vários produtos de uma vez.
        /// </summary>
        /// <param name="codigos">Identificadores dos pedidos.</param>
        /// <response code="204">Pedidos excluídos com sucesso.</response>
        /// <response code="400">Dados inválidos.</response>
        [HttpDelete]
        public IHttpActionResult ExcluaVarios([FromUri]int[] codigos)
        {
            try
            {
                Servico.ExcluaVarios(codigos);

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ExcecaoDadosInvalidos e)
            {
                return BadRequest(e.Message);
            }
            catch (SqlException)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_BANCO_DE_DADOS));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_INTERNO));
            }
        }
        #endregion

        #region Consulta

        /// <summary>
        /// Endpoint responsável por buscar todos os pedidos cadastrados.
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, BUSCA_OK, typeof(IList<DtoPedido>))]
        [SwaggerResponse(HttpStatusCode.NoContent, BUSCA_SEM_RESULTADOS, typeof(IList<DtoPedido>))]
        public IHttpActionResult ObtenhaTodos()
        {
            try
            {
                var pedidos = Servico.ObtenhaTodos();

                if (pedidos.Count == 0) return StatusCode(HttpStatusCode.NoContent);

                return Ok(pedidos);
            }
            catch (SqlException)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_BANCO_DE_DADOS));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_INTERNO));
            }
        }
        /// <summary>
        /// Endpoint responsável por buscar um pedido atráves do código.
        /// </summary>
        /// <param name="codigo">Identificador do pedido.</param>
        /// <reponse code="404">Pedido não encontrado na base de dados.</reponse>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, BUSCA_OK, typeof(DtoPedido))]
        public IHttpActionResult ObtenhaPorCodigo(int codigo)
        {
            try
            {
                return Ok(Servico.ObtenhaPorCodigo(codigo));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (SqlException)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_BANCO_DE_DADOS));
            }
            catch (Exception)
            {
                return InternalServerError(new Exception(MensagensExcecaoErroInterno.ERRO_INTERNO));
            }
        }
        #endregion
    }
}
