namespace TodoCli;

public sealed record TarefaItem(
    Guid Id,
    string Titulo,
    bool Concluida,
    DateTimeOffset CriadaEm)
{
    public static TarefaItem Criar(string titulo, DateTimeOffset? agora = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.", nameof(titulo));

        return new TarefaItem(
            Id: Guid.NewGuid(),
            Titulo: titulo.Trim(),
            Concluida: false,
            CriadaEm: agora ?? DateTimeOffset.UtcNow);
    }

    public TarefaItem Concluir() => this with { Concluida = true };
    public TarefaItem Reabrir() => this with { Concluida = false };
}