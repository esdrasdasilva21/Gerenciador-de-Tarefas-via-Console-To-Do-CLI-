namespace TodoCli;

public class TarefaItem
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Concluida { get; set; }
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;
    public string Categoria { get; set; } = "Geral";
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
        Categoria = string.IsNullOrWhiteSpace(categoria) ? "Geral" : categoria.Trim();
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