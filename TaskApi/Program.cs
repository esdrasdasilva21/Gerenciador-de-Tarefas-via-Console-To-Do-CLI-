using TaskFlow.Api;
using TodoCli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepositorioTarefas>(_ => new RepositorioTarefas("tarefas.json"));
builder.Services.AddSingleton<ServicoTarefas>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    aplicacao = "TaskFlow API",
    versao = "1.0",
    endpoints = new[]
    {
        "GET /tarefas",
        "GET /tarefas/pendentes",
        "GET /tarefas/concluidas",
        "GET /tarefas/atrasadas",
        "GET /tarefas/categoria/{categoria}",
        "GET /tarefas/buscar?termo=...",
        "POST /tarefas",
        "PATCH /tarefas/{id}/concluir",
        "DELETE /tarefas/{id}"
    }
}));

app.MapGet("/tarefas", (ServicoTarefas servico) =>
    Results.Ok(servico.ListarTodas()));

app.MapGet("/tarefas/pendentes", (ServicoTarefas servico) =>
    Results.Ok(servico.FiltrarPendentes()));

app.MapGet("/tarefas/concluidas", (ServicoTarefas servico) =>
    Results.Ok(servico.FiltrarConcluidas()));

app.MapGet("/tarefas/atrasadas", (ServicoTarefas servico) =>
    Results.Ok(servico.FiltrarAtrasadas()));

app.MapGet("/tarefas/categoria/{categoria}", (string categoria, ServicoTarefas servico) =>
    Results.Ok(servico.FiltrarPorCategoria(categoria)));

app.MapGet("/tarefas/buscar", (string? termo, ServicoTarefas servico) =>
{
    if (string.IsNullOrWhiteSpace(termo))
    {
        return Results.BadRequest(new { erro = "Informe o parâmetro 'termo' na query string." });
    }

    return Results.Ok(servico.BuscarPorPalavraChave(termo));
});

app.MapPost("/tarefas", (CriarTarefaRequest request, ServicoTarefas servico) =>
{
    try
    {
        TarefaItem criada = servico.Adicionar(
            request.Titulo,
            request.Prioridade,
            request.Categoria,
            request.DataLimite
        );

        return Results.Created($"/tarefas/{criada.Id}", criada);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { erro = ex.Message });
    }
});

app.MapPatch("/tarefas/{id:int}/concluir", (int id, ServicoTarefas servico) =>
{
    TarefaItem? atualizada = servico.AlternarConclusao(id);
    if (atualizada is null)
    {
        return Results.NotFound(new { erro = $"Tarefa #{id} não encontrada." });
    }

    return Results.Ok(atualizada);
});

app.MapDelete("/tarefas/{id:int}", (int id, ServicoTarefas servico) =>
{
    bool removida = servico.RemoverPorId(id);
    if (!removida)
    {
        return Results.NotFound(new { erro = $"Tarefa #{id} não encontrada." });
    }

    return Results.NoContent();
});

app.Run();