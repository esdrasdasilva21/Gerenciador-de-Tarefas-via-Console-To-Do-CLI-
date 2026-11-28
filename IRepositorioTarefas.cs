namespace TodoCli;

public sealed record TarefaItem
{
    public Guid Id { get; init; }
    public string Titulo { get; init; }
    public bool Concluida { get; init; }
    public DateTimeOffset CriadaEm { get; init; }

    public TarefaItem(Guid id, string titulo, bool concluida, DateTimeOffset criadaEm)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID da tarefa não pode ser vazio.", nameof(id));

        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.", nameof(titulo));

        Id = id;
        Titulo = titulo.Trim();
        Concluida = concluida;
        CriadaEm = criadaEm;
    }

    public TarefaItem MarcarConcluida() => this with { Concluida = true };
    public TarefaItem Reabrir() => this with { Concluida = false };
    public TarefaItem Renomear(string novoTitulo) => this with { Titulo = ValidarTitulo(novoTitulo) };

    private static string ValidarTitulo(string titulo) =>
        string.IsNullOrWhiteSpace(titulo)
            ? throw new ArgumentException("O título da tarefa é obrigatório.", nameof(titulo))
            : titulo.Trim();
}