# Semogly

Backend RESTful desenvolvido em **.NET Core** com **Clean Architecture**, criado como projeto pessoal para aplicar boas práticas de desenvolvimento de software em um cenário real.

## 🚀 Tecnologias

- **[.NET Core](https://dotnet.microsoft.com/)** — Framework principal da aplicação
- - **[PostgreSQL](https://www.postgresql.org/)** — Banco de dados relacional
  - - **[Entity Framework Core](https://learn.microsoft.com/ef/core/)** — ORM para acesso a dados
    - - **[JWT](https://jwt.io/)** — Autenticação e autorização segura
      - - **[RabbitMQ](https://www.rabbitmq.com/)** — Mensageria assíncrona
        - - **[Redis](https://redis.io/)** — Cache e performance
          - - **[Azure Key Vault](https://azure.microsoft.com/products/key-vault)** — Gerenciamento seguro de segredos
            - - **[Mailgun](https://www.mailgun.com/)** — Serviço de envio de e-mails
              - - **[Docker](https://www.docker.com/)** — Containerização da aplicação
               
                - ## 🏗️ Arquitetura
               
                - O projeto segue os princípios da **Clean Architecture**, organizado nas seguintes camadas:
               
                - ```
                  Semogly.Core/
                  ├── Semogly.Core.Api/            # Camada de apresentação (Controllers, endpoints)
                  ├── Semogly.Core.Application/    # Regras de negócio e casos de uso
                  ├── Semogly.Core.Domain/         # Entidades e interfaces do domínio
                  ├── Semogly.Core.Infrastructure/ # Implementações externas (banco, cache, e-mail)
                  └── Semogly.Worker/              # Worker service para processamento assíncrono
                  ```

                  ## ⚙️ Como rodar localmente

                  ### Pré-requisitos

                  - [.NET 8+](https://dotnet.microsoft.com/download)
                  - - [Docker](https://www.docker.com/)
                   
                    - ### 1. Clone o repositório
                   
                    - ```bash
                      git clone https://github.com/semog-dev/semogly.git
                      cd semogly/Semogly.Core
                      ```

                      ### 2. Configure as variáveis de ambiente

                      Copie o arquivo de exemplo e preencha as variáveis:

                      ```bash
                      cp appsettings.Example.json appsettings.json
                      ```

                      ### 3. Suba os serviços com Docker

                      ```bash
                      docker-compose -f compose.yaml up -d
                      ```

                      Isso irá iniciar:
                      - **PostgreSQL** na porta `5432`
                      - - **Redis** na porta `6379`
                        - - **RabbitMQ** na porta `5672`
                         
                          - ### 4. Execute a aplicação
                         
                          - ```bash
                            dotnet run --project Semogly.Core.Api
                            ```

                            ## 👤 Autor

                            **Fernando Pereira**
                            [LinkedIn](https://www.linkedin.com/in/fernando-pereira-dev) · [GitHub](https://github.com/semog-dev)
