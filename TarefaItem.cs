namespace TodoCli;

public class TarefaItem
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Concluida { get; set; }
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;

    public TarefaItem() { }

    public TarefaItem(
        int id,
        string titulo,
        PrioridadeTarefa prioridade = PrioridadeTarefa.Media,
        bool concluida = false)
    {
        Id = id;
        Titulo = titulo;
        Prioridade = prioridade;
        Concluida = concluida;
    }

    public override string ToString()
    {
        string marcador = Concluida ? "[X]" : "[ ]";
        return $"{marcador} #{Id} [{Prioridade}] - {Titulo}";
    }
}