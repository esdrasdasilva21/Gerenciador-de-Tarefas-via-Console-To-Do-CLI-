static void AdicionarTarefa(List<TarefaItem> tarefas, ref int proximoId)
{
    Console.Write("Digite o título da nova tarefa: ");
    string? titulo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(titulo))
    {
        Console.WriteLine("O título não pode ficar vazio.");
        return;
    }

    Console.Write("Prioridade (1 - Baixa, 2 - Média, 3 - Alta) [padrão 2]: ");
    string? entradaPrioridade = Console.ReadLine();

    PrioridadeTarefa prioridade = entradaPrioridade switch
    {
        "1" => PrioridadeTarefa.Baixa,
        "3" => PrioridadeTarefa.Alta,
        _ => PrioridadeTarefa.Media
    };

    Console.Write("Categoria (ex.: Estudo, Trabalho, Projeto) [padrão Geral]: ");
    string? entradaCategoria = Console.ReadLine();
    string categoria = string.IsNullOrWhiteSpace(entradaCategoria)
        ? "Geral"
        : entradaCategoria.Trim();

    Console.Write("Data limite (dd/MM/yyyy) ou Enter para sem prazo: ");
    string? entradaData = Console.ReadLine();
    DateOnly? dataLimite = null;

    if (!string.IsNullOrWhiteSpace(entradaData))
    {
        if (DateOnly.TryParseExact(
            entradaData.Trim(),
            new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
            out DateOnly dataConvertida))
        {
            dataLimite = dataConvertida;
        }
        else
        {
            Console.WriteLine("Data inválida. A tarefa será salva sem data limite.");
        }
    }

    tarefas.Add(new TarefaItem(proximoId, titulo.Trim(), prioridade, categoria, dataLimite));
    proximoId++;
    Console.WriteLine($"Tarefa adicionada na categoria \"{categoria}\"!");
}