namespace TodoCli;

public class TarefaItem
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public bool Concluida { get; set; }

    public TarefaItem(int id, string titulo, bool concluida = false)
    {
        Id = id;
        Titulo = titulo;
        Concluida = concluida;
    }

    public override string ToString()
    {
        string marcador = Concluida ? "[X]" : "[ ]";
        return $"{marcador} #{Id} - {Titulo}";
    }
}