using TodoCli;

const string nomeApp = "TaskFlow C#";
const string versao = "1.2";

Console.WriteLine($"=== {nomeApp} (v{versao}) ===");
Console.Write("Digite seu nome: ");
string? nomeUsuario = Console.ReadLine();

if (string.IsNullOrWhiteSpace(nomeUsuario))
{
    nomeUsuario = "Desenvolvedor";
}

Console.WriteLine($"Olá, {nomeUsuario}! Vamos organizar suas tarefas.");

IRepositorioTarefas repositorio = new RepositorioTarefas();
ServicoTarefas servico = new(repositorio);

bool executando = true;

while (executando)
{
    ExibirResumo(servico);
    ExibirMenu();
    string? opcao = Console.ReadLine()?.Trim();

    switch (opcao)
    {
        case "1":
            ExibirLista("Suas tarefas", servico.ListarTodas());
            break;
        case "2":
            FluxoAdicionar(servico);
            break;
        case "3":
            FluxoRemover(servico);
            break;
        case "4":
            FluxoAlternarConclusao(servico);
            break;
        case "5":
            FluxoFiltrar(servico);
            break;
        case "6":
            FluxoBuscar(servico);
            break;
        case "7":
            FluxoOrdenar(servico);
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

static void ExibirResumo(ServicoTarefas servico)
{
    var resumo = servico.ObterResumo();
    Console.WriteLine($"\nResumo: {resumo.Concluidas}/{resumo.Total} concluídas | {resumo.Atrasadas} atrasada(s)");
}

static void ExibirMenu()
{
    Console.WriteLine("\n--- Menu ---");
    Console.WriteLine("1 - Listar tarefas");
    Console.WriteLine("2 - Adicionar tarefa");
    Console.WriteLine("3 - Remover tarefa");
    Console.WriteLine("4 - Concluir / reabrir tarefa");
    Console.WriteLine("5 - Filtrar tarefas");
    Console.WriteLine("6 - Buscar por palavra-chave");
    Console.WriteLine("7 - Ordenar tarefas");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");
}

static void ExibirLista(string titulo, IEnumerable<TarefaItem> itens)
{
    List<TarefaItem> lista = itens.ToList();
    Console.WriteLine($"\n{titulo}:");

    if (lista.Count == 0)
    {
        Console.WriteLine("Nenhuma tarefa encontrada.");
        return;
    }

    foreach (TarefaItem item in lista)
    {
        Console.WriteLine(item);
    }
}

static void FluxoAdicionar(ServicoTarefas servico)
{
    Console.Write("Digite o título da nova tarefa: ");
    string? titulo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(titulo))
    {
        Console.WriteLine("O título não pode ficar vazio.");
        return;
    }

    Console.Write("Prioridade (1 - Baixa, 2 - Média, 3 - Alta) [padrão 2]: ");
    PrioridadeTarefa prioridade = Console.ReadLine() switch
    {
        "1" => PrioridadeTarefa.Baixa,
        "3" => PrioridadeTarefa.Alta,
        _ => PrioridadeTarefa.Media
    };

    Console.Write("Categoria [padrão Geral]: ");
    string? categoriaEntrada = Console.ReadLine();
    string categoria = string.IsNullOrWhiteSpace(categoriaEntrada) ? "Geral" : categoriaEntrada.Trim();

    Console.Write("Data limite (dd/MM/yyyy) ou Enter para sem prazo: ");
    string? dataEntrada = Console.ReadLine();
    DateOnly? dataLimite = null;

    if (!string.IsNullOrWhiteSpace(dataEntrada))
    {
        if (DateOnly.TryParseExact(
            dataEntrada.Trim(),
            new[] { "dd/MM/yyyy", "yyyy-MM-dd" },
            out DateOnly dataConvertida))
        {
            dataLimite = dataConvertida;
        }
        else
        {
            Console.WriteLine("Data inválida. Salvando sem data limite.");
        }
    }

    TarefaItem criada = servico.Adicionar(titulo, prioridade, categoria, dataLimite);
    Console.WriteLine($"Tarefa #{criada.Id} adicionada!");
}

static void FluxoRemover(ServicoTarefas servico)
{
    Console.Write("Digite o ID da tarefa para remover: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    Console.WriteLine(servico.RemoverPorId(id)
        ? $"Tarefa #{id} removida."
        : "Tarefa não encontrada.");
}

static void FluxoAlternarConclusao(ServicoTarefas servico)
{
    Console.Write("Digite o ID da tarefa para alternar conclusão: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("ID inválido.");
        return;
    }

    TarefaItem? atualizada = servico.AlternarConclusao(id);
    if (atualizada is null)
    {
        Console.WriteLine("Tarefa não encontrada.");
        return;
    }

    string estado = atualizada.Concluida ? "concluída" : "pendente";
    Console.WriteLine($"Tarefa #{atualizada.Id} agora está {estado}.");
}

static void FluxoFiltrar(ServicoTarefas servico)
{
    Console.WriteLine("1 - Apenas pendentes");
    Console.WriteLine("2 - Apenas concluídas");
    Console.WriteLine("3 - Apenas atrasadas");
    Console.WriteLine("4 - Por categoria");
    Console.Write("Escolha o filtro: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            ExibirLista("Tarefas pendentes", servico.FiltrarPendentes());
            break;
        case "2":
            ExibirLista("Tarefas concluídas", servico.FiltrarConcluidas());
            break;
        case "3":
            ExibirLista("Tarefas atrasadas", servico.FiltrarAtrasadas());
            break;
        case "4":
            Console.Write("Digite a categoria: ");
            string? categoria = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(categoria))
            {
                Console.WriteLine("Categoria inválida.");
                return;
            }
            ExibirLista($"Categoria: {categoria.Trim()}", servico.FiltrarPorCategoria(categoria));
            break;
        default:
            Console.WriteLine("Filtro inválido.");
            break;
    }
}

static void FluxoBuscar(ServicoTarefas servico)
{
    Console.Write("Digite a palavra-chave: ");
    string? termo = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(termo))
    {
        Console.WriteLine("Termo de busca inválido.");
        return;
    }

    ExibirLista($"Resultados para \"{termo.Trim()}\"", servico.BuscarPorPalavraChave(termo));
}

static void FluxoOrdenar(ServicoTarefas servico)
{
    Console.WriteLine("1 - Por prioridade (Alta -> Baixa)");
    Console.WriteLine("2 - Por status e prioridade");
    Console.Write("Escolha a ordenação: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            ExibirLista("Ordenadas por prioridade", servico.OrdenarPorPrioridade());
            break;
        case "2":
            ExibirLista("Ordenadas por status e prioridade", servico.OrdenarPorStatusEPrioridade());
            break;
        default:
            Console.WriteLine("Opção de ordenação inválida.");
            break;
    }
}