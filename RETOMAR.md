# 📌 Retomar treino — Sistema de Retirada de Notebooks

> Arquivo de handoff pra continuar o trabalho em outro computador.
> **No outro PC:** abra o Claude Code na pasta `C:\checkbook` e diga *"leia o RETOMAR.md e continue de onde paramos"*.

## O projeto
Sistema web para **controle de retirada/devolução de notebooks** (substitui a ficha de papel
"Controle de Uso de Materiais Eletrônicos"). Especificação completa em:
`informações/Planejamento_Sistema_Retirada_Notebooks.pdf`

- **Back-end (futuro):** C# ASP.NET Core Web API + EF Core + JWT
- **Front-end (futuro):** Angular
- **Banco (futuro):** SQL Server / PostgreSQL / SQLite

## Fase atual: TREINO DA LÓGICA no terminal (antes de criar o projeto real)
Estamos prototipando a **lógica de domínio** num console C# simples, para refrescar conceitos,
**antes** de montar a estrutura de API + Angular.

- Projeto de treino: `C:\checkbook\prototipo` (console .NET 10). Rodar com `dotnet run`.
- Arquivo onde a lógica está sendo escrita: `prototipo/Dominio.cs`

## ⚙️ Como o Roberto quer trabalhar (IMPORTANTE — respeitar)
**Modo "Você guia, eu codo":** o Claude explica o conceito e propõe o desafio;
**o Roberto escreve o código**; o Claude revisa, corrige e explica os erros.
NÃO escrever a solução por ele — dar dicas e exemplos neutros (não do domínio) e deixá-lo codar.
Conversa em **português**.

## ✅ O que já está feito
<<<<<<< HEAD
No `prototipo/Dominio.cs`, os **4 enums** (revisados e compilando, 0 erros):
=======
Estado atual do `prototipo/Dominio.cs` (compilando, 0 erros, 0 warnings):

>>>>>>> 9ea3642 (novos arquivos)
```csharp
enum PerfilUsuario     { Comum, Admin }
enum StatusCadastro    { PendenteAprovacao, Ativo, Recusado }
enum StatusNotebook    { Disponivel, Emprestado, Manutencao }
enum StatusSolicitacao { Pendente, Recusada, EmUso, Devolvida }
<<<<<<< HEAD
```
Conceitos já treinados: o que é enum (lista fixa de nomes, sem tipo/aspas/`=`), e a regra
de **não usar acentos em identificadores** (compila, mas é contra a convenção).

## ▶️ PRÓXIMO PASSO (onde paramos)
Roberto vai escrever a **classe `Usuario`** (seção 4.1 do PDF) no `Dominio.cs`, com propriedades:
| Campo | Tipo |
|-------|------|
| Id | `int` |
| Nome | `string` |
| Email | `string` |
| SenhaHash | `string` |
| Perfil | enum `PerfilUsuario` |
| StatusCadastro | enum `StatusCadastro` |

Conceito sendo introduzido: **classe + auto-propriedades** `public Tipo Nome { get; set; }`.
Já foi avisado que as `string` vão gerar **warning amarelo (nullable)** — isso é DE PROPÓSITO,
será o **próximo conceito** (nullable / `required`). Por ora, basta compilar sem erro vermelho.

## 🗺️ Plano do treino de domínio (ordem)
1. Enums ✅
2. Classe `Usuario` ⬅️ **AQUI**
3. Nullable / `required` (resolver o warning das strings)
4. Classes `Carrinho` e `Notebook` (+ relação Notebook→Carrinho via `CarrinhoId`)
5. Classe `Solicitacao` (campos opcionais/nullable: MotivoRecusa, DataHoraRetirada, etc.)
6. Tabela de ligação `SolicitacaoNotebook` (vários notebooks por solicitação)
7. Lógica de comportamento: máquina de estados da solicitação, disponibilidade de notebooks,
   cálculo de previsão de devolução (aulas de 50 min) e detecção de atraso.
=======

class Usuario
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string SenhaHash { get; set; }
    public PerfilUsuario Perfil { get; set; }
    public StatusCadastro Status { get; set; }
}

class Carrinho
{
    public int Id { get; set; }
    public required string Nome { get; set; }
}

class Notebook
{
    public int Id { get; set; }
    public required string Marca { get; set; }
    public required string Modelo { get; set; }
    public required string Patrimonio { get; set; }
    public StatusNotebook Status { get; set; }
    public int CarrinhoId { get; set; }
}

