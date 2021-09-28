using LG.POC.InterfaceFabricas.ContratosDeServico.Servicos;
using System.Collections.Generic;
using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Interfaces.Mapeadores;
using LG.POC.InterfaceFabricas.Fabricas.Mapeadores;
using LG.POC.ServicoMapeadores.Validacoes;
using LG.POC.InterfaceFabricas.Excecao;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;
using LG.POC.InterfaceFabricas.Fabricas.Servicos;

namespace LG.POC.ServicoMapeadores.Servicos.ContratosImplementados
{
    public class ServicoDeClientesImpl : ServicoDeClientes
    {
        #region Propriedades
        private IMapeadorDeClientes Mapeador { get; }
        #endregion

        #region Construtor
        public ServicoDeClientesImpl()
        {
            Mapeador = FabricaDeMapeadorDeClientes.Crie();
        }
        #endregion

        #region Manutenção

        public void Atualize(DtoCliente dto)
        {
            var cliente = ObtenhaPorCodigo(dto.Codigo);

            if (cliente == null) throw new KeyNotFoundException();

            var clientes = ObtenhaTodos();

            dto.ValideDados(clientes);

            Mapeador.Atualize(dto);
        }

        public void Exclua(int codigo)
        {
            var cliente = ObtenhaPorCodigo(codigo);

            if (cliente == null) throw new KeyNotFoundException();

            var servicoDePedidos = FabricaDeServicoDePedidos.Crie();

            var pedidos = servicoDePedidos.ObtenhaTodos();

            cliente.ValidePendencias(pedidos);

            Mapeador.Exclua(codigo);
        }

        public void ExcluaVarios(int[] codigos)
        {
            if (codigos == null || codigos.Length == 0) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.DADOS_NULOS);

            var clientes = new List<DtoCliente>();

            var servicoDePedidos = FabricaDeServicoDePedidos.Crie();

            var pedidos = servicoDePedidos.ObtenhaTodos();

            foreach (var codigo in codigos)
            {
                clientes.Add(ObtenhaPorCodigo(codigo));
            }

            foreach (var cliente in clientes)
            {
                cliente.ValidePendencias(pedidos);
            }

            Mapeador.ExcluaVarios(codigos);
        }

        public DtoCliente Insira(DtoClienteInsercao dto)
        {
            var clientes = ObtenhaTodos();

            dto.ValideDados(clientes);

            return Mapeador.Insira(dto);
        }
        #endregion

        #region Consulta

        public DtoCliente ObtenhaPorCodigo(int codigo)
        {
            var cliente = Mapeador.ObtenhaPorCodigo(codigo);

            if (cliente == null) throw new KeyNotFoundException();

            return cliente;
        }

        public IList<DtoCliente> ObtenhaTodos()
        {
            return Mapeador.ObtenhaTodos(); ;
        }
        #endregion
    }
}
