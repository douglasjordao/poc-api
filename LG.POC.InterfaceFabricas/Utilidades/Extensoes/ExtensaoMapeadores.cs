using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace LG.POC.InterfaceFabricas.Utilidades.Extensoes
{
    public static class ExtensaoMapeadores
    {
        public static void AdicioneParametros<T>(this SqlCommand comando, T dto) where T : class
        {
            var propriedades = dto.GetType().GetProperties();

            var parametros = Regex.Matches(comando.CommandText, @"@[A-Z]+");

            foreach (var param in parametros)
            {
                foreach (var prop in propriedades)
                {
                    if (prop.Name.ToUpper() == param.ToString().Remove(0, 1))
                    {
                        var valor = (dto.GetType().GetProperty(prop.Name).GetValue(dto) == null)
                            ? DBNull.Value
                            : dto.GetType().GetProperty(prop.Name).GetValue(dto);

                        comando.Parameters.AddWithValue(param.ToString(), valor);

                        break;
                    }
                }
            }
        }
    }
}
