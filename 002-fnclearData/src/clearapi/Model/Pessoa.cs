
public class Pessoa
{
    public int Id { get; set; }  // Chave prim�ria

    public string Nacionalidade { get; set; }  // Nacionalidade da pessoa

    public int NumeroFilhos { get; set; }  // N�mero de filhos

    public string Documento { get; set; }  // Documento (deve ser �nico e n�o nulo)

    public DateTime? DtNascimento { get; set; }  // Data de nascimento (nullable)

    public string Nome { get; set; }  // Nome da pessoa (n�o nulo)

    public string Situacao { get; set; }  // Situa��o (ex: ativo, inativo)

    public int? Idade { get; set; }  // Idade (nullable, pois pode ser derivado de dtNascimento)

    public string Cidade { get; set; }  // Cidade

    public string Bairro { get; set; }  // Bairro
}
