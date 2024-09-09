namespace api_consulta.Model
{
    namespace mspersistence.Model
    {
        public class Pessoa
        {

            public int Id { get; set; }
            public string Nome { get; set; }
            public string Documento { get; set; }
            public DateTime? DtNascimento { get; set; }
            public int? Idade { get; set; } // Tipo anulável
            public string Nacionalidade { get; set; }
            public int? NumeroFilhos { get; set; } // Tipo anulável
            public string Cidade { get; set; }
            public string Bairro { get; set; }
            public string Situacao { get; set; }
        }
    }

}


