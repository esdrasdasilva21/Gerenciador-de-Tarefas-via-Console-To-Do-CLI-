using System.Text.Json;

namespace TodoCli;

public class RepositorioTarefas : IRepositorioTarefas
{
    private readonly string _caminhoArquivo;
    private readonly JsonSerializerOptions _opcoesJson;

    public RepositorioTarefas(string caminhoArquivo = "tarefas.json")
    {
        _caminhoArquivo = caminhoArquivo;
        _opcoesJson = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }

    public List<TarefaItem> Carregar()
    {
        if (!File.Exists(_caminhoArquivo))
        {
            return new List<TarefaItem>();
        }

        string json = File.ReadAllText(_caminhoArquivo);
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<TarefaItem>();
        }

        return JsonSerializer.Deserialize<List<TarefaItem>>(json, _opcoesJson)
               ?? new List<TarefaItem>();
    }

    public void Salvar(List<TarefaItem> tarefas)
    {
        string json = JsonSerializer.Serialize(tarefas, _opcoesJson);
        File.WriteAllText(_caminhoArquivo, json);
    }
}