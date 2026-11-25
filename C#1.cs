string nomeApp = "TaskFlow C#";
string versao = "0.2";

Console.WriteLine($"=== {nomeApp} (v{versao}) ===");
Console.Write("Digite seu nome: ");
string? nomeUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(nomeUsuario))
{
    nomeUsuario = "Desenvolvedor";
}

Console.WriteLine($"Olá, {nomeUsuario}! Vamos organizar suas tarefas.");

List<string> tarefas = new()
{
    "Estudar sintaxe básica de C#",
    "Configurar o ambiente .NET"
};

bool executando = true;

while (executando)
{
    Console.WriteLine("\n--- Menu ---");
    Console.WriteLine("1 - Listar tarefas");
    Console.WriteLine("2 - Adicionar tarefa");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");

    string? opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            Console.WriteLine("\nSuas tarefas:");
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada.");
            }
            else
            {
                for (int i = 0; i < tarefas.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {tarefas[i]}");
                }
            }
            break;

        case "2":
            Console.Write("Digite a nova tarefa: ");
            string? novaTarefa = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(novaTarefa))
            {
                tarefas.Add(novaTarefa.Trim());
                Console.WriteLine("Tarefa adicionada com sucesso!");
            }
            else
            {
                Console.WriteLine("A tarefa não pode ficar vazia.");
            }
            break;


            // Adicione a opção "3 - Remover tarefa" no menu e trate no switch:
case "3":
    if (tarefas.Count == 0)
    {
        Console.WriteLine("Não há tarefas para remover.");
        break;
    }

    for (int i = 0; i < tarefas.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {tarefas[i]}");
    }

    Console.Write("Digite o número da tarefa para remover: ");
    string? entradaNumero = Console.ReadLine();

    if (int.TryParse(entradaNumero, out int numero) && numero >= 1 && numero <= tarefas.Count)
    {
        string removida = tarefas[numero - 1];
        tarefas.RemoveAt(numero - 1);
        Console.WriteLine($"Tarefa \"{removida}\" removida.");
    }
    else
    {
        Console.WriteLine("Número inválido.");
    }
    break;

    // No menu:
Console.WriteLine("5 - Filtrar tarefas (pendentes / concluídas)");

// No switch:
case "5":
    Console.WriteLine("1 - Apenas pendentes");
    Console.WriteLine("2 - Apenas concluídas");
    Console.Write("Escolha o filtro: ");
    string? filtro = Console.ReadLine();

    IEnumerable<TarefaItem> filtradas = filtro switch
    {
        "1" => tarefas.Where(t => !t.Concluida),
        "2" => tarefas.Where(t => t.Concluida),
        _ => Enumerable.Empty<TarefaItem>()
    };

    List<TarefaItem> resultadoFiltro = filtradas.ToList();

    if (filtro is not ("1" or "2"))
    {
        Console.WriteLine("Filtro inválido.");
    }
    else if (resultadoFiltro.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa encontrada para esse filtro.");
    }
    else
    {
        Console.WriteLine("\nResultado:");
        foreach (TarefaItem item in resultadoFiltro)
        {
            Console.WriteLine(item);
        }
    }


    // No menu:
Console.WriteLine("6 - Buscar por palavra-chave");

// No switch:
case "6":
    Console.Write("Digite a palavra ou trecho para buscar: ");
    string? termo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(termo))
    {
        Console.WriteLine("Digite um termo válido para busca.");
        break;
    }

    string termoLimpo = termo.Trim();
    List<TarefaItem> encontradas = tarefas
        .Where(t => t.Titulo.Contains(termoLimpo, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (encontradas.Count == 0)
    {
        Console.WriteLine($"Nenhuma tarefa contém \"{termoLimpo}\".");
    }
    else
    {
        Console.WriteLine($"\nTarefas encontradas para \"{termoLimpo}\":");
        foreach (TarefaItem item in encontradas)
        {
            Console.WriteLine(item);
        }
    }
    break;

    // No menu:
Console.WriteLine("7 - Ordenar tarefas");

// No switch:
case "7":
    if (tarefas.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa para ordenar.");
        break;
    }

    Console.WriteLine("1 - Por prioridade (Alta -> Baixa)");
    Console.WriteLine("2 - Por status (Pendentes primeiro)");
    Console.Write("Escolha a ordenação: ");
    string? ordem = Console.ReadLine();

    IEnumerable<TarefaItem> ordenadas = ordem switch
    {
        "1" => tarefas
            .OrderByDescending(t => t.Prioridade)
            .ThenBy(t => t.Id),
        "2" => tarefas
            .OrderBy(t => t.Concluida)
            .ThenByDescending(t => t.Prioridade),
        _ => Enumerable.Empty<TarefaItem>()
    };

    List<TarefaItem> listaOrdenada = ordenadas.ToList();

    if (listaOrdenada.Count == 0)
    {
        Console.WriteLine("Opção de ordenação inválida.");
    }
    else
    {
        Console.WriteLine("\nTarefas ordenadas:");
        foreach (TarefaItem item in listaOrdenada)
        {
            Console.WriteLine(item);
        }
    }
    break;
    
    break;

        case "0":
            executando = false;
            Console.WriteLine("Até logo!");
            break;

        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}