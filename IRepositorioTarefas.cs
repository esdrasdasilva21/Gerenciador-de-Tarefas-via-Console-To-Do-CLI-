namespace TodoCli;

public enum PrioridadeTarefa
{
    Baixa = 0,
    Media = 1,
    Alta = 2
}

public sealed record TarefaItem(
    Guid Id,
    string Titulo,
    bool Concluida,
    PrioridadeTarefa Prioridade,
    DateTimeOffset CriadaEm,
    DateTimeOffset? ConcluidaEm);