static void ExibirResumo(IEnumerable<TarefaItem>? tarefas)
{
    if (tarefas is null)
    {
        Console.WriteLine("\nResumo: lista indisponível.");
        return;
    }

    int total = 0;
    int concluidas = 0;
    int atrasadas = 0;

    foreach (var tarefa in tarefas)
    {
        if (tarefa is null) continue;
        total++;
        if (tarefa.Concluida) concluidas++;
        if (tarefa.EstaAtrasada) atrasadas++;
    }

    double percentual = total > 0 ? (double)concluidas / total * 100 : 0;
    Console.WriteLine($"\nResumo: {concluidas}/{total} ({percentual:F0}%) concluídas | {atrasadas} atrasada(s)");
}