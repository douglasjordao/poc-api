using LG.POC.InterfaceFabricas.Utilidades.Extensoes;
using System;
using System.Reflection;

namespace LG.POC.InterfaceFabricas.Fabricas
{
    public static class FabricaGenerica
    {
        #region Métodos Principais

        public static T Crie<T>() where T : class
        {
            return (T)Activator.CreateInstance(DefineTipoDaClasse<T>());
        }

        public static T Crie<T>(params object[] args) where T : class
        {
            return (T)Activator.CreateInstance(DefineTipoDaClasse<T>(), args);
        }
        #endregion

        #region Métodos Privados
        private static Type DefineTipoDaClasse<T>() where T : class
        {

            if (typeof(T).EhMapeador()) return CarregueAssembly().GetType($"LG.POC.ServicoMapeadores.Mapeadores.{typeof(T).Name.Remove(0, 1)}");

            if (typeof(T).EhServico()) return CarregueAssembly().GetType($"LG.POC.ServicoMapeadores.Servicos.ContratosImplementados.{typeof(T).Name + "Impl"}");

            return typeof(T);
        }

        private static Assembly CarregueAssembly()
        {
            return Assembly.Load("LG.POC.ServicoMapeadores, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
        }
        #endregion
    }
}
