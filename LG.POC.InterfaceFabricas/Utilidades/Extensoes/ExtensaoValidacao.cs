using LG.POC.InterfaceFabricas.Enumeradores;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LG.POC.InterfaceFabricas.Utilidades.Extensoes
{
    public static class ExtensaoValidacao
    {
        public static bool JaExiste<T>(this int codigo, IList<T> lista) where T : class
        {
            IList<int> codigos = new List<int>();

            foreach (var item in lista)
            {
                codigos.Add((int)item.GetType().GetProperty("Codigo").GetValue(item));
            }

            return codigos.Contains(codigo);
        }

        public static bool JaExiste<T>(this string texto, IList<T> lista, EnumPropriedadeDto propriedade) where T : class
        {
            IList<string> textos = new List<string>();

            foreach (var item in lista)
            {
                switch (propriedade)
                {
                    case EnumPropriedadeDto.Descricao:
                        textos.Add(item.GetType().GetProperty("Descricao").GetValue(item).ToString());

                        break;
                    case EnumPropriedadeDto.Nome:
                        textos.Add(item.GetType().GetProperty("Nome").GetValue(item).ToString());

                        break;
                    case EnumPropriedadeDto.Email:
                        var email = item.GetType().GetProperty("Email").GetValue(item);
                        
                        if (email != null) textos.Add(email.ToString());

                        break;
                    case EnumPropriedadeDto.Contato:
                        textos.Add(item.GetType().GetProperty("Contato").GetValue(item).ToString());

                        break;
                }
            }

            return textos.Contains(texto);
        }

        public static string RemovaEspacosDesnecessarios(this string nome)
        {
            nome = nome.Trim(' ');

            nome = Regex.Replace(nome, @"\s\s+", " ");

            return nome;
        }

        public static bool TemCaracteresEspeciais(this string texto)
        {
            if (!Regex.IsMatch(texto, @"^[a-zA-Z0-9à-úÀ-Ú\s]+$")) return true;

            return false;
        }

        public static bool TemNumeros(this string texto)
        {
            if (Regex.IsMatch(texto, @"\d+")) return true;

            return false;
        }

        public static bool EhVazioOuNulo(this string texto)
        {
            if (texto == null) return true;

            texto = RemovaEspacosDesnecessarios(texto);

            if (texto.Length == 0) return true;

            return false;
        }

        public static int Palavras(this string texto)
        {
            return texto.Split(' ').Length;
        }
    }
}
