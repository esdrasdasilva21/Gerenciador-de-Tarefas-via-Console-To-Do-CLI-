using System.Globalization;

namespace TodoCli;

public sealed record NovaTarefaInput(
    string Titulo,
    PrioridadeTarefa Prioridade,
    string Categoria,
    DateOnly? DataLimite);

public static class FluxoAdicionarTarefa
{
    private static readonly string[] FormatosData = ["dd/MM/yyyy", "yyyy-MM-dd"];

    public static async Task ExecutarAsync(
        List<TarefaItem> tarefas,
        IRepositorioTarefas repositorio,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(tarefas);
        ArgumentNullException.ThrowIfNull(repositorio);

        NovaTarefaInput? entrada = ColetarEntrada();
        if (entrada is null)
        {
            return;
        }

        TarefaItem novaTarefa = CriarTarefa(tarefas, entrada);
        tarefas.Add(novaTarefa);

        await repositorio.SalvarAsync(tarefas, ct);

        Console.WriteLine(
            $"Tarefa #{novaTarefa.Id} adicionada na categoria \"{novaTarefa.Categoria}\"!");
    }

    public static NovaTarefaInput? ColetarEntrada()
    {
        string? titulo = LerTituloObrigatorio();
        if (titulo is null)
        {
            return null;
        }

        PrioridadeTarefa prioridade = LerPrioridade();
        string categoria = LerCategoria();
        DateOnly? dataLimite = LerDataLimite();

        return new NovaTarefaInput(titulo, prioridade, categoria, dataLimite);
    }

    public static TarefaItem CriarTarefa(IEnumerable<TarefaItem> tarefasExistentes, NovaTarefaInput entrada)
    {
        ArgumentNullException.ThrowIfNull(tarefasExistentes);
        ArgumentNullException.ThrowIfNull(entrada);

        int proximoId = tarefasExistentes.Select(t => t.Id).DefaultIfEmpty(0).Max() + 1;

        return new TarefaItem(
            proximoId,
            entrada.Titulo.Trim(),
            entrada.Prioridade,
            NormalizarCategoria(entrada.Categoria),
            entrada.DataLimite);
    }

    private static string? LerTituloObrigatorio()
    {
        Console.Write("Digite o título da nova tarefa: ");
        string? titulo = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(titulo))
        {
            Console.WriteLine("O título não pode ficar vazio.");
            return null;
        }

        return titulo;
    }

    private static PrioridadeTarefa LerPrioridade()
    {
        while (true)
        {
            Console.Write("Prioridade (1 - Baixa, 2 - Média, 3 - Alta) [padrão 2]: ");
            string? entrada = Console.ReadLine()?.Trim();

            PrioridadeTarefa? prioridade = entrada switch
            {
                "" or "2" => PrioridadeTarefa.Media,
                "1" => PrioridadeTarefa.Baixa,
                "3" => PrioridadeTarefa.Alta,
                _ => null
            };

            if (prioridade is not null)
            {
                return prioridade.Value;
            }

            Console.WriteLine("Opção inválida. Digite 1, 2, 3 ou pressione Enter.");
        }
    }

    private static string LerCategoria()
    {
        Console.Write("Categoria (ex.: Estudo, Trabalho, Projeto) [padrão Geral]: ");
        string? entrada = Console.ReadLine();
        return NormalizarCategoria(entrada);
    }

    private static string NormalizarCategoria(string? categoria)
    {
        return string.IsNullOrWhiteSpace(categoria) ? "Geral" : categoria.Trim();
    }

    private static DateOnly? LerDataLimite()
    {
        while (true)
        {
            Console.Write("Data limite (dd/MM/yyyy ou yyyy-MM-dd) ou Enter para sem prazo: ");
            string? entrada = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(entrada))
            {
                return null;
            }

            if (DateOnly.TryParseExact(
                entrada,
                FormatosData,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly dataConvertida))
            {
                return dataConvertida;
            }

            Console.WriteLine("Data inválida. Tente novamente ou pressione Enter para deixar sem prazo.");
        }
    }
}