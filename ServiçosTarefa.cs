namespace TodoCli;

public class ServicoTarefas
{
    private readonly IRepositorioTarefas _repositorio;
    private readonly List<TarefaItem> _tarefas;
    private int _proximoId;

    public ServicoTarefas(IRepositorioTarefas repositorio)
    {
        _repositorio = repositorio;
        _tarefas = CarregarInicial();
        _proximoId = _tarefas.Count > 0 ? _tarefas.Max(t => t.Id) + 1 : 1;
    }

    public IReadOnlyList<TarefaItem> ListarTodas() => _tarefas.AsReadOnly();

    public TarefaItem Adicionar(
        string titulo,
        PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
        string categoria = "Geral",
        DateOnly? dataLimite = null)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("O título da tarefa não pode ser vazio.", nameof(titulo));
        }

        TarefaItem nova = new(_proximoId, titulo.Trim(), prioridade, categoria, dataLimite);
        _proximoId++;
        _tarefas.Add(nova);
        SalvarComSeguranca();
        return nova;
    }

    public bool RemoverPorId(int id)
    {
        TarefaItem? alvo = _tarefas.FirstOrDefault(t => t.Id == id);
        if (alvo is null)
        {
            return false;
        }

        _tarefas.Remove(alvo);
        SalvarComSeguranca();
        return true;
    }

    public TarefaItem? AlternarConclusao(int id)
    {
        TarefaItem? tarefa = _tarefas.FirstOrDefault(t => t.Id == id);
        if (tarefa is null)
        {
            return null;
        }

        tarefa.Concluida = !tarefa.Concluida;
        SalvarComSeguranca();
        return tarefa;
    }

    public List<TarefaItem> FiltrarPendentes() =>
        _tarefas.Where(t => !t.Concluida).ToList();

    public List<TarefaItem> FiltrarConcluidas() =>
        _tarefas.Where(t => t.Concluida).ToList();

    public List<TarefaItem> FiltrarAtrasadas() =>
        _tarefas.Where(t => t.EstaAtrasada).ToList();

    public List<TarefaItem> FiltrarPorCategoria(string categoria) =>
        _tarefas
            .Where(t => t.Categoria.Equals(categoria.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();

    public List<TarefaItem> BuscarPorPalavraChave(string termo) =>
        _tarefas
            .Where(t => t.Titulo.Contains(termo.Trim(), StringComparison.OrdinalIgnoreCase))
            .ToList();

    public List<TarefaItem> OrdenarPorPrioridade() =>
        _tarefas
            .OrderByDescending(t => t.Prioridade)
            .ThenBy(t => t.Id)
            .ToList();

    public List<TarefaItem> OrdenarPorStatusEPrioridade() =>
        _tarefas
            .OrderBy(t => t.Concluida)
            .ThenByDescending(t => t.Prioridade)
            .ToList();

    public (int Total, int Concluidas, int Atrasadas) ObterResumo() =>
        (
            _tarefas.Count,
            _tarefas.Count(t => t.Concluida),
            _tarefas.Count(t => t.EstaAtrasada)
        );

    private List<TarefaItem> CarregarInicial()
    {
        try
        {
            List<TarefaItem> carregadas = _repositorio.Carregar();
            if (carregadas.Count > 0)
            {
                return carregadas;
            }

            List<TarefaItem> iniciais = new()
            {
                new TarefaItem(1, "Estudar sintaxe básica de C#", PrioridadeTarefa.Alta, "Estudo"),
                new TarefaItem(2, "Configurar o ambiente .NET", PrioridadeTarefa.Media, "Projeto")
            };

            _repositorio.Salvar(iniciais);
            return iniciais;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Aviso ao carregar tarefas: {ex.Message}");
            return new List<TarefaItem>();
        }
    }

    private void SalvarComSeguranca()
    {
        try
        {
            _repositorio.Salvar(_tarefas);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar tarefas: {ex.Message}");
        }
    }
}

