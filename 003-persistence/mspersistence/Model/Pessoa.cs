namespace Model
{
    public class Pessoa
    {
        public int Id { get; set; }  // Chave primária

        public string Nacionalidade { get; set; }  // Nacionalidade da pessoa

        public int NumeroFilhos { get; set; }  // Número de filhos

        public string Documento { get; set; }  // Documento (deve ser único e não nulo)

        public DateTime? DtNascimento { get; set; }  // Data de nascimento (nullable)

        public string Nome { get; set; }  // Nome da pessoa (não nulo)

        public string Situacao { get; set; }  // Situação (ex: ativo, inativo)

        public int? Idade { get; set; }  // Idade (nullable, pois pode ser derivado de dtNascimento)

        public string Cidade { get; set; }  // Cidade

        public string Bairro { get; set; }  // Bairro
    }

}
