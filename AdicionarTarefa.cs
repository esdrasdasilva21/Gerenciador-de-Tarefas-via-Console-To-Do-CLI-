static void AdicionarTarefa(List<TarefaItem> tarefas, ref int proximoId)
{
    ArgumentNullException.ThrowIfNull(tarefas);

    Console.Write("Digite o título da nova tarefa: ");
    string? titulo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(titulo))
    {
        Console.WriteLine("O título não pode ficar vazio.");
        return;
    }

    // ... restante do fluxo ...

    tarefas.Add(new TarefaItem(proximoId, titulo.Trim(), prioridade, categoria, dataLimite));
    proximoId++;
    Console.WriteLine($"Tarefa adicionada na categoria \"{categoria}\"!");
}