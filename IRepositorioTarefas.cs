namespace TodoCli;

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