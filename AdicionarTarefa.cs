public sealed record NovaTarefaInput(
    string Titulo,
    PrioridadeTarefa Prioridade,
    string Categoria,
    DateOnly? DataLimite);