class Solicitacao
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataHoraSolicitacao { get; set; }
    public int AulasPrevistas { get; set; }
    public StatusSolicitacao Status { get; set; }
    public string? MotivoRecusa { get; set; }
    public DateTime? DataHoraRetirada { get; set; }
    public DateTime? DataHoraDevolucao { get; set; }

    public void Aprovar()
    {
        if (Status == StatusSolicitacao.Pendente)
        {
            Status = StatusSolicitacao.EmUso;
            DataHoraRetirada = DateTime.Now;
        }
        else
            throw new InvalidOperationException("A transição não pode ser concluída, porque o computador já está em uso");
    }

    public void Devolver()
    {
        if (Status == StatusSolicitacao.EmUso)
        {
            Status = StatusSolicitacao.Devolvida;
            DataHoraDevolucao = DateTime.Now;
        }
        else
            throw new InvalidOperationException("O computador não está em uso, não é possível devolver.");
    }

    public void Recusar(string motivo)
    {
        if (Status == StatusSolicitacao.Pendente)
        {
            Status = StatusSolicitacao.Recusada;
            MotivoRecusa = motivo;
        }
        else
            throw new InvalidOperationException("A solicitação não pode ser recusada, pois já foi aprovada ou recusada.");
    }
}

class SolicitacaoNotebook
{
    public int SolicitacaoId { get; set; }
    public int NotebookId { get; set; }
}
```

### Conceitos já treinados
- **Enums:** lista fixa de nomes, sem tipo/aspas/`=`, sem acentos em identificadores
- **Classe + auto-propriedades:** `public Tipo Nome { get; set; }`
- **PascalCase em propriedades:** convenção C# (não camelCase)
- **Nomenclatura sem redundância:** dentro de `class Usuario`, a propriedade é `Nome`, não `NomeUsuario`
- **`required`:** garante em tempo de compilação que a propriedade seja preenchida ao criar o objeto
- **Nullable (`?`):** `string?` e `DateTime?` para campos que podem ser nulos
- **Relação um-para-muitos via chave estrangeira:** `Notebook` tem `CarrinhoId` referenciando `Carrinho`
- **Tabela de ligação muitos-para-muitos:** `SolicitacaoNotebook` com dois campos int
- **Classe vs. objeto:** classe é o molde, objeto é a instância concreta na memória
- **Métodos de instância:** `Aprovar()`, `Devolver()`, `Recusar(motivo)` — máquina de estados da solicitação
- **`static` vs. instância:** método sem `static` acessa propriedades do objeto atual (`this`)
- **`InvalidOperationException`:** exceção correta para operações inválidas no estado atual (não `InvalidCastException`)

## ▶️ PRÓXIMO PASSO (onde paramos)
Roberto vai escrever a **propriedade calculada `PrevisaoDevolucao`** dentro da classe `Solicitacao`.

- Tipo: `DateTime?`
- Sem `set` — só calcula e retorna
- Lógica: se `DataHoraRetirada` for `null`, retorna `null`; senão retorna `DataHoraRetirada + (AulasPrevistas × 50 minutos)`

Dicas já dadas:
```csharp
// Propriedade somente leitura (expression body):
public int Dobro => Valor * 2;

// Adicionar minutos a um DateTime:
data.AddMinutes(quantidade)
```

## 🗺️ Plano do treino de domínio (ordem)
1. Enums ✅
2. Classe `Usuario` ✅
3. Nullable / `required` ✅
4. Classes `Carrinho` e `Notebook` ✅
5. Classe `Solicitacao` ✅
6. Classe `SolicitacaoNotebook` ✅
7. Máquina de estados: `Aprovar()`, `Devolver()`, `Recusar()` ✅
8. Propriedade calculada `PrevisaoDevolucao` ⬅️ **AQUI**
9. Detecção de atraso (propriedade `EstaAtrasada`)
>>>>>>> 9ea3642 (novos arquivos)

## Como levar pro outro PC
A pasta `C:\checkbook` inteira precisa ir junto (o histórico do chat NÃO sincroniza).
Opções: colocar num drive de nuvem (OneDrive/Drive), pendrive, ou inicializar um repositório
git e dar push. Este arquivo + o `prototipo/Dominio.cs` + o PDF é o essencial.
