PETCARE MVC - INSTRUÇÕES DE CRIAÇÃO DO BANCO DE DADOS E USUÁRIO ADMINISTRADOR

Este documento contém as instruções necessárias para preparar o banco de dados do sistema PetCare MVC
e criar o usuário administrador responsável pelo primeiro acesso ao sistema.

---------------------------------------------------------------------
1. PRÉ-REQUISITOS
---------------------------------------------------------------------

Antes de iniciar, certifique-se de possuir:

- .NET SDK 8 ou superior instalado
- SQL Server instalado
- SQL Server Management Studio (SSMS) ou ferramenta similar
- Projeto PetCare MVC configurado na máquina

---------------------------------------------------------------------
2. CONFIGURAR STRING DE CONEXÃO (OPCIONAL PARA OUTROS SERVIDORES)
---------------------------------------------------------------------

No arquivo:

appsettings.json

Localize a seção:

"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=PetCareDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

Altere SEU_SERVIDOR para o nome do seu servidor SQL.

Exemplo:

Server=localhost;Database=PetCareDB;Trusted_Connection=True;TrustServerCertificate=True;

---------------------------------------------------------------------
3. CRIAÇÃO DO BANCO DE DADOS
---------------------------------------------------------------------

Abra o terminal dentro da pasta do projeto e execute os comandos abaixo
para criar a estrutura do banco de dados utilizando Entity Framework:

dotnet ef migrations add InitialCreate

Após a criação da migration, execute:

dotnet ef database update

Esse processo irá criar automaticamente o banco de dados:

PetCareDB

com todas as tabelas necessárias para o funcionamento do sistema.

---------------------------------------------------------------------
4. CRIAÇÃO DO USUÁRIO ADMINISTRADOR
---------------------------------------------------------------------

Após a criação do banco de dados, é necessário inserir manualmente
um usuário administrador para permitir o primeiro acesso ao sistema.

Abra o SQL Server Management Studio (SSMS), conecte-se ao banco PetCareDB
e execute o seguinte comando SQL:

INSERT INTO Usuarios
(
    Nome,
    Email,
    Senha,
    Perfil
)
VALUES
(
    'Administrador',
    'admin@petcare.com',
    '123456',
    'Administrador'
);

---------------------------------------------------------------------
5. DADOS DE ACESSO INICIAL
---------------------------------------------------------------------

Após executar o comando SQL, utilize os seguintes dados para acessar o sistema:

Email: admin@petcare.com
Senha: 123456

Este usuário possui perfil de administrador e poderá acessar as
funcionalidades administrativas do sistema.

---------------------------------------------------------------------
6. EXECUÇÃO DO SISTEMA
---------------------------------------------------------------------

Após concluir a configuração do banco de dados e criar o usuário administrador,
execute o sistema com o comando:

dotnet run

O sistema ficará disponível normalmente no endereço exibido pelo .NET,
geralmente:

https://localhost:xxxx

---------------------------------------------------------------------
Fim das instruções.