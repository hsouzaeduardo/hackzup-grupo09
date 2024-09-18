using System.Text.Json;

public class Serializer
{
    public string SerializarParaJson(List<Pessoa> pessoas)
    {
        return JsonSerializer.Serialize(pessoas);
    }
}
