# TodoCli

Aplicação de linha de comando (CLI) em **C# / .NET** para gerenciamento de tarefas com persistência em arquivo JSON, validação de entrada e suporte a execução assíncrona.

## Funcionalidades

- **Adição interativa de tarefas:** coleta título obrigatório, prioridade (`Baixa`, `Média` ou `Alta`), categoria (padrão `Geral`) e data limite opcional (`dd/MM/yyyy` ou `yyyy-MM-dd`).
- **Revalidação amigável:** solicita nova tentativa quando prioridade ou data são inválidas, permitindo confirmar valores padrão com `Enter`.
- **Persistência em JSON com `RepositorioTarefas`:**
  - Escrita atômica via arquivo temporário (`.tmp`) e `File.Move(..., overwrite: true)` para evitar arquivos corrompidos.
  - Streaming assíncrono (`FileStream` + `JsonSerializer.SerializeAsync` / `DeserializeAsync`) com `CancellationToken`.
  - Controle de concorrência com `SemaphoreSlim` e implementação de `IDisposable`.
  - Backup automático (`.corrompido.<timestamp>.bak`) quando o JSON é inválido.

 Estrutura do Projeto

```text
TodoCli/
├── Program.cs                 # Ponto de entrada da CLI e orquestração dos comandos
├── TarefaItem.cs              # Modelo de domínio da tarefa
├── IRepositorioTarefas.cs     # Contrato do repositório assíncrono
├── RepositorioTarefas.cs      # Implementação de persistência em JSON
└── FluxoAdicionarTarefa.cs    # Coleta de entrada no console e criação de tarefas
```
 Como Executar

1. Certifique-se de ter o **.NET SDK** instalado.
2. Na raiz da solução/projeto, execute:

```bash
dotnet run
```

Por padrão, as tarefas são salvas em `tarefas.json` no diretório de execução.
 Como Testar
 1. Teste Manual na CLI

1. Execute `dotnet run` e inicie o fluxo de adicionar tarefa.
2. Teste um título vazio: deve bloquear e informar que o título não pode ficar vazio.
3. Teste prioridade inválida (ex.: `9` ou `abc`): deve avisar e pedir novamente; pressione `Enter` para assumir `Média`.
4. Teste datas válidas (`25/12/2026` ou `2026-12-25`), data inválida e `Enter` para deixar sem prazo.
5. Encerre e abra a aplicação novamente: confirme que os itens foram carregados de `tarefas.json`.

 2. Testes de Resiliência e Persistência

- **Arquivo inexistente ou vazio:** remova `tarefas.json` ou deixe-o vazio; `CarregarAsync` deve retornar uma lista vazia sem lançar erro.
- **JSON corrompido:** insira um JSON inválido (ex.: `{ invalido`) em `tarefas.json` e chame `CarregarAsync`; deve gerar um arquivo `.bak` ao lado e lançar `InvalidDataException` com o caminho do backup.
- **Caminho em subpasta:** instancie `new RepositorioTarefas("dados/subpasta/tarefas.json")`; ao salvar, o diretório deve ser criado automaticamente.

### 3. Testes Automatizados (xUnit)

Se você tiver um projeto de testes (ex.: `TodoCli.Tests`), rode:

```bash
dotnet test
```

Cenários recomendados para cobrir:

- `FluxoAdicionarTarefa.CriarTarefa`: geração incremental de `Id` (incluindo lista vazia e IDs fora de ordem), `.Trim()` no título e fallback para categoria `"Geral"`.
- `RepositorioTarefas`: ciclo completo de `SalvarAsync` e `CarregarAsync`, leitura de arquivo vazio e tratamento de JSON corrompido com criação de backup.
