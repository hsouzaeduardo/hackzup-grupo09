using api_consulta.Model.mspersistence.Model;
using api_consulta.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("familias")]
public class FamiliasController : ControllerBase
{
    private readonly FamiliasRepository _repository;

    public FamiliasController(FamiliasRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFamilias()
    {
        var familias = await _repository.GetAllFamiliasAsync();

        var json = JsonSerializer.Serialize(familias.ToList(), JsonContext.Default.IEnumerablePessoa);

        return new ContentResult
        {
            Content = json,
            ContentType = "application/json",
            StatusCode = 200
        };
    }


    [HttpGet("cidade")]
    public async Task<IActionResult> GetFamiliasByCidade([FromQuery] string cidade)
    {
        var familias = await _repository.GetFamiliasByCidadeAsync(cidade);

        var json = JsonSerializer.Serialize(familias.ToList(), JsonContext.Default.IEnumerablePessoa);

        return new ContentResult
        {
            Content = json,
            ContentType = "application/json",
            StatusCode = 200
        };

    }

    [HttpGet("idade")]
    public async Task<IActionResult> GetFamiliasByIdade([FromQuery] int idade)
    {
        var familias = await _repository.GetFamiliasByIdadeAsync(idade);

        var json = JsonSerializer.Serialize(familias.ToList(), JsonContext.Default.IEnumerablePessoa);

        return new ContentResult
        {
            Content = json,
            ContentType = "application/json",
            StatusCode = 200
        };
    }

    [HttpGet("situacao")]
    public async Task<IActionResult> GetFamiliasBySituacao([FromQuery] string situacao)
    {
        var familias = await _repository.GetFamiliasBySituacaoAsync(situacao);

        var json = JsonSerializer.Serialize(familias.ToList(), JsonContext.Default.IEnumerablePessoa);

        return new ContentResult
        {
            Content = json,
            ContentType = "application/json",
            StatusCode = 200
        };
    }
}
