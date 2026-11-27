namespace TodoCli;

public interface IRepositorioTarefas
{
    Task<IReadOnlyList<TarefaItem>> ListarAsync(
        bool? apenasConcluidas = null,
        string? termoBusca = null,
        CancellationToken cancellationToken = default);

    Task<TarefaItem?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AdicionarAsync(TarefaItem tarefa, CancellationToken cancellationToken = default);
    Task AtualizarAsync(TarefaItem tarefa, CancellationToken cancellationToken = default);
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}