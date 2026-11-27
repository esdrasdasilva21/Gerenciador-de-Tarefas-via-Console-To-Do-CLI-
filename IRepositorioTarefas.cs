namespace TodoCli;

public interface IRepositorioTarefas
{
    Task<IReadOnlyList<TarefaItem>> CarregarAsync(CancellationToken cancellationToken = default);
    Task SalvarAsync(IReadOnlyList<TarefaItem> tarefas, CancellationToken cancellationToken = default);
}