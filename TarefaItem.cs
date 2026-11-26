using System.Text;

namespace TodoCli;

public class TarefaItem
{
    private int _id;
    private string _titulo = string.Empty;
    private string _categoria = "Geral";

    public int Id
    {
        get => _id;
        set => _id = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "O ID não pode ser negativo.");
    }

    public string Titulo
    {
        get => _titulo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(value));

            _titulo = value.Trim();
        }
    }

    public bool Concluida { get; set; }
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;

    public string Categoria
    {
        get => _categoria;
        set => _categoria = string.IsNullOrWhiteSpace(value) ? "Geral" : value.Trim();
    }

    public DateOnly? DataLimite { get; set; }

    public bool EstaAtrasada =>
        !Concluida &&
        DataLimite.HasValue &&
        DataLimite.Value < DateOnly.FromDateTime(DateTime.Today);

    public TarefaItem() { }

    public TarefaItem(
        int id,
        string titulo,
        PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
        string categoria = "Geral",
        DateOnly? dataLimite = null,
        bool concluida = false)
    {
        Id = id;
        Titulo = titulo;
        Prioridade = prioridade;
        Categoria = categoria;
        DataLimite = dataLimite;
        Concluida = concluida;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(Concluida ? "[X]" : "[ ]");
        sb.Append($" #{Id} [{Prioridade}] ({Categoria}) - {Titulo}");

        if (DataLimite.HasValue)
            sb.Append($" | Prazo: {DataLimite.Value:dd/MM/yyyy}");

        if (EstaAtrasada)
            sb.Append(" [ATRASADA]");

        return sb.ToString();
    }
}