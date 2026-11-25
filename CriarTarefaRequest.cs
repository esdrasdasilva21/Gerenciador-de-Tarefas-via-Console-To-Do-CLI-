using TodoCli;

namespace TaskFlow.Api;

public record CriarTarefaRequest(
    string Titulo,
    PrioridadeTarefa Prioridade = PrioridadeTarefa.Media,
    string Categoria = "Geral",
    DateOnly? DataLimite = null
);