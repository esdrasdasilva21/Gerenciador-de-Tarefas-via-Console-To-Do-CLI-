using System.Text.Json;
using System.Text.Json.Serialization;

namespace TodoCli;

public interface IRepositorioTarefas : IDisposable
{
    Task<IReadOnlyList<TarefaItem>> CarregarAsync(CancellationToken ct = default);
    Task SalvarAsync(IEnumerable<TarefaItem> tarefas, CancellationToken ct = default);
}

public sealed class RepositorioTarefas : IRepositorioTarefas
{
    private readonly string _caminhoArquivo;
    private readonly SemaphoreSlim _semaforo = new(1, 1);
    private bool _disposed;

    private static readonly JsonSerializerOptions _opcoesJson = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public RepositorioTarefas(string caminhoArquivo = "tarefas.json")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminhoArquivo);
        _caminhoArquivo = Path.GetFullPath(caminhoArquivo);
    }

    public async Task<IReadOnlyList<TarefaItem>> CarregarAsync(CancellationToken ct = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        await _semaforo.WaitAsync(ct);
        try
        {
            if (!File.Exists(_caminhoArquivo))
            {
                return [];
            }

            await using FileStream fluxo = new(
                _caminhoArquivo,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true);

            if (fluxo.Length == 0)
            {
                return [];
            }

            return await JsonSerializer.DeserializeAsync<List<TarefaItem>>(fluxo, _opcoesJson, ct) ?? [];
        }
        catch (JsonException ex)
        {
            string backup = CriarBackupCorrompido();
            throw new InvalidDataException(
                $"Falha ao interpretar o JSON de tarefas. Uma cópia de segurança foi salva em '{backup}'.", ex);
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task SalvarAsync(IEnumerable<TarefaItem> tarefas, CancellationToken ct = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(tarefas);

        await _semaforo.WaitAsync(ct);
        string? caminhoTemp = null;

        try
        {
            GarantirDiretorioExiste();
            caminhoTemp = $"{_caminhoArquivo}.{Guid.NewGuid():N}.tmp";

            await using (FileStream fluxo = new(
                caminhoTemp,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await JsonSerializer.SerializeAsync(fluxo, tarefas, _opcoesJson, ct);
                await fluxo.FlushAsync(ct);
            }

            File.Move(caminhoTemp, _caminhoArquivo, overwrite: true);
            caminhoTemp = null; // Move concluído com sucesso; não precisa limpar
        }
        finally
        {
            if (caminhoTemp is not null && File.Exists(caminhoTemp))
            {
                File.Delete(caminhoTemp);
            }
            _semaforo.Release();
        }
    }

    private void GarantirDiretorioExiste()
    {
        string? diretorio = Path.GetDirectoryName(_caminhoArquivo);
        if (!string.IsNullOrEmpty(diretorio))
        {
            Directory.CreateDirectory(diretorio);
        }
    }

    private string CriarBackupCorrompido()
    {
        string backup = $"{_caminhoArquivo}.corrompido.{DateTime.UtcNow:yyyyMMddHHmmss}.bak";
        File.Copy(_caminhoArquivo, backup, overwrite: true);
        return backup;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _semaforo.Dispose();
        _disposed = true;
    }
}