using TaskFlow.Api;
using TodoCli;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IRepositorioTarefas>(_ => new RepositorioTarefas("tarefas.json"));
builder.Services.AddSingleton<ServicoTarefas>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    aplicacao = "TaskFlow API",
    versao = "1.1",
    endpoints = new[]
    {
        "GET /tarefas?ordenarPor=prioridade&pagina=1&tamanho=10",
        "GET /tarefas/pendentes",
        "GET /tarefas/concluidas",
        "GET /tarefas/atrasadas",
        "GET /tarefas/categoria/{categoria}",
        "GET /tarefas/buscar?termo=...",
        "POST /tarefas",
        "PUT /tarefas/{id}",
        "PATCH /tarefas/{id}/concluir",
        "DELETE /tarefas/{id}"
    }
}));

app.MapGet("/tarefas", (
    string? ordenarPor,
    int pagina,
    int tamanho,
    ServicoTarefas servico) =>
{
    int paginaAtual = pagina < 1 ? 1 : pagina;
    int tamanhoPagina = tamanho < 1 ? 10 : Math.Min(tamanho, 50);

    IEnumerable<TarefaItem> consulta = ordenarPor?.Trim().ToLowerInvariant() switch
    {
        "prioridade" => servico.OrdenarPorPrioridade(),
        "status" => servico.OrdenarPorStatusEPrioridade(),
        _ => servico.ListarTodas()
    };

    List<TarefaItem> listaCompleta = consulta.ToList();
    List<TarefaItem> itensPagina = listaCompleta
        .Skip((paginaAtual - 1) * tamanhoPagina)
        .Take(tamanhoPagina)
        .ToList();

    return Results.Ok(new
    {
        pagina = paginaAtual,
        tamanho = tamanhoPagina,
        totalItens = listaCompleta.Count,
        totalPaginas = (int)Math.Ceiling(listaCompleta.Count / (double)tamanhoPagina),
        itens = itensPagina
    });
});

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
    var erros = ValidarTarefa(request.Titulo, request.Prioridade);
    if (erros.Count > 0)
    {
        return Results.ValidationProblem(erros);
    }

    TarefaItem criada = servico.Adicionar(
        request.Titulo,
        request.Prioridade,
        request.Categoria,
        request.DataLimite
    );

    return Results.Created($"/tarefas/{criada.Id}", criada);
});

app.MapPut("/tarefas/{id:int}", (int id, AtualizarTarefaRequest request, ServicoTarefas servico) =>
{
    var erros = ValidarTarefa(request.Titulo, request.Prioridade);
    if (erros.Count > 0)
    {
        return Results.ValidationProblem(erros);
    }

    TarefaItem? atualizada = servico.Atualizar(
        id,
        request.Titulo,
        request.Prioridade,
        request.Categoria,
        request.DataLimite,
        request.Concluida
    );

    if (atualizada is null)
    {
        return Results.NotFound(new { erro = $"Tarefa #{id} não encontrada." });
    }

    return Results.Ok(atualizada);
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

static Dictionary<string, string[]> ValidarTarefa(string titulo, PrioridadeTarefa prioridade)
{
    Dictionary<string, string[]> erros = new();

    if (string.IsNullOrWhiteSpace(titulo))
    {
        erros["Titulo"] = new[] { "O título da tarefa é obrigatório." };
    }
    else if (titulo.Trim().Length > 120)
    {
        erros["Titulo"] = new[] { "O título deve ter no máximo 120 caracteres." };
    }

    if (!Enum.IsDefined(typeof(PrioridadeTarefa), prioridade))
    {
        erros["Prioridade"] = new[] { "Prioridade inválida. Use Baixa (1), Media (2) ou Alta (3)." };
    }

    return erros;
}