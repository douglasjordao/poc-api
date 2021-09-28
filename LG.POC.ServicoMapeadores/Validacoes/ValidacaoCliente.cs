using LG.POC.InterfaceFabricas.ContratosDeServico.Dados;
using LG.POC.InterfaceFabricas.Excecao;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LG.POC.InterfaceFabricas.Enumeradores;
using LG.POC.InterfaceFabricas.Utilidades.Excecao;
using LG.POC.InterfaceFabricas.Utilidades.Extensoes;
using LG.POC.InterfaceFabricas.Fabricas.Negocio;
using System;

namespace LG.POC.ServicoMapeadores.Validacoes
{
    public static class ValidacaoCliente
    {
        public static void ValideDados<T>(this T dto, IList<DtoCliente> lista) where T : class
        {
            if (dto == null) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.DADOS_NULOS);
            ValideNome(dto);
            ValideEmail(dto, lista);
            ValideContato(dto, lista);
        }

        public static void ValidePendencias(this DtoCliente dto, IList<DtoPedido> pedidos)
        {
            var codigo = dto.Codigo;

            var codigosDeClientes = new List<int>();

            foreach (var pedido in pedidos)
            {
                codigosDeClientes.Add(pedido.CodigoCliente);
            }

            if (codigosDeClientes.Contains(codigo))
            {
                throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.CLIENTE_COM_PENDENCIA);
            }
        }

        #region Métodos de Validação

        private static void ValideNome<T>(T dto) where T : class
        {
            var prop = dto.GetType().GetProperty("Nome").GetValue(dto);

            if (prop == null || prop.ToString() == "") throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.NOME_OBRIGATORIO);

            string nome = prop.ToString().RemovaEspacosDesnecessarios();

            if (nome.TemCaracteresEspeciais() || nome.TemNumeros()) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.NOME_CARACTERES_INVALIDOS);

            if (nome.Palavras() < 2) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.NOME_DUAS_PALAVRAS);

            if (nome.Length < 10 || nome.Length > 60) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.NOME_TAMANHO_INVALIDO);

            dto.GetType().GetProperty("Nome").SetValue(dto, nome);
        }

        private static void ValideEmail<T>(T dto, IList<DtoCliente> lista) where T : class
        {
            var prop = dto.GetType().GetProperty("Email").GetValue(dto);

            if (prop == null || prop.ToString() == "")
            {
                dto.GetType().GetProperty("Email").SetValue(dto, null);
            }
            else
            {
                string email = dto.GetType().GetProperty("Email").GetValue(dto).ToString();

                var padrao = @"^[^_\-.\s](?!.*?\.\.)[-a-z0-9.]+@[-a-z0-9]+(\.[-a-z]+)?\.[a-z]+(\.[a-z]+)?$";

                var regex = new Regex(padrao, RegexOptions.IgnoreCase);

                if (!regex.IsMatch(email)) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.EMAIL_INVALIDO);

                if (dto.GetType().GetProperty("Codigo") != null) RemoveDtoDaLista(dto, lista);

                if (email.JaExiste(lista, EnumPropriedadeDto.Email)) throw new ExcecaoDadosDuplicados(MensagensExcecaoCliente.EMAIL_DUPLICADO);
            }
        }

        private static void ValideContato<T>(T dto, IList<DtoCliente> lista) where T : class
        {
            var prop = dto.GetType().GetProperty("Contato").GetValue(dto);

            if (prop == null || prop.ToString() == "")
            {
                throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.CONTATO_OBRIGATORIO);
            }
            else
            {
                string contato = dto.GetType().GetProperty("Contato").GetValue(dto).ToString();

                if (!Regex.IsMatch(contato, @"^[0-9]{2}[0-9]{9}$") && !Regex.IsMatch(contato, @"^[0-9]{2}[0-9]{8}$")) throw new ExcecaoDadosInvalidos(MensagensExcecaoCliente.CONTATO_INVALIDO);

                if (dto.GetType().GetProperty("Codigo") != null) RemoveDtoDaLista(dto, lista);

                if (contato.JaExiste(lista, EnumPropriedadeDto.Contato)) throw new ExcecaoDadosDuplicados(MensagensExcecaoCliente.CONTATO_DUPLICADO);

            }
        }

        #endregion

        #region Métodos Auxiliares

        private static void RemoveDtoDaLista<T>(T dto, IList<DtoCliente> lista) where T : class
        {
            int codigo = (int)dto.GetType().GetProperty("Codigo").GetValue(dto);

            var cliente = FabricaDeDtoCliente.Crie();

            foreach (var item in lista)
            {
                if (codigo == item.Codigo)
                {
                    cliente = item;
                }
            }

            lista.Remove(cliente);
        }
        #endregion
    }
}
