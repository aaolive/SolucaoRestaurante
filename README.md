## Visão Geral do Projeto: Sistema de Restaurante com Microserviços

Este projeto demonstra a implementação de um sistema de backend para um restaurante, utilizando uma arquitetura de microserviços. O objetivo principal é criar um ecossistema de serviços desacoplados, resilientes e escaláveis, onde a comunicação é gerenciada de forma assíncrona através de um message broker (fila de mensagens).

A arquitetura foi desenhada para refletir um fluxo de negócio real: um cliente faz um pedido (ServicoCardapio), um garçom o gerencia e o encaminha (ServicoGarcom), e a cozinha o prepara e notifica quando está pronto (ServicoCozinha).

## 🛠️ Tecnologias Utilizadas (O "Stack")

### A escolha de tecnologias foi focada em criar uma solução moderna, performática e de fácil manutenção dentro do ecossistema .NET.

.NET 8 (e C#):

Descrição: A mais recente plataforma de desenvolvimento da Microsoft. É usada como base para todos os microserviços, fornecendo um ambiente de execução rápido, seguro e multiplataforma.

ASP.NET Core:

Descrição: Embora nem todos os serviços exponham APIs HTTP, o ASP.NET Core é usado como o host (hospedeiro) padrão. O ServicoCardapio o utiliza para criar uma Web API (para receber o pedido), enquanto os serviços Garcom e Cozinha o utilizam para executar seus processos em background (os consumers de mensagens).

RabbitMQ:

Descrição: É o "coração" da nossa comunicação. Trata-se de um Message Broker (corretor de mensagens) robusto e popular. Ele atua como um carteiro: os serviços não conversam diretamente entre si; eles publicam mensagens no RabbitMQ, que se encarrega de entregá-las aos serviços corretos que estão "ouvindo" as filas apropriadas.

MassTransit:

Descrição: Esta é uma biblioteca (framework) C# de código aberto que abstrai o RabbitMQ. Em vez de lidarmos manualmente com conexões, canais, exchanges (trocas) e queues (filas), o MassTransit nos permite simplesmente "publicar" uma classe C# (um contrato) e configurar "consumidores" para ela. Ele gerencia toda a complexidade da infraestrutura de mensageria, incluindo padrões como Publish/Subscribe, Retry (tentativas) e Circuit Breaker.

Entity Framework Core (EF Core):

Descrição: É o ORM (Object-Relational Mapper) padrão do .NET. Ele faz a "ponte" entre nosso código C# (nossas classes, como Pedido) e o banco de dados relacional. Ele nos permite escrever consultas e salvar dados usando C#, e o EF Core traduz isso para o SQL nativo do banco.

SQLite:

Descrição: Um mecanismo de banco de dados leve e baseado em arquivo. Foi escolhido para este projeto pela sua simplicidade (não requer um servidor separado), sendo ideal para desenvolvimento e prototipagem. Graças ao EF Core, poderíamos trocar para um banco mais robusto (como PostgreSQL ou SQL Server) em produção com o mínimo de esforço.
