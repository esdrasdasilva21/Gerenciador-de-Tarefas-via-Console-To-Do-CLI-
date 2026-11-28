namespace TodoCli;

public sealed record TarefaItem(
    Guid Id,
    string Titulo,
    bool Concluida,
    DateTimeOffset CriadaEm)
{
    public static TarefaItem Nova(string titulo, DateTimeOffset? agora = null) =>
        new(
            Id: Guid.NewGuid(),
            Titulo: titulo.Trim(),
            Concluida: false,
            CriadaEm: agora ?? DateTimeOffset.UtcNow);
}
