namespace TodoCli;

public enum ResultadoOperacao
{
    Sucesso,
    NaoEncontrado,
    Conflito,
    Invalido
}

public enum PrioridadeTarefa
{
    Baixa = 0,
    Media = 1,
    Alta = 2
}

public enum OrdenacaoTarefa
{
    CriacaoDesc,
    CriacaoAsc,
    PrioridadeDesc,
    TituloAsc
}

public sealed record TarefaItem
{
    public Guid Id { get; init; }
    public string Titulo { get; init; }
    public bool Concluida { get; init; }
    public PrioridadeTarefa Prioridade { get; init; }
    public DateTimeOffset CriadaEm { get; init; }
    public DateTimeOffset? ConcluidaEm { get; init; }

    public TarefaItem(
        Guid id,
        string titulo,
        bool concluida,
        PrioridadeTarefa prioridade,
        DateTimeOffset criadaEm,
        DateTimeOffset? concluidaEm)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID da tarefa não pode ser vazio.", nameof(id));

        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.", nameof(titulo));

        if (concluida && concluidaEm is null)
            throw new ArgumentException("Tarefas concluídas devem ter data de conclusão.", nameof(concluidaEm));

        if (!concluida && concluidaEm is not null)
            throw new ArgumentException("Tarefas não concluídas não devem ter data de conclusão.", nameof(concluidaEm));

        Id = id;
        Titulo = titulo.Trim();
        Concluida = concluida;
        Prioridade = prioridade;
        CriadaEm = criadaEm;
        ConcluidaEm = concluidaEm;
    }

    public static TarefaItem Criar(
        string titulo,
        PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
        DateTimeOffset? agora = null) =>
        new(
            id: Guid.NewGuid(),
            titulo: titulo,
            concluida: false,
            prioridade: prioridade,
            criadaEm: agora ?? DateTimeOffset.UtcNow,
            concluidaEm: null);

    public TarefaItem Concluir(DateTimeOffset? agora = null) =>
        Concluida
            ? this
            : this with { Concluida = true, ConcluidaEm = agora ?? DateTimeOffset.UtcNow };

    public TarefaItem Reabrir() =>
        !Concluida
            ? this
            : this with { Concluida = false, ConcluidaEm = null };
}

public sealed record FiltroTarefas(
    bool? ApenasConcluidas = null,
    PrioridadeTarefa? PrioridadeMinima = null,
    string? TermoBusca = null,
    OrdenacaoTarefa OrdenarPor = OrdenacaoTarefa.CriacaoDesc,
    int? Limite = null)
{
    public static readonly FiltroTarefas Todos = new();
}

public sealed record ResultadoConsulta<T>(
    IReadOnlyList<T> Itens,
    int TotalEncontrado);

public interface IRepositorioTarefas
{
    Task<ResultadoConsulta<TarefaItem>> ListarAsync(
        FiltroTarefas? filtro = null,
        CancellationToken cancellationToken = default);

    Task<TarefaItem?> ObterPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ResultadoOperacao> AdicionarAsync(
        TarefaItem tarefa,
        CancellationToken cancellationToken = default);

    Task<ResultadoOperacao> AtualizarAsync(
        TarefaItem tarefa,
        CancellationToken cancellationToken = default);

    Task<ResultadoOperacao> RemoverAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}