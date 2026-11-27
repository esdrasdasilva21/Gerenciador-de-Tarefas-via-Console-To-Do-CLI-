namespace TodoCli;

public interface IRepositorioTarefas
{
    IReadOnlyList<TarefaItem> Carregar();
    void Salvar(IReadOnlyList<TarefaItem> tarefas);
}