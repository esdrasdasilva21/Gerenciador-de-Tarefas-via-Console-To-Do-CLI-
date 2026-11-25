using TodoCli;

namespace TodoCli.Tests;

public class RepositorioMemoriaFake : IRepositorioTarefas
{
    public List<TarefaItem> Dados { get; set; } = new();
    public int ContagemSalvamentos { get; private set; }

    public List<TarefaItem> Carregar()
    {
        return Dados.Select(t => new TarefaItem(
            t.Id,
            t.Titulo,
            t.Prioridade,
            t.Categoria,
            t.DataLimite,
            t.Concluida
        )).ToList();
    }

    public void Salvar(List<TarefaItem> tarefas)
    {
        ContagemSalvamentos++;
        Dados = tarefas.Select(t => new TarefaItem(
            t.Id,
            t.Titulo,
            t.Prioridade,
            t.Categoria,
            t.DataLimite,
            t.Concluida
        )).ToList();
    }
}