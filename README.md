Projeto desenvolvido para gerenciamento de uma pequena empresa de doces, centralizando o controle de **produtos, clientes, vendas e despesas** em uma aplicação web.
O projeto foi desenvolvido com foco em **backend utilizando C# e .NET**, aplicando conceitos de organização em camadas, autenticação, persistência de dados e desenvolvimento de APIs RESTful.

## Sobre o projeto

O **AdmDocesECia** surgiu com o objetivo de facilitar a gestão de uma empresa de doces, permitindo centralizar informações que normalmente seriam controladas manualmente.
A API disponibiliza os recursos necessários para o gerenciamento das principais operações do sistema, enquanto o frontend consome esses endpoints para apresentar a interface de gestão.

### Principais funcionalidades

* Autenticação de usuários com JWT
* Cadastro e gerenciamento de produtos
* Cadastro e gerenciamento de clientes
* Registro e gerenciamento de vendas
* Controle de despesas
* Dados para acompanhamento da gestão
* Ativação e inativação de produtos e clientes
* Geração de relatório de produtos, clientes e vendas
* Validação de dados de entrada
* Testes


## Tecnologias

### Backend

* **C#**
* **.NET**
* **ASP.NET Core**
* **Entity Framework Core**
* **SQL Server**
* **JWT (JSON Web Token)**
* **FluentValidation**
* **Swagger / OpenAPI**
* **ClosedXML**

### Frontend

* **React**
* **TypeScript**
* **Vite**
* **Tailwind CSS**
* **shadcn/ui**
  

## Autenticação

A API utiliza **JWT (JSON Web Token)** para autenticação.
O usuário realiza o login fornecendo suas credenciais e, após a validação, a API retorna um token que deve ser utilizado nas requisições protegidas.

Exemplo:
POST /api/Auth/login
Resposta:
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2026-09-16T15:00:00Z"
}

O token deve ser enviado no header das requisições autenticadas:
Authorization: Bearer {token}


## Produtos

* Cadastro de produtos
* Consulta de produtos
* Atualização de produtos
* Ativação/inativação de produtos
* Geração de relatório
  

## Clientes

O módulo de clientes permite cadastrar e gerenciar os clientes da empresa.
As informações podem ser utilizadas posteriormente no registro das vendas, mantendo o relacionamento entre clientes e pedidos realizados.


## Vendas

* Cliente
* Produtos
* Forma de pagamento
* Descrição
* Valores da venda

A estrutura foi pensada para manter os dados das vendas organizados e facilitar o acompanhamento financeiro da empresa.


## Despesas

O sistema também possui um módulo destinado ao controle de despesas, permitindo registrar os gastos da empresa e utilizá-los no acompanhamento financeiro.


## Relatórios

A API possui geração de relatório de produtos utilizando a biblioteca **ClosedXML**, permitindo exportar os dados para planilhas.


## Validação

Os dados recebidos pela API passam por validações antes de serem processados.
O projeto utiliza **FluentValidation** para centralizar regras de validação dos DTOs de entrada, evitando que dados inválidos sejam processados pela aplicação.

Exemplo de fluxo:
Request > DTO > Validação > Service > Database


## Testes

O projeto possui testes unitários para os principais serviços da aplicação, utilizando:

- **xUnit** — framework para execução dos testes;
- **Moq** — criação de mocks para dependências;
- **EF Core InMemory** — banco em memória para testar operações com o contexto.


## Objetivos técnicos

Além de resolver uma necessidade de gestão, o projeto foi desenvolvido como uma oportunidade para praticar e consolidar conhecimentos em:

* Desenvolvimento de APIs RESTful
* C# e ASP.NET Core
* Entity Framework Core
* SQL Server
* Autenticação e autorização com JWT
* DTOs e separação de responsabilidades
* Validação de dados
* Testes automatizados
* Integração entre frontend e backend
* Organização de aplicações em camadas
* Persistência e relacionamento de dados

https://marcelo-doceiro.vercel.app/
https://adm-doces-e-cia.vercel.app/

