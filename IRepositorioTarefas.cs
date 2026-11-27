namespace TodoCli;

public interface IRepositorioTarefas
{
    Task<IReadOnlyList<TarefaItem>> CarregarAsync();
    Task SalvarAsync(IReadOnlyList<TarefaItem> tarefas);
}