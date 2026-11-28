namespace TodoCli;

public enum ResultadoOperacao
{
    Sucesso,
    NaoEncontrado,
    Conflito
}

public sealed record TarefaItem(
    Guid Id,
    string Titulo,
    bool Concluida,
    DateTimeOffset CriadaEm);