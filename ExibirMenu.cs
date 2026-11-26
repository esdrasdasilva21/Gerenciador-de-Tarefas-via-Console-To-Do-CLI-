static void ExibirResumo(IEnumerable<TarefaItem>? tarefas)
{
    if (tarefas is null)
    {
        Console.WriteLine("\nResumo: nenhuma tarefa para exibir.");
        return;
    }

    int total = 0;
    int concluidas = 0;
    int atrasadas = 0;
    int pendentes = 0;

    foreach (var t in tarefas)
    {
        if (t is null) continue;
        total++;

        if (t.Concluida)
            concluidas++;
        else if (t.EstaAtrasada)
            atrasadas++;
        else
            pendentes++;
    }

    double percentual = total > 0 ? (concluidas * 100.0) / total : 0;

    Console.WriteLine();
    Console.WriteLine($"Resumo ({total} tarefa(s) - {percentual:0.#}% concluído):");
    Console.WriteLine($"  Concluídas : {concluidas}");
    Console.WriteLine($"  Pendentes  : {pendentes}");
    Console.WriteLine($"  Atrasadas  : {atrasadas}");
}