namespace TodoCli;

public class TarefaItem
{
    private string _titulo = string.Empty;
    private string _categoria = "Geral";

    public int Id { get; set; }

    public string Titulo
    {
        get => _titulo;
        set => _titulo = value?.Trim() ?? string.Empty;
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
        string marcador = Concluida ? "[X]" : "[ ]";
        string prazoTexto = DataLimite.HasValue
            ? $" | Prazo: {DataLimite.Value:dd/MM/yyyy}"
            : string.Empty;
        string alertaAtraso = EstaAtrasada ? " [ATRASADA]" : string.Empty;

        return $"{marcador} #{Id} [{Prioridade}] ({Categoria}) - {Titulo}{prazoTexto}{alertaAtraso}";
    }
}