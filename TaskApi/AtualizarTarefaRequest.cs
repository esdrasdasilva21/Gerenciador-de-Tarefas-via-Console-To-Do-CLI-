using TodoCli;

namespace TaskFlow.Api;

public record AtualizarTarefaRequest(
    string Titulo,
    PrioridadeTarefa Prioridade = PrioridadeTarefa.Media,
    string Categoria = "Geral",
    DateOnly? DataLimite = null,
    bool Concluida = false
);