# Controle Financeiro

Este projeto é um sistema de controle financeiro que permite o gerenciamento de lançamentos de receitas e despesas, possibilitando a visualização de um fluxo de caixa. O sistema oferece recursos para que o usuário registre, edite e visualize transações financeiras, exibindo os valores de forma detalhada e organizada. Além disso, ele destaca saldos positivos e negativos para uma análise rápida e eficiente da saúde financeira.

## 🚀 Funcionalidades

* Cadastro e edição de lançamentos financeiros (receitas e despesas).
* Exibição dos lançamentos em uma tabela com cores indicativas (vermelho para débitos e verde para créditos).
* Visualização do fluxo de caixa, com saldo final formatado e sinalizado conforme o valor.
* Tratamento de datas e valores monetários.
* Integração com uma API backend para persistência de dados.
* Login e controle de sessão de usuário, com logout automático após inatividade.

## 🛠️ Tecnologias

### 💻 Frontend

* React com TypeScript: Utilizado para criar uma interface de usuário interativa e dinâmica, aproveitando os benefícios do TypeScript para garantir segurança e tipagem estática no código.
* Axios: Biblioteca para realizar chamadas HTTP ao backend, facilitando a comunicação com a API.
* Bootstrap: Framework CSS utilizado para estilizar a aplicação e garantir uma interface responsiva e intuitiva.

### ⚙️ Backend

* ASP.NET Core: Framework utilizado para criar a API REST, que fornece os dados para o frontend e lida com a lógica de negócio, incluindo validações e manipulação de dados.
* Entity Framework Core: Utilizado para gerenciar o acesso ao banco de dados, facilitando operações CRUD (Create, Read, Update, Delete) e mapeamento objeto-relacional (ORM).

### 💽 Banco de Dados

* SQL Server: Base de dados utilizada para armazenar os lançamentos financeiros e informações de usuários.

### 🔒 Segurança

* JWT (JSON Web Token): Utilizado para autenticação e controle de acesso. O token é armazenado no frontend e enviado em cada requisição para validar o usuário.
* HTTPS: Comunicação segura entre frontend e backend utilizando HTTPS, garantindo a criptografia dos dados transmitidos.

## Técnicas

### 📦 Componentização

O React permite a criação de componentes reutilizáveis, o que facilita a manutenção e escalabilidade do projeto. Cada parte da interface, como tabelas, botões e formulários, foi construída como um componente independente.

### 🔧 Comunicação

* Uso do Axios para realizar requisições HTTP (GET, POST, PUT, DELETE) ao backend.
* Manipulação de erros e respostas da API para fornecer feedback adequado ao usuário (por exemplo, mensagens de erro ao receber status 400 ou 500).

### 👤 Autenticação

* Implementação de login com armazenamento do token JWT no frontend.
* Função de logout automático quando o token expira ou o usuário está inativo por muito tempo.

### ✅ Testes

A robustez do sistema foi garantida por meio de testes automatizados, implementados para verificar a funcionalidade das operações mais críticas da aplicação. Utilizando o framework de testes xUnit e o Moq para mocking, os testes cobrem cenários como a criação de usuários, login e controle de autenticação, garantindo que as funcionalidades críticas estejam funcionando corretamente. Além disso, o sistema foi projetado para detectar e responder adequadamente a erros, com a devida manipulação de exceções e retornos de status HTTP apropriados, como BadRequest em caso de falhas.

## 📌 Versão

Versão Inicial v1.0.0. 

## ✒️ Autores

* **Desenvolvedor** - *Rodrigo Galvão* - [digaogalvao](https://github.com/digaogalvao)

## 📄 Licença

Este projeto é opensource.

## 🎁 Expressões de gratidão

* Agradeço a minha família 📢
