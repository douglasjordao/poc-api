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
    /// Controller de Produtos
    /// </summary>
    [RoutePrefix("[controller]")]
    public class ProdutoController : ApiController
    {
        #region Constantes

        private const string CADASTRADO = "Produto cadastrado com sucesso";
        private const string DADO_INVALIDOS_OU_DUPLICADOS = "Dados informados são inválidos ou duplicados.";
        private const string BUSCA_OK = "Busca concluída.";
        private const string BUSCA_SEM_RESULTADOS = "Busca concluída, mas sem resultados";
        #endregion

        #region Propriedades

        private ServicoDeProdutos Servico { get; set; }
        #endregion

        #region Construtor

        public ProdutoController()
        {
            Servico = FabricaDeServicoDeProduto.Crie();
        }
        #endregion

        #region Manutenção

        /// <summary>
        /// Endpoint responsável por cadastrar um produto.
        /// </summary>
        /// <param name="produto">Descrição é obrigatória e não pode ser cadastrada em duplicidade. Valor e Quantidade devem ser números positivos.</param>
        [HttpPost]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, CADASTRADO, typeof(DtoProduto))]
        [SwaggerResponse(HttpStatusCode.BadRequest, DADO_INVALIDOS_OU_DUPLICADOS)]
        public IHttpActionResult Insira([FromBody] DtoProdutoCadastro produto)
        {
            try
            {
                var _produto = Servico.Insira(produto);

                return CreatedAtRoute("DefaultApi", new { codigo = _produto.Codigo }, _produto);
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
        /// Endpoint responsável por excluir um produto.
        /// </summary>
        /// <param name="codigo">Identificador do produto</param>
        /// <response code="204">Produto excluído com sucesso.</response>
        /// <response code="404">Produto não encontrado na base de dados.</response>
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
        /// Endpoint responsável por excluir vários produtos de uma vez.
        /// </summary>
        /// <param name="codigos">Identificadores dos produtos.</param>
        /// <response code="204">Produtos excluídos com sucesso.</response>
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
        /// Endpoint responsável por atualizar todos os dados de um produto.
        /// </summary>
        /// <param name="produto">Código é obrigatório.</param>
        /// <response code="204">Produto atualizado com sucesso.</response>
        /// <response code="400">Dados informados são inválidos ou duplicados.</response>
        /// <response code="404">Produto não encontrado na base de dados.</response>
        [HttpPut]
        public IHttpActionResult Atualize([FromBody] DtoProduto produto)
        {
            try
            {
                Servico.Atualize(produto);

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
        /// Endpoint responsável por buscar todos os produtos cadastrados.
        /// </summary>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, BUSCA_OK, typeof(IList<DtoProduto>))]
        [SwaggerResponse(HttpStatusCode.NoContent, BUSCA_SEM_RESULTADOS, typeof(IList<DtoProduto>))]
        public IHttpActionResult ObtenhaTodos()
        {
            try
            {
                var produtos = Servico.ObtenhaTodos();

                if (produtos.Count == 0) return StatusCode(HttpStatusCode.NoContent);

                return Ok(produtos);
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
        /// Endpoint responsável por buscar um produto atráves do código.
        /// </summary>
        /// <param name="codigo">Identificador do produto.</param>
        /// <response code="404">Produto não encontrado na base de dados.</response>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, BUSCA_OK, typeof(DtoProduto))]
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
