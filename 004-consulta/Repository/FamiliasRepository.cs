namespace api_consulta.Repository
{
    using api_consulta.Model.mspersistence.Model;
    using Dapper;
    using Npgsql;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class FamiliasRepository
    {
        private readonly string _connectionString;

        public FamiliasRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método privado responsável por gerenciar a conexão com o banco
        private async Task<T> ExecuteAsync<T>(Func<IDbConnection, Task<T>> queryFunction)
        {
            using (var dbConnection = new NpgsqlConnection(_connectionString))
            {
                await dbConnection.OpenAsync();
                return await queryFunction(dbConnection);
            }
        }

        // Método para buscar todas as famílias
        public async Task<IList<Pessoa>> GetAllFamiliasAsync()
        {
            string query = "SELECT * FROM pessoa";

            return await ExecuteAsync(async dbConnection =>
            {
                var resultList = new List<Pessoa>();
                using (var command = new NpgsqlCommand(query, (NpgsqlConnection)dbConnection))
                {
                    await CreateResultObject(resultList, command);
                }
                return resultList;
            });
        }

        private static async Task CreateResultObject(List<Pessoa> resultList, NpgsqlCommand command)
        {
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var pessoa = new Pessoa
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Nome = reader.GetString(reader.GetOrdinal("nome")),
                        Documento = reader.GetString(reader.GetOrdinal("documento")),
                        DtNascimento = reader.IsDBNull(reader.GetOrdinal("dtnascimento")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("dtnascimento")),
                        Idade = reader.IsDBNull(reader.GetOrdinal("idade")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("idade")),
                        Nacionalidade = reader.GetString(reader.GetOrdinal("nacionalidade")),
                        NumeroFilhos = reader.IsDBNull(reader.GetOrdinal("numerofilhos")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("numerofilhos")),
                        Cidade = reader.GetString(reader.GetOrdinal("cidade")),
                        Bairro = reader.GetString(reader.GetOrdinal("bairro")),
                        Situacao = reader.GetString(reader.GetOrdinal("situacao"))
                    };

                    resultList.Add(pessoa);
                }
            }
        }

        // Método para buscar famílias por cidade
        public async Task<IEnumerable<Pessoa>> GetFamiliasByCidadeAsync(string cidade)
        {
            string query = "SELECT * FROM pessoa WHERE cidade = @Cidade";
            return await ExecuteAsync(async dbConnection =>
            {
                var resultList = new List<Pessoa>();
                using (var command = new NpgsqlCommand(query, (NpgsqlConnection)dbConnection))
                {
                    command.Parameters.AddWithValue("@Cidade", cidade);

                    await CreateResultObject(resultList, command);
                }
                return resultList;
            });
        }

        // Método para buscar famílias por idade
        public async Task<IEnumerable<Pessoa>> GetFamiliasByIdadeAsync(int idade)
        {
            string query = "SELECT * FROM pessoa WHERE idade = @Idade";
            return await ExecuteAsync(async dbConnection =>
            {
                var resultList = new List<Pessoa>();
                using (var command = new NpgsqlCommand(query, (NpgsqlConnection)dbConnection))
                {
                    command.Parameters.AddWithValue("@Idade", idade);

                    await CreateResultObject(resultList, command);
                }
                return resultList;
            });
        }

        // Método para buscar famílias por situação
        public async Task<IEnumerable<Pessoa>> GetFamiliasBySituacaoAsync(string situacao)
        {
            string query = "SELECT * FROM pessoa WHERE situacao = @Situacao";

            return await ExecuteAsync(async dbConnection =>
            {
                var resultList = new List<Pessoa>();
                using (var command = new NpgsqlCommand(query, (NpgsqlConnection)dbConnection))
                {
                    command.Parameters.AddWithValue("@Situacao", situacao);

                    await CreateResultObject(resultList, command);
                }
                return resultList;
            });
        }
    }
}
