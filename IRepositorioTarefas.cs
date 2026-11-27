namespace TodoCli;

public enum ResultadoOperacao
{
    Sucesso,
    NaoEncontrado,
    Conflito
}

public interface IRepositorioTarefas
{
    Task<IReadOnlyList<TarefaItem>> ListarAsync(
        bool? apenasConcluidas = null,
        string? termoBusca = null,
        CancellationToken cancellationToken = default);

    Task<TarefaItem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ResultadoOperacao> AdicionarAsync(TarefaItem tarefa, CancellationToken cancellationToken = default);
    Task<ResultadoOperacao> AtualizarAsync(TarefaItem tarefa, CancellationToken cancellationToken = default);
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}