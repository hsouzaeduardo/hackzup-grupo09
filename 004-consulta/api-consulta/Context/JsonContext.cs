using System.Text.Json.Serialization;
using api_consulta.Model.mspersistence.Model; // Referência ao modelo Pessoa

[JsonSerializable(typeof(IEnumerable<Pessoa>))]
internal partial class JsonContext : JsonSerializerContext
{
}
