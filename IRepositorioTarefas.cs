namespace TodoCli;

public interface IRepositorioTarefas
{
    List<TarefaItem> Carregar();
    void Salvar(List<TarefaItem> tarefas);
}