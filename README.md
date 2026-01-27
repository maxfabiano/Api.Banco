🏦 Projeto BankMore - API Bancária
Este projeto consiste em uma arquitetura de microserviços desenvolvida em .NET 8, focada em alta disponibilidade e consistência transacional.

🛠 Tecnologias e Padrões
Banco de Dados (MySQL): Utilizamos o MySQL como storage principal.

Consistência Bancária (Serializable): Para garantir a segurança total das transações e evitar problemas como Phantom Reads ou Race Conditions em transferências, configuramos o nível de isolamento do banco como SERIALIZABLE. É o nível mais alto de isolamento, garantindo que cada transação seja processada de forma única e segura.

Modelagem de Dados: * Valores monetários utilizam o tipo decimal(18,2) para precisão absoluta (evitando erros de arredondamento de tipos float/double).

IDs (Chaves Primárias e Estrangeiras) utilizam int com relacionamentos via Foreign Keys devidamente mapeados.

Arquitetura:

MediatR: Implementação do padrão CQRS para desacoplar a lógica de negócio das Controllers.

Entity Framework Core: ORM utilizado para mapeamento das entidades e gerenciamento de relacionamentos complexos.

🚀 Como rodar o projeto
Não é necessário configurar nada manualmente. O ambiente está totalmente containerizado.

Certifique-se de ter o Docker instalado.

Na raiz do projeto, execute:

Bash
docker-compose up -d
O que acontece automaticamente:

O container do MySQL sobe e configura o usuário/senha.

O script de inicialização cria o banco de dados.

As Migrations do Entity Framework rodam sozinhas, criando as tabelas e os relacionamentos.

As APIs ficam disponíveis para uso imediato via Swagger.

Por que isso é bom?
Zero Setup: O desenvolvedor novo não perde tempo configurando banco.

Integridade: Com o isolamento Serializable e o uso de Decimals, o sistema trata o dinheiro do cliente com o rigor técnico que uma instituição financeira exige
