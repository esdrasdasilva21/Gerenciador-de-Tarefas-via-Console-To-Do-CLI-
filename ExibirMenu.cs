static void ExibirResumo(List<TarefaItem> tarefas)
{
    int total = tarefas.Count;
    int concluidas = tarefas.Count(t => t.Concluida);
    int atrasadas = tarefas.Count(t => t.EstaAtrasada);

    Console.WriteLine($"\nResumo: {concluidas}/{total} concluídas | {atrasadas} atrasada(s)");
}