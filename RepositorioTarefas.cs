using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoCli;

public class RepositorioTarefas : IRepositorioTarefas
{
    private readonly string _caminhoArquivo;
    private static readonly JsonSerializerOptions _opcoesJson = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public RepositorioTarefas(string caminhoArquivo = "tarefas.json")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminhoArquivo);
        _caminhoArquivo = caminhoArquivo;
    }

    public List<TarefaItem> Carregar()
    {
        if (!File.Exists(_caminhoArquivo)) return [];

        string json = File.ReadAllText(_caminhoArquivo);
        if (string.IsNullOrWhiteSpace(json)) return [];

        return JsonSerializer.Deserialize<List<TarefaItem>>(json, _opcoesJson) ?? [];
    }

    public void Salvar(List<TarefaItem> tarefas)
    {
        ArgumentNullException.ThrowIfNull(tarefas);
        GarantirDiretorioExiste();

        string json = JsonSerializer.Serialize(tarefas, _opcoesJson);
        File.WriteAllText(_caminhoArquivo, json);
    }

    private void GarantirDiretorioExiste()
    {
        string? diretorio = Path.GetDirectoryName(_caminhoArquivo);
        if (!string.IsNullOrEmpty(diretorio))
        {
            Directory.CreateDirectory(diretorio);
        }
    }
}

public interface IRepositorioTarefas
{
    IReadOnlyList<TarefaItem> Carregar();
    void Salvar(IEnumerable<TarefaItem> tarefas);
}


public void Salvar(IEnumerable<TarefaItem> tarefas)
    {
        ArgumentNullException.ThrowIfNull(tarefas);
        GarantirDiretorioExiste();

        string caminhoTemp = $"{_caminhoArquivo}.{Guid.NewGuid():N}.tmp";
        string json = JsonSerializer.Serialize(tarefas, _opcoesJson);

        File.WriteAllText(caminhoTemp, json);
        File.Move(caminhoTemp, _caminhoArquivo, overwrite: true);
    }