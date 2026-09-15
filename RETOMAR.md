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
No `prototipo/Dominio.cs`, os **4 enums** (revisados e compilando, 0 erros):
```csharp
enum PerfilUsuario     { Comum, Admin }
enum StatusCadastro    { PendenteAprovacao, Ativo, Recusado }
enum StatusNotebook    { Disponivel, Emprestado, Manutencao }
enum StatusSolicitacao { Pendente, Recusada, EmUso, Devolvida }
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

## Como levar pro outro PC
A pasta `C:\checkbook` inteira precisa ir junto (o histórico do chat NÃO sincroniza).
Opções: colocar num drive de nuvem (OneDrive/Drive), pendrive, ou inicializar um repositório
git e dar push. Este arquivo + o `prototipo/Dominio.cs` + o PDF é o essencial.
