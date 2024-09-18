using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
public class AzureOpenAIHelper
{

    private readonly string _deploymentName;  // O nome do deployment no Azure
    private readonly AzureOpenAIClient _client;  // O cliente do Azure OpenAI
    private readonly ChatClient _chatClient;
    private readonly string _criptKey;  
    private readonly string criptKeyIV;
    public AzureOpenAIHelper(string endpoint, string apiKey, string deploymentName, string criptKey, string IV)
    {
        AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        _client  = new(new Uri(endpoint), credential);      
        _chatClient = _client.GetChatClient(deploymentName);
        _criptKey = criptKey;
        criptKeyIV = IV;

    }

   public async Task<string> GenerateCompletionAsync(string csvContent)
    {
        var prompt = $"Converta o seguinte conteúdo CSV em comandos SQL INSERT para uma tabela de banco de dados POSTGRES, CONSIDERE QUE A TABELA JÁ EXISTE:\n\n{csvContent}\n\n Considerar os seguintes campos na tabela pessoa (Nacionalidade, NumeroFilhos, Documento, dtNascimento, Nome, Situacao, Idade, Cidade, Bairro) Comandos SQL";

        ChatCompletion completion = _chatClient.CompleteChat(
        [
        new SystemChatMessage("Você é um assistente de Conversão de arquivos para SQL sem trabalha apenas os scripts"),
        new UserChatMessage(prompt)
        ]);

        Console.WriteLine($"{completion.Role}: {completion.Content[0].Text}");

    
        return completion.Content[0].Text.ExtractSqlFromResponse().Encrypt(_criptKey, criptKeyIV);

    }
}
