using TodoCli;

const string nomeApp = "TaskFlow C#";
const string versao = "1.0";

Console.WriteLine($"=== {nomeApp} (v{versao}) ===");
Console.Write("Digite seu nome: ");
string? nomeUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(nomeUsuario))
{
    nomeUsuario = "Desenvolvedor";
}

Console.WriteLine($"Olá, {nomeUsuario}! Vamos organizar suas tarefas.");

RepositorioTarefas repositorio = new();
List<TarefaItem> tarefas = CarregarTarefasSeguras(repositorio);

if (tarefas.Count == 0)
{
    tarefas.AddRange(new[]
    {
        new TarefaItem(1, "Estudar sintaxe básica de C#", PrioridadeTarefa.Alta),
        new TarefaItem(2, "Configurar o ambiente .NET", PrioridadeTarefa.Media)
    });
    SalvarTarefasSeguras(repositorio, tarefas);
}

int proximoId = tarefas.Count > 0 ? tarefas.Max(t => t.Id) + 1 : 1;
bool executando = true;

while (executando)
{
    ExibirMenu();
    string? opcao = Console.ReadLine()?.Trim();

    switch (opcao)
    {
        case "1":
            ListarTarefas(tarefas);
            break;
        case "2":
            AdicionarTarefa(tarefas, ref proximoId);
            SalvarTarefasSeguras(repositorio, tarefas);
            break;
        case "3":
            RemoverTarefa(tarefas);
            SalvarTarefasSeguras(repositorio, tarefas);
            break;
        case "4":
            AlternarConclusao(tarefas);
            SalvarTarefasSeguras(repositorio, tarefas);
            break;
        case "5":
            FiltrarTarefas(tarefas);
            break;
        case "6":
            BuscarPorPalavraChave(tarefas);
            break;
        case "7":
            OrdenarTarefas(tarefas);
            break;
        case "0":
            SalvarTarefasSeguras(repositorio, tarefas);
            executando = false;
            Console.WriteLine("Até logo! Suas tarefas foram salvas.");
            break;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

static void ExibirMenu()
{
    Console.WriteLine("\n--- Menu ---");
    Console.WriteLine("1 - Listar tarefas");
    Console.WriteLine("2 - Adicionar tarefa");
    Console.WriteLine("3 - Remover tarefa");
    Console.WriteLine("4 - Concluir / reabrir tarefa");
    Console.WriteLine("5 - Filtrar tarefas (pendentes / concluídas)");
    Console.WriteLine("6 - Buscar por palavra-chave");
    Console.WriteLine("7 - Ordenar tarefas");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");
}

static void ListarTarefas(List<TarefaItem> tarefas)
{
    Console.WriteLine("\nSuas tarefas:");
    if (tarefas.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa cadastrada.");
        return;
    }

    foreach (TarefaItem tarefa in tarefas)
    {
        Console.WriteLine(tarefa);
    }
}

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

    tarefas.Add(new TarefaItem(proximoId, titulo.Trim(), prioridade));
    proximoId++;
    Console.WriteLine($"Tarefa adicionada com prioridade {prioridade}!");
}

static void RemoverTarefa(List<TarefaItem> tarefas)
{
    if (tarefas.Count == 0)
    {
        Console.WriteLine("Não há tarefas para remover.");
        return;
    }

    Console.Write("Digite o ID da tarefa para remover: ");
    string? entradaId = Console.ReadLine();

    if (!int.TryParse(entradaId, out int idRemover))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    TarefaItem? alvo = tarefas.FirstOrDefault(t => t.Id == idRemover);
    if (alvo is null)
    {
        Console.WriteLine("Tarefa não encontrada.");
        return;
    }

    tarefas.Remove(alvo);
    Console.WriteLine($"Tarefa #{alvo.Id} (\"{alvo.Titulo}\") removida.");
}

static void AlternarConclusao(List<TarefaItem> tarefas)
{
    if (tarefas.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa cadastrada.");
        return;
    }

    Console.Write("Digite o ID da tarefa para alternar conclusão: ");
    string? entradaToggle = Console.ReadLine();

    if (!int.TryParse(entradaToggle, out int idToggle))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    TarefaItem? tarefa = tarefas.FirstOrDefault(t => t.Id == idToggle);
    if (tarefa is null)
    {
        Console.WriteLine("Tarefa não encontrada.");
        return;
    }

    tarefa.Concluida = !tarefa.Concluida;
    string estado = tarefa.Concluida ? "concluída" : "pendente";
    Console.WriteLine($"Tarefa #{tarefa.Id} agora está marcada como {estado}.");
}

static void FiltrarTarefas(List<TarefaItem> tarefas)
{
    Console.WriteLine("1 - Apenas pendentes");
    Console.WriteLine("2 - Apenas concluídas");
    Console.Write("Escolha o filtro: ");
    string? filtro = Console.ReadLine();

    IEnumerable<TarefaItem> consulta = filtro switch
    {
        "1" => tarefas.Where(t => !t.Concluida),
        "2" => tarefas.Where(t => t.Concluida),
        _ => Enumerable.Empty<TarefaItem>()
    };

    if (filtro is not ("1" or "2"))
    {
        Console.WriteLine("Filtro inválido.");
        return;
    }

    List<TarefaItem> resultado = consulta.ToList();
    if (resultado.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa encontrada para esse filtro.");
        return;
    }

    Console.WriteLine("\nResultado:");
    foreach (TarefaItem item in resultado)
    {
        Console.WriteLine(item);
    }
}

static void BuscarPorPalavraChave(List<TarefaItem> tarefas)
{
    Console.Write("Digite a palavra ou trecho para buscar: ");
    string? termo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(termo))
    {
        Console.WriteLine("Digite um termo válido para busca.");
        return;
    }

    string termoLimpo = termo.Trim();
    List<TarefaItem> encontradas = tarefas
        .Where(t => t.Titulo.Contains(termoLimpo, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (encontradas.Count == 0)
    {
        Console.WriteLine($"Nenhuma tarefa contém \"{termoLimpo}\".");
        return;
    }

    Console.WriteLine($"\nTarefas encontradas para \"{termoLimpo}\":");
    foreach (TarefaItem item in encontradas)
    {
        Console.WriteLine(item);
    }
}

static void OrdenarTarefas(List<TarefaItem> tarefas)
{
    if (tarefas.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa para ordenar.");
        return;
    }

    Console.WriteLine("1 - Por prioridade (Alta -> Baixa)");
    Console.WriteLine("2 - Por status (Pendentes primeiro)");
    Console.Write("Escolha a ordenação: ");
    string? ordem = Console.ReadLine();

    IEnumerable<TarefaItem> consulta = ordem switch
    {
        "1" => tarefas.OrderByDescending(t => t.Prioridade).ThenBy(t => t.Id),
        "2" => tarefas.OrderBy(t => t.Concluida).ThenByDescending(t => t.Prioridade),
        _ => Enumerable.Empty<TarefaItem>()
    };

    List<TarefaItem> ordenadas = consulta.ToList();
    if (ordenadas.Count == 0)
    {
        Console.WriteLine("Opção de ordenação inválida.");
        return;
    }

    Console.WriteLine("\nTarefas ordenadas:");
    foreach (TarefaItem item in ordenadas)
    {
        Console.WriteLine(item);
    }
}

static List<TarefaItem> CarregarTarefasSeguras(RepositorioTarefas repositorio)
{
    try
    {
        return repositorio.Carregar();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Aviso: não foi possível carregar o arquivo de tarefas ({ex.Message}). Iniciando lista vazia.");
        return new List<TarefaItem>();
    }
}

static void SalvarTarefasSeguras(RepositorioTarefas repositorio, List<TarefaItem> tarefas)
{
    try
    {
        repositorio.Salvar(tarefas);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao salvar tarefas no disco: {ex.Message}");
    }
}