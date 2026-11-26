static void ExibirResumo(List<TarefaItem>? tarefas)
{
    if (tarefas is null || tarefas.Count == 0)
    {
        Console.WriteLine("\nResumo: nenhuma tarefa encontrada.");
        return;
    }

    int total = tarefas.Count;
    int concluidas = tarefas.Count(t => t.Concluida);
    int atrasadas = tarefas.Count(t => t.EstaAtrasada);

    Console.WriteLine($"\nResumo: {concluidas}/{total} concluídas | {atrasadas} atrasada(s)");
}