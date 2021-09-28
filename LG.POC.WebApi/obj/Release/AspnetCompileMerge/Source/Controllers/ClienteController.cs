using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;
using System;
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
        /// Endpoint responsável por inserir um cliente.
        /// </summary>
        /// <param name="dto">Cliente</param>
        /// <returns>Os dados do <see cref="Models.Cliente"/> inseridos e sua url de localização</returns>
        [HttpPost]
        public IHttpActionResult Insira([FromBody] DtoCliente dto)
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

        /// <summary>
        /// Endpoint responsável por excluir um cliente
        /// </summary>
        /// <param name="codigo">Codigo</param>
        /// <returns></returns>
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

        /// <summary>
        /// Endpoint responsável por atualizar um cliente
        /// </summary>
        /// <param name="dto">Cliente</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Atualize([FromBody] DtoCliente dto)
        {
            try
            {
                var obj = Servico.ObtenhaPorCodigo(dto.Codigo);

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
        /// <summary>
        /// Endpoint responsável por buscar todos os clientes cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ObtenhaTodos()
        {
            try
            {
                var clientes = Servico.ObtenhaTodos();

                if (clientes.Count > 0)
                {
                    return Ok(clientes);
                }

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return InternalServerError(new Exception("Ocorreu um erro interno"));
            }
        }

        /// <summary>
        /// Endpoint responsável por buscar um cliente atráves do código
        /// </summary>
        /// <param name="codigo">Identificador do tipo <see cref="int"/> usado para identificar o objeto <see cref="Models.Cliente"/></param>
        /// <returns>
        /// Uma instância de <see cref="Models.Cliente"/> com valores dos atributos correspontesde ao cliente do <paramref name="codigo"/> informado
        /// </returns>
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
