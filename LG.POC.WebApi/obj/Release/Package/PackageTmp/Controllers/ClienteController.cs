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
    /// Controller de Clientes
    /// </summary>
    [RoutePrefix("[controller]")]
    public class ClienteController : ApiController
    {
        #region Constantes

        private const string CADASTRADO = "Cliente cadastrado com sucesso";
        private const string DADOS_INVALIDOS_OU_DUPLICADOS = "Dados informados são inválidos ou duplicados.";
        private const string BUSCA_OK = "Busca concluída.";
        private const string BUSCA_SEM_RESULTADOS = "Busca concluída, mas sem resultados";
        #endregion

        #region Propriedades

        private ServicoDeClientes Servico { get; }
        #endregion

        #region Construtor

        public ClienteController()
        {
            Servico = FabricaDeServicoDeClientes.Crie();
        }
        #endregion

        #region Manutenção

        /// <summary>
        /// Endpoint responsável por cadastrar um cliente.
        /// </summary>
        /// <param name="cliente">Nome e Contato são obrigatórios. Email deve ser um e-mail válido.
        /// Email e Contato não podem ser cadastrados em duplicidade.
        /// </param>
        [HttpPost]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, CADASTRADO, typeof(DtoCliente))]
        [SwaggerResponse(HttpStatusCode.BadRequest, DADOS_INVALIDOS_OU_DUPLICADOS)]
        public IHttpActionResult Insira([FromBody] DtoClienteInsercao cliente)
        {
            try
            {
                var _cliente = Servico.Insira(cliente);

                return CreatedAtRoute("DefaultApi", new { codigo = _cliente.Codigo }, _cliente);
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
        /// Endpoint responsável por excluir um cliente.
        /// </summary>
        /// <param name="codigo">Identificador do cliente.</param>
        /// <response code="204">Cliente excluído com sucesso.</response>
        /// <response code="400">Há um ou mais clientes selecionados com ocorrência à um ou mais pedidos.</response>
        /// <response code="404">Cliente não encontrado na base de dados.</response>
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

        /// <summary>
        /// Endpoint responsável por excluir vários clientes de uma vez.
        /// </summary>
        /// <param name="codigos">Identificadores dos clientes.</param>
        /// <response code="204">Clientes excluídos com sucesso.</response>
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

        /// <summary>
        /// Endpoint responsável por atualizar todos os dados de um cliente.
        /// </summary>
        /// <param name="cliente">Código é obrigatório.</param>
        /// <response code="204">Cliente atualizado com sucesso.</response>
        /// <response code="400">Dados informados são inválidos ou duplicados.</response>
        /// <response code="404">Cliente não encontrado na base de dados.</response>
        [HttpPut]
        public IHttpActionResult Atualize([FromBody] DtoCliente cliente)
        {
            try
            {
                Servico.Atualize(cliente);

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
        #endregion

        #region Consulta

        /// <summary>
        /// Endpoint responsável por buscar todos os clientes cadastrados.
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, BUSCA_OK, typeof(IList<DtoCliente>))]
        [SwaggerResponse(HttpStatusCode.NoContent, BUSCA_SEM_RESULTADOS, typeof(IList<DtoCliente>))]
        public IHttpActionResult ObtenhaTodos()
        {
            try
            {
                var clientes = Servico.ObtenhaTodos();

                if (clientes.Count == 0) return StatusCode(HttpStatusCode.NoContent);

                return Ok(clientes);
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
        /// Endpoint responsável por buscar um cliente atráves do código.
        /// </summary>
        /// <param name="codigo">Identificador do cliente.</param>
        /// <response code="404">Cliente não encontrado na base de dados.</response>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, BUSCA_OK, typeof(DtoCliente))]
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
