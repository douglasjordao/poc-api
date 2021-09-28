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
    public class ProdutoController : ApiController
    {
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

        [HttpPost]
        public IHttpActionResult Insira([FromBody] DtoProduto dto)
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
                var dto = Servico.ObtenhaPorCodigo(codigo);

                if (dto != null)
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

        [HttpPut]
        public IHttpActionResult Atualize([FromBody] DtoProduto dto)
        {
            try
            {
                var obj = ObtenhaPorCodigo(dto.Codigo);

                if (obj != null)
                {
                    Servico.Atualize(dto);

                    return Ok();
                }

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
                var produtos = Servico.ObtenhaTodos();

                if (produtos.Count > 0)
                {
                    return Ok(produtos);
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
                var dto = Servico.ObtenhaPorCodigo(codigo);

                if (dto != null)
                {
                    return Ok(dto);
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